namespace Avalonia.FuncUI.ControlCatalog.Views

open System
open Elmish
open Avalonia
open Avalonia.Controls
open Avalonia.Layout
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI
open Avalonia.FuncUI.Elmish

module GridSplitterDemo =
    type State =
        { dragIncrement: int
          keyboardIncrement: int
          customPreview: bool
          resizeBehavior: GridResizeBehavior
          resizeDirection: GridResizeDirection}

    let init () =
        { dragIncrement = 1
          keyboardIncrement = 20
          customPreview = false
          resizeBehavior = GridResizeBehavior.BasedOnAlignment
          resizeDirection = GridResizeDirection.Auto}

    type Msg =
    | SetDragIncrement of int
    | SetKeyboardIncrement of int
    | SetCustomPreview of bool
    | SetGridResizeBehavior of GridResizeBehavior
    | SetGridResizeDirection of GridResizeDirection

    let update (msg: Msg) (state: State) : State =
        match msg with
        | SetDragIncrement increment -> { state with dragIncrement = increment }
        | SetKeyboardIncrement increment -> { state with keyboardIncrement = increment }
        | SetCustomPreview isChecked -> { state with customPreview = isChecked }
        | SetGridResizeBehavior behavior -> { state with resizeBehavior = behavior}
        | SetGridResizeDirection direction -> { state with resizeDirection = direction}

    let gridCellView col row text =
        Border.create [
            Grid.column col
            Grid.row row
            Border.background Media.Colors.AliceBlue
            Border.borderThickness 2
            Border.borderBrush (Media.Colors.Black.ToString())
            Border.child (
                TextBlock.create [
                    TextBlock.dock Dock.Top
                    TextBlock.fontSize 48.0
                    TextBlock.verticalAlignment VerticalAlignment.Center
                    TextBlock.horizontalAlignment HorizontalAlignment.Center
                    TextBlock.text text
                ]
                )
        ]
           
    let view (state: State) (dispatch) =
        DockPanel.create [
            DockPanel.children [
                TextBlock.create [
                    TextBlock.dock Dock.Top
                    TextBlock.margin 5.0
                    TextBlock.text "GridSplitter properties"
                    TextBlock.fontSize 24.
                ]
                StackPanel.create [
                    StackPanel.dock Dock.Top
                    StackPanel.orientation Orientation.Horizontal
                    StackPanel.spacing 16.0
                    StackPanel.children [
                        StackPanel.create [
                            StackPanel.orientation Orientation.Vertical
                            StackPanel.spacing 16.0
                            StackPanel.children [
                                DockPanel.create [
                                    DockPanel.lastChildFill false
                                    DockPanel.children [
                                        TextBlock.create [
                                            TextBlock.text "Drag increment:" 
                                            TextBlock.dock Dock.Left
                                            TextBlock.margin(0.,0.,16.,0.)
                                            TextBlock.verticalAlignment VerticalAlignment.Center
                                        ]
                                        TextBox.create [
                                            TextBox.text (string state.dragIncrement)
                                            TextBox.width 64.
                                            TextBox.dock Dock.Right
                                            TextBlock.margin(0.,0.,16.,0.)
                                            TextBox.onTextChanged (fun text ->
                                                match text |> Int32.TryParse with
                                                | true, inc -> inc |> SetDragIncrement |> dispatch
                                                | false, _ -> ()
                                            )
                                        ]
                                    ]
                                ]
                                DockPanel.create [
                                    DockPanel.lastChildFill false
                                    DockPanel.children [
                                        TextBlock.create [
                                            TextBlock.text "Keyboard increment:" 
                                            TextBlock.verticalAlignment VerticalAlignment.Center
                                            TextBlock.dock Dock.Left
                                            TextBlock.margin(0.,0.,16.,0.)
                                        ]
                                        TextBox.create [
                                            TextBox.text (string state.keyboardIncrement)
                                            TextBlock.width 64.
                                            TextBox.dock Dock.Right
                                            TextBlock.margin(0.,0.,16.,0.)
                                            TextBox.onTextChanged (fun text ->
                                                match text |> Int32.TryParse with
                                                | true, inc -> inc |> SetKeyboardIncrement |> dispatch
                                                | false, _ -> ()
                                            )
                                        ]
                                    ]
                                ]
                            ]
                        ]
                        StackPanel.create [
                            StackPanel.orientation Orientation.Vertical
                            StackPanel.spacing 16.0
                            StackPanel.children [
                                DockPanel.create [
                                    DockPanel.lastChildFill false
                                    DockPanel.children [
                                        TextBlock.create [
                                           TextBlock.verticalAlignment VerticalAlignment.Center
                                           TextBlock.text "Grid resize bahavior:" 
                                           TextBlock.dock Dock.Left
                                           TextBlock.margin(0.,0.,16.,0.)
                                        ]
                                        ComboBox.create [
                                            ComboBox.dataItems [
                                                GridResizeBehavior.BasedOnAlignment
                                                GridResizeBehavior.CurrentAndNext
                                                GridResizeBehavior.PreviousAndCurrent
                                                GridResizeBehavior.PreviousAndNext
                                            ]
                                            ComboBox.dock Dock.Right
                                            ComboBox.horizontalAlignment HorizontalAlignment.Stretch
                                            ComboBox.selectedItem state.resizeBehavior
                                            ComboBox.onSelectedItemChanged (
                                                tryUnbox >> Option.iter(SetGridResizeBehavior >> dispatch)
                                            )
                                        ]
                                    ]
                                ]
                                DockPanel.create [
                                    DockPanel.lastChildFill false
                                    DockPanel.children [
                                        TextBlock.create [
                                           TextBlock.verticalAlignment VerticalAlignment.Center
                                           TextBlock.text "Grid resize direction:" 
                                           TextBlock.margin(0.,0.,16.,0.)
                                        ]
                                        ComboBox.create [
                                            ComboBox.dataItems [
                                                GridResizeDirection.Auto
                                                GridResizeDirection.Columns
                                                GridResizeDirection.Rows
                                            ]
                                            ComboBox.dock Dock.Right
                                            ComboBox.horizontalAlignment HorizontalAlignment.Stretch
                                            ComboBox.selectedItem state.resizeDirection
                                            ComboBox.onSelectedItemChanged (
                                                tryUnbox >> Option.iter(SetGridResizeDirection >> dispatch)
                                            )
                                        ]
                                    ]
                                ]
                            ]
                        ]
                    ]
                ]
                StackPanel.create [
                    StackPanel.dock Dock.Top
                    StackPanel.orientation Orientation.Horizontal
                    StackPanel.spacing 16.0
                    StackPanel.verticalAlignment VerticalAlignment.Bottom
                    StackPanel.children [
                        ToggleSwitch.create [
                            ToggleSwitch.content "Show Preview"
                            ToggleSwitch.onContent "Enabled"
                            ToggleSwitch.offContent "Disabled"
                            ToggleSwitch.isChecked state.customPreview
                            ToggleSwitch.onIsCheckedChanged (
                                (fun _ -> SetCustomPreview (not state.customPreview) |> dispatch),
                                SubPatchOptions.OnChangeOf state)
                        ]
                    ]
                ]
                Grid.create [
                    Grid.columnDefinitions "1*,1*,1*"
                    Grid.rowDefinitions "1*,1*"
                    Grid.showGridLines true
                    Grid.margin(10., 10., 20., 20.)
                    Grid.children [
                        gridCellView 0 0 "A"
                        gridCellView 1 0 "B"
                        gridCellView 2 0 "C"
                        gridCellView 0 2 "D"
                        gridCellView 1 1 "E"
                        gridCellView 2 1 "F"
                        GridSplitter.create [
                            GridSplitter.dragIncrement state.dragIncrement
                            GridSplitter.keyboardIncrement state.keyboardIncrement
                            GridSplitter.showsPreview state.customPreview
                            GridSplitter.resizebehavior state.resizeBehavior
                            GridSplitter.resizeDirection state.resizeDirection
                            Grid.column 1
                            Grid.rowSpan 2
                            Grid.horizontalAlignment HorizontalAlignment.Left
                            Grid.verticalAlignment VerticalAlignment.Stretch
                            Grid.width 5.0
                        ]
                        GridSplitter.create [
                            GridSplitter.dragIncrement state.dragIncrement
                            GridSplitter.keyboardIncrement state.keyboardIncrement
                            GridSplitter.showsPreview state.customPreview
                            GridSplitter.resizebehavior state.resizeBehavior
                            GridSplitter.resizeDirection state.resizeDirection
                            Grid.column 2
                            Grid.rowSpan 2
                            Grid.horizontalAlignment HorizontalAlignment.Left
                            Grid.verticalAlignment VerticalAlignment.Stretch
                            Grid.width 5.0
                        ]
                        GridSplitter.create [
                            GridSplitter.dragIncrement state.dragIncrement
                            GridSplitter.keyboardIncrement state.keyboardIncrement
                            GridSplitter.showsPreview state.customPreview
                            GridSplitter.resizebehavior state.resizeBehavior
                            GridSplitter.resizeDirection state.resizeDirection
                            Grid.columnSpan 3
                            Grid.row 1
                            Grid.horizontalAlignment HorizontalAlignment.Stretch
                            Grid.verticalAlignment VerticalAlignment.Top
                            Grid.height 5.0
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
        
