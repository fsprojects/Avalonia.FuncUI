namespace Examples.GeneticAlgorithm.Lib

type SimulationState =
    | Simulating = 0
    | Won = 1
    | Lost = 2

type GameState =
    { World : World
      SimState: SimulationState
      TurnsLeft: int }
