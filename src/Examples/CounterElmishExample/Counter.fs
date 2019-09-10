namespace CounterElmishSample

open Avalonia.Controls
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI
open Avalonia.Layout

type CustomControl() =
    inherit Control()

    member val Text: string = "" with get, set

[<AutoOpen>]
module ViewExt =
    ()

module Counter =
    open Avalonia.FuncUI.DSL
    
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
    
    let view (state: CounterState) (dispatch) =
        DockPanel.create [
            DockPanel.children [
                Button.create [
                    Button.dock Dock.Bottom
                    //Button.onClick (fun args -> dispatch Decrement)
                    Button.content "-"
                ]
                Button.create [
                    Button.dock Dock.Bottom
                    //Button.onClick (fun args -> dispatch Increment)
                    Button.content "+"
                ]
                CheckBox.create [
                    CheckBox.dock Dock.Bottom
                    CheckBox.content "Text"
                ]                
                TextBlock.create [
                    TextBlock.dock Dock.Top
                    TextBlock.fontSize 48.0
                    TextBlock.verticalAlignment VerticalAlignment.Center
                    TextBlock.horizontalAlignment HorizontalAlignment.Center
                    TextBlock.text (string state.count)
                ]
            ]
        ]       
