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
            Window.title $"Hello {state}!"
            Window.width 400
            Window.height 100
            Window.child (
                StackPanel.create [
                    StackPanel.margin 10
                    StackPanel.spacing 10
                    StackPanel.children [
                        TextBlock.create [
                            TextBlock.text "Type your name here to change the window title:"
                        ]
                        TextBox.create [
                            TextBox.text state
                            TextBox.onTextChanged (Update >> dispatch)
                        ]
                    ]
                ]
            )
        ]
