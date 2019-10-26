namespace Avalonia.FuncUI.ControlCatalog.Views

open System
open Avalonia.Controls
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Components
open Avalonia.FuncUI.Elmish
open Elmish
open Avalonia.Layout
open Avalonia.Media

module ToolTipDemo =
    type State =
        { isOpen: bool
          tip: string
          placement: PlacementMode
          horizontalOffset: float
          verticalOffset: float
          showDelay: int }

    let init =
        { isOpen = true
          tip = "Hover me to see the ToolTip";
          placement = PlacementMode.Bottom;
          horizontalOffset = 0.0;
          verticalOffset = 0.0;
          showDelay = 1000 }

    type Msg =
    | ToggleIsOpen
    | SetTip of string
    | SetPlacement of PlacementMode
    | SetHOffset of float
    | SetVOffset of float
    | SetDelay of int

    let update (msg: Msg) (state: State) : State =
        match msg with
        | ToggleIsOpen -> {state with isOpen = not state.isOpen}
        | SetTip tip -> {state with tip = tip}
        | SetPlacement placement -> {state with placement = placement}
        | SetHOffset offset -> {state with horizontalOffset = offset}
        | SetVOffset offset -> {state with verticalOffset = offset}
        | SetDelay delay -> {state with showDelay = delay}
           
    let view (state: State) (dispatch) =
        DockPanel.create [
            DockPanel.children [
                TextBlock.create [
                    DockPanel.dock Dock.Top
                    TextBlock.margin 5.0
                    TextBlock.text "Hover the TextBlock to show the tooltip"
                ]
                CheckBox.create [
                    CheckBox.content "IsOpen"
                    CheckBox.isChecked state.isOpen
                    CheckBox.onClick (fun _ -> ToggleIsOpen |> dispatch)
                ]
                TextBox.create [
                    DockPanel.dock Dock.Top
                    TextBox.text (string state.tip)
                    TextBox.onTextChanged (fun text -> (SetTip text |> dispatch))
                ]
                TextBox.create [
                    DockPanel.dock Dock.Top
                    ToolTip.tip "Horizontal Offset"
                    TextBox.text (string state.horizontalOffset)
                    TextBox.onTextChanged (fun text ->
                        try
                            text |> Double.Parse |> SetHOffset |> dispatch
                        with ex -> Console.WriteLine(ex) 
                    )
                ]
                TextBlock.create [
                    TextBlock.text "Hover me to see the ToolTip"
                    ToolTip.tip state.tip
                    ToolTip.placement state.placement
                    ToolTip.horizontalOffset state.horizontalOffset
                    ToolTip.verticalOffset state.verticalOffset
                    ToolTip.showDelay state.showDelay
                    ToolTip.isOpen state.isOpen
                ]
            ]
        ]
        
    type Host() as this =
        inherit Hosts.HostControl()
        do
            Elmish.Program.mkSimple (fun () -> init) update view
            |> Program.withHost this
            |> Program.withConsoleTrace
            |> Program.run