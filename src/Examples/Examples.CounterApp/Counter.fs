namespace Examples.CounterApp

open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.DSL

module Counter =
    open Avalonia.Controls
    open Avalonia.FuncUI.DSL
    open Avalonia.Layout
    
    type State = { count : int }
    let init = { count = 0 }

    type Msg = Increment | Decrement | Reset

    let update (msg: Msg) (state: State) : State =
        match msg with
        | Increment -> { state with count = state.count + 1 }
        | Decrement -> { state with count = state.count - 1 }
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
                TextBlock.create [
                    TextBlock.dock Dock.Top
                    TextBlock.fontSize 48.0
                    TextBlock.verticalAlignment VerticalAlignment.Center
                    TextBlock.horizontalAlignment HorizontalAlignment.Center
                    TextBlock.text (string state.count)
                ]
            ]
        ]       