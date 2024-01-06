namespace Avalonia.FuncUI.Experimental

open Avalonia.FuncUI

[<AutoOpen>]
module CustomOperators =

    let (.=) (state: IWritable<'t>) (value: 't) =
        state.Set value