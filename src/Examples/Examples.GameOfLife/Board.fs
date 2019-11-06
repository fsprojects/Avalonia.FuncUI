namespace Examples.GameOfLife
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.DSL

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
                |> Array2D.mapi (fun x y cell ->
                    Button.create [
                        Button.onClick (fun _ -> { x = x; y = y } |> KillCell |> dispatch)
                    ]
                )
                
            )        
                                
                
            
        ]
        |> generalize