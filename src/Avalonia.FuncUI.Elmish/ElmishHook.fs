module Avalonia.FuncUI.Elmish.ElmishHook

open Avalonia.FuncUI

[<RequireQualifiedAccess>]
type Cmd<'Msg> = 
        | None
        | OfMsg of 'Msg
        static member none = Cmd<'Msg>.None
        static member ofMsg msg = Cmd<'Msg>.OfMsg msg

type ElmishState<'Model, 'Msg>(writableModel: IWritable<'Model>, update) = 

    member this.Model = writableModel.Current

    member this.Dispatch (msg: 'Msg) =
        let model, msg = this.Update msg writableModel.Current
        match msg with
        | Cmd.None -> 
            writableModel.Set model
        | Cmd.OfMsg cmdMsg -> 
            let model2, msg2 = this.Update cmdMsg model
            writableModel.Set model2

    member this.Update : 'Msg -> 'Model -> 'Model * Cmd<'Msg> = update


type IComponentContext with
    
    /// Provides Elmish style updates for a writable model.
    member this.useElmish<'Model, 'Msg> (writableModel: IWritable<'Model>, update) = 
        let state = ElmishState<'Model, 'Msg>(writableModel, update)
        state.Model, state.Dispatch

    /// Provides Elmish style updates for a model via the useState hook.
    member this.useElmish<'Model, 'Msg> (init: 'Model, update) = 
        let writableModel = this.useState (init, true)
        let state = ElmishState<'Model, 'Msg>(writableModel, update)
        state.Model, state.Dispatch