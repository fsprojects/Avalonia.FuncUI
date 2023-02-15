namespace Avalonia.FuncUI.ControlCatalog.Views

open Elmish
open Avalonia.Controls
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI
open Avalonia.FuncUI.Elmish

module ToggleSwitchDemo = 
    // ToDo: replace with a proper sample
    let update _ _ = ()

    let view _ _ =
        StackPanel.create [
            StackPanel.children [
                TextBlock.create [
                    TextBlock.text "Toggle samples"
                ]

                ToggleSwitch.create [
                    ToggleSwitch.onContent "On"
                    ToggleSwitch.offContent "Off"
                    ToggleSwitch.content "Some content"
                    ToggleSwitch.isChecked true
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