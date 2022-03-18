namespace Examples.CounterApp

module Main =
    open Avalonia.Controls
    open Avalonia.FuncUI
    open Avalonia.FuncUI.DSL
    open Avalonia.Layout
    
    let view =
        Component
            (fun ctx ->
                let state = ctx.useState 0
    
                DockPanel.create [
                    DockPanel.children [
                        Button.create [
                            Button.dock Dock.Bottom
                            Button.onClick (fun _ -> state.Set 0)
                            Button.content "reset"
                            Button.horizontalAlignment HorizontalAlignment.Stretch
                        ]
                        Button.create [
                            Button.dock Dock.Bottom
                            Button.onClick (fun _ -> state.Current - 1 |> state.Set)
                            Button.content "-"
                            Button.horizontalAlignment HorizontalAlignment.Stretch
                        ]
                        Button.create [
                            Button.dock Dock.Bottom
                            Button.onClick (fun _ -> state.Current + 1 |> state.Set)
                            Button.content "+"
                            Button.horizontalAlignment HorizontalAlignment.Stretch
                        ]
                        Button.create [
                            Button.dock Dock.Bottom
                            Button.onClick (
                                (fun _ -> state.Current * 2 |> state.Set),
                                SubPatchOptions.OnChangeOf state.Current
                            )
                            Button.content "x2"
                            Button.horizontalAlignment HorizontalAlignment.Stretch
                        ]
                        TextBox.create [
                            TextBox.dock Dock.Bottom
                            TextBox.onTextChanged (
                                (fun text ->
                                    let isNumber, number = System.Int32.TryParse text
                                    if isNumber then number |> state.Set)
                            )
                            TextBox.text (string state.Current)
                            TextBox.horizontalAlignment HorizontalAlignment.Stretch
                        ]
                        TextBlock.create [
                            TextBlock.dock Dock.Top
                            TextBlock.fontSize 48.0
                            TextBlock.verticalAlignment VerticalAlignment.Center
                            TextBlock.horizontalAlignment HorizontalAlignment.Center
                            TextBlock.text (string state.Current)
                        ]
                    ]
                ])
