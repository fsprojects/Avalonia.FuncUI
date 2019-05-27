namespace CounterElmishSample

open Avalonia.FuncUI.Core
open Avalonia.FuncUI.Builders
open Avalonia.Controls
open Avalonia.FuncUI.Core.Model
open Avalonia.Media

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

        let backgroundColor = 
            if state.count < 0 then
                SolidColorBrush(Color.Parse("#e74c3c"))//.ToImmutable()
            else if state.count > 0 then
                SolidColorBrush(Color.Parse("#27ae60"))//.ToImmutable()
            else
                SolidColorBrush(Colors.Transparent)//.ToImmutable()

        dockpanel {
            background backgroundColor
            lastChildFill true
            children [
                button {
                    contentView (textblock { text "Increment" })
                    command (Command.from (fun _ -> dispatch Msg.Increment))
                    dockpanel_dock Dock.Bottom
                };
                button {
                    contentView (textblock { text "Decrement" })
                    command (Command.from (fun _ -> dispatch Msg.Decrement))
                    dockpanel_dock Dock.Bottom
                };
                textblock {
                    text (sprintf "count %i" state.count)
                    dockpanel_dock Dock.Top
                };
            ]
        }
