namespace Examples.GeneticAlgorithm.Lib

module Population =

    let simulateGeneration states actors =
      actors
      |> Seq.map (fun b -> Simulator.simulate b states)
      |> Seq.sortByDescending (fun r -> r.totalScore)

    let buildInitialPopulation random =
      Seq.init<ActorChromosome> 20 (fun _ -> Genes.getRandomChromosome random)

    let simulateFirstGeneration states random =
      buildInitialPopulation random |> simulateGeneration states

    let mutateBrains (random: System.Random, brains: ActorChromosome list): ActorChromosome list =
      let numBrains = brains.Length
      let survivors = [ brains.[0]; brains.[1]; ]
      let randos = Seq.init (numBrains - 4) (fun _ -> Genes.getRandomChromosome random) |> Seq.toList

      let children = [
        Genes.createChild(random, survivors.[0].genes, survivors.[1].genes, 0.25);
        Genes.createChild(random, survivors.[0].genes, survivors.[1].genes, 0.5);
      ]

      List.append children randos |> List.append survivors

    let mutateAndSimulateGeneration (random: System.Random, worlds: World list, results: SimulationResult list) =
      let brains = Seq.map (fun b -> b.brain) results |> Seq.toList
      mutateBrains(random, brains) |> simulateGeneration worlds

    let mutateAndSimulateMultiple (random: System.Random, worlds: World list, generations: int, results: SimulationResult list) =
      let mutable currentResults = results
      for _ = 1 to generations do
        let brains = Seq.map (fun b -> b.brain) currentResults |> Seq.toList
        currentResults <- mutateBrains(random, brains) |> simulateGeneration worlds |> Seq.toList
      currentResults
