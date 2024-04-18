namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module Inline =
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
    open Avalonia.Controls.Documents
    open Avalonia.Media

    type Inline with
        static member textDecorations<'t when 't :> Inline>(value: TextDecorationCollection) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<TextDecorationCollection>(Inline.TextDecorationsProperty, value, ValueNone)

        static member textDecorations<'t when 't :> Inline>(textDecorations: TextDecoration list) : IAttr<'t> =
            TextDecorationCollection(textDecorations) |> Inline.textDecorations
        
        static member textDecorations<'t when 't :> Inline>(textDecorationViews: IView<TextDecoration> list) : IAttr<'t> =
            let name = nameof Unchecked.defaultof<'t>.TextDecorations
            let getter : ('t -> obj) = (fun control -> control.TextDecorations :> obj)
            let textDecorationViews = textDecorationViews |> List.map (fun x -> x :> IView)
            AttrBuilder<'t>.CreateContentMultiple(name, ValueSome getter, ValueNone, textDecorationViews)

        static member baselineAlignment<'t when 't :> Inline>(value: BaselineAlignment) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<BaselineAlignment>(Inline.BaselineAlignmentProperty, value, ValueNone)
        