namespace Avalonia.FuncUI.ControlCatalog.Views

open System
open Avalonia.Controls
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Components
open Avalonia.FuncUI.Elmish
open Avalonia.Layout
open Elmish

module GridPatchDemo =
    type State = { orientation: Orientation }
    let init = { orientation = Orientation.Horizontal }

    type Msg = Toggle

    let update (msg: Msg) (state: State) : State =
        match msg with
        | Toggle ->
            { state with orientation =
                match state.orientation with
                | Orientation.Horizontal -> Orientation.Vertical
                | Orientation.Vertical -> Orientation.Horizontal }
           
    let view (state: State) (dispatch) =
        DockPanel.create [
            DockPanel.children [
                Button.create [
                    Button.dock Dock.Top
                    Button.content "toggle"
                    Button.onClick (fun args -> dispatch Toggle)
                ]
                Grid.create [
                    yield
                        match state.orientation with
                        | Orientation.Horizontal -> Grid.rowDefinitions "1*, 1*"
                        | Orientation.Vertical -> Grid.columnDefinitions "1*, 1*"
                    
                    yield Grid.showGridLines true
                    yield Grid.children [
                        Border.create [
                            Border.background "green"
                        ]
                        Border.create [
                            yield Border.background "red"
                            yield
                                match state.orientation with
                                | Orientation.Horizontal -> Border.row 1
                                | Orientation.Vertical -> Border.column 1
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
        
        
        

