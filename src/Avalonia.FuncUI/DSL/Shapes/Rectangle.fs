namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module Rectangle =
    open Avalonia.Controls.Shapes
    open Avalonia.FuncUI.Builder
    open Avalonia.FuncUI.Types

    let create (attrs: IAttr<Rectangle> list): IView<Rectangle> =
        ViewBuilder.Create<Rectangle>(attrs)

    type Rectangle with
        static member radiusX<'t when 't :> Rectangle>(value: double) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<double>(Rectangle.RadiusXProperty, value, ValueNone)

        static member radiusY<'t when 't :> Rectangle>(value: double) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<double>(Rectangle.RadiusYProperty, value, ValueNone)
