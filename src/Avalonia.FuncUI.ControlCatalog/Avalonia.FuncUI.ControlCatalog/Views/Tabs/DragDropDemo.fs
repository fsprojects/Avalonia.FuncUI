namespace Avalonia.FuncUI.ControlCatalog.Views

open System
open Elmish
open Avalonia.Controls
open Avalonia.Input
open Avalonia.FuncUI
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Elmish
open Avalonia.Layout
open Avalonia.Threading

module DragDropDemo =
    type State =
        { dropText: string
          dragText: string
          dragCount: int }

    let init =
        { dropText = "Drop some text or files here"
          dragText = "Drag Me"
          dragCount = 0 }, Cmd.none

    type Msg =
        | BeginDrag of PointerPressedEventArgs
        | Dragged of string
        | Dropped of string

    let doDrag (e, dragCount) =
        async {
            let dragData = DataObject()
            dragData.Set(DataFormats.Text, $"You have dragged text %d{dragCount} times")

            let! result = Dispatcher.UIThread.InvokeAsync<DragDropEffects>
                              (fun _ -> DragDrop.DoDragDrop(e, dragData, DragDropEffects.Copy)) |> Async.AwaitTask
            return match result with
                   | DragDropEffects.Copy -> "The text was copied"
                   | DragDropEffects.Link -> "The text was linked"
                   | DragDropEffects.None -> "The drag operation was canceled"
                   | _ -> "That was unexpected"
        }

    let update (msg: Msg) (state: State): State * Cmd<_> =
        match msg with
        | BeginDrag e ->
            let dragCount = state.dragCount + 1
            { state with dragCount = dragCount }, Cmd.OfAsync.perform doDrag (e, dragCount) Dragged
        | Dragged s ->
            { state with dragText = s }, Cmd.none
        | Dropped s -> { state with dropText = s }, Cmd.none

    let view (state: State) (dispatch) =
        StackPanel.create
            [ StackPanel.orientation Orientation.Vertical
              StackPanel.spacing 4.0
              StackPanel.children
                  [ TextBlock.create
                      [ TextBlock.classes [ "h1" ]
                        TextBlock.text "Drag+Drop" ]
                    TextBlock.create
                        [ TextBlock.classes [ "h2" ]
                          TextBlock.text "Example of Drag+Drop capabilities" ]
                    StackPanel.create
                        [ StackPanel.orientation Orientation.Horizontal
                          StackPanel.margin (0.0, 16.0, 0.0, 0.0)
                          StackPanel.horizontalAlignment HorizontalAlignment.Center
                          StackPanel.spacing 16.0
                          StackPanel.children
                              [ Border.create
                                  [ Border.classes [ "drag" ]
                                    Border.borderThickness 2.0
                                    Border.padding 16.0
                                    Border.child (TextBlock.create [ TextBlock.text state.dragText ])
                                    Border.onPointerPressed (fun e ->
                                                             // Set Handled to true, otherwise this will fire twice
                                                             e.Handled <- true
                                                             BeginDrag e |> dispatch) ]
                                Border.create
                                    [ Border.classes [ "drop" ]
                                      Border.padding 16.0
                                      Control.allowDrop true
                                      Border.child
                                          (TextBlock.create
                                              [ TextBlock.text state.dropText
                                                Control.onDrop (fun e ->
                                                    if e.Data.Contains(DataFormats.Text) then
                                                        Dropped(e.Data.GetText()) |> dispatch
                                                    elif e.Data.Contains(DataFormats.Files) then
                                                        Dropped
                                                            (e.Data.GetFiles()
                                                            |> Seq.map (fun item -> item.Name)
                                                            |> String.concat Environment.NewLine)
                                                        |> dispatch
                                                    )
                                                Control.onDragOver (fun e ->
                                                    e.DragEffects <-
                                                        if e.Data.Contains(DataFormats.Text)
                                                           || e.Data.Contains(DataFormats.Files) then
                                                            e.DragEffects
                                                            &&& (DragDropEffects.Copy ||| DragDropEffects.Link)
                                                        else
                                                            DragDropEffects.None) ]) ] ] ] ] ]


    type Host() as this =
        inherit Hosts.HostControl()
        do
            this.Styles.Load "avares://Avalonia.FuncUI.ControlCatalog/Views/Tabs/Styles.xaml"

            Elmish.Program.mkProgram (fun () -> init) update view
            |> Program.withHost this
            |> Program.withConsoleTrace
            |> Program.runWithAvaloniaSyncDispatch ()
