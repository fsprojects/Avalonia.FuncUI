namespace Examples.CounterApp

open Avalonia.FuncUI.DSL

module HelloWorld =
    open Avalonia.Controls
    
    type State = string
    let init() = "World"

    type Msg = Update of string

    let update (msg: Msg) (state: State) : State =
        match msg with
        | Update str -> str
    
    let view (state: State) (dispatch) =
        Window.create [
            Window.title $"Hello {state}"
            Window.width 400
            Window.height 200
            Window.child (
                TextBox.create [
                    TextBox.text state
                    TextBox.onTextChanged (Update >> dispatch)
                ]
            )
        ]
