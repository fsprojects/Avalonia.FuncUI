namespace Avalonia.FuncUI.VirtualDom

module internal rec Patcher =
    open System
    open System.Collections
    
    open Avalonia.Controls
    open Avalonia

    open Avalonia.FuncUI.VirtualDom.Delta
    open Avalonia.FuncUI.Core.Domain    
    
    let private create (viewType: Type) : IControl =
        let control = Activator.CreateInstance(viewType) :?> IControl
        control.SetValue(Tagging.ViewTag.ViewIdProperty, Guid.NewGuid())
        control
        
    let private patchProperty (view: IControl) (attr: PropertyDelta) : unit =
        match attr.accessor with
        | Accessor.Avalonia avaloniaProperty ->
            match attr.value with
            | Some value -> view.SetValue(avaloniaProperty, value);
            | None ->
                // TODO: create PR - include 'ClearValue' in interface 'IAvaloniaObject'
                (view :?> AvaloniaObject).ClearValue(avaloniaProperty);
                
        | Accessor.Instance propertyName ->
            let propertyInfo = view.GetType().GetProperty(propertyName);
            
            match attr.value with
            | Some value ->
                match propertyInfo.CanWrite with
                | true -> propertyInfo.SetValue(view, value)
                | false ->
                    raise (Exception "cant set read only instance property")
            | None ->
                let defaultValue =
                    if propertyInfo.PropertyType.IsValueType
                    then Activator.CreateInstance(propertyInfo.PropertyType)
                    else null
                                
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
                                let newItem = Patcher.create viewElement.viewType
                                patch(newItem, viewElement)
                                collection.[index] <- newItem
                        else
                            // replace
                            let newItem = Patcher.create viewElement.viewType
                            patch(newItem, viewElement)
                            collection.[index] <- newItem
                    else
                        // create
                        let newItem = Patcher.create viewElement.viewType
                        patch(newItem, viewElement)
                        collection.Add(newItem) |> ignore

                )

                while delta.Length < collection.Count do
                    collection.RemoveAt (collection.Count - 1)

        (* read only, so there must be a get accessor *)
        let patch_IEnumerable (collection: IEnumerable) : IEnumerable =
            let newList = System.Collections.Generic.List<obj>()

            if List.isEmpty delta then
                () // list is empty by default
            else
                let mutable index = 0
                for item in collection do  
                    if index + 1 <= delta.Length then
                        if item.GetType() = delta.[index].GetType() then
                            newList.Add delta.[index]
                        else
                            let newItem = Patcher.create delta.[index].viewType
                            patch(newItem, delta.[index])
                            newList.Add delta.[index]
                    else ()

                if index + 1 < delta.Length then
                    let _, remaining = delta |> List.splitAt index

                    for item in remaining do
                        let newItem = Patcher.create delta.[index].viewType
                        patch(newItem, delta.[index])
                        newList.Add delta.[index]

            (newList :> IEnumerable)
        
        
        let patch (getValue: unit -> obj, setValue: obj -> unit) =
            let value = getValue()
            
            match value with
            | :? IList as collection ->
                patch_IList collection
                
            | :? IEnumerable as enumerable ->
                setValue (patch_IEnumerable enumerable)
                
            | _ -> raise (Exception("type does not implement IEnumerable or IList. This is required for view patching"))
        
        match accessor with
        | Instance propertyName ->
            let propertyInfo = view.GetType().GetProperty(propertyName)
            let getter = fun () -> propertyInfo.GetValue(view)
            let setter = fun obj -> propertyInfo.SetValue(view, obj)
            patch (getter, setter)
            
        | Avalonia property ->
            let getter = fun () -> view.GetValue(property)
            let setter = fun obj -> view.SetValue(property, obj)
            patch (getter, setter)
                
    let private patchContentSingle (view: IControl) (accessor: Accessor) (viewElement: ViewDelta option) : unit =
        
        let patch_avalonia (property: AvaloniaProperty) =
            match viewElement with
            | Some viewElement ->
                let value = view.GetValue(property)
                
                if value <> null && value.GetType() = viewElement.viewType then
                    Patcher.patch(value :?> IControl, viewElement)
                else
                    let createdControl = Patcher.create(viewElement.viewType)
                    Patcher.patch(createdControl, viewElement)
                    view.SetValue(property, createdControl)
            | None ->
                (view :?> AvaloniaObject).ClearValue(property)
                
        let patch_instance (propertyName: string) =
            let propertyInfo = view.GetType().GetProperty(propertyName);
            match viewElement with
            | Some viewElement ->
                let value = propertyInfo.GetValue(view)
                
                if value <> null && value.GetType() = viewElement.viewType then
                    Patcher.patch(value :?> IControl, viewElement)
                else
                    let createdControl = Patcher.create(viewElement.viewType)
                    Patcher.patch(createdControl, viewElement)
                    propertyInfo.SetValue(view, createdControl)
            | None ->
                propertyInfo.SetValue(view, null)        
        
        match accessor with
        | Instance propertyName -> patch_instance propertyName
        | Avalonia property -> patch_avalonia property

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
