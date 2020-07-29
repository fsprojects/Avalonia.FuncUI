namespace Avalonia.FuncUI.ControlCatalog.Views

open System
open Elmish
open Avalonia.Controls
open Avalonia.Input
open Avalonia.FuncUI
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Components
open Avalonia.FuncUI.Elmish
open Avalonia.Layout
open Avalonia.Threading

module DragDropDemo =
    type State =
        { dropText: string
          dragText: string

          // For some reason we get two onPointerPressed events when the user presses the mouse button over the drag
          // source.  This is used to ignore the second one and avoid initiating two drag events.
          dragging: bool
          dragCount: int }

    let init =
        { dropText = "Drop some text or files here"
          dragText = "Drag Me"
          dragging = false
          dragCount = 0 }, Cmd.none

    type Msg =
        | BeginDrag of PointerPressedEventArgs
        | Dragged of string
        | DragFailed of Exception
        | Dropped of string

    let doDrag (e, dragCount) =
        async {
            let dragData = DataObject()
            dragData.Set(DataFormats.Text, sprintf "You have dragged text %d times" dragCount)

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
            // If dragging has already been started, do nothing.  This works around a problem where we recieve two
            // PointerPressedEvents.
            if state.dragging then
                state, Cmd.none
            else
                let dragCount = state.dragCount + 1
                { state with
                      dragging = true
                      dragCount = dragCount }, Cmd.OfAsync.either doDrag (e, dragCount) Dragged DragFailed
        | Dragged s ->
            { state with
                  dragText = s
                  dragging = false }, Cmd.none
        | DragFailed _ -> { state with dragging = false }, Cmd.none
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
                                    Border.onPointerPressed (BeginDrag >> dispatch) ]
                                Border.create
                                    [ Border.classes [ "drop" ]
                                      Border.padding 16.0
                                      DragDrop.allowDrop true
                                      Border.child
                                          (TextBlock.create
                                              [ TextBlock.text state.dropText
                                                DragDrop.onDrop (fun e ->
                                                    if e.Data.Contains(DataFormats.Text) then
                                                        Dropped(e.Data.GetText()) |> dispatch
                                                    elif e.Data.Contains(DataFormats.FileNames) then
                                                        Dropped
                                                            (e.Data.GetFileNames() |> String.concat Environment.NewLine)
                                                        |> dispatch
                                                    )
                                                DragDrop.onDragOver (fun e ->
                                                    e.DragEffects <-
                                                        if e.Data.Contains(DataFormats.Text)
                                                           || e.Data.Contains(DataFormats.FileNames) then
                                                            e.DragEffects
                                                            &&& (DragDropEffects.Copy ||| DragDropEffects.Link)
                                                        else
                                                            DragDropEffects.None) ]) ] ] ] ] ]


    type Host() as this =
        inherit Hosts.HostControl()
        do
            this.Styles.Load "avares://Avalonia.FuncUI.ControlCatalog/Views/Tabs/Styles.xaml"

            /// we use this function because sometimes we dispatch messages
            /// from another thread
            let syncDispatch (dispatch: Dispatch<'msg>): Dispatch<'msg> =
                match Dispatcher.UIThread.CheckAccess() with
                | true -> fun msg -> Dispatcher.UIThread.Post(fun () -> dispatch msg)
                | false -> dispatch

            Elmish.Program.mkProgram (fun () -> init) update view
            |> Program.withHost this
            |> Program.withSyncDispatch syncDispatch
            |> Program.withConsoleTrace
            |> Program.run
