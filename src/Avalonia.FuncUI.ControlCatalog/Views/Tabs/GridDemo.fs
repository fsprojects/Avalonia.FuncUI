namespace Avalonia.FuncUI.ControlCatalog.Views

open System
open Avalonia.Controls
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Components
open Avalonia.FuncUI.Elmish
open Elmish

module GridDemo =
    type State =
        { cellWidth: int
          cellHeight: int }

    let init =
        { cellWidth = 100
          cellHeight = 200 }

    type Msg =
    | SetCellWidth of int
    | SetCellHeight of int

    let update (msg: Msg) (state: State) : State =
        match msg with
        | SetCellWidth width -> { state with cellWidth = width }
        | SetCellHeight height -> { state with cellHeight = height }
           
    let view (state: State) (dispatch) =
        DockPanel.create [
            DockPanel.children [
                TextBlock.create [
                    TextBlock.dock Dock.Top
                    TextBlock.margin 5.0
                    TextBlock.text "Grid is below"
                ]
                TextBox.create [
                    TextBlock.dock Dock.Top
                    TextBox.text (string state.cellHeight)
                    TextBox.onTextChanged ((fun text ->
                        text |> Int32.Parse |> SetCellHeight |> dispatch
                    ), SubPatchOptions.Never)
                ]
                TextBox.create [
                    TextBlock.dock Dock.Top
                    TextBox.text (string state.cellWidth)
                    TextBox.onTextChanged ((fun text ->
                        text |> Int32.Parse |> SetCellWidth |> dispatch
                    ), SubPatchOptions.Never)
                ]
                Grid.create [
                    Grid.columnDefinitions (sprintf "%i, 1*" state.cellWidth)
                    Grid.rowDefinitions (sprintf "%i, 1*" state.cellHeight)
                    Grid.showGridLines true
                    Grid.children [
                        Border.create [
                            Border.background "green"
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
        
        
        

