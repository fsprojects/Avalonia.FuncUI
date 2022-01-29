namespace Avalonia.FuncUI.ControlCatalog.Views

open Elmish
open Avalonia.Controls
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI
open Avalonia.FuncUI.Elmish

module NumericUpDownDemo =

    type State = { count : double }

    let init = { count = 0.0 }

    type Msg =
    | Reset
    | Set of double

    let update (msg: Msg) (state: State) : State =
        match msg with
        | Reset -> init
        | Set value -> { state with count = value }

    let view (state: State) (dispatch) =
        StackPanel.create [
            StackPanel.margin 20.0
            StackPanel.children [
                NumericUpDown.create [
                    NumericUpDown.value state.count
                    NumericUpDown.onValueChanged (fun value -> value |> Set |> dispatch)
                    NumericUpDown.increment 0.25
                    NumericUpDown.minimum 0.0
                    NumericUpDown.maximum 1.0
                    NumericUpDown.clipValueToMinMax false
                ]
                NumericUpDown.create [
                    NumericUpDown.value state.count
                    NumericUpDown.onValueChanged (fun value -> value |> Set |> dispatch)
                    NumericUpDown.increment 0.25
                    NumericUpDown.minimum 0.0
                    NumericUpDown.maximum 1.0
                    NumericUpDown.clipValueToMinMax false
                    NumericUpDown.buttonSpinnerLocation Location.Left
                ]
                NumericUpDown.create [
                    NumericUpDown.value state.count
                    NumericUpDown.onValueChanged (fun value -> value |> Set |> dispatch)
                    NumericUpDown.increment 0.25
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