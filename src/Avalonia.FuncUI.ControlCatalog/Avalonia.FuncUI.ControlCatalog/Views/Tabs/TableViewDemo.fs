namespace Avalonia.FuncUI.ControlCatalog.Views

open Elmish
open Avalonia.Controls
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI
open Avalonia.FuncUI.Elmish

module TableViewDemo =
    open System.Collections.ObjectModel
    open Avalonia.Data

    type Person (name, age, male) =
        member val Name = name with get, set
        member val Age = age with get, set
        member val IsMale = male with get, set

    type State = { People: Person ObservableCollection }

    let init () =
        { People = 
            ObservableCollection [
                Person("John", 20, true)
                Person("Jane", 21, false)
                Person("Bob", 22, true)
            ]
        }

    type Msg =
    | Noop

    let update (msg: Msg) (state: State) : State =
        match msg with
        | Noop -> state
           
    let view (state: State) (dispatch) =
        DockPanel.create [
            DockPanel.children [
                TextBlock.create [
                    TextBlock.dock Dock.Top
                    TextBlock.margin 5.0
                    TextBlock.text "Table is below"
                ]
                
                TableView.create [
                    TableView.dataItems state.People
                    TableView.canUserResizeColumns true
                    TableView.columns [
                        TableViewColumn.create [
                            TableViewColumn.header "Name"
                            TableViewColumn.binding (Binding "Name")
                        ]
                        TableViewColumn.create [
                            TableViewColumn.header "Age"
                            TableViewColumn.binding (Binding "Age")
                        ]
                        TableViewColumn.create [
                            TableViewColumn.header "Is Male"
                            TableViewColumn.binding (Binding "IsMale")
                        ]
                    ]
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
        
        
        

