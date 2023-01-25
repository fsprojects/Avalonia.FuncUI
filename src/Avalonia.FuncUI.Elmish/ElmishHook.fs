module Avalonia.FuncUI.Elmish.ElmishHook

open Avalonia.FuncUI
open Elmish

/// Starts an Elmish loop and provides a Dispatch method that calls the given setModel fn.
type ElmishState<'model, 'msg, 'arg>(mkProgram : unit -> Program<'arg, 'model, 'msg, unit>, arg: 'arg, setModel) =
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
        |> Program.runWith arg

    member this.Model = _model
    member this.Dispatch = _dispatch

let ignoreView = (fun _ _ -> ())

type IComponentContext with

    /// Starts an Elmish loop with an existing IWritable.
    member this.useElmish<'model, 'msg> 
        (
            writableModel: IWritable<'model>, 
            update: 'msg -> 'model -> 'model * Cmd<'msg>, 
            ?mapProgram: Program<unit, 'model, 'msg, unit> -> Program<unit, 'model, 'msg, unit>
        ) =
        
        let mapProgram = defaultArg mapProgram id
        let elmishState = this.useState(None, false)

        // Start Elmish loop
        this.useEffect(
            fun () -> 
                let mkProgram() = 
                    let init() = writableModel.Current, Cmd.none
                    Program.mkProgram init update ignoreView 
                    |> mapProgram

                ElmishState(mkProgram, (), writableModel.Set) 
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
            initArg: 'arg, 
            ?mapProgram: Program<'arg, 'model, 'msg, unit> -> Program<'arg, 'model, 'msg, unit>
        ) =
        
        let mapProgram = mapProgram |> Option.defaultValue id

        let elmishState = this.useState(None, false)
        let writableModel = this.useState(init initArg |> fst, true)

        // Start Elmish loop
        this.useEffect(
            fun () -> 
                let mkProgram() = 
                    Program.mkProgram init update ignoreView
                    |> mapProgram

                ElmishState(mkProgram, initArg, writableModel.Set) 
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
            update: 'msg -> 'model -> 'model * Cmd<'msg>, 
            ?mapProgram: Program<unit, 'model, 'msg, unit> -> Program<unit, 'model, 'msg, unit>
        ) =

        let mapProgram = defaultArg mapProgram id
        this.useElmish(init, update, (), mapProgram)
