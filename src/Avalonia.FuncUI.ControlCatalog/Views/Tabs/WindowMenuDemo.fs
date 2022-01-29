namespace Avalonia.FuncUI.ControlCatalog.Views

open Elmish
open Avalonia.Controls
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI
open Avalonia.FuncUI.Elmish

module WindowMenuDemo =
    type State = { color: string }

    let init = { color = "gray" }

    type Msg =
    | SetColor of string

    let update (msg: Msg) (state: State) : State =
        match msg with
        | SetColor color -> { state with color = color }
           
    let view (state: State) (dispatch) =
        StackPanel.create [
            StackPanel.background state.color
            
            StackPanel.children [
                Menu.create [
                    Menu.viewItems [
                        MenuItem.create [
                            MenuItem.header "Red"
                            MenuItem.viewItems [
                                MenuItem.create [
                                    MenuItem.header "Light"
                                    MenuItem.onClick (fun _ -> "#e74c3c" |> SetColor |> dispatch)
                                ]
                                MenuItem.create [
                                    MenuItem.header "Dark"
                                    MenuItem.onClick (fun _ -> "#c0392b" |> SetColor |> dispatch)
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
            Elmish.Program.mkSimple (fun () -> init) update view
            |> Program.withHost this
            |> Program.withConsoleTrace
            |> Program.run
        