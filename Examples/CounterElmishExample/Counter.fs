namespace CounterElmishSample

open Avalonia.Controls
open Avalonia.Media
open Avalonia.FuncUI.Types
open Avalonia.FuncUI.Views
open Avalonia.FuncUI.View.Lifecycle

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
    
    let view (state: CounterState) (dispatch): View =
        stackpanel [
            orientation Orientation.Horizontal
            children [
                textblock [
                    text (sprintf "the count is %i" state.count)
                ]
                button [
                    click (fun sender args -> dispatch Increment)
                    content (textblock [
                        text "click to increment"
                    ])
                ]
                button [
                    click (fun sender args -> dispatch Decrement)
                    content (textblock [
                        text "click to decrement"
                    ])
                ]
            ]
        ]       
