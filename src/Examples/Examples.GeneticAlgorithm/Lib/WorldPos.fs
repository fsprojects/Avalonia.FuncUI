namespace Examples.GeneticAlgorithm.Lib

type WorldPos =
  { X: int32
    Y: int32 }

module WorldPos =

  let newPos x y = {X = x; Y = y}

  let isAdjacentTo (posA: WorldPos) (posB: WorldPos): bool =
    let xDiff = abs (posA.X - posB.X)
    let yDiff = abs (posA.Y - posB.Y)
    let result = xDiff <= 1 && yDiff <= 1
    result

  let getRandomPos(maxX:int32, maxY:int32, getRandom): WorldPos =
    let x = 1 + getRandom maxX
    let y = 1 + getRandom maxY
    newPos x y

  let getDistance(a: WorldPos, b: WorldPos): float =
    let x1 = float(a.X)
    let x2 = float(b.X)
    let y1 = float(a.Y)
    let y2 = float(b.Y)
    System.Math.Sqrt((x1-x2)*(x1-x2) + (y1-y2)*(y1-y2)) // Calculate distance via C^2 = A^2 + B^2