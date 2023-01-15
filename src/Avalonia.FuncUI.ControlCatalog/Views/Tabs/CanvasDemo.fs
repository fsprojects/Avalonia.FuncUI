namespace Avalonia.FuncUI.ControlCatalog.Views

open Elmish
open Avalonia
open Avalonia.Controls
open Avalonia.Controls.Shapes
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI
open Avalonia.FuncUI.Elmish
open Avalonia.Media

module CanvasDemo =
    type State = { itemCount: int }

    let init () = { itemCount = 100 }

    type Msg =
    | SetCount of int

    let update (msg: Msg) (state: State) : State =
        match msg with
        | SetCount count -> { state with itemCount = count }
           
    let view (_state: State) (_dispatch) =
        Canvas.create [
            Canvas.width 300.0
            Canvas.height 400.0
            Canvas.background "yellow"
            Canvas.children [
                Rectangle.create [
                    Rectangle.fill "blue"
                    Rectangle.width 63.0
                    Rectangle.height 41.0
                    Rectangle.left 40.0
                    Rectangle.top 31.0
                ]
                
                Ellipse.create [
                    Ellipse.fill "green"
                    Ellipse.width 58.0
                    Ellipse.height 58.0
                    Ellipse.left 88.0
                    Ellipse.top 100.0
                ]
                
                Path.create [
                    Path.fill "orange"
                    Path.data "M 0,0 c 0,0 50,0 50,-50 c 0,0 50,0 50,50 h -50 v 50 l -50,-50 Z"
                    Path.left 30.0
                    Path.top 250.0
                ]
                
                Line.create [
                    Line.startPoint (120.0, 185.0)
                    Line.endPoint (30.0, 115.0)
                    Line.strokeLineCap PenLineCap.Round
                    Line.strokeJoinCap PenLineJoin.Bevel
                    Line.stroke "red"
                    Line.strokeThickness 2.0
                ]
                
                Polygon.create [
                    Polygon.points [
                        Point(75.0, 0.0)
                        Point(120.0, 120.0)
                        Point(0.0, 45.0)
                        Point(150.0, 45.0)
                        Point(30.0, 120.0)
                    ]
                    Polygon.fill "Violet"
                    Polygon.stroke "DarkBlue"
                    Polygon.strokeThickness 1.0
                    Polygon.left 150.0
                    Polygon.top 31.0
                ]
                
                Polyline.create [
                    Polyline.points [
                        Point(0.0, 0.0)
                        Point(65.0, 0.0)
                        Point(78.0, -26.0)
                        Point(91.0, 39.0)
                        Point(104.0, -39.0)
                        Point(117.0, 13.0)
                        Point(130.0, 0.0)
                        Point(195.0, 0.0)
                    ]
                    Polyline.stroke "Brown"
                    Polyline.strokeThickness 1.0
                    Polyline.left 30.0
                    Polyline.top 350.0
                ]
            ]
        ]
        
    type Host() as this =
        inherit Hosts.HostControl()
        do
            Elmish.Program.mkSimple init update view
            |> Program.withHost this
            |> Program.withConsoleTrace
            |> Program.runWith ()
        
        
        

