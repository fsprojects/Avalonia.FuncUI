module Avalonia.FuncUI.Elmish.ElmishHook

open Avalonia.FuncUI

[<RequireQualifiedAccess>]
type Cmd<'Msg> = 
        | None
        | OfMsg of 'Msg
        static member none = Cmd<'Msg>.None
        static member ofMsg msg = Cmd<'Msg>.OfMsg msg

type ElmishState<'Model, 'Msg>(writableModel: IWritable<'Model>, update) =
    
    let rec updateRecursive (msg: 'Msg) (model: 'Model) =
        let model, cmd = update msg model
        match cmd with
        | Cmd.None -> model
        | Cmd.OfMsg msg -> updateRecursive msg model

    member this.Model = writableModel.Current

    member this.Dispatch (msg: 'Msg) =
        let model = updateRecursive msg writableModel.Current
        writableModel.Set model

    member this.Update : 'Msg -> 'Model -> 'Model * Cmd<'Msg> = update


type IComponentContext with
    
    /// Provides Elmish style updates for a writable model.
    member this.useElmish<'Model, 'Msg> (writableModel: IWritable<'Model>, update) = 
        let state = ElmishState<'Model, 'Msg>(writableModel, update)
        state.Model, state.Dispatch

    /// Provides Elmish style updates for a model via the useState hook.
    member this.useElmish<'Arg, 'Model, 'Msg> (init: 'Arg -> 'Model * Cmd<'Msg>, update, arg: 'Arg) = 
        let initModel, initCmd = init arg
        let writableModel = this.useState (initModel, true)
        let state = ElmishState<'Model, 'Msg>(writableModel, update)        

        match initCmd with
        | Cmd.OfMsg msg -> 
            this.useEffect(
                handler = (fun _ ->
                    state.Dispatch msg
                ),
                triggers = [
                    EffectTrigger.AfterInit
                ]
            )
        | _ -> ()
        
        state.Model, state.Dispatch

    /// Provides Elmish style updates for a model via the useState hook.
    member this.useElmish<'Model, 'Msg> (init: unit -> 'Model * Cmd<'Msg>, update) = 
        this.useElmish(init, update, ())
