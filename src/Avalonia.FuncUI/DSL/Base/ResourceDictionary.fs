namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module ResourceDictionary =
    open Avalonia
    open Avalonia.Controls
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
    open Avalonia.Styling

    let create (attrs: IAttr<ResourceDictionary> list) : IView<ResourceDictionary> =
        ViewBuilder.Create<ResourceDictionary>(attrs)

    /// Type that indicates ResourceDictionary.item has been cleared
    type private ClearValue = ClearValue

    type ResourceDictionary with
        static member keyValue<'t when 't :> ResourceDictionary>(key: obj, value: obj) : IAttr<'t> =
            let name = $"Item.{key}"
            let value: obj option = Some value

            /// `defaultValueFactory` is called during property initialization. Returning `ClearValue` allows the setter to detect when the property is being reset to its default state.
            let factory: unit -> obj option = (fun () -> Some(box ClearValue))

            let getter: ('t -> obj option) = (fun control -> Option.ofObj control[key])

            /// When `ClearValue` is passed, the item is removed; otherwise, the value is set.
            let setter: ('t * obj option -> unit) =
                (fun (control, value) ->
                    match value with
                    | Some(:? ClearValue) when control.ContainsKey(key) -> control.Remove(key) |> ignore
                    | _ -> control.[key] <- Option.toObj value)


            AttrBuilder<'t>
                .CreateProperty<obj option>(name, value, ValueSome getter, ValueSome setter, ValueNone, factory)

        static member keyValue<'t, 'v when 't :> ResourceDictionary>(key: obj, view: IView<'v>) : IAttr<'t> =
            let name = $"Item.{key}"
            let singleContent = Some(view :> IView)
            let getter: ('t -> obj) = (fun control -> control[key])

            let setter: ('t * obj -> unit) =
                (fun (control, value) ->
                    match value with
                    | null -> control.Remove(key) |> ignore
                    | _ -> control.[key] <- value)

            AttrBuilder<'t>
                .CreateContentSingle(name, ValueSome getter, ValueSome setter, singleContent)

        static member mergedDictionaries<'t when 't :> ResourceDictionary>(dictionaries: IView list) : IAttr<'t> =
            let name = nameof Unchecked.defaultof<'t>.MergedDictionaries

            let getter = (fun (control: 't) -> control.MergedDictionaries :> obj)

            AttrBuilder<'t>
                .CreateContentMultiple(name, ValueSome getter, ValueNone, dictionaries)

        static member themeDictionariesKeyValue<'t, 'v when 't :> ResourceDictionary and 'v :> IThemeVariantProvider>
            (key: ThemeVariant, view: IView<'v>)
            : IAttr<'t> =
            let name = $"ThemeDictionaries.{key.Key}"
            let singleContent = Some(view :> IView)

            let getter: ('t -> obj) =
                (fun control ->
                    match control.ThemeDictionaries.TryGetValue(key) with
                    | true, value -> value
                    | false, _ -> null)

            let setter: ('t * obj -> unit) =
                (fun (control, value) ->
                    match value with
                    | null -> control.ThemeDictionaries.Remove(key) |> ignore
                    | _ -> control.ThemeDictionaries.[key] <- value :?> IThemeVariantProvider)

            AttrBuilder<'t>
                .CreateContentSingle(name, ValueSome getter, ValueSome setter, singleContent)
