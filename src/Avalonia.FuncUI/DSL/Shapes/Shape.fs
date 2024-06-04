namespace Avalonia.FuncUI.DSL
open Avalonia.Collections
open Avalonia.Media.Immutable

[<AutoOpen>]
module Shape =
    open Avalonia.Controls.Shapes
    open Avalonia.Media
    open Avalonia.FuncUI
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder

    type Shape with

        static member fill<'t when 't :> Shape>(brush: IBrush) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<IBrush>(Shape.FillProperty, brush, ValueNone)

        static member fill<'t when 't :> Shape>(color: string) : IAttr<'t> =
            color |> Color.Parse |> ImmutableSolidColorBrush |> Shape.fill

        static member fill<'t when 't :> Shape>(color: Color) : IAttr<'t> =
            color |> ImmutableSolidColorBrush |> Shape.fill

        static member stretch<'t when 't :> Shape>(value: Stretch) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<Stretch>(Shape.StretchProperty, value, ValueNone)

        static member stroke<'t when 't :> Shape>(brush: IBrush) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<IBrush>(Shape.StrokeProperty, brush, ValueNone)

        static member stroke<'t when 't :> Shape>(color: string) : IAttr<'t> =
            color |> Color.Parse |> ImmutableSolidColorBrush |> Shape.stroke

        static member stroke<'t when 't :> Shape>(color: Color) : IAttr<'t> =
            color |> ImmutableSolidColorBrush |> Shape.stroke

        static member strokeThickness<'t when 't :> Shape>(value: double) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<double>(Shape.StrokeThicknessProperty, value, ValueNone)

        static member strokeDashArray<'t when 't :> Shape>(value: AvaloniaList<double>) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<AvaloniaList<double>>(Shape.StrokeDashArrayProperty, value, ValueNone)

        static member strokeDashArray<'t when 't :> Shape>(value: double list) : IAttr<'t> =
            let name = nameof Unchecked.defaultof<'t>.StrokeDashArray
            let getter: 't -> double list = fun x -> Seq.toList x.StrokeDashArray
            let setter: 't * double list -> unit = fun (x, value) -> Setters.avaloniaList x.StrokeDashArray value

            AttrBuilder<'t>.CreateProperty<double list>(name, value, ValueSome getter, ValueSome setter, ValueNone)

        static member strokeDashOffset<'t when 't :> Shape>(value: double) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<double>(Shape.StrokeDashOffsetProperty, value, ValueNone)

        static member strokeLineCap<'t when 't :> Shape>(value: PenLineCap) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<PenLineCap>(Shape.StrokeLineCapProperty, value, ValueNone)

        static member strokeJoin<'t when 't :> Shape>(value: PenLineJoin) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<PenLineJoin>(Shape.StrokeJoinProperty, value, ValueNone)
