namespace Examples.GeneticAlgorithm.Lib

module Fitness =

  let evaluateFitness (gameStates: GameState[], fitnessFunction): float = fitnessFunction(gameStates)

  let standardFitnessFunction (gameStates: GameState[]): float =
    let lastState: GameState = Seq.last gameStates

    let gameLength = float(gameStates.Length)

    let gotAcornBonus =
      match lastState.World.Acorn.IsActive with
      | true  -> 100.0
      | false -> 0.0

    let finalStateBonus =
      match lastState.SimState with
      | SimulationState.Won  -> 1000.0 - (gameLength * 10.0) // Reward quick wins
      | _ -> -50.0 + gameLength

    gotAcornBonus + finalStateBonus

  let killRabbitFitnessFunction (gameStates: GameState[]): float =
    let lastState: GameState = Seq.last gameStates

    let gameLength = float(gameStates.Length)

    let gotAcornBonus =
      match lastState.World.Acorn.IsActive with
      | true  -> 100.0
      | false -> 0.0

    let isRabbitAlive = lastState.World.Rabbit.IsActive

    let finalStateBonus =
      match lastState.SimState with
      | SimulationState.Won  -> match isRabbitAlive with
                                | false -> 1000.0 // Heavily reward dead rabbits
                                | true -> 250.0 - (gameLength * 10.0) // Reward quick wins
      | _ -> match isRabbitAlive with
             | true -> -50.0 + gameLength
             | false -> gameLength

    gotAcornBonus + finalStateBonus