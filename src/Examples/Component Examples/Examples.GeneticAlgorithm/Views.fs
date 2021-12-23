namespace Examples.GeneticAlgorithm

open System
open Avalonia.Controls
open Avalonia.Controls.Primitives
open Avalonia.FuncUI.Components
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Types
open Avalonia.Layout
open Avalonia.FuncUI
open Avalonia.Media
open Avalonia.Media.Immutable
open Examples.GeneticAlgorithm.Lib

type StateStore =
    { Random: Random
      ShowHeatMap: IWritable<bool>
      Worlds: IWritable<World list>
      Squirrels: IWritable<SimulationResult list>

      SelectedSquirrelId: IWritable<Guid option>
      SelectedSquirrel: IWritable<SimulationResult option>

      SelectedWorldResultIdx: IWritable<int>
      SelectedWorldResult: IReadable<IndividualWorldResult option>

      SelectedGameStateIdx: IWritable<int>
      SelectedGameState: IReadable<GameState option> }

    member this.NextGeneration () : unit =
        let brains =
            this.Squirrels.Current
            |> List.map (fun squirrel -> squirrel.brain)

        let brains = Population.mutateBrains(this.Random, brains)
        let generation = Population.simulateGeneration this.Worlds.Current brains

        this.Squirrels.Set (List.ofSeq generation)

    member this.NextGenerations (count: int) : unit =
        for _ = 1 to count do
            this.NextGeneration ()

    static member Init () =
        let seed = Random 42
        let worlds = WorldGeneration.makeWorlds(seed, 10)
        let firstGeneration = Population.simulateFirstGeneration worlds seed

        let showHeatMap: IWritable<bool> = new State<bool>(false) :> _
        let worlds: IWritable<World list> = new State<World list>(List.ofSeq worlds) :> _
        let population: IWritable<SimulationResult list> = new State<_>(List.ofSeq firstGeneration) :> _

        let selectedSimulationResultId: IWritable<Guid option> = new State<_>(None) :> _

        let selectedSimulationResult: IWritable<SimulationResult option> =
            population
            |> State.tryFindByKey (fun c -> Some c.identity) selectedSimulationResultId

        let selectedWorldIdx: IWritable<int>  = new State<int>(0) :> _
        let selectedGameStateIdx: IWritable<int> = new State<int>(0) :> _

        let selectedWorld: IReadable<IndividualWorldResult option> =
            selectedSimulationResult
            |> State.readMap (fun c ->
                c
                |> Option.map (fun c -> c.results)
                |> Option.defaultValue List.empty
                |> List.mapi (fun idx v -> (idx, v))
            )
            |> State.readTryFindByKey (fun (idx, _) -> idx) selectedWorldIdx
            |> State.readMap (fun world ->
                world
                |> Option.map snd
            )

        let selectedGameState: IReadable<GameState option> =
            selectedWorld
            |> State.readMap (fun world ->
                world
                |> Option.map (fun result ->
                    result.states
                    |> List.mapi (fun idx v -> (idx, v))
                )
                |> Option.defaultValue List.empty
            )
            |> State.readTryFindByKey (fun (idx, _) -> idx) selectedGameStateIdx
            |> State.readMap (fun gameState ->
                printfn $"game state changed {gameState}"

                gameState
                |> Option.map snd
            )

        { Random = seed
          ShowHeatMap = showHeatMap
          Worlds = worlds
          Squirrels = population
          SelectedSquirrelId = selectedSimulationResultId
          SelectedSquirrel = selectedSimulationResult
          SelectedWorldResultIdx = selectedWorldIdx
          SelectedWorldResult = selectedWorld
          SelectedGameStateIdx = selectedGameStateIdx
          SelectedGameState = selectedGameState }


[<RequireQualifiedAccess>]
module StateStore =
    let shared = StateStore.Init ()


