module Avalonia.FuncUI.Elmish.ElmishHook

open Avalonia.FuncUI
open Elmish

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

    /// Starts an Elmish loop using an existing IWritable.
    member this.useElmish<'model, 'msg> 
        (
            writableModel: IWritable<'model>, 
            update: 'msg -> 'model -> 'model * Cmd<'msg>, 
            mapProgram: Program<unit, 'model, 'msg, unit> -> Program<unit, 'model, 'msg, unit>
        ) =
        
        let elmishStateRef = ref None

        // Start Elmish loop
        this.useEffect(
            fun () -> 
                let mkProgram() = 
                    Program.mkProgram (fun () -> writableModel.Current, Cmd.none) update ignoreView
                    |> mapProgram

                let setModel model = 
                    writableModel.Set model

                let es = ElmishState(mkProgram, (), setModel)
                elmishStateRef := Some es
            , [ EffectTrigger.AfterInit ]
        )
        
        let dispatch map = 
            elmishStateRef.Value 
            |> Option.iter (fun es -> es.Dispatch map)

        writableModel.Current, dispatch

    /// Starts an Elmish loop using an existing IWritable.
    member this.useElmish<'model, 'msg> 
        (
            writableModel: IWritable<'model>, 
            update: 'msg -> 'model -> 'model * Cmd<'msg>
        ) =

        this.useElmish(writableModel, update, id)
    
    /// Starts an Elmish loop.
    member this.useElmish<'arg, 'model, 'msg> 
        (
            init : 'arg -> 'model * Cmd<'msg>, 
            update: 'msg -> 'model -> 'model * Cmd<'msg>, 
            initArg: 'arg, 
            mapProgram: Program<'arg, 'model, 'msg, unit> -> Program<'arg, 'model, 'msg, unit>
        ) =
        
        let elmishStateRef = ref None
        let writableModel = this.useState(init initArg |> fst, true)

        // Start Elmish loop
        this.useEffect(
            fun () -> 
                let mkProgram() = 
                    Program.mkProgram init update ignoreView
                    |> mapProgram

                let setModel model = 
                    writableModel.Set model

                let es = ElmishState(mkProgram, initArg, setModel)
                elmishStateRef := Some es
            , [ EffectTrigger.AfterInit ]
        )
        
        let dispatch map = 
            elmishStateRef.Value 
            |> Option.iter (fun es -> es.Dispatch map)

        writableModel.Current, dispatch

    /// Starts an Elmish loop.
    member this.useElmish<'arg, 'model, 'msg> 
        (
            init : 'arg -> 'model * Cmd<'msg>, 
            update: 'msg -> 'model -> 'model * Cmd<'msg>, 
            initArg: 'arg
        ) =

        this.useElmish(init, update, initArg, id)
