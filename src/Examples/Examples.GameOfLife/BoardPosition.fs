namespace Examples.GameOfLife

type BoardPosition =
    { x : int
      y : int  }

module BoardPosition =
    
    let leftOf (pos: BoardPosition) : BoardPosition =
        { pos with x = pos.x - 1 }
        
    let rightOf (pos: BoardPosition) : BoardPosition =
        { pos with x = pos.x + 1 }

    let below (pos: BoardPosition) : BoardPosition =
        { pos with y = pos.y - 1 }
        
    let above (pos: BoardPosition) : BoardPosition =
        { pos with y = pos.y + 1 }