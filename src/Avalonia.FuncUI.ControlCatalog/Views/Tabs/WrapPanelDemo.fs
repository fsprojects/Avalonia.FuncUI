namespace Avalonia.FuncUI.ControlCatalog.Views

open System
open Avalonia.Controls
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Components
open Avalonia.FuncUI.Elmish
open Elmish
open Avalonia.Layout
open Avalonia.Media

module WrapPanelDemo =
    type State =
        { itemWidth: float
          itemHeight: float
          orientation: Orientation }

    let init =
        { itemWidth = 100.0
          itemHeight = 200.0
          orientation = Orientation.Horizontal}

    type Msg =
    | SetItemWidth of float
    | SetItemHeight of float
    | FlipOrientation

    let update (msg: Msg) (state: State) : State =
        match msg with
        | SetItemWidth width -> { state with itemWidth = width }
        | SetItemHeight height -> { state with itemHeight = height }
        | FlipOrientation -> { state with orientation = match state.orientation with
                                                        | Orientation.Horizontal -> Orientation.Vertical
                                                        | _ -> Orientation.Horizontal }
           
    let view (state: State) (dispatch) =
        DockPanel.create [
            DockPanel.children [
                TextBlock.create [
                    TextBlock.dock Dock.Top
                    TextBlock.margin 5.0
                    TextBlock.text "The WrapPanel is below"
                ]
                TextBox.create [
                    TextBlock.dock Dock.Top
                    TextBox.text (string state.itemHeight)
                    TextBox.onTextChanged (fun text ->
                        try
                            text |> Double.Parse |> SetItemHeight |> dispatch
                        with ex -> Console.WriteLine(ex)
                    )
                ]
                TextBox.create [
                    TextBlock.dock Dock.Top
                    TextBox.text (string state.itemWidth)
                    TextBox.onTextChanged (fun text ->
                        try
                            text |> Double.Parse |> SetItemWidth |> dispatch
                        with ex -> Console.WriteLine(ex) 
                    )
                ]
                CheckBox.create[
                    CheckBox.dock Dock.Top
                    CheckBox.content "Is Horizontal"
                    CheckBox.onClick (fun e -> dispatch FlipOrientation)
                ]
                WrapPanel.create [
                    WrapPanel.itemHeight state.itemHeight
                    WrapPanel.itemWidth state.itemWidth
                    WrapPanel.orientation state.orientation
                    WrapPanel.children [
                        Border.create [
                            Border.background Brushes.Green
                        ]
                        Border.create [
                            Border.background Brushes.Red
                        ]
                        Border.create [
                            Border.background Brushes.Blue
                        ]
                        Border.create [
                            Border.background Brushes.Yellow
                        ]
                        Border.create [
                            Border.background Brushes.Purple
                        ]
                    ]
                ]
            ]
        ]
        
    type Host() as this =
        inherit Hosts.HostControl()
        do
            Elmish.Program.mkSimple (fun () -> init) update view
            |> Program.withHost this
            |> Program.withConsoleTrace
            |> Program.run
        
        
        