[<AbstractClass; Sealed>]
type Views =

    static member brainView () =
        Component.create ("side-panel", fun ctx ->
            let brain =
                StateStore.shared.SelectedSquirrel
                |> State.readMap (fun squirrel ->
                    squirrel
                    |> Option.map (fun squirrel -> squirrel.brain.genes)
                    |> Option.map (fun genes ->
                        let geneNames = [
                            "Doggo"
                            "Acorn"
                            "Rabbit"
                            "Tree"
                            "Squirrel"
                            "NextToDoggo"
                            "NextToRabbit"
                        ]

                        (genes, geneNames)
                        ||> List.map2 (fun (gene: double) (name: string) -> (name, gene))
                    )
                )
                |> ctx.usePassedRead

            StackPanel.create [
                StackPanel.spacing 10.0
                StackPanel.orientation Orientation.Vertical
                StackPanel.children [
                    match brain.Current with
                    | Some brain ->

                        for name, gene in brain do

                            TextBlock.create [
                                TextBlock.text name
                            ]

                            ProgressBar.create [
                                ProgressBar.value gene
                                ProgressBar.maximum 1.0
                                ProgressBar.minimum -1.0
                            ]

                    | None ->
                        TextBlock.create [
                            TextBlock.text "No squirrel/brain selected"
                        ]
                ]
            ]
            :> IView
        )

    static member sidePanelView () =
        Component.create ("side-panel", fun ctx ->
            let showHeatMap = ctx.usePassed StateStore.shared.ShowHeatMap
            let population = ctx.usePassed StateStore.shared.Squirrels
            let selectedId = ctx.usePassed StateStore.shared.SelectedSquirrelId
            let selectedSquirrel = ctx.usePassed StateStore.shared.SelectedSquirrel
            let selectedWorldIdx = ctx.usePassed StateStore.shared.SelectedWorldResultIdx

            StackPanel.create [
                StackPanel.spacing 10.0
                StackPanel.margin (10.0, 10.0)
                StackPanel.orientation Orientation.Vertical
                StackPanel.children [

                    (* Next Generation *)
                    Button.create [
                        Button.content "Next Generation"
                        Button.onClick (ignore >> StateStore.shared.NextGeneration)
                        Button.horizontalAlignment HorizontalAlignment.Stretch
                    ]

                    (* Next 10 Generation *)
                    Button.create [
                        Button.content "Next 10 Generation"
                        Button.onClick (fun _ ->  StateStore.shared.NextGenerations 10)
                        Button.horizontalAlignment HorizontalAlignment.Stretch
                    ]

                    (* Show Heat Map *)
                    CheckBox.create [
                        CheckBox.content "Show Heat Map"
                        CheckBox.isChecked showHeatMap.Current
                        CheckBox.onChecked (fun _args ->
                            showHeatMap.Set true
                        )
                        CheckBox.onUnchecked (fun _args ->
                            showHeatMap.Set false
                        )
                        CheckBox.horizontalAlignment HorizontalAlignment.Stretch
                    ]

                    (* Population / Squirrel *)
                    ComboBox.create [
                        ComboBox.dataItems population.Current
                        ComboBox.itemTemplate (
                            DataTemplateView.create<_, _>(fun (data: SimulationResult) ->
                                TextBlock.create [
                                    TextBlock.text $"Generation {data.brain.age} Score {data.totalScore}"
                                ]
                            )
                        )
                        ComboBox.onSelectedIndexChanged (fun idx ->
                            population.Current
                            |> List.tryItem idx
                            |> Option.map (fun c -> c.identity)
                            |> selectedId.Set
                        )
                        ComboBox.horizontalAlignment HorizontalAlignment.Stretch
                    ]

                    (* World *)
                    ComboBox.create [
                        ComboBox.dataItems (
                            selectedSquirrel.Current
                            |> Option.map (fun c -> c.results)
                            |> Option.defaultValue []
                            |> List.mapi (fun idx _ -> idx)
                        )
                        ComboBox.itemTemplate (
                            DataTemplateView.create<_, _>(fun (worldIdx: int) ->
                                TextBlock.create [
                                    TextBlock.text $"World {worldIdx+1}"
                                ]
                            )
                        )
                        ComboBox.onSelectedIndexChanged (fun idx ->
                            selectedWorldIdx.Set idx
                        )
                        ComboBox.horizontalAlignment HorizontalAlignment.Stretch
                    ]

                    Views.brainView ()
                ]
            ]
            :> IView
        )

    static member selectedGameStateView () =
        Component.create ("selected-game-state", fun ctx ->
            let squirrel = ctx.usePassedRead StateStore.shared.SelectedSquirrel
            let gameState = ctx.usePassedRead StateStore.shared.SelectedGameState
            let showHeatMap = ctx.usePassedRead StateStore.shared.ShowHeatMap

            match gameState.Current, squirrel.Current with
            | Some gameState, Some squirrel ->
                let world = gameState.World

                UniformGrid.create [
                    UniformGrid.columns world.MaxX
                    UniformGrid.rows world.MaxY
                    UniformGrid.background "#27ae60"
                    UniformGrid.children [
                        (* world cords state at 1 *)
                        for x = 1 to world.MaxX do
                            for y = 1 to world.MaxY do
                                let actor =
                                    world.Actors
                                    |> Array.tryFind (fun actor -> actor.Pos.X = x && actor.Pos.Y = y)
                                    |> Option.map (fun actor -> actor.ActorKind)

                                let background =
                                    if showHeatMap.Current then
                                        let heat = Genes.evaluateTile squirrel.brain gameState.World (WorldPos.newPos x y)
                                        ImmutableSolidColorBrush (Colors.White, 1.0 - float heat)

                                    else
                                        ImmutableSolidColorBrush (Colors.White, 0.0)

                                ContentControl.create [
                                    ContentControl.row y
                                    ContentControl.column x
                                    ContentControl.margin 0.5

                                    ContentControl.fontSize 24.0
                                    ContentControl.verticalContentAlignment VerticalAlignment.Center
                                    ContentControl.horizontalContentAlignment HorizontalAlignment.Center
                                    ContentControl.background background

                                    yield! [
                                        match actor with
                                        | Some ActorKind.Acorn ->
                                            ContentControl.content "🌰"
                                        | Some (ActorKind.Squirrel true) ->
                                            ContentControl.content "🐿🌰"
                                        | Some (ActorKind.Squirrel false) ->
                                            ContentControl.content "🐿"
                                        | Some ActorKind.Tree ->
                                            ContentControl.content "🌲"
                                        | Some ActorKind.Rabbit ->
                                            ContentControl.content "🐇"
                                        | Some ActorKind.Doggo ->
                                            ContentControl.content "🐕"
                                        | None ->
                                            () //ContentControl.content ""
                                    ]
                                ]


                        TextBlock.create [
                            TextBlock.text "Content"
                        ]
                    ]
                ] :> _
            | _ ->
                TextBlock.create [
                    TextBlock.text "No Game State Selected"
                ] :> _
        )
    static member simulationView () =
        Component.create ("content-view", fun ctx ->
            let worldResult = ctx.usePassedRead StateStore.shared.SelectedWorldResult
            let gameStateIdx = ctx.usePassed StateStore.shared.SelectedGameStateIdx

            match worldResult.Current with
            | Some worldResult ->
                DockPanel.create [
                    DockPanel.lastChildFill true
                    DockPanel.children [
                        DockPanel.create [
                            DockPanel.dock Dock.Bottom
                            DockPanel.children [

                                Slider.create [
                                    Slider.dock Dock.Top
                                    Slider.minimum (double 0)
                                    Slider.maximum (double (worldResult.states.Length - 1))
                                    Slider.tickFrequency (double 1)
                                    Slider.value (double gameStateIdx.Current)
                                    Slider.onValueChanged (fun value ->
                                        gameStateIdx.Set (int value)
                                    )
                                ]

                                StackPanel.create [
                                    StackPanel.dock Dock.Top
                                    StackPanel.children [
                                        TextBlock.create [
                                            TextBlock.text $"world score: {worldResult.score}"
                                        ]

                                        TextBlock.create [
                                            TextBlock.text (
                                                let state =
                                                    match int (List.last worldResult.states).SimState with
                                                    | 0 ->
                                                        "running"
                                                    | 1 ->
                                                        "won"
                                                    | 2 ->
                                                        "lost"
                                                    | _ ->
                                                        "unknown"

                                                $"result: {state}"
                                            )



                                        ]
                                    ]

                                ]
                            ]
                        ]

                        Views.selectedGameStateView ()
                    ]

                ] :> IView
            | None ->
                TextBlock.create [
                    TextBlock.text "No World Result Selected"
                ] :> IView
        )

    static member contentView () =
        Component.create ("content-view", fun ctx ->

            DockPanel.create [
                DockPanel.lastChildFill true
                DockPanel.children [
                    Views.simulationView ()
                ]
            ]
            :> IView
        )

    static member mainView () =
        Component (fun ctx ->
            DockPanel.create [
                DockPanel.lastChildFill true
                DockPanel.children [
                    SplitView.create [
                        SplitView.isPaneOpen true
                        SplitView.displayMode SplitViewDisplayMode.CompactInline

                        SplitView.pane (
                            Views.sidePanelView ()
                        )

                        SplitView.content (
                            Views.contentView ()
                        )
                    ]
                ]
            ]
            :> IView
        )