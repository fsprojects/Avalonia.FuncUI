namespace Avalonia.FuncUI.ControlCatalog.Views

open System
open Elmish
open Avalonia.Controls
open Avalonia.Layout
open Avalonia.Media
open Avalonia.FuncUI
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI
open Avalonia.FuncUI.Elmish

module TickBarDemo =
    type State = 
      { max: float
        min: float
        color: Color
        orientation: Orientation
        frequency: float
        placement : TickBarPlacement
        isDirectionReversed : bool }

    let init () = 
        { min = 0.0
          max = 50.0
          color = Colors.LightGreen
          orientation = Orientation.Horizontal
          frequency = 10.0
          placement = TickBarPlacement.Top
          isDirectionReversed = false }

    type Msg =
    | SetMin of float
    | SetMax of float
    | SetColor of Color
    | SetOrientation of Orientation
    | SetFrequency of float
    | SetPlacement of TickBarPlacement
    | SetIsDirectionReversed of bool

    let update (msg: Msg) (state: State) : State =
        match msg with
        | SetMin min -> { state with min = min }
        | SetMax max -> { state with max = max }
        | SetColor color -> { state with color = color }
        | SetOrientation orientation -> { state with orientation = orientation }
        | SetFrequency frequency -> { state with frequency = frequency }
        | SetPlacement placement -> { state with placement = placement }
        | SetIsDirectionReversed isRev -> { state with isDirectionReversed = isRev }

    let view (state: State) (dispatch) =
        Grid.create [
            Grid.rowDefinitions "50, *, 50, Auto, 50"

            Grid.children [

                TickBar.create [
                    Grid.row 1
                    TickBar.margin (10.0, 0.0)
                    TickBar.minimum state.min
                    TickBar.maximum state.max
                    TickBar.fill state.color
                    TickBar.orientation state.orientation
                    TickBar.tickFrequency state.frequency
                    TickBar.placement state.placement
                    TickBar.isDirectionReversed state.isDirectionReversed
                ]
                
                Grid.create [
                    Grid.row 3

                    Grid.columnDefinitions "*, *"
                    Grid.rowDefinitions "Auto, Auto, Auto, Auto, Auto, Auto"

                    Grid.children [
                        TextBlock.create [
                            Grid.column 0
                            Grid.row 0

                            TextBlock.text "Minimum:"
                        ]
                        
                        TextBox.create [
                            Grid.column 1
                            Grid.row 0

                            TextBox.text (state.min |> string)
                            TextBox.onTextChanged (fun txt ->
                                match Double.TryParse txt with
                                | true, d -> 
                                    d
                                    |> Msg.SetMin
                                    |> dispatch
                                | _ ->()
                            )
                        ]

                        TextBlock.create [
                            Grid.column 0
                            Grid.row 1

                            TextBlock.text "Maximum:"
                        ]

                        TextBox.create [
                            Grid.column 1
                            Grid.row 1

                            TextBox.text (state.max |> string)
                            TextBox.onTextChanged (fun txt ->
                                match Double.TryParse txt with
                                | true, d -> 
                                    d
                                    |> Msg.SetMax
                                    |> dispatch
                                | _ ->()
                            )
                        ]

                        TextBlock.create [
                            Grid.column 0
                            Grid.row 2

                            TextBlock.text "Frequency:"
                        ]

                        TextBox.create [
                            Grid.column 1
                            Grid.row 2

                            TextBox.text (state.frequency |> string)
                            TextBox.onTextChanged (fun txt ->
                                match Double.TryParse txt with
                                | true, d -> 
                                    d
                                    |> Msg.SetFrequency
                                    |> dispatch
                                | _ ->()
                            )
                        ]

                        CheckBox.create [
                            Grid.column 0
                            Grid.row 3

                            CheckBox.content "IsDirectionReversed"
                            CheckBox.isChecked state.isDirectionReversed
                            CheckBox.onChecked(fun _ ->
                                true
                                |> Msg.SetIsDirectionReversed
                                |> dispatch
                            )

                            CheckBox.onUnchecked(fun _ ->
                                false
                                |> Msg.SetIsDirectionReversed
                                |> dispatch
                            )
                        ]

                        ToggleSwitch.create [
                            Grid.column 0
                            Grid.row 4

                            ToggleSwitch.content "Orientation"
                            ToggleSwitch.onContent Orientation.Horizontal
                            ToggleSwitch.offContent Orientation.Vertical
                            ToggleSwitch.isChecked (state.orientation = Orientation.Horizontal)
                            
                            ToggleSwitch.onChecked(fun _ ->
                                Orientation.Horizontal
                                |> Msg.SetOrientation
                                |> dispatch
                            )

                            ToggleSwitch.onUnchecked (fun _ ->
                                Orientation.Vertical
                                |> Msg.SetOrientation
                                |> dispatch
                            )
                        ]

                        StackPanel.create [
                            Grid.column 1
                            Grid.row 4

                            StackPanel.children [
                                TextBlock.create [
                                    TextBlock.textAlignment TextAlignment.Center
                                    TextBlock.text "Placement"
                                ]

                                ComboBox.create [
                                    ComboBox.dataItems [
                                        TickBarPlacement.Top
                                        TickBarPlacement.Bottom
                                        TickBarPlacement.Left
                                        TickBarPlacement.Right
                                    ]
                                    ComboBox.horizontalAlignment HorizontalAlignment.Stretch
                                    ComboBox.selectedItem state.placement
                                    ComboBox.onSelectedItemChanged (
                                        tryUnbox >> Option.iter(Msg.SetPlacement >> dispatch)
                                    )
                                ]
                            ]
                        ]
                        
                        TextBlock.create [
                            Grid.column 0
                            Grid.row 5

                            TextBlock.text "Color"
                            TextBlock.verticalAlignment VerticalAlignment.Center
                        ]

                        StackPanel.create [
                            Grid.column 1
                            Grid.row 5
                            StackPanel.orientation Orientation.Horizontal
                            StackPanel.spacing 7.0
                            StackPanel.children [
                                Border.create [
                                    Border.background state.color
                                    Border.width 24.0
                                    Border.height 24.0
                                    Border.cornerRadius 3.0
                                ]

                                TextBox.create [
                                    TextBox.text (state.color.ToString())
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
                    ]
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
