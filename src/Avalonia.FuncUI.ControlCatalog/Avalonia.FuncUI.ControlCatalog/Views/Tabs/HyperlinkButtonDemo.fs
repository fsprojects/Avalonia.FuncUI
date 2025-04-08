namespace Avalonia.FuncUI.ControlCatalog.Views

open Elmish
open Avalonia.Controls
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI
open Avalonia.FuncUI.Elmish

module HyperlinkButtonDemo = 

    let update _ _ = ()

    let view _ _ =
        StackPanel.create [
            StackPanel.children [
                TextBlock.create [
                    TextBlock.text "Hyperlink samples"
                ]

                HyperlinkButton.create [
                    HyperlinkButton.navigateUri (System.Uri("https://funcui.avaloniaui.net/"))
                    HyperlinkButton.content "https://funcui.avaloniaui.net/"
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