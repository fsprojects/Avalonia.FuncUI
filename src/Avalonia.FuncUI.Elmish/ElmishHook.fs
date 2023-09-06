module Avalonia.FuncUI.Elmish.ElmishHook

open Avalonia.FuncUI
open Elmish
open System

/// Starts an Elmish loop and provides a Dispatch method that calls the given setModel fn.
type ElmishState<'model, 'msg, 'msg2, 'arg>(mkProgram : unit -> Program<'arg, 'model, 'msg, unit>, mapProgram : Program<'arg, 'model, 'msg, unit> -> Program<'arg, 'model, 'msg2, unit>, arg: 'arg, setModel) =
    let program = mkProgram()

    let mutable _model = Program.init program arg |> fst
    let mutable _dispatch = fun (_: 'msg) -> ()

    let setState model dispatch =
        _dispatch <- dispatch
        if not (obj.ReferenceEquals(model, _model)) then
            _model <- model
            setModel model
    do
        program
        |> Program.withSetState setState
        |> mapProgram
        |> Program.runWith arg

    member this.Model = _model
    member this.Dispatch = _dispatch

let ignoreView = (fun _ _ -> ())

type IComponentContext with

    /// Starts an Elmish loop with an existing IWritable.
    member this.useElmish<'model, 'msg>
        (
            writableModel: IWritable<'model>,
            update: 'msg -> 'model -> 'model * Cmd<'msg>
        ) =

        let elmishState = this.useState(None, false)

        // Start Elmish loop
        this.useEffect(
            fun () ->
                let mkProgram() =
                    let init() = writableModel.Current, Cmd.none
                    Program.mkProgram init update ignoreView

                ElmishState(mkProgram, id, (), writableModel.Set)
                |> Some
                |> elmishState.Set

            , [ EffectTrigger.AfterInit ]
        )

        let dispatch map =
            elmishState.Current
            |> Option.iter (fun es -> es.Dispatch map)

        writableModel.Current, dispatch

    /// Starts an Elmish loop with an existing IWritable and customizations to the Program.
    member this.useElmish<'model, 'msg, 'msg2>
        (
            writableModel: IWritable<'model>,
            update: 'msg -> 'model -> 'model * Cmd<'msg>,
            mapProgram: Program<unit, 'model, 'msg, unit> -> Program<unit, 'model, 'msg2, unit>
        ) =

        let elmishState = this.useState(None, false)

        // Start Elmish loop
        this.useEffect(
            fun () ->
                let mkProgram() =
                    let init() = writableModel.Current, Cmd.none
                    Program.mkProgram init update ignoreView

                ElmishState(mkProgram, mapProgram, (), writableModel.Set)
                |> Some
                |> elmishState.Set

            , [ EffectTrigger.AfterInit ]
        )

        let dispatch map =
            elmishState.Current
            |> Option.iter (fun es -> es.Dispatch map)

        writableModel.Current, dispatch

    /// Starts an Elmish loop with an init arg.
    member this.useElmish<'arg, 'model, 'msg>
        (
            init : 'arg -> 'model * Cmd<'msg>,
            update: 'msg -> 'model -> 'model * Cmd<'msg>,
            initArg: 'arg
        ) =

        let elmishState = this.useState(None, false)
        let writableModel = this.useState(init initArg |> fst, true)

        // Start Elmish loop
        this.useEffect(
            fun () ->
                let mkProgram() =
                    Program.mkProgram init update ignoreView

                ElmishState(mkProgram, id, initArg, writableModel.Set)
                |> Some
                |> elmishState.Set

            , [ EffectTrigger.AfterInit ]
        )

        let dispatch map =
            elmishState.Current
            |> Option.iter (fun es -> es.Dispatch map)

        writableModel.Current, dispatch

    /// Starts an Elmish loop with an init arg and customizations to the Program.
    member this.useElmish<'arg, 'model, 'msg, 'msg2>
        (
            init : 'arg -> 'model * Cmd<'msg>,
            update: 'msg -> 'model -> 'model * Cmd<'msg>,
            initArg: 'arg,
            mapProgram: Program<'arg, 'model, 'msg, unit> -> Program<'arg, 'model, 'msg2, unit>
        ) =

        let elmishState = this.useState(None, false)
        let writableModel = this.useState(init initArg |> fst, true)

        // Start Elmish loop
        this.useEffect(
            fun () ->
                let mkProgram() =
                    Program.mkProgram init update ignoreView

                ElmishState(mkProgram, mapProgram, initArg, writableModel.Set)
                |> Some
                |> elmishState.Set

            , [ EffectTrigger.AfterInit ]
        )

        let dispatch map =
            elmishState.Current
            |> Option.iter (fun es -> es.Dispatch map)

        writableModel.Current, dispatch

    /// Starts an Elmish loop.
    member this.useElmish<'model, 'msg>
        (
            init : unit -> 'model * Cmd<'msg>,
            update: 'msg -> 'model -> 'model * Cmd<'msg>
        ) =

        this.useElmish(init, update, ())

    /// Starts an Elmish loop with customizations to the Program.
    member this.useElmish<'model, 'msg, 'msg2>
        (
            init : unit -> 'model * Cmd<'msg>,
            update: 'msg -> 'model -> 'model * Cmd<'msg>,
            mapProgram: Program<unit, 'model, 'msg, unit> -> Program<unit, 'model, 'msg2, unit>
        ) =

        this.useElmish(init, update, (), mapProgram)

