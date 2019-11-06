namespace Examples.GameOfLife

module Array2D =
    let set (array: 't[,]) (x: int) (y: int) (value: 't) : 't[,] =
        let copy = Array2D.copy array
        Array2D.set copy x y value
        copy
