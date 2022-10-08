namespace Examples.InlineText

open Avalonia.FuncUI.DSL
open Avalonia.Media
open Avalonia.Media.Immutable

module View =
    open Avalonia.Controls
    open Avalonia.Controls.Documents
    open Avalonia.FuncUI
    open Avalonia.Layout

    let view =
        Component (fun ctx ->
            let colorMode = ctx.useState 0

            let inlineItem =
                let x = Run("Inline")
                let brush =
                    if colorMode.Current = 0 then
                        0x0ffB2474Du
                    else
                        0x0ff47B2A6u
                    |> Color.FromUInt32
                    |> ImmutableSolidColorBrush
                
                x.Background <- brush
                x

            StackPanel.create [
                StackPanel.verticalAlignment VerticalAlignment.Center
                StackPanel.horizontalAlignment HorizontalAlignment.Center
                StackPanel.children [
                    Button.create [
                        Button.content "Flip color!"
                        Button.onClick (fun _ ->
                            if colorMode.Current = 0 then
                                colorMode.Set 1
                            else
                                colorMode.Set 0)
                    ]
                    RichTextBlock.create [
                        RichTextBlock.dock Dock.Top
                        RichTextBlock.fontSize 48.0
                        RichTextBlock.horizontalAlignment HorizontalAlignment.Center
                        RichTextBlock.inlines [
                            Run("You") :> Inline
                            inlineItem
                        ]
                    ]
                ]
            ])
