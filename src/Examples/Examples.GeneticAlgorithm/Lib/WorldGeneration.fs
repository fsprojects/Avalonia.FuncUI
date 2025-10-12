namespace Examples.GeneticAlgorithm.Lib

module WorldGeneration =

  let hasInvalidlyPlacedItems (actors: Actor array, maxX: int32, maxY: int32): bool =
    let mutable hasIssues = false

    for actA in actors do
      // Don't allow items to spawn in corners
      if (actA.Pos.X = 1 || actA.Pos.X = maxX) && (actA.Pos.Y = 1 || actA.Pos.Y = maxY) then
        hasIssues <- true

      for actB in actors do
        if actA <> actB then

          // Don't allow two objects to start next to each other
          if WorldPos.isAdjacentTo actA.Pos actB.Pos then
            hasIssues <- true

    hasIssues

  let buildActors (maxX:int32, maxY:int32, getRandom): Actor array =
    [|  { Pos = WorldPos.getRandomPos(maxX, maxY, getRandom); ActorKind = Squirrel false; IsActive = true }
        { Pos = WorldPos.getRandomPos(maxX, maxY, getRandom); ActorKind = Tree; IsActive = true }
        { Pos = WorldPos.getRandomPos(maxX, maxY, getRandom); ActorKind = Doggo; IsActive = true }
        { Pos = WorldPos.getRandomPos(maxX, maxY, getRandom); ActorKind = Acorn; IsActive = true }
        { Pos = WorldPos.getRandomPos(maxX, maxY, getRandom); ActorKind = Rabbit; IsActive = true }
    |]

  let generate (maxX:int32, maxY:int32, getRandom): Actor array =
    let mutable items: Actor array = buildActors(maxX, maxY, getRandom)

    // It's possible to generate items in invalid starting configurations. Make sure we don't do that.
    while hasInvalidlyPlacedItems(items, maxX, maxY) do
      items <- buildActors(maxX, maxY, getRandom)

    items

  let makeWorld maxX maxY random =
    let actors = generate(maxX, maxY, random)
    { MaxX = maxX
      MaxY = maxY
      Squirrel = actors.[0]
      Tree = actors.[1]
      Doggo = actors.[2]
      Acorn = actors.[3]
      Rabbit = actors.[4] }

  let makeWorlds (random: System.Random, count: int) =
    Seq.init count (fun _ -> makeWorld 15 15 random.Next) |> Seq.toArray

  let makeDefaultWorld() =
    let random = new System.Random()
    makeWorld 21 21 random.Next

  let makeTestWorld hasAcorn =
    {
      MaxX = 13;
      MaxY = 13;
      Squirrel = {ActorKind = Squirrel hasAcorn; Pos = {X=1; Y=3}; IsActive = true};
      Tree = {ActorKind = Tree; Pos = {X = 8; Y = 10}; IsActive = true};
      Doggo = {ActorKind = Doggo; Pos = {X = 2; Y = 6}; IsActive = true};
      Acorn = {ActorKind = Acorn; Pos = {X = 5; Y = 7}; IsActive = true};
      Rabbit = {ActorKind = Rabbit; Pos = {X = 11; Y = 8}; IsActive = true};
    }