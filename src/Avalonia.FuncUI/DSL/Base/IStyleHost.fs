namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module IStyleHost =
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
    open Avalonia.Styling


    module private Internals =

        let patchStyles (styles: Styles) (newValues: IStyle seq) =
            let newValues = newValues |> Seq.toList

            if List.isEmpty newValues then
                styles.Clear()
            else
                styles |> Seq.except newValues |> styles.RemoveAll

                for newIndex, newStyle in List.indexed newValues do
                    let oldIndex = styles |> Seq.tryFindIndex ((=) newStyle)

                    match oldIndex with
                    | Some oldIndex when oldIndex = newIndex -> ()
                    | Some oldIndex -> styles.Move(oldIndex, newIndex)
                    | None -> styles.Insert(newIndex, newStyle)

    type IStyleHost with

        /// Use 'classes' instead when possible.
        static member styles<'t when 't :> IStyleHost>(value: Styles) : IAttr<'t> =
            let getter: ('t -> Styles) = (fun control -> control.Styles)

            let setter: ('t * Styles -> unit) =
                (fun (control, value) ->
                    Internals.patchStyles control.Styles value
                    control.Styles.Resources <- value.Resources)

            let compare: (obj * obj -> bool) =
                fun (a, b) ->
                    match a, b with
                    | (:? Styles as a), (:? Styles as b) ->
                        System.Linq.Enumerable.SequenceEqual(a, b)
                        && System.Linq.Enumerable.SequenceEqual(a.Resources, b.Resources)
                        && System.Linq.Enumerable.SequenceEqual(
                            a.Resources.MergedDictionaries,
                            b.Resources.MergedDictionaries
                        )
                        && System.Linq.Enumerable.SequenceEqual(
                            a.Resources.ThemeDictionaries,
                            b.Resources.ThemeDictionaries
                        )
                    | _ -> a = b

            let factory = fun () -> Styles()

            AttrBuilder<'t>.CreateProperty<Styles>("Styles", value, ValueSome getter, ValueSome setter, ValueSome compare, factory)

        /// Use 'classes' instead when possible.
        static member styles<'t when 't :> IStyleHost>(styles: IStyle list) : IAttr<'t> =
            let getter: ('t -> (IStyle list)) = (fun control -> control.Styles |> Seq.toList)

            let setter: ('t * IStyle list -> unit) =
                (fun (control, value) -> Internals.patchStyles control.Styles value)

            let compare: (obj * obj -> bool) =
                fun (a, b) ->
                    match a, b with
                    | (:? list<IStyle> as a), (:? list<IStyle> as b) -> System.Linq.Enumerable.SequenceEqual(a, b)
                    | _ -> a = b

            let factory = fun () -> []

            AttrBuilder<'t>.CreateProperty<IStyle list>("Styles", styles, ValueSome getter, ValueSome setter, ValueSome compare, factory)
