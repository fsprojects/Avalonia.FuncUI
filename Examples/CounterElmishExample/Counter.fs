namespace CounterElmishSample

open Avalonia.Controls
open Avalonia.Media
open Avalonia.FuncUI.Types
open Avalonia.FuncUI

type CustomControl() =
    inherit Control()

    member val Background: IBrush = (SolidColorBrush.Parse("#440000") :> IBrush) with get, set

[<AutoOpen>]
module ViewExt =
    type Views with
        static member customControl (attrs: TypedAttr<CustomControl> list): View =
            Views.create<CustomControl>(attrs)

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
        Views.stackpanel [
            Attrs.orientation Orientation.Horizontal
            Attrs.children [
                Views.customControl [
                    Attrs.background (SolidColorBrush.Parse("ff0000"))
                ]
                Views.textblock [
                    Attrs.text (sprintf "the count is %i" state.count)
                ]
                Views.button [
                    Attrs.click (fun sender args -> dispatch Increment)
                    Attrs.content (
                        Views.textblock [
                            Attrs.text "click to increment"
                        ]
                    )
                ]
                Views.button [
                    Attrs.click (fun sender args -> dispatch Decrement)
                    Attrs.content (
                        Views.textblock [
                            Attrs.text "click to decrement"
                        ]
                    )
                ]
            ]
        ]       
