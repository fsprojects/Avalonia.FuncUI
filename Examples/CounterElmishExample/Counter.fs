namespace CounterElmishSample

open Avalonia.FuncUI.Core
open Avalonia.FuncUI.Builders
open Avalonia.Controls
open Avalonia.FuncUI.Core.Model
open Avalonia.Media

module Counter =
    open Avalonia.Layout

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

        let foregroundColor = 
            if state.count < 0 then SolidColorBrush(Color.Parse("#e74c3c")).ToImmutable()
            else if state.count > 0 then SolidColorBrush(Color.Parse("#27ae60")).ToImmutable()
            else SolidColorBrush(Colors.White).ToImmutable()

        dockpanel {
            lastChildFill true
            children [
                button {
                    dockpanel_dock Dock.Bottom
                    contentView (textblock { text "Increment" })
                    command (Command.from (fun _ -> dispatch Msg.Increment))
                };
                button {
                    dockpanel_dock Dock.Bottom
                    contentView (textblock { text "Decrement" })
                    command (Command.from (fun _ -> dispatch Msg.Decrement))
                };
                textblock {
                    dockpanel_dock Dock.Top
                    text (sprintf "Count: %i" state.count)
                    fontSize 20.0
                    foreground foregroundColor
                    verticalAlignment VerticalAlignment.Center
                    horizontalAlignment HorizontalAlignment.Center
                }
                
            ]
        }
