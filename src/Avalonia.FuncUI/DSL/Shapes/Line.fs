namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module Line =
    open Avalonia
    open Avalonia.Controls.Shapes
    open Avalonia.FuncUI.Builder
    open Avalonia.FuncUI.Types

    let create (attrs: Attr<Line> list): IView<Line> =
        ViewBuilder.Create<Line>(attrs)

    type Line with

        static member startPoint<'t when 't :> Line>(value: Point) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<Point>(Line.StartPointProperty, value, ValueNone)

        static member startPoint<'t when 't :> Line>(x: double, y: double) : Attr<'t> =
            (x, y) |> Point |> Line.startPoint

        static member endPoint<'t when 't :> Line>(value: Point) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<Point>(Line.EndPointProperty, value, ValueNone)

        static member endPoint<'t when 't :> Line>(x: double, y: double) : Attr<'t> =
            (x, y) |> Point |> Line.endPoint