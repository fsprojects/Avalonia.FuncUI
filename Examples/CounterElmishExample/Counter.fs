namespace CounterElmishSample

open Avalonia.FuncUI.Core
open Avalonia.Controls
open Avalonia.Media
open Avalonia.FuncUI

module Counter =
    open Avalonia.Layout
    open Avalonia.FuncUI.Core.Types
    open Avalonia.FuncUI.View
    open Avalonia.FuncUI.Attr

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

        let foregroundColor = 
            if state.count < 0 then SolidColorBrush(Color.Parse("#e74c3c")).ToImmutable()
            else if state.count > 0 then SolidColorBrush(Color.Parse("#27ae60")).ToImmutable()
            else SolidColorBrush(Colors.White).ToImmutable()

        stackpanel [
            //background foregroundColor
            orientation Orientation.Horizontal
            children [
                textblock [
                    text (sprintf "the count is %i" state.count)
                ]
                button [
                    click (fun sender args ->
                        dispatch Increment
                        args.Handled <- true
                    )
                    content (textblock [
                        text "click to increment"
                    ])
                ]
                button [
                    click (fun sender args ->
                        dispatch Decrement
                        args.Handled <- true
                    )
                    content (textblock [
                        text "click to decrement"
                    ])
                ]
            ]
        ]       
