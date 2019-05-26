namespace CounterElmishSample

open Avalonia.FuncUI.Core
open Avalonia.FuncUI.Builders
open Avalonia.Controls
open Avalonia.FuncUI.Core.Model

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
    (*
    let view (state: CounterState) (dispatch): ViewElement =
        button {
            contentView (textblock {
                text (sprintf "Count:  %i" state.count)
            })
        }*)

    
    let view (state: CounterState) (dispatch): ViewElement =
        dockpanel {
            lastChildFill true
            children [
                textblock {
                    text (sprintf "count %i" state.count)
                    dockpanel_dock Dock.Top
                };
                button {
                    contentView (textblock { text "Increment" })
                    dockpanel_dock Dock.Top
                };
                button {
                    contentView (textblock { text "Decrement" })
                    dockpanel_dock Dock.Top
                };
            ]
        }
