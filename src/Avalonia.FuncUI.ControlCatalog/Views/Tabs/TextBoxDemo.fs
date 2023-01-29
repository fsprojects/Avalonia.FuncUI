namespace Avalonia.FuncUI.ControlCatalog.Views

open Avalonia.Controls
open Avalonia.Layout
open Elmish
open Avalonia.FuncUI
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Elmish

module TextBoxDemo =
    type State = { watermark: string }

    let init () = { watermark = "" }

    type Msg =
    | SetWatermark of string

    let update (msg: Msg) (state: State) : State =
        match msg with
        | SetWatermark text -> { state with watermark = text }
           
    let view (state: State) (dispatch) =
        StackPanel.create [
            StackPanel.spacing 10.0
            StackPanel.children [
                TextBox.create [
                  TextBox.watermark state.watermark
                  TextBox.horizontalAlignment HorizontalAlignment.Stretch
                ]

                Button.create [
                    Button.content "Set Watermark"
                    Button.onClick (fun _ -> dispatch (SetWatermark "I'm the watermark"))
                    Button.horizontalAlignment HorizontalAlignment.Stretch
                ]
                
                Button.create [
                    Button.content "Unset Watermark"
                    Button.onClick (fun _ -> dispatch (SetWatermark ""))
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
        
        
        

