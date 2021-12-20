namespace Examples.GameOfLife
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI

module Board =
    open Avalonia.FuncUI.Types
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
                |> Array.map (fun (x, y, cell) ->
                    let cellPosition = { x = x; y = y }
                    
                    Button.create [
                        match cell with
                        | Alive ->
                            yield Button.onClick ((fun _ -> cellPosition |> KillCell |> dispatch), SubPatchOptions.OnChangeOf cellPosition)
                            yield Button.background "green"
                        | Dead ->
                            yield Button.onClick ((fun _ -> cellPosition |> ReviveCell |> dispatch), SubPatchOptions.OnChangeOf cellPosition)
                            yield Button.background "gray"
                        
                    ] |> generalize                     
                )
                |> Array.toList
            )        
        ]
        |> generalize