namespace Examples.CounterApp

open Avalonia.FuncUI.DSL
open Avalonia.Input

module HelloWorld =
    open Avalonia.Controls
    
    type State =
        {
            Name : string
            FullScreen : bool
        }
    let init() =
        {
            Name = "World"
            FullScreen = false
        }

    type Msg =
        | NameChanged of string
        | FullScreen of bool

    let update (msg: Msg) (state: State) : State =
        match msg with
        | NameChanged name ->
            { state with Name = name }
        | FullScreen fullScreen ->
            { state with FullScreen = fullScreen }
    
    let view (state: State) (dispatch) =
        Window.create [
            Window.title $"Hello {state}!"
            Window.sizeToContent SizeToContent.WidthAndHeight
            Window.windowState (
                if state.FullScreen then WindowState.FullScreen
                else WindowState.Normal)
            Window.child (
                StackPanel.create [
                    StackPanel.margin 10
                    StackPanel.spacing 10
                    StackPanel.children [
                        TextBlock.create [
                            TextBlock.text "Type your name here to change the window title:"
                        ]
                        TextBox.create [
                            TextBox.text state.Name
                            TextBox.onTextChanged (NameChanged >> dispatch)
                        ]
                        TextBlock.create [
                            TextBlock.text "Or use F11 key to enter full screen mode and Esc key to exit."
                        ]
                    ]
                ]
            )
            Window.keyBindings [
                KeyBinding.create [
                    KeyBinding.key Key.F11
                    KeyBinding.execute (fun _ ->
                        FullScreen true
                            |> dispatch)
                ]
                KeyBinding.create [
                    KeyBinding.key Key.Escape
                    KeyBinding.execute (fun _ ->
                        FullScreen false
                            |> dispatch)
                ]
            ]
        ]
