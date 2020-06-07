namespace Avalonia.FuncUI.VirtualDom

module internal rec Patcher =
    open System
    open System.Collections
    open System.Collections.Concurrent
    open Avalonia.Controls
    open Avalonia
    open Avalonia.FuncUI.VirtualDom.Delta
    open Avalonia.FuncUI.Library
    open Avalonia.FuncUI.Types
    open System.Threading

    let private patchSubscription (view: IControl) (attr: SubscriptionDelta) : unit =
        let subscriptions =
            match ViewMetaData.GetViewSubscriptions(view) with
            | null ->
                let dict = ConcurrentDictionary<_, _>()
                ViewMetaData.SetViewSubscriptions(view, dict)
                dict
            | value -> value

        match attr.func with
        // add or update
        | Some handler ->
            let cts = attr.subscribe(view, handler)

            let addFactory = Func<string, CancellationTokenSource>(fun key -> cts)

            let updateFactory = Func<string, CancellationTokenSource, CancellationTokenSource>(fun key old_cts ->
                old_cts.Cancel()
                cts
            )

            subscriptions.AddOrUpdate(attr.UniqueName, addFactory, updateFactory) |> ignore

        // remove
        | None ->
            let hasValue, value = subscriptions.TryGetValue(attr.UniqueName)
            if hasValue then
                value.Cancel()
                subscriptions.TryRemove(attr.UniqueName) |> ignore

    let private patchProperty (view: IControl) (attr: PropertyDelta) : unit =
        match attr.accessor with
        | Accessor.AvaloniaProperty avaloniaProperty ->
            match attr.value with
            | Some value ->
                // TODO: check if the default binding priority is what we want here
                view.SetValue(avaloniaProperty, value) |> ignore
            | None ->
                match attr.defaultValueFactory with
                | ValueNone ->
                    view.ClearValue (avaloniaProperty)
                | ValueSome factory ->
                    let value = factory()
                    view.SetValue(avaloniaProperty, value) |> ignore

        | Accessor.InstanceProperty instanceProperty ->
            let propertyInfo = view.GetType().GetProperty(instanceProperty.name);

            match attr.value with
            | Some value ->
                match propertyInfo.CanWrite with
                | true -> propertyInfo.SetValue(view, value)
                | false -> raise (Exception "Can't set readonly instance property")
            | None ->
                let defaultValue =
                    match attr.defaultValueFactory with
                    | ValueSome factory -> factory()
                    | ValueNone ->
                        if propertyInfo.PropertyType.IsValueType then
                            Activator.CreateInstance(propertyInfo.PropertyType)
                        else
                            null

                propertyInfo.SetValue(view, defaultValue)

    let private patchContentMultiple (view: IControl) (accessor: Accessor) (delta: ViewDelta list) : unit =
        (* often lists only have a get accessor *)
        let patch_IList (collection: IList) : unit =
            if List.isEmpty delta then
                collection.Clear()
            else
                delta |> Seq.iteri (fun index viewElement ->
                    // try patch / reuse
                    if index + 1 <= collection.Count then
                        let item = collection.[index]

                        if item.GetType() = viewElement.viewType then
                            // patch
                            match item with
                            | :? Avalonia.Controls.IControl as control -> patch(control, viewElement)
                            | _ ->
                                // replace
                                let newItem = Patcher.create viewElement
                                collection.[index] <- newItem
                        else
                            // replace
                            let newItem = Patcher.create viewElement
                            collection.[index] <- newItem
                    else
                        // create
                        let newItem = Patcher.create viewElement
                        collection.Add(newItem) |> ignore

                )

                while delta.Length < collection.Count do
                    collection.RemoveAt (collection.Count - 1)

        (* read only, so there must be a get accessor *)
        let patch_IEnumerable (collection: IEnumerable) : IEnumerable =
            let newList =
                collection
                |> Seq.cast<obj>
                |> ResizeArray
                
            patch_IList newList

            (newList :> IEnumerable)

        let patch (getValue: (unit -> obj) option, setValue: (obj -> unit) option) =
            let value =
                match getValue with
                | Some get -> get()
                | _ -> failwith "accessor must have a getter"

            match value with
            | :? IList as collection ->
                patch_IList collection

            | :? IEnumerable as enumerable ->
                match setValue with
                | Some set -> set (patch_IEnumerable enumerable)
                | _ -> failwith "accessor must have a setter"

            | _ -> raise (Exception("type does not implement IEnumerable or IList. This is required for view patching"))

        match accessor with
        | Accessor.InstanceProperty instanceProperty ->
            let getter =
                match instanceProperty.getter with
                | ValueSome getter -> Some (fun () -> getter(view))
                | ValueNone -> None

            let setter =
                match instanceProperty.setter with
                | ValueSome setter -> Some (fun value -> setter(view, value))
                | ValueNone -> None

            patch (getter ,setter)

        | Accessor.AvaloniaProperty property ->
            let getter = Some (fun () -> view.GetValue(property))
            let setter = Some (fun obj -> view.SetValue(property, obj) |> ignore)
            patch (getter, setter)

    let private patchContentSingle (view: IControl) (accessor: Accessor) (viewElement: ViewDelta option) : unit =

        let patch_avalonia (property: AvaloniaProperty) =
            match viewElement with
            | Some viewElement ->
                let value = view.GetValue(property)

                if value <> null && value.GetType() = viewElement.viewType then
                    Patcher.patch(value :?> IControl, viewElement)
                else
                    let createdControl = Patcher.create(viewElement)
                    view.SetValue(property, createdControl) |> ignore
            | None ->
                (view :?> AvaloniaObject).ClearValue(property)

        let patch_instance (property: PropertyAccessor) =
            match viewElement with
            | Some viewElement ->
                let value =
                    match property.getter with
                    | ValueSome getter -> getter(view)
                    | _ -> failwith "Property Accessor needs a getter"

                if value <> null && value.GetType() = viewElement.viewType then
                    Patcher.patch(value :?> IControl, viewElement)
                else
                    let createdControl = Patcher.create(viewElement)

                    match property.setter with
                    | ValueSome setter -> setter(view, createdControl)
                    | _ -> failwith "Property Accessor needs a setter"
            | None ->
                match property.setter with
                | ValueSome setter -> setter(view, null)
                | _ -> failwith "Property Accessor needs a setter"

        match accessor with
        | Accessor.InstanceProperty instanceProperty -> patch_instance instanceProperty
        | Accessor.AvaloniaProperty property -> patch_avalonia property

    let private patchContent (view: IControl) (attr: ContentDelta) : unit =
        match attr.content with
        | ViewContentDelta.Single single ->
            patchContentSingle view attr.accessor single
        | ViewContentDelta.Multiple multiple ->
            patchContentMultiple view attr.accessor multiple

    let patch (view: IControl, viewElement: ViewDelta) : unit =
        for attr in viewElement.attrs do
            match attr with
            | AttrDelta.Property property -> patchProperty view property
            | AttrDelta.Content content -> patchContent view content
            | AttrDelta.Subscription subscription -> patchSubscription view subscription

    let create (viewElement: ViewDelta) : IControl =
        let control = viewElement.viewType |> Activator.CreateInstance |> Utils.cast<IControl>

        control.SetValue(ViewMetaData.ViewIdProperty, Guid.NewGuid())
        Patcher.patch (control, viewElement)
        control