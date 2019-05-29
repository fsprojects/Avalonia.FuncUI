namespace TodoElmishSample

open Avalonia.FuncUI.Core
open Avalonia.FuncUI.Builders
open Avalonia.Controls
open Avalonia.FuncUI.Core.Model
open Avalonia.Media
open System
open Avalonia

[<RequireQualifiedAccess>]
module TodoItem =

    type State = {
        Id : Guid
        Name : string
        Done : bool
    }

    let init() = {
        Id = Guid.NewGuid()
        Name = "new item"
        Done = false
    }

    let view (state: State) dispatch =
        dockpanel {
            margin (Thickness(5.0))
            children [
                textblock {
                    text state.Name
                    fontSize 20.0
                }
            ]
        }


[<RequireQualifiedAccess>]
module TodoItems =

    type State = TodoItem.State list

    let init = [
        TodoItem.init();
        TodoItem.init();
        TodoItem.init();
    ]

    type Msg =
    | Delete of Guid
    | Rename of Guid * string
    | SetDone of Guid * bool

    let update (msg: Msg) (state: State) : State =
        match msg with
        | Delete guid -> 
            state
            |> List.filter (fun item -> item.Id <> guid)
        | Rename (guid, name) ->
            state
            |> List.map (fun item -> 
                if item.Id = guid then
                    { item with Name = name }
                else item
            )
        | SetDone (guid, isDone) ->
            state
            |> List.map (fun item -> 
                if item.Id = guid then
                    { item with Done = isDone }
                else item
            )

    let view (state: State) dispatch =
        stackpanel {
            margin (Thickness(5.0))
            dockpanel_dock Dock.Top
            orientation Orientation.Vertical
            children [
                for item in state do
                    yield TodoItem.view item dispatch
            ]
        }

module Todo =

    type State = {
        Items : TodoItems.State
        NewItem : string
    }

    let init = {
        Items = TodoItems.init
        NewItem = ""
    }

    type Msg =
    | TodoItems of TodoItems.Msg
    | AddNewItem of string

    let update (msg: Msg) (state: State) : State =
        match msg with
        | TodoItems todos ->
            { state with Items = TodoItems.update todos state.Items }
        | AddNewItem name ->
            let newItem = {
               TodoItem.Id = Guid.NewGuid()
               TodoItem.Name = name
               TodoItem.Done = false
            }
            { state with Items = newItem :: state.Items ; NewItem = "" }

    let view (state: State) (dispatch): ViewElement =
        dockpanel {
            lastChildFill true
            children [
                dockpanel {
                    dockpanel_dock Dock.Bottom
                    children [
                        button {
                            dockpanel_dock Dock.Right
                            contentView (textblock {
                                text "Add"
                            })
                        }
                        textbox {
                            font 20.0
                            pass
                        }
                    ]
                }
                TodoItems.view state.Items dispatch 
            ]
        }
