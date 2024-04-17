namespace Avalonia.FuncUI.DSL

open Avalonia.Controls
open Avalonia.Controls.Primitives

[<AutoOpen>]
module IStyleHost =
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
    open Avalonia.Styling


    module private Internals =
        open System.Collections.Generic
        open System.Linq

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

        let compareStyleSeq<'e when 'e :> seq<IStyle>> (a: obj, b: obj) =
            match a, b with
            | (:? 'e as a), (:? 'e as b) -> Enumerable.SequenceEqual(a, b)
            | _ -> a = b

    type IStyleHost with

        /// Use 'classes' instead when possible.
        static member styles<'t when 't :> IStyleHost>(value: Styles) : IAttr<'t> =
            let getter: ('t -> Styles) = (fun control -> control.Styles)

            let setter: ('t * Styles -> unit) =
                (fun (control, value) ->
                    Internals.patchStyles control.Styles value)

            AttrBuilder<'t>
                .CreateProperty<Styles>(
                    "Styles",
                    value,
                    ValueSome getter,
                    ValueSome setter,
                    ValueSome Internals.compareStyleSeq<Styles>,
                    fun () -> Styles()
                )

        /// Use 'classes' instead when possible.
        static member styles<'t when 't :> IStyleHost>(styles: IStyle list) : IAttr<'t> =

            let getter: ('t -> (IStyle list)) = (fun control -> control.Styles |> Seq.toList)

            let setter: ('t * IStyle list -> unit) =
                (fun (control, value) -> Internals.patchStyles control.Styles value)

            let factory = fun () -> []

            AttrBuilder<'t>
                .CreateProperty<IStyle list>(
                    "Styles",
                    styles,
                    ValueSome getter,
                    ValueSome setter,
                    ValueSome Internals.compareStyleSeq<list<IStyle>>,
                    factory
                )
