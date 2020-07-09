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
      { date: DateTime Nullable
        watermark: string
        customFormat: string
        selectedFormat: CalendarDatePickerFormat }

    let init = 
        { date = Nullable(DateTime.Today)
          watermark = ""
          customFormat = ""
          selectedFormat = CalendarDatePickerFormat.Long
        }

    type Msg =
    | SetDate of DateTime Nullable
    | SetWatermark of string
    | SetDatePickerFormat of CalendarDatePickerFormat * customFormat: string option

    let update (msg: Msg) (state: State) : State =
        match msg with
        | SetDate date -> { state with date = date }
        | SetWatermark text -> { state with watermark = text }
        | SetDatePickerFormat (format, custom) ->
          match format with
          | CalendarDatePickerFormat.Short 
          | CalendarDatePickerFormat.Long -> 
            { state with selectedFormat = format }
          | CalendarDatePickerFormat.Custom ->
            match custom with
            | Some customFormat -> 
              { state with selectedFormat = format; customFormat = customFormat }
            | None -> { state with selectedFormat = CalendarDatePickerFormat.Long; customFormat = "" }
          | _ -> { state with selectedFormat = CalendarDatePickerFormat.Long; customFormat = "" }
              
           
    let view (state: State) (dispatch) =
        StackPanel.create [
            StackPanel.spacing 10.0
            StackPanel.children [
                CalendarDatePicker.create [
                  yield CalendarDatePicker.selectedDate state.date
                  yield CalendarDatePicker.selectedDateFormat state.selectedFormat
                  if state.customFormat.Length > 0 then 
                    yield CalendarDatePicker.customDateFormatString state.customFormat
                  yield CalendarDatePicker.watermark state.watermark
                ]

                Button.create [
                  Button.content "Set Long Date Picker Format"
                  Button.onClick(fun _ -> dispatch (SetDatePickerFormat (CalendarDatePickerFormat.Long, None)))
                ]

                Button.create [
                  Button.content "Set Short Date Picker Format"
                  Button.onClick(fun _ -> dispatch (SetDatePickerFormat (CalendarDatePickerFormat.Short, None)))
                ]

                Button.create [
                    Button.content """Set Custom "MMMM dd, yyyy" Date Picker Format """
                    Button.onClick(fun _ -> dispatch (SetDatePickerFormat (CalendarDatePickerFormat.Custom, Some "MMMM dd, yyyy")))
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
        
        
        

