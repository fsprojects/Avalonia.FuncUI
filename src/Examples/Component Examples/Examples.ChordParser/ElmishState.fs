namespace Examples.ChordParser

open Avalonia.FuncUI

/// Wraps a `useState` `IWritable<'t>` to provide Elmish style updates.
type ElmishState<'Model, 'Msg>(writableModel: IWritable<'Model>, update) = 

    member this.Model = writableModel.Current

    member this.Dispatch (msg: 'Msg) =
        let model, msg = this.Update msg writableModel.Current
        match msg with
        | None -> 
            writableModel.Set model
        | OfMsg cmdMsg -> 
            let model2, msg2 = this.Update cmdMsg model
            writableModel.Set model2

    member this.Update : 'Msg -> 'Model -> 'Model * Cmd<'Msg> = update

and Cmd<'Msg> = 
    | None
    | OfMsg of 'Msg
    static member none = Cmd<'Msg>.None
    static member ofMsg msg = Cmd<'Msg>.OfMsg msg
