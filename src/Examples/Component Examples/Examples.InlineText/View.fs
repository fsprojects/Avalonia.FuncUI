namespace Examples.InlineText

module View =
    open Avalonia.FuncUI.DSL
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI
    open Avalonia.Controls
    open Avalonia.Layout
    open Avalonia.Media
    open Avalonia.Media.Immutable
    open Avalonia.Controls.Documents

    let private redBrush = 0x0ffB2474Du |> Color.FromUInt32 |> ImmutableSolidColorBrush
    let private blueBrush = 0x0ff47B2A6u |> Color.FromUInt32 |> ImmutableSolidColorBrush
    
    let view =
        Component (fun ctx ->
            let colorMode = ctx.useState 0

            StackPanel.create [
                StackPanel.verticalAlignment VerticalAlignment.Center
                StackPanel.horizontalAlignment HorizontalAlignment.Center
                StackPanel.children [
                    Button.create [
                        Button.content "Invert color!"
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
                            Run.create [
                                Run.text "You"
                            ] :> IView
                            Run.create [
                                Run.text "Inline"
                                if colorMode.Current = 0 then
                                    redBrush   
                                else
                                    blueBrush
                                |> Run.background
                            ]
                            
                            LineBreak.simple
                            
                            Span.create [
                                Span.inlines [
                                    Bold.simple "Oh, so bold!" :> IView
                                    LineBreak.simple
                                    
                                    Italic.simple "Although, "
                                    Run.create [
                                        Run.text "I always wanted to be "
                                    ]
                                    Underline.simple "underlined"
                                ]
                            ]
                        ]
                    ]
                ]
            ])
