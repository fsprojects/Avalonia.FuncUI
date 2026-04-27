namespace Avalonia.FuncUI.ControlCatalog.Views

open Elmish
open Avalonia.Controls
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI
open Avalonia.FuncUI.Elmish

module GroupBoxDemo = 

    let update _ _ = ()

    let view _ _ =
        StackPanel.create [
            StackPanel.children [
                TextBlock.create [
                    TextBlock.text "GroupBox samples"
                ]

                GroupBox.create [
                    GroupBox.header "This is a GroupBox"
                    GroupBox.content(
                        TextBlock.create [
                            TextBlock.text "Essentially a restyled HeaderedContentControl"
                        ]
                    )
                ]
            ]
        ]

    type Host() as this =
        inherit Hosts.HostControl()
        do
            Elmish.Program.mkSimple id update view
            |> Program.withHost this
            |> Program.withConsoleTrace
            |> Program.run