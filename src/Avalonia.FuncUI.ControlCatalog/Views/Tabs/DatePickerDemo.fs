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
        selectedFormat: DatePickerFormat }

    let init = 
        { date = Nullable(DateTime.Today)
          watermark = ""
          customFormat = ""
          selectedFormat = DatePickerFormat.Long
        }

    type Msg =
    | SetDate of DateTime Nullable
    | SetWatermark of string
    | SetDatePickerFormat of DatePickerFormat * customFormat: string option

    let update (msg: Msg) (state: State) : State =
        match msg with
        | SetDate date -> { state with date = date }
        | SetWatermark text -> { state with watermark = text }
        | SetDatePickerFormat (format, custom) ->
          match format with
          | DatePickerFormat.Short 
          | DatePickerFormat.Long -> 
            { state with selectedFormat = format }
          | DatePickerFormat.Custom ->
            match custom with
            | Some customFormat -> 
              { state with selectedFormat = format; customFormat = customFormat }
            | None -> { state with selectedFormat = DatePickerFormat.Long; customFormat = "" }
          | _ -> { state with selectedFormat = DatePickerFormat.Long; customFormat = "" }
              
           
    let view (state: State) (dispatch) =
        StackPanel.create [
            StackPanel.spacing 10.0
            StackPanel.children [
                DatePicker.create [
                  yield DatePicker.selectedDate state.date
                  yield DatePicker.selectedDateFormat state.selectedFormat
                  if state.customFormat.Length > 0 then 
                    yield DatePicker.customDateFormatString state.customFormat
                  yield DatePicker.watermark state.watermark
                ]

                Button.create [
                  Button.content "Set Long Date Picker Format"
                  Button.onClick((fun _ -> dispatch (SetDatePickerFormat (DatePickerFormat.Long, None))), SubPatchOptions.Never)
                ]

                Button.create [
                  Button.content "Set Short Date Picker Format"
                  Button.onClick((fun _ -> dispatch (SetDatePickerFormat (DatePickerFormat.Short, None))), SubPatchOptions.Never)
                ]

                Button.create [
                    Button.content """Set Custom "MMMM dd, yyyy" Date Picker Format """
                    Button.onClick((fun _ -> dispatch (SetDatePickerFormat (DatePickerFormat.Custom, Some "MMMM dd, yyyy"))), SubPatchOptions.Never)
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
        
        
        

