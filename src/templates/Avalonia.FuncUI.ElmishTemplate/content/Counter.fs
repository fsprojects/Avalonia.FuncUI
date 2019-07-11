namespace CounterElmishSample

open Avalonia.Controls
open Avalonia.Media
open Avalonia.FuncUI.Types
open Avalonia.FuncUI
open Avalonia.Layout


module Counter =

    // The model holds data that you want to keep track of while the application is running
    type CounterState = {
        count : int
    }

    //The initial state of of the application
    let initialState = {
        count = 0
    }

    // The Msg type defines what events/actions can occur while the application is running
    // the state of the application changes *only* in reaction to these events
    type Msg =
    | Increment
    | Decrement

    // The update function computes the next state of the application based on the current state and the incoming messages
    let update (msg: Msg) (state: CounterState) : CounterState =
        match msg with
        | Increment -> { state with count =  state.count + 1 }
        | Decrement -> { state with count =  state.count - 1 }
    
    // The view function returns the view of the application depending on its current state. Messages can be passed to the dispatch function.
    let view (state: CounterState) (dispatch): View =
        Views.dockpanel [
            Attrs.children [
                Views.button [
                    Attrs.dockPanel_dock Dock.Bottom
                    Attrs.onClick (fun sender args -> dispatch Decrement)
                    Attrs.content "-"
                ]
                Views.button [
                    Attrs.dockPanel_dock Dock.Bottom
                    Attrs.onClick (fun sender args -> dispatch Increment)
                    Attrs.content "+"
                ]
                Views.textBlock [
                    Attrs.dockPanel_dock Dock.Top
                    Attrs.fontSize 48.0
                    Attrs.verticalAlignment VerticalAlignment.Center
                    Attrs.horizontalAlignment HorizontalAlignment.Center
                    Attrs.text (string state.count)
                ]
            ]
        ]       
