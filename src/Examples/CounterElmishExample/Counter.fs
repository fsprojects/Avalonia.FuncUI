namespace CounterElmishSample

open System
open Avalonia.Controls
open Avalonia.Controls
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Components
open Avalonia.FuncUI.Components
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Types
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
        numbers: int list
    }

    let init = {
        count = 0
        numbers = [0 .. 100_000]
    }

    type Msg =
    | Increment
    | Decrement
    | Specific of int
    | RemoveNumber of int

    let update (msg: Msg) (state: CounterState) : CounterState =
        match msg with
        | Increment -> { state with count = state.count + 1 }
        | Decrement -> { state with count = state.count - 1 }
        | Specific number -> { state with count = number }
        | RemoveNumber number -> { state with numbers = List.except [number] state.numbers }
    
    let view (state: CounterState) (dispatch) =
        DockPanel.create [
            DockPanel.children [
                (*
                Button.create [
                    Button.dock Dock.Bottom
                    Button.onClick (fun args -> dispatch Decrement)
                    Button.content "-"
                ]
                Button.create [
                    Button.dock Dock.Bottom
                    Button.onClickRouted (fun args -> dispatch Increment)
                    Button.content "+"
                ]
                *)
                ListBox.create [
                    ListBox.items state.numbers
                    ListBox.itemTemplate (
                        DataTemplateView.create(fun data ->
                            Button.create [
                                Button.content (sprintf "%i" data)
                                Button.onClick (fun args -> dispatch (Msg.RemoveNumber data))
                            ]                                                                   
                        )                  
                    )
                ]
                (*
                TextBox.create [
                    TextBox.dock Dock.Bottom
                    TextBox.text (sprintf "%i" state.count)
                    TextBox.onTextChanged (fun text ->
                        printfn "new Text: %s" text
                     )
                ]
                TextBlock.create [
                    TextBlock.dock Dock.Top
                    TextBlock.fontSize 48.0
                    TextBlock.foreground "blue"
                    TextBlock.verticalAlignment VerticalAlignment.Center
                    TextBlock.horizontalAlignment HorizontalAlignment.Center
                    TextBlock.text (string state.count)
                ]
                LazyView.create [
                    LazyView.args dispatch
                    LazyView.state state.count
                    LazyView.viewFunc (fun state dispatch ->
                        let view = 
                            TextBlock.create [
                                TextBlock.dock Dock.Top
                                TextBlock.fontSize 48.0
                                TextBlock.foreground "green"
                                TextBlock.verticalAlignment VerticalAlignment.Center
                                TextBlock.horizontalAlignment HorizontalAlignment.Center
                                TextBlock.text (string state)
                            ]
                            
                        view |> fun a -> a :> IView
                    )
                ]
                *)
            ]
        ]       
