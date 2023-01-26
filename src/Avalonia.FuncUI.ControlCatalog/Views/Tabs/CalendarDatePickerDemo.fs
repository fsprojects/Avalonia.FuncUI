namespace Avalonia.FuncUI.ControlCatalog.Views

open System
open Elmish
open Avalonia.Controls
open Avalonia.Layout
open Avalonia.FuncUI
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Elmish

module CalendarDatePickerDemo =
    type State = 
      { date: DateTime Nullable
        watermark: string
        customFormat: string
        selectedFormat: CalendarDatePickerFormat }

    let init () = 
        { date = Nullable(DateTime.Today)
          watermark = ""
          customFormat = ""
          selectedFormat = CalendarDatePickerFormat.Long
        }

    type Msg =
    | SetDate of DateTime Nullable
    | SetWatermark of string
    | SetFormat of CalendarDatePickerFormat * customFormat: string option

    let update (msg: Msg) (state: State) : State =
        match msg with
        | SetDate date -> { state with date = date }
        | SetWatermark text -> { state with watermark = text }
        | SetFormat (format, custom) ->
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
                  CalendarDatePicker.selectedDate state.date
                  CalendarDatePicker.selectedDateFormat state.selectedFormat
                  if state.customFormat.Length > 0 then 
                   CalendarDatePicker.customDateFormatString state.customFormat
                  CalendarDatePicker.watermark state.watermark
                  CalendarDatePicker.horizontalAlignment HorizontalAlignment.Stretch
                ]

                Button.create [
                  Button.content "Set Long Format"
                  Button.onClick(fun _ -> dispatch (SetFormat (CalendarDatePickerFormat.Long, None)))
                  Button.horizontalAlignment HorizontalAlignment.Stretch
                ]

                Button.create [
                  Button.content "Set Short Format"
                  Button.onClick(fun _ -> dispatch (SetFormat (CalendarDatePickerFormat.Short, None)))
                  Button.horizontalAlignment HorizontalAlignment.Stretch
                ]

                Button.create [
                    Button.content """Set Custom "MMMM dd, yyyy" Format """
                    Button.onClick(fun _ -> dispatch (SetFormat (CalendarDatePickerFormat.Custom, Some "MMMM dd, yyyy")))
                    Button.horizontalAlignment HorizontalAlignment.Stretch
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
        
        
        

