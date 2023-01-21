module Avalonia.FuncUI.Elmish.ElmishHook

open Avalonia.FuncUI
open Avalonia.Threading

[<RequireQualifiedAccess>]
type Cmd<'Msg> = 
    private
        | None
        | OfMsg of 'Msg
        | OfAsyncMsg of Async<'Msg>

[<RequireQualifiedAccess>]
module Cmd = 
    let none = Cmd<'Msg>.None
    let ofMsg msg = Cmd<'Msg>.OfMsg msg
    let ofAsyncMsg (asyncMsg: Async<'Msg>) = Cmd<'Msg>.OfAsyncMsg asyncMsg

type ElmishState<'Model, 'Msg>(writableModel: IWritable<'Model>, update) =
    
    let queue = new System.Collections.Generic.Queue<Async<'Msg>>()

    let rec updateRecursive (msg: 'Msg) (model: 'Model) =
        let model, cmd = update msg model
        match cmd with
        | Cmd.None -> model
        | Cmd.OfMsg msg -> updateRecursive msg model
        | Cmd.OfAsyncMsg task -> 
            queue.Enqueue(task)
            model
    
    member this.Model = writableModel.Current

    member this.Dispatch (msg: 'Msg) =
        let model = updateRecursive msg writableModel.Current
        writableModel.Set model

    member this.Update : 'Msg -> 'Model -> 'Model * Cmd<'Msg> = update

    member this.AsyncMessages = queue

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

        // Check for init msg
        match initCmd with
        | Cmd.OfMsg msg -> 
            this.useEffect(
                fun _ -> state.Dispatch msg
                , [ EffectTrigger.AfterInit ]
            )
        | _ -> ()

        // Check for async msg
        this.useEffect(
            fun _ -> 
                let hasAsyncMsg, asyncMsg = state.AsyncMessages.TryDequeue()
                if hasAsyncMsg then
                    async {
                        let! msg = asyncMsg
                        state.Dispatch msg
                    }
                    |> Async.StartImmediate
            , [ EffectTrigger.AfterChange writableModel ]
        )
        
        state.Model, state.Dispatch

    /// Provides Elmish style updates for a model via the useState hook.
    member this.useElmish<'Model, 'Msg> (init: unit -> 'Model * Cmd<'Msg>, update) = 
        this.useElmish(init, update, ())
