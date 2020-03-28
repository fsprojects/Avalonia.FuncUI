namespace Examples.CounterApp

open System
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.DSL
open Avalonia.Media
open SharpDX.Direct2D1

module Counter =
    open Avalonia.Controls
    open Avalonia.FuncUI.DSL
    open Avalonia.Layout
    
    type State = { count : int }
    let init = { count = 0 }

    type Msg =
    | Increment
    | Decrement
    | SetCount of int
    | Reset 

    let update (msg: Msg) (state: State) : State =
        match msg with
        | Increment -> { state with count = state.count + 1 }
        | Decrement -> { state with count = state.count - 1 }
        | SetCount count  -> { state with count = count } 
        | Reset -> init
    
    let view (state: State) (dispatch) =
        DockPanel.create [
            DockPanel.children [
                Button.create [
                    Button.dock Dock.Bottom
                    Button.onClick ((fun _ -> dispatch Reset), SubPatchOptions.Never)
                    Button.content "reset"
                ]                
                Button.create [
                    Button.dock Dock.Bottom
                    Button.onClick ((fun _ -> dispatch Decrement), SubPatchOptions.Never)
                    Button.content "-"
                ]
                Button.create [
                    Button.dock Dock.Bottom
                    Button.onClick ((fun _ -> dispatch Increment), SubPatchOptions.Never)
                    Button.content "+"
                ]
                Button.create [
                    Button.dock Dock.Bottom
                    Button.onClick ((fun _ -> state.count * 2 |> SetCount |> dispatch), SubPatchOptions.OnChangeOf state.count)
                    Button.content "x2"
                ]
                TextBox.create [
                    TextBox.dock Dock.Bottom
                    TextBox.onTextChanged ((fun text ->
                        let isNumber, number = System.Int32.TryParse text
                        if isNumber then
                            number |> SetCount |> dispatch)
                            
                    , SubPatchOptions.Never)
                    TextBox.text (string state.count)
                ]
                TextBlock.create [
                    TextBlock.dock Dock.Top
                    TextBlock.fontSize 48.0
                    TextBlock.verticalAlignment VerticalAlignment.Center
                    TextBlock.horizontalAlignment HorizontalAlignment.Center
                    TextBlock.text (string state.count)
                    TextBox.useEffect (fun control ->
                        
                        let random = new Random();
                        let color = String.Format("#{0:X6}", random.Next(0x1000000))
                        
                        control.Background <- SolidColorBrush.Parse color
                    )
                ]
            ]
        ]       