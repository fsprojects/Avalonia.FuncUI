namespace Avalonia.FuncUI.ControlCatalog.Views

open Avalonia.Controls
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Components
open Avalonia.FuncUI.Elmish
open Elmish

module MenuDemo =
    type State = { color: string }

    let init = { color = "gray" }

    type Msg =
    | SetColor of string

    let update (msg: Msg) (state: State) : State =
        match msg with
        | SetColor color -> { state with color = color }
           
    let view (state: State) (dispatch) =
        Button.create [
            Button.content "right-click me"
            Button.background state.color
            Button.contextMenu (
                ContextMenu.create [
                    ContextMenu.viewItems [
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
                            MenuItem.header "Green"
                            MenuItem.viewItems [
                                MenuItem.create [
                                    MenuItem.header "Light"
                                    MenuItem.onClick (fun _ -> "#2ecc71" |> SetColor |> dispatch)
                                ]
                                MenuItem.create [
                                    MenuItem.header "Dark"
                                    MenuItem.onClick (fun _ -> "#27ae60" |> SetColor |> dispatch)
                                ]  
                            ]
                        ] 
                    ]
                ]  
            )
        ]                  
        
    type Host() as this =
        inherit Hosts.HostControl()
        do
            Elmish.Program.mkSimple (fun () -> init) update view
            |> Program.withHost this
            |> Program.withConsoleTrace
            |> Program.run
        
        
        

