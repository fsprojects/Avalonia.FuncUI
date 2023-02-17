namespace Avalonia.FuncUI.ControlCatalog.Views

open Elmish
open Avalonia.Controls
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI
open Avalonia.FuncUI.Elmish

module TreeViewDemo =

    type Taxonomy = {
        Name: string;
        Children: Taxonomy seq
    }

    let food = { Name = "Food"
                 Children = [
                    { Name = "Fruit"
                      Children = [
                        {Name = "Tomato"; Children = []}
                        {Name = "Apple"; Children = []}
                      ]}
                    { Name = "Vegetables"
                      Children = [
                        {Name = "Carrot"; Children = []}
                        {Name = "Salad"; Children = []}
                      ]}
                 ]}

    type State =
        { noop : bool }

    let init () =
        { noop = false }

    type Msg =
    | Noop

    let update (msg: Msg) (state: State) : State =
        match msg with
        | Noop -> state
           
    let view (_state: State) (_dispatch) =
        DockPanel.create [
            DockPanel.children [
                TreeView.create [
                    TreeView.dataItems [food]
                    TreeView.itemTemplate (DataTemplateView<Taxonomy>.create((fun data -> data.Children), (fun data -> 
                        TextBlock.create [
                            TextBlock.text data.Name
                        ]
                    ))) 
                ]
            ]
        ]
        
    type Host() as this =
        inherit Hosts.HostControl()
        do
            Elmish.Program.mkSimple init update view
            |> Program.withHost this
            |> Program.withConsoleTrace
            |> Program.run