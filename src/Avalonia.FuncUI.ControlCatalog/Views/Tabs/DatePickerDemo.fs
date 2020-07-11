namespace Avalonia.FuncUI.ControlCatalog.Views

open System
open Elmish
open Avalonia.Controls
open Avalonia.FuncUI
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Components
open Avalonia.FuncUI.Elmish

module DatePickerDemo =
    type State = 
      { date: DateTimeOffset Nullable
        header: string
        isDayVisible: bool
        isMonthVisible: bool
        isYearVisible: bool
        dayFormat : string
        monthFormat : string
        yearFormat : string }

    let init = 
        { date = Nullable(DateTimeOffset(DateTime.Today))
          header = "Header"
          isDayVisible = true
          isMonthVisible = true
          isYearVisible = true
          dayFormat = "dd"
          monthFormat = "MM"
          yearFormat = "yyyy" }

    type Msg =
    | SetDate of DateTimeOffset Nullable
    | SetHeader of string
    | SetIsDayVisible of bool
    | SetIsMonthVisible of bool
    | SetIsYearVisible of bool
    | SetDayFormat of string
    | SetMonthFormat of string
    | SetYearFormat of string

    let update (msg: Msg) (state: State) : State =
        match msg with
        | SetDate date -> { state with date = date }
        | SetHeader header -> { state with header = header }
        | SetIsDayVisible b -> { state with isDayVisible = b }
        | SetIsMonthVisible b -> { state with isMonthVisible = b }
        | SetIsYearVisible b -> { state with isYearVisible = b }
        | SetDayFormat format -> { state with dayFormat = format }
        | SetMonthFormat format  -> { state with monthFormat = format }
        | SetYearFormat format  -> { state with yearFormat = format }

    let view (state: State) (dispatch) =
        Grid.create [
            Grid.rowDefinitions "Auto, *"
            Grid.children [
                DatePicker.create [
                    Grid.row 0

                    DatePicker.dayVisible state.isDayVisible
                    DatePicker.monthVisible state.isMonthVisible
                    DatePicker.yearVisible state.isYearVisible

                    DatePicker.dayFormat state.dayFormat
                    DatePicker.monthFormat state.monthFormat
                    DatePicker.yearFormat state.yearFormat

                    DatePicker.header state.header
                    DatePicker.selectedDate state.date

                    DatePicker.onSelectedDateChanged (
                        Msg.SetDate >> dispatch
                    )
                ]
                
                StackPanel.create [
                    Grid.row 1
                    
                    StackPanel.children [
                        Grid.create [
                            Grid.columnDefinitions "*, *, *"
                            Grid.rowDefinitions "Auto, Auto, Auto"

                            Grid.children [
                                CheckBox.create [
                                    Grid.row 0
                                    Grid.column 0

                                    CheckBox.content "DayVisible"
                                    CheckBox.isChecked state.isDayVisible
                                    
                                    CheckBox.onChecked(fun _ ->
                                        true 
                                        |> Msg.SetIsDayVisible
                                        |> dispatch
                                    )

                                    CheckBox.onUnchecked(fun _ ->
                                        false
                                        |> Msg.SetIsDayVisible
                                        |> dispatch
                                    )
                                ]

                                TextBlock.create [
                                    Grid.row 1
                                    Grid.column 0

                                    TextBlock.text "Day Format"
                                ]

                                TextBox.create [
                                    Grid.row 2
                                    Grid.column 0

                                    TextBox.isReadOnly (state.isDayVisible |> not)
                                    TextBox.text state.dayFormat
                                    TextBox.onTextChanged(
                                        Msg.SetDayFormat >> dispatch
                                    )
                                ]

                                CheckBox.create [
                                    Grid.row 0
                                    Grid.column 1

                                    CheckBox.content "MonthVisible"
                                    CheckBox.isChecked state.isMonthVisible
                                    
                                    CheckBox.onChecked(fun _ ->
                                        true 
                                        |> Msg.SetIsMonthVisible
                                        |> dispatch
                                    )

                                    CheckBox.onUnchecked(fun _ ->
                                        false
                                        |> Msg.SetIsMonthVisible
                                        |> dispatch
                                    )
                                ]

                                TextBlock.create [
                                    Grid.row 1
                                    Grid.column 1

                                    TextBlock.text "Month Format"
                                ]

                                TextBox.create [
                                    Grid.row 2
                                    Grid.column 1

                                    TextBox.watermark "MonthFormat"
                                    TextBox.isReadOnly (state.isMonthVisible |> not)
                                    
                                    TextBox.text state.monthFormat
                                    TextBox.onTextChanged(
                                        Msg.SetMonthFormat >> dispatch
                                    )
                                ]

                                CheckBox.create [
                                    Grid.row 0
                                    Grid.column 2

                                    CheckBox.content "YearVisible"
                                    CheckBox.isChecked state.isYearVisible
                                    
                                    CheckBox.onChecked(fun _ ->
                                        true 
                                        |> Msg.SetIsYearVisible
                                        |> dispatch
                                    )

                                    CheckBox.onUnchecked(fun _ ->
                                        false
                                        |> Msg.SetIsYearVisible
                                        |> dispatch
                                    )
                                ]

                                TextBlock.create [
                                    Grid.row 1
                                    Grid.column 2

                                    TextBlock.text "Year Format"
                                ]

                                TextBox.create [
                                    Grid.row 2
                                    Grid.column 2

                                    TextBox.isReadOnly (state.isYearVisible |> not)
                                                                       
                                    TextBox.text state.yearFormat
                                    TextBox.onTextChanged(
                                        Msg.SetYearFormat >> dispatch
                                    )
                                ]
                            ]
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
                                state.date
                                |> Option.ofNullable
                                |> Option.map(fun d -> d.ToString())
                                |> Option.defaultValue ""
                            )
                        ]
                    ]
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
        
