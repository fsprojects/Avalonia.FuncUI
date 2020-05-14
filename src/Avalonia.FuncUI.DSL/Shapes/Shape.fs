namespace Avalonia.FuncUI.DSL
open Avalonia.Collections
open Avalonia.Media.Immutable

[<AutoOpen>]
module Shape =
    open Avalonia.Controls.Shapes
    open Avalonia.Media
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder

    type Shape with

        static member fill<'t when 't :> Shape>(brush: IBrush) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, IBrush>(Shape.FillProperty, brush, ValueNone)
            
        static member fill<'t when 't :> Shape>(color: string) : IAttr<'t> =
            color |> Color.Parse |> ImmutableSolidColorBrush |> Shape.fill
        
        static member stretch<'t when 't :> Shape>(value: Stretch) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, Stretch>(Shape.StretchProperty, value, ValueNone)
            
        static member stroke<'t when 't :> Shape>(brush: IBrush) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, IBrush>(Shape.StrokeProperty, brush, ValueNone)
            
        static member stroke<'t when 't :> Shape>(color: string) : IAttr<'t> =
            color |> Color.Parse |> ImmutableSolidColorBrush |> Shape.stroke
            
        static member strokeThickness<'t when 't :> Shape>(value: double) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, double>(Shape.StrokeThicknessProperty, value, ValueNone)
            
        static member strokeDashArray<'t when 't :> Shape>(value: AvaloniaList<double>) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, AvaloniaList<double>>(Shape.StrokeDashArrayProperty, value, ValueNone)
            
        static member strokeDashArray<'t when 't :> Shape>(value: double list) : IAttr<'t> =
            value |> AvaloniaList |> Shape.strokeDashArray
            
        static member strokeDashOffset<'t when 't :> Shape>(value: double) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, double>(Shape.StrokeDashOffsetProperty, value, ValueNone)
            
        static member strokeLineCap<'t when 't :> Shape>(value: PenLineCap) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, PenLineCap>(Shape.StrokeLineCapProperty, value, ValueNone)

        static member strokeJoinCap<'t when 't :> Shape>(value: PenLineJoin) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, PenLineJoin>(Shape.StrokeJoinProperty, value, ValueNone)