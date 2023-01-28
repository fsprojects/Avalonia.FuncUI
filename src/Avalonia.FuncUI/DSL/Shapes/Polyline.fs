namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module Polyline =
    open System.Collections.Generic
    open Avalonia
    open Avalonia.Controls.Shapes
    open Avalonia.FuncUI.Builder
    open Avalonia.FuncUI.Types

    let create (attrs: Attr<Polyline> list): View<Polyline> =
        ViewBuilder.Create<Polyline>(attrs)

    type Polyline with

        static member points<'t when 't :> Polyline>(points: IList<Point>) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<IList<Point>>(Polyline.PointsProperty, points, ValueNone)

        static member points<'t when 't :> Polyline>(points: Point list) : Attr<'t> =
            points |> ResizeArray |> Polyline.points