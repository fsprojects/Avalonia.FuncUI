namespace Examples.ClockApp
open Avalonia
open Avalonia.FuncUI.DSL

module Clock =
    open System
    open Avalonia.Controls
    open Avalonia.Controls.Shapes
    
    type State = { time : DateTime }
    let init () = { time = DateTime.Now }

    type Msg = Tick of DateTime

    let update (msg: Msg) (state: State) : State =
        match msg with
        | Tick time -> { state with time = time }
    
    type PointerType = Hour | Minute | Second
    
    let calcPointerPosition (pointer: PointerType, time: DateTime) : Point =
        let percent =
            match pointer with
            | Hour -> (float time.Hour) / 12.0
            | Minute -> (float time.Minute) / 60.0
            | Second -> (float time.Second) / 60.0
            
        let length =
            match pointer with
            | Hour -> 50.0
            | Minute -> 60.0
            | Second -> 70.0
            
        let angle = 2.0 * Math.PI * percent
        let handX = (100.0 + length * cos (angle - Math.PI / 2.0))
        let handY = (100.0 + length * sin (angle - Math.PI / 2.0))
        Point(handX, handY)
        
    
    let view (state: State) (_dispatch) =
        Canvas.create [
            Canvas.background "#2c3e50"
            Canvas.children [
                
                Ellipse.create [
                    Ellipse.top 10.0
                    Ellipse.left 10.0
                    Ellipse.width 180.0
                    Ellipse.height 180.0
                    Ellipse.fill "#ecf0f1"
                ]
                
                Line.create [
                    Line.startPoint (100.0, 100.0)
                    Line.endPoint (calcPointerPosition(Second, state.time))
                    Line.strokeThickness 2.0
                    Line.stroke "#e74c3c"
                ]
                
                Line.create [
                    Line.startPoint (100.0, 100.0)
                    Line.endPoint (calcPointerPosition(Minute, state.time))
                    Line.strokeThickness 4.0
                    Line.stroke "#7f8c8d"
                ]
                
                Line.create [
                    Line.startPoint (100.0, 100.0)
                    Line.endPoint (calcPointerPosition(Hour, state.time))
                    Line.strokeThickness 6.0
                    Line.stroke "black"
                ]
                
                Ellipse.create [
                    Ellipse.top 95.0
                    Ellipse.left 95.0
                    Ellipse.width 10.0
                    Ellipse.height 10.0
                    Ellipse.fill "#95a5a6"
                ]
            ]
        ]       