namespace Avalonia.FuncUI.ControlCatalog.Views

open System
open Avalonia.Controls
open Elmish
open Avalonia.FuncUI
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Components
open Avalonia.FuncUI.Elmish
open Avalonia.Input
open Avalonia.Interactivity
open Avalonia.Layout

module MaskedTextBoxDemo =
    type State = { count: int }

    let init = { count = 0 }

    type Msg =
    | SetCount of int

    let update (msg: Msg) (state: State) : State =
        match msg with
        | SetCount number -> { state with count = number }
           
    let view (state: State) (dispatch) =
        StackPanel.create [
            StackPanel.spacing 10.0
            StackPanel.children [
                TextBox.create [
                    TextBox.text (string state.count)
                    TextBox.onTextChanged (fun text ->
                        let isNumber, number = Int32.TryParse text
                        if isNumber then
                            dispatch (SetCount number)
                            
                    , SubPatchOptions.Never) // <- this is annoying.
                                             // We should have this optional with the default being `Never`
                    
                    TextBox.useEffect (fun control ->
                        
                        let action = EventHandler<TextInputEventArgs>(fun sender args ->
                            let isNumber, _ = Int32.TryParse args.Text
                            if not isNumber then
                                args.Text <- String.Empty
                        )
                            
                        (TextBox.TextInputEvent, action, RoutingStrategies.Tunnel, false)
                        |> control.AddHandler
                        |> ignore
                    )
                ]
                TextBlock.create [
                    TextBlock.horizontalAlignment HorizontalAlignment.Center
                    TextBlock.verticalAlignment VerticalAlignment.Center
                    TextBlock.fontSize 32.0
                    TextBlock.text (string state.count)
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
        
        
        

