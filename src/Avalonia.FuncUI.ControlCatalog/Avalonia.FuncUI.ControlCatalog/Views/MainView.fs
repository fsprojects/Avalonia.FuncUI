namespace Avalonia.FuncUI.ControlCatalog.Views

open Avalonia.Controls
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Builder

module MainView =

    type CounterState = {
        dataTemplateState : DataTemplateDemo.State
    }

    let init () = {
        dataTemplateState = DataTemplateDemo.init()
    }

    type Msg =
    | DataTemplateDemoMsg of DataTemplateDemo.Msg

    let update (msg: Msg) (state: CounterState) : CounterState =
        match msg with
        | DataTemplateDemoMsg msg -> { state with dataTemplateState = DataTemplateDemo.update msg state.dataTemplateState }

    let view (state: CounterState) dispatch =
        TabControl.create [
            TabControl.tabStripPlacement Dock.Left
            TabControl.viewItems [
                TabItem.create [
                    TabItem.header "Data Template Demo"
                    TabItem.content (DataTemplateDemo.view state.dataTemplateState (DataTemplateDemoMsg >> dispatch))
                ]
                TabItem.create [
                    TabItem.header "Grid Demo"
                    TabItem.content (ViewBuilder.Create<GridDemo.Host>([]))
                ]
                TabItem.create [
                    TabItem.header "Gridsplitter Demo"
                    TabItem.content (ViewBuilder.Create<GridSplitterDemo.Host>([]))
                ]
                TabItem.create [
                    TabItem.header "WrapPanel Demo"
                    TabItem.content (ViewBuilder.Create<WrapPanelDemo.Host>([]))
                ]
                TabItem.create [
                    TabItem.header "ToolTip Demo"
                    TabItem.content (ViewBuilder.Create<ToolTipDemo.Host>([]))
                ]
                TabItem.create [
                    TabItem.header "TreeView Demo"
                    TabItem.content (ViewBuilder.Create<TreeViewDemo.Host>([]))
                ]
                TabItem.create [
                    TabItem.header "NumericUpDown Demo"
                    TabItem.content (ViewBuilder.Create<NumericUpDownDemo.Host>([]))
                ]
                TabItem.create [
                    TabItem.header "Spinner Demo"
                    TabItem.content (ViewBuilder.Create<SpinnerDemo.Host>([]))
                ]
                TabItem.create [
                    TabItem.header "Canvas Demo"
                    TabItem.content (ViewBuilder.Create<CanvasDemo.Host>([]))
                ]
                TabItem.create [
                    TabItem.header "Context Menu Demo"
                    TabItem.content (ViewBuilder.Create<ContextMenuDemo.Host>([]))
                ]
                TabItem.create [
                    TabItem.header "Window Menu Demo"
                    TabItem.content (ViewBuilder.Create<WindowMenuDemo.Host>([]))
                ]
                // ToDo: return it back when styles will be worked
                //TabItem.create [
                //    TabItem.header "Styles Demo"
                //    TabItem.content (ViewBuilder.Create<StylesDemo.Host>([]))
                //]
                TabItem.create [
                    TabItem.header "TextBox Demo"
                    TabItem.content (ViewBuilder.Create<TextBoxDemo.Host>([]))
                ]
                TabItem.create [
                    TabItem.header "CalendarDatePicker Demo"
                    TabItem.content (ViewBuilder.Create<CalendarDatePickerDemo.Host>([]))
                ]
                TabItem.create [
                    TabItem.header "Grid patch Demo"
                    TabItem.content (ViewBuilder.Create<GridPatchDemo.Host>([]))
                ]
                TabItem.create [
                    TabItem.header "ToggleSwitch Demo"
                    TabItem.content (ViewBuilder.Create<ToggleSwitchDemo.Host>([]))
                ]
                TabItem.create [
                    TabItem.header "DatePicker Demo"
                    TabItem.content (ViewBuilder.Create<DatePickerDemo.Host>([]))
                ]
                TabItem.create [
                    TabItem.header "TimePicker Demo"
                    TabItem.content (ViewBuilder.Create<TimePickerDemo.Host>([]))
                ]
                TabItem.create [
                    TabItem.header "SplitView Demo"
                    TabItem.content (ViewBuilder.Create<SplitViewDemo.Host>([]))
                ]
                TabItem.create [
                    TabItem.header "TickBar Demo"
                    TabItem.content (ViewBuilder.Create<TickBarDemo.Host>([]))
                ]
                TabItem.create [
                    TabItem.header "Flyout Demo"
                    TabItem.content (ViewBuilder.Create<FlyoutDemo.Host>([]))
                ]
                TabItem.create [
                    TabItem.header "Drag+Drop Demo"
                    TabItem.content (ViewBuilder.Create<DragDropDemo.Host>([]))
                ]
            ]
        ]
