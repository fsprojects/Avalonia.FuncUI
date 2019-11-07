namespace Examples.GameOfLife
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.DSL
open Avalonia.Input

module Board =
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.DSL
    open Avalonia.Controls
    open Avalonia.Controls.Primitives

    type Msg =
        | Evolve
        | KillCell of BoardPosition
        | ReviveCell of BoardPosition
    
    let update (msg: Msg) (board: BoardMatrix) : BoardMatrix =
        match msg with
        | Evolve -> BoardMatrix.evolve board
        | KillCell pos -> BoardMatrix.placeDeadCell (board, pos)
        | ReviveCell pos -> BoardMatrix.placeAliveCell (board, pos)
        
    let view (board: BoardMatrix) (dispatch: Msg -> unit) : IView =
        UniformGrid.create [
            UniformGrid.columns board.width
            UniformGrid.rows board.height
            UniformGrid.children (
                board.cells
                |> Array2D.flati
                |> Array.map (fun item ->
                    let x, y, cell = item 
                    let cellPosition = { x = x; y = y }
                    
                    Button.create [
                        match cell with
                        | Alive ->
                            yield Button.onClick (fun _ -> cellPosition |> KillCell |> dispatch)
                            yield Button.background "green"
                        | Dead ->
                            yield Button.onClick (fun _ -> cellPosition |> ReviveCell |> dispatch)
                            yield Button.background "gray"
                        
                    ] |> generalize                     
                )
                |> Array.toList
            )        
        ]
        |> generalize