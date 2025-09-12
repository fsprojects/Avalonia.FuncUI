namespace Examples.CounterApp

open Avalonia.FuncUI.DSL

module HelloWorld =
    open Avalonia.Controls
    
    type State = string
    let init() = "World"

    type Msg = NameChanged of string

    let update (msg: Msg) (_state: State) : State =
        match msg with
        | NameChanged name -> name
    
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
                            TextBox.onTextChanged (NameChanged >> dispatch)
                        ]
                    ]
                ]
            )
        ]
