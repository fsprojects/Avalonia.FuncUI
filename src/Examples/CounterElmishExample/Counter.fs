namespace CounterElmishSample

open Avalonia.Controls
open Avalonia.FuncUI.Types
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
    
    let view (state: CounterState) (dispatch): View =
        DockPanel.create [
            DockPanel.attrChildren [
                Button.create [
                    Attrs.dockPanel_dock Dock.Bottom
                    Attrs.onClick (fun sender args -> dispatch Decrement)
                    Button.attrContent "-"
                ]
                Button.create [
                    Attrs.dockPanel_dock Dock.Bottom
                    Attrs.onClick (fun sender args -> dispatch Increment)
                    Button.attrContent "+"
                ]
                TextBlock.create [
                    Attrs.dockPanel_dock Dock.Top
                    Attrs.fontSize 48.0
                    Attrs.verticalAlignment VerticalAlignment.Center
                    Attrs.horizontalAlignment HorizontalAlignment.Center
                    TextBlock.attrText (string state.count)
                ]
                Views.checkBox [
                    CheckBox.attrContent "Text"
                ]
            ]
        ]       
