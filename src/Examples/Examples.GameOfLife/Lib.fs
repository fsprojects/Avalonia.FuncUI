namespace Examples.GameOfLife

module Array2D =
    let set (array: 't[,]) (x: int) (y: int) (value: 't) : 't[,] =
        let copy = Array2D.copy array
        Array2D.set copy x y value
        copy
        
    let flat (arr: 'a[,]) : 'a[] =
        [|
            for x in [0..(Array2D.length1 arr) - 1] do 
                for y in [0..(Array2D.length2 arr) - 1] do 
                    yield arr.[x, y]
        |]
        
    let flati (arr: 'a[,]) : (int * int * 'a)[] =
        [|
            for x in [0..(Array2D.length1 arr) - 1] do 
                for y in [0..(Array2D.length2 arr) - 1] do 
                    yield (x, y, arr.[x, y])
        |]
