namespace Examples.CounterApp

open Avalonia.FuncUI.DSL

module Counter =
    open Avalonia.Controls
    open Avalonia.Layout
    
    type State = NoState
    let init() = NoState

    type Msg = NoMsg

    let update (msg: Msg) (state: State) : State =
        match msg with
        | NoMsg -> state
    
    let view (state: State) (dispatch) =
        Window.create [
            Window.title "Hello World"
            Window.width 400
            Window.height 200
            Window.child (
                TextBox.create [
                    TextBox.text "Hello from FuncUI!"
                    TextBox.isReadOnly true
                ]
            )
        ]
