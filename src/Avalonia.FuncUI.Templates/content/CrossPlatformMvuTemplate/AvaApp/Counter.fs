namespace AvaApp

module Counter =
    open Avalonia.Controls
    open Avalonia.FuncUI.DSL
    open Avalonia.Layout
    
    type State = { count : int; text: string }
    let init = { count = 0; text = "" }

    type Msg = Increment | Decrement | Reset | SetText of string

    let update (msg: Msg) (state: State) : State =
        match msg with
        | Increment -> { state with count = state.count + 1 }
        | Decrement -> { state with count = state.count - 1 }
        | Reset -> init
        | SetText t -> { state with text = t }
    
    let view (state: State) (dispatch) =
        DockPanel.create [
            DockPanel.children [
                StackPanel.create [
                    StackPanel.dock Dock.Bottom
                    StackPanel.margin 5.0
                    StackPanel.spacing 5.0
                    StackPanel.children [
                        TextBox.create [
                            TextBox.text state.text
                            TextBox.onTextChanged (SetText >> dispatch)
                            TextBox.watermark "Put any text here"
                        ]
                        Button.create [
                            Button.onClick (fun _ -> dispatch Increment)
                            Button.content "+"
                            Button.classes [ "plus" ]
                        ]
                        Button.create [
                            Button.onClick (fun _ -> dispatch Decrement)
                            Button.content "-"
                            Button.classes [ "minus" ]
                        ]
                        Button.create [
                            Button.onClick (fun _ -> dispatch Reset)
                            Button.content "reset"
                        ]                         
                    ]
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