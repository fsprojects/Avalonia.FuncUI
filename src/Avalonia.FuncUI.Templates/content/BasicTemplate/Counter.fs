namespace BasicTemplate

module Counter =
    open Avalonia.Controls
    open Avalonia.FuncUI
    open Avalonia.FuncUI.DSL
    open Avalonia.Layout

    let buttonStyles: Types.IAttr<Button> list =  [
        Button.width 64
        Button.fontSize 16.0
        Button.horizontalAlignment HorizontalAlignment.Center
        Button.horizontalContentAlignment HorizontalAlignment.Center
        Button.margin 2.
    ]

    let view =
        Component(fun ctx ->
            let state = ctx.useState 0
            DockPanel.create [
                DockPanel.children [
                    Button.create [
                        Button.dock Dock.Bottom
                        Button.content "Reset"
                        Button.onClick (fun _ -> state.Set 0)
                        yield! buttonStyles
                    ]
                    Button.create [
                        Button.dock Dock.Bottom
                        Button.content "-"
                        Button.onClick (fun _ -> state.Current - 1 |> state.Set)
                        yield! buttonStyles
                    ]
                    Button.create [
                        Button.dock Dock.Bottom
                        Button.content "+"
                        Button.onClick (fun _ -> state.Current + 1 |> state.Set)
                        yield! buttonStyles
                    ]
                    TextBlock.create [
                        TextBlock.dock Dock.Top
                        TextBlock.fontSize 48.0
                        TextBlock.horizontalAlignment HorizontalAlignment.Center
                        TextBlock.text (string state.Current)
                    ]
                ]
            ]
        )
