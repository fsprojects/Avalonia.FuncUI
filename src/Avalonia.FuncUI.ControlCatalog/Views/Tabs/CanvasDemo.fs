namespace Avalonia.FuncUI.ControlCatalog.Views

open System
open Avalonia.Controls
open Avalonia.Controls.Shapes
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Components
open Avalonia.FuncUI.Elmish
open Elmish
open Avalonia.Layout
open Avalonia.Media
open SharpDX.Direct3D11

module CanvasDemo =
    type State = { itemCount: int }

    let init = { itemCount = 100 }

    type Msg =
    | SetCount of int

    let update (msg: Msg) (state: State) : State =
        match msg with
        | SetCount count -> { state with itemCount = count }
           
    let view (state: State) (dispatch) =
        Canvas.create [
            Canvas.width 300.0
            Canvas.height 300.0
            Canvas.background "white"
            Canvas.children [
                Line.create [
                    Line.left 10.0
                    Line.top 10.0
                    Line.startPoint (0.0, 0.0)
                    Line.endPoint (0.0, 280.0)
                    Line.strokeLineCap PenLineCap.Round
                    Line.strokeJoinCap PenLineJoin.Bevel
                    Line.stroke "orange"
                    Line.fill "red"
                    Line.strokeThickness 2.0
                ]
            ]
        ]
        
    type Host() as this =
        inherit Hosts.HostControl()
        do
            Elmish.Program.mkSimple (fun () -> init) update view
            |> Program.withHost this
            |> Program.withConsoleTrace
            |> Program.run
        
        
        

