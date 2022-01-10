namespace Examples.ChordParser

open Avalonia.FuncUI

[<AbstractClass>]
type BaseState<'Model, 'Msg>(writableModel: IWritable<'Model>) = 

    member this.Model = writableModel.Current

    member this.Dispatch (msg: 'Msg) =
        let model = this.Update msg writableModel.Current
        writableModel.Set model

    abstract member Update : 'Msg -> 'Model -> 'Model
