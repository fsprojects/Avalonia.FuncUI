namespace Avalonia.FuncUI.ControlCatalog.Views

open System
open Elmish
open Avalonia.Controls
open Avalonia.FuncUI
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Elmish

module TimePickerDemo =
    type State = 
      { time: TimeSpan Nullable
        header: string
        minuteIncrement: int
        clockIdentifier: string 
        secondIncrement: int
        useSeconds: bool }

    let init () = 
        { time = Nullable(DateTime.Today.TimeOfDay)
          header = "Header"
          minuteIncrement = 1
          clockIdentifier = "12HourClock"
          secondIncrement = 1 
          useSeconds = false }

    type Msg =
    | SetTime of TimeSpan Nullable
    | SetHeader of string
    | SetMinuteIncrement of int
    | SetClockIdentifier of string
    | SetSecondIncrement of int
    | SetUseSeconds of bool

    let update (msg: Msg) (state: State) : State =
        match msg with
        | SetTime time -> { state with time = time }
        | SetHeader header -> { state with header = header }
        | SetMinuteIncrement m -> { state with minuteIncrement = m }
        | SetClockIdentifier ci -> { state with clockIdentifier = ci }
        | SetSecondIncrement s -> { state with secondIncrement = s }
        | SetUseSeconds b -> { state with useSeconds = b }

    let view (state: State) (dispatch) =
        StackPanel.create [
            StackPanel.spacing 7.0
            StackPanel.children [
                TextBlock.create [
                    TextBlock.text state.header
                ]

                TimePicker.create [
                    TimePicker.clockIdentifier state.clockIdentifier
                    TimePicker.minuteIncrement state.minuteIncrement
                    TimePicker.secondIncrement state.secondIncrement

                    TimePicker.selectedTime state.time
                    TimePicker.useSeconds state.useSeconds

                    TimePicker.onSelectedTimeChanged (
                        Msg.SetTime >> dispatch
                    )
                ]
                
                TextBlock.create [
                    TextBlock.text "Minutes increment:"
                ]

                TextBox.create [
                    TextBox.text (state.minuteIncrement |> string)
                    TextBox.onTextChanged (fun txt ->
                        match Int32.TryParse txt with
                        | true, i -> 
                            i
                            |> Msg.SetMinuteIncrement
                            |> dispatch
                        | _ ->()
                    )
                ]
                
                TextBlock.create [
                    TextBlock.text "Seconds increment:"
                ]

                TextBox.create [
                    TextBox.text (state.secondIncrement |> string)
                    TextBox.onTextChanged (fun txt ->
                        match Int32.TryParse txt with
                        | true, i -> 
                            i
                            |> Msg.SetSecondIncrement
                            |> dispatch
                        | _ ->()
                    )
                ]

                TextBox.create [
                    TextBox.watermark "Header"
                    TextBox.text state.header
                    TextBox.onTextChanged (
                        Msg.SetHeader >> dispatch
                    )
                ]

                TextBlock.create [
                    TextBlock.text (
                        state.time
                        |> Option.ofNullable
                        |> Option.map(fun d -> d.ToString())
                        |> Option.defaultValue ""
                    )
                ]

                ComboBox.create [
                    ComboBox.dataItems [
                        "12HourClock"
                        "24HourClock"
                    ]

                    ComboBox.selectedItem state.clockIdentifier

                    ComboBox.onSelectedItemChanged(
                        tryUnbox
                        >> Option.iter(Msg.SetClockIdentifier >> dispatch)
                    )
                ]

                CheckBox.create [
                    CheckBox.content "Use Seconds"
                    CheckBox.isChecked state.useSeconds
                    
                    CheckBox.onIsCheckedChanged ((fun args ->
                        state.useSeconds
                        |> not 
                        |> Msg.SetUseSeconds
                        |> dispatch),
                        SubPatchOptions.OnChangeOf state
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
        