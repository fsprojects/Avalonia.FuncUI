module Avalonia.FuncUI.Elmish.ElmishHook

open Avalonia.FuncUI
open Elmish

type ElmishState<'model, 'msg, 'arg>(mkProgram : unit -> Program<'arg, 'model, 'msg, unit>, arg: 'arg, setModel) =
    let program = mkProgram()

    let mutable state, cmd =
        let model, cmd = Program.init program arg
        (model, fun (_: 'msg) -> ()), cmd

    let setState model dispatch =
        let oldModel = fst state
        if not (obj.ReferenceEquals(model, oldModel)) then
            state <- model, dispatch
            setModel model
    do  
        program
        |> Program.withSetState setState
        |> Program.runWith arg

    member this.Model = fst state
    member this.Dispatch = snd state

let noView = (fun _ _ -> ())

type IComponentContext with
    
    member this.useElmish<'arg, 'model, 'msg> 
        (
            init : 'arg -> 'model * Cmd<'msg>, 
            update: 'msg -> 'model -> 'model * Cmd<'msg>, 
            arg: 'arg, 
            mapProgram: Program<'arg, 'model, 'msg, unit> -> Program<'arg, 'model, 'msg, unit>
        ) =
        
        let elmishStateRef = ref None
        let writableModel = this.useState(init arg |> fst, true)

        // Start Elmish loop
        this.useEffect(
            fun () -> 
                let mkProgram() = 
                    Program.mkProgram init update noView
                    |> mapProgram

                let setModel model = 
                    writableModel.Set model

                let es = ElmishState(mkProgram, arg, setModel)
                elmishStateRef := Some es
            , [ EffectTrigger.AfterInit ]
        )
        
        let dispatch map = 
            elmishStateRef.Value 
            |> Option.iter (fun es -> es.Dispatch map)

        writableModel.Current, dispatch

    member this.useElmish<'arg, 'model, 'msg> 
        (
            init : 'arg -> 'model * Cmd<'msg>, 
            update: 'msg -> 'model -> 'model * Cmd<'msg>, 
            arg: 'arg
        ) =

        this.useElmish(init, update, arg, id)

    //member this.useElmish<'model, 'msg> (mkProgram : unit -> Program<unit, 'model, 'msg, unit>) =
    //    this.useElmish(mkProgram, ())

    //member this.useElmish<'arg, 'model, 'msg> (init : 'arg -> 'model * Cmd<'msg>, update: 'msg -> 'model -> 'model * Cmd<'msg>, arg: 'arg) =
    //    let mkProgram() = Program.mkProgram init update noView
    //    this.useElmish(mkProgram, arg)

    //member this.useElmish<'model, 'msg> (init : unit -> 'model * Cmd<'msg>, update: 'msg -> 'model -> 'model * Cmd<'msg>) =
    //    let mkProgram() = Program.mkProgram init update noView
    //    this.useElmish(mkProgram, ())