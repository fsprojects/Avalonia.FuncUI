namespace Avalonia.FuncUI.ControlCatalog.Views

open Avalonia.Layout
open Elmish
open Avalonia.Controls
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI
open Avalonia.FuncUI.Elmish

module FlyoutDemo =
    type State = {
        placement: PlacementMode
        showMode: FlyoutShowMode
    }
        
    let init () = {
        placement = PlacementMode.Pointer
        showMode = FlyoutShowMode.Standard
    }

    type Msg =
    | SetPlacementMode of PlacementMode
    | SetShowMode of FlyoutShowMode

    let update (msg: Msg) (state: State) : State =
        match msg with
        | SetPlacementMode pm ->
            { state with placement = pm }
        | SetShowMode sm ->
            { state with showMode = sm }
        
    let view (state: State) dispatch =
        StackPanel.create [
            StackPanel.orientation Orientation.Vertical
            StackPanel.children [
                StackPanel.create [
                    StackPanel.orientation Orientation.Horizontal
                    StackPanel.children [
                        TextBlock.create [
                            TextBlock.verticalAlignment VerticalAlignment.Center
                            TextBlock.text "FlyoutPlacementMode: "
                        ]
                        ComboBox.create [
                            ComboBox.dataItems [
                                PlacementMode.AnchorAndGravity
                                PlacementMode.Bottom
                                PlacementMode.Center
                                PlacementMode.Left
                                PlacementMode.Pointer
                                PlacementMode.Right
                                PlacementMode.Top
                                PlacementMode.BottomEdgeAlignedLeft
                                PlacementMode.BottomEdgeAlignedRight
                                PlacementMode.LeftEdgeAlignedBottom
                                PlacementMode.LeftEdgeAlignedTop
                                PlacementMode.RightEdgeAlignedBottom
                                PlacementMode.RightEdgeAlignedTop
                                PlacementMode.TopEdgeAlignedLeft
                                PlacementMode.TopEdgeAlignedRight
                            ]
                            ComboBox.selectedItem state.placement
                            ComboBox.onSelectedItemChanged (tryUnbox >> Option.iter(Msg.SetPlacementMode >> dispatch))
                        ]
                    ]
                ]
                
                StackPanel.create [
                    StackPanel.orientation Orientation.Horizontal
                    StackPanel.children [
                        TextBlock.create [
                            TextBlock.verticalAlignment VerticalAlignment.Center
                            TextBlock.text "FlyoutShowMode: "
                        ]
                        ComboBox.create [
                            ComboBox.dataItems [
                                FlyoutShowMode.Standard
                                FlyoutShowMode.Transient
                                FlyoutShowMode.TransientWithDismissOnPointerMoveAway
                            ]
                            ComboBox.selectedItem state.showMode
                            ComboBox.onSelectedItemChanged (tryUnbox >> Option.iter(Msg.SetShowMode >> dispatch))
                        ]
                    ]
                ]
                
                Button.create [
                    Button.content "Click me to see flyout"
                    Button.flyout (
                        Flyout.create [
                            Flyout.placement state.placement
                            Flyout.showMode state.showMode
                            Flyout.content (
                                TextBlock.create [
                                    TextBlock.text "Hi, I am flyout"
                                ]
                            )
                        ]
                    )
                ]
                
                Button.create [
                    Button.content "Click me to see menu flyout"
                    Button.flyout (
                        MenuFlyout.create [
                            MenuFlyout.placement state.placement
                            MenuFlyout.showMode state.showMode
                            MenuFlyout.dataItems [
                                "We"
                                "are"
                                "items"
                                "of"
                                "MenuFlyout"
                            ]
                        ]
                    )
                ]
            ]
        ]
    
        
    type Host() as this =
        inherit Hosts.HostControl()
        do
            Elmish.Program.mkSimple init update view
            |> Program.withHost this
            |> Program.withConsoleTrace
            |> Program.run