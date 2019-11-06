namespace Examples.GameOfLife

type Cell = Alive | Dead

type BoardMatrix =
    { width : int
      height : int
      cells : Cell[,] }

module BoardMatrix =
    let constructBlank (width: int, height: int) : BoardMatrix =
        { width = width
          height = height
          cells = Array2D.create width height Dead }
    
    let private setCell (board: BoardMatrix, pos: BoardPosition, cell: Cell) : BoardMatrix =
        { board with cells = Array2D.set board.cells pos.x pos.y cell }
        
    let private getCell (board: BoardMatrix, pos: BoardPosition) : Cell =
        Array2D.get board.cells pos.x pos.y
        
    let private tryGetCell (board: BoardMatrix, pos: BoardPosition) : Cell option =
        let xInRange = pos.x >= 0 && pos.x < board.width
        let yInRange = pos.y >= 0 && pos.y < board.height
        if (xInRange && yInRange) then
            try Some (Array2D.get board.cells pos.x pos.y)
            with _ -> None
        else None
        
    let private getNeighbors (board: BoardMatrix, pos: BoardPosition) : Cell list =
        let neighborPositions =
            [
                pos |> BoardPosition.above
                pos |> BoardPosition.above |> BoardPosition.leftOf
                pos |> BoardPosition.above |> BoardPosition.rightOf
                pos |> BoardPosition.leftOf
                pos |> BoardPosition.rightOf
                pos |> BoardPosition.below
                pos |> BoardPosition.below |> BoardPosition.leftOf
                pos |> BoardPosition.below |> BoardPosition.rightOf
            ]
            
        neighborPositions
        |> List.map (fun pos -> tryGetCell(board, pos))
        |> List.map (function | None -> Dead | Some cell -> cell)
            
        
    let placeAliveCell (board: BoardMatrix, pos: BoardPosition) : BoardMatrix =
        setCell(board, pos, Alive)
        
    let placeDeadCell (board: BoardMatrix, pos: BoardPosition) : BoardMatrix =
        setCell(board, pos, Dead)
        
    let isAliveCell (board: BoardMatrix, pos: BoardPosition) : bool =
        getCell(board, pos) = Alive
        
    let isDeadCell (board: BoardMatrix, pos: BoardPosition) : bool =
        getCell(board, pos) = Dead
        
    let evolveCellDead (neighbors: Cell list) : Cell =
        let aliveNeighbors =
            neighbors
            |> List.filter (fun cell -> cell = Alive)
            |> List.length
            
        match aliveNeighbors with
        | 3 -> Alive
        | _ -> Dead
        
    let evolveCellAlive (neighbors: Cell list) : Cell =
        let aliveNeighbors =
            neighbors
            |> List.filter (fun cell -> cell = Alive)
            |> List.length
            
        match aliveNeighbors with
        | 0 | 1 -> Dead
        | 2 | 3 -> Alive
        | _ -> Dead
        
    let evolveCell (board: BoardMatrix, pos: BoardPosition, self: Cell) : Cell =
        let neighbors = getNeighbors(board, pos)
        match self with
        | Dead -> evolveCellDead neighbors
        | Alive -> evolveCellAlive neighbors
        
    let evolve (board: BoardMatrix) : BoardMatrix =
        let apply x y cell =
            evolveCell(board, { x = x; y = y }, cell)
        
        { board with cells = board.cells |> Array2D.mapi apply }
