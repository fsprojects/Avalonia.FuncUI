namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module Polyline =
    open System.Collections.Generic
    open Avalonia
    open Avalonia.Controls.Shapes
    open Avalonia.FuncUI.Builder
    open Avalonia.FuncUI.Types
    
    let create (attrs: IAttr<Polyline> list): IView<Polyline> =
        ViewBuilder.Create<Polyline>(attrs)
     
    type Polyline with

        static member points<'t when 't :> Polyline>(points: IList<Point>) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, IList<Point>>(Polyline.PointsProperty, points, ValueNone)
            
        static member points<'t when 't :> Polyline>(points: Point list) : IAttr<'t> =
            points |> ResizeArray |> Polyline.points