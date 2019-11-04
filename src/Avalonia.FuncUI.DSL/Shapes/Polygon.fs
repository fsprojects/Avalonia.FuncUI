namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module Polygon =
    open System.Collections.Generic
    open Avalonia
    open Avalonia.Controls.Shapes
    open Avalonia.FuncUI.Builder
    open Avalonia.FuncUI.Types
    
    let create (attrs: IAttr<Polygon> list): IView<Polygon> =
        ViewBuilder.Create<Polygon>(attrs)
     
    type Polygon with

        static member points<'t when 't :> Polygon>(points: IList<Point>) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<IList<Point>>(Polygon.PointsProperty, points, ValueNone)
            
        static member points<'t when 't :> Polygon>(points: Point list) : IAttr<'t> =
            points |> ResizeArray |> Polygon.points