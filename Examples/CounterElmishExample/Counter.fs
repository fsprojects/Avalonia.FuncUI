namespace CounterElmishSample

open Avalonia.FuncUI.Core
open Avalonia.FuncUI.Builders

module Counter =

    type CounterState = {
        count : int
    }

    let init = {
        count = 0
    }

    type Msg =
    | Increment
    | Decrement

    let update (msg: Msg) (state: CounterState) : CounterState =
        match msg with
        | Increment -> { state with count =  state.count + 1 }
        | Decrement -> { state with count =  state.count - 1 }

    let view (state: CounterState) (dispatch): ViewElement =
        textblock {
            text (sprintf "count %i" state.count)
        }