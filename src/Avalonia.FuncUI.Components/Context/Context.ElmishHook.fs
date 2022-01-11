module Avalonia.FuncUI.ElmishHook

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
