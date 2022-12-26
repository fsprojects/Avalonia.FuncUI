namespace Avalonia.FuncUI.ControlCatalog.Views

open Elmish
open Avalonia.Controls
open Avalonia.Layout
open Avalonia.Media
open Avalonia.FuncUI
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Elmish
open System

module SplitViewDemo =
    type State = 
      { isPaneOpen: bool
        useLightDismissOverlayMode: bool
        panePlacement: SplitViewPanePlacement
        compactPaneLength : float
        openPaneLength: float
        displayMode: SplitViewDisplayMode
        paneBackground: Color }

    let init = 
        { isPaneOpen = true
          useLightDismissOverlayMode = false
          panePlacement = SplitViewPanePlacement.Left
          compactPaneLength = 50.0
          openPaneLength = 150.0
          displayMode = SplitViewDisplayMode.Inline
          paneBackground = Colors.Green }

    type Msg =
    | SetIsPaneOpen of bool
    | SetUseLightDismissOverlayMode of bool
    | SetCompactPaneLength of float
    | SetOpenPaneLength of float
    | SetPanePlacement of SplitViewPanePlacement
    | SetColor of Color
    | SetDisplayMode of SplitViewDisplayMode

    let update (msg: Msg) (state: State) : State =
        match msg with
        | SetIsPaneOpen b -> 
            { state with isPaneOpen = b }
        | SetUseLightDismissOverlayMode b -> 
            { state with useLightDismissOverlayMode = b }
        | SetCompactPaneLength v ->
            { state with compactPaneLength = v }
        | SetOpenPaneLength v ->
            { state with openPaneLength = v }
        | SetPanePlacement v ->
            { state with panePlacement = v }
        | SetColor c ->
            { state with paneBackground = c }
        | SetDisplayMode v ->
            { state with displayMode = v }

    let view (state: State) dispatch =
        Grid.create [
            Grid.rowDefinitions "40, Auto, *"

            Grid.children [
            
                Grid.create [
                    Grid.row 1
                    Grid.columnDefinitions "*, *"
                    
                    Grid.children [

                        StackPanel.create [
                            Grid.column 0
                            StackPanel.spacing 5.0

                            StackPanel.children [
                                
                                CheckBox.create [
                                    CheckBox.content "IsPaneOpen"
                                    CheckBox.isChecked state.isPaneOpen
                                    CheckBox.onChecked(fun _ ->
                                        true
                                        |> Msg.SetIsPaneOpen
                                        |> dispatch
                                    )

                                    CheckBox.onUnchecked(fun _ ->
                                        false
                                        |> Msg.SetIsPaneOpen
                                        |> dispatch
                                    )
                                ]

                                CheckBox.create [
                                    CheckBox.content "UseLightDismissOverlayMode"
                                    CheckBox.isChecked state.useLightDismissOverlayMode
                                    CheckBox.onChecked(fun _ ->
                                        true
                                        |> Msg.SetUseLightDismissOverlayMode
                                        |> dispatch
                                    )

                                    CheckBox.onUnchecked(fun _ ->
                                        false
                                        |> Msg.SetUseLightDismissOverlayMode
                                        |> dispatch
                                    )
                                ]

                                ToggleSwitch.create [
                                    ToggleSwitch.onContent SplitViewPanePlacement.Left
                                    ToggleSwitch.offContent SplitViewPanePlacement.Right
                                    ToggleSwitch.content "SplitViewPanePlacement"
                                    ToggleSwitch.isChecked (state.panePlacement = SplitViewPanePlacement.Left)
                                    
                                    ToggleSwitch.onChecked(fun _ ->
                                        SplitViewPanePlacement.Left
                                        |> Msg.SetPanePlacement
                                        |> dispatch
                                    )

                                    ToggleSwitch.onUnchecked (fun _ ->
                                        SplitViewPanePlacement.Right
                                        |> Msg.SetPanePlacement
                                        |> dispatch
                                    )
                                ]
                            
                                ComboBox.create [
                                    ComboBox.dataItems [
                                        SplitViewDisplayMode.Inline
                                        SplitViewDisplayMode.CompactInline
                                        SplitViewDisplayMode.CompactOverlay
                                        SplitViewDisplayMode.Overlay
                                    ]
                                    ComboBox.minWidth 150.0
                                    ComboBox.selectedItem state.displayMode

                                    ComboBox.onSelectedItemChanged (
                                        tryUnbox >> Option.iter(Msg.SetDisplayMode >> dispatch)
                                    )
                                ]
                            ]
                        ]

                        StackPanel.create [
                            Grid.column 1
                            StackPanel.spacing 5.0

                            StackPanel.children [

                                TextBlock.create [
                                    TextBlock.text "Color"
                                    TextBlock.textAlignment TextAlignment.Center
                                    TextBlock.horizontalAlignment HorizontalAlignment.Stretch
                                ]

                                StackPanel.create [
                                    StackPanel.orientation Orientation.Horizontal
                                    StackPanel.spacing 7.0
                                    StackPanel.children [
                                        Border.create [
                                            Border.background state.paneBackground
                                            Border.width 24.0
                                            Border.height 24.0
                                            Border.cornerRadius 3.0
                                        ]

                                        TextBox.create [
                                            TextBox.text (state.paneBackground.ToString())
                                            TextBox.minWidth 120.0
                                            TextBox.horizontalAlignment HorizontalAlignment.Stretch
                                            TextBox.textAlignment TextAlignment.Center
                                            TextBox.onTextChanged (fun txt ->
                                                match Color.TryParse txt with
                                                | (true, color) -> 
                                                    color 
                                                    |> Msg.SetColor 
                                                    |> dispatch
                                                | _ -> ()
                                            )
                                        ]
                                    ]
                                ]

                                TextBlock.create [
                                    TextBlock.text "CompactPaneLength"
                                    TextBlock.textAlignment TextAlignment.Center
                                    TextBlock.horizontalAlignment HorizontalAlignment.Stretch
                                ]

                                TextBox.create [
                                    TextBox.text (string state.compactPaneLength)
                                    TextBox.minWidth 120.0
                                    TextBox.horizontalAlignment HorizontalAlignment.Stretch
                                    TextBox.textAlignment TextAlignment.Center
                                    TextBox.onTextChanged (fun txt ->
                                        match Double.TryParse txt with
                                        | (true, f) -> 
                                            f
                                            |> Msg.SetCompactPaneLength
                                            |> dispatch
                                        | _ -> ()
                                    )
                                ]

                                TextBlock.create [
                                    TextBlock.text "OpenPaneLength"
                                    TextBlock.textAlignment TextAlignment.Center
                                    TextBlock.horizontalAlignment HorizontalAlignment.Stretch
                                ]

                                TextBox.create [
                                    TextBox.text (string state.openPaneLength)
                                    TextBox.minWidth 120.0
                                    TextBox.horizontalAlignment HorizontalAlignment.Stretch
                                    TextBox.textAlignment TextAlignment.Center
                                    TextBox.onTextChanged (fun txt ->
                                        match Double.TryParse txt with
                                        | (true, f) -> 
                                            f
                                            |> Msg.SetOpenPaneLength
                                            |> dispatch
                                        | _ -> ()
                                    )
                                ]
                            ]
                        ]

                    ]
                ]

                SplitView.create [
                    Grid.row 2

                    SplitView.paneBackground state.paneBackground
                    SplitView.displayMode state.displayMode
                    SplitView.panePlacement state.panePlacement
                    SplitView.useLightDismissOverlayMode state.useLightDismissOverlayMode
                    SplitView.isPaneOpen state.isPaneOpen
                    SplitView.openPaneLength state.openPaneLength
                    SplitView.compactPaneLengthProperty state.compactPaneLength


                    StackPanel.create [
                        StackPanel.children [
                            TextBlock.create [
                                TextBlock.text "Сontent"
                                TextBlock.textAlignment TextAlignment.Center
                            ]
                        ]
                    ]
                    |> SplitView.content

                    StackPanel.create [
                        StackPanel.children [
                            TextBlock.create [
                                TextBlock.text "Pane"
                                TextBlock.textAlignment TextAlignment.Center
                            ]
                        ]
                    ]
                    |> SplitView.pane
                
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
        
