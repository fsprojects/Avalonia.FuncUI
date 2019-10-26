namespace Avalonia.FuncUI.ControlCatalog.Views

open Avalonia.Controls
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Builder
open Avalonia.Layout

module MainView =
    
    type CounterState = {
        dataTemplateState : DataTemplateDemo.State
    }

    let init = {
        dataTemplateState = DataTemplateDemo.init
    }

    type Msg =
    | DataTemplateDemoMsg of DataTemplateDemo.Msg 

    let update (msg: Msg) (state: CounterState) : CounterState =
        match msg with
        | DataTemplateDemoMsg msg -> { state with dataTemplateState = DataTemplateDemo.update msg state.dataTemplateState }
    
    let view (state: CounterState) (dispatch) =
        DockPanel.create [
            DockPanel.children [
               TabControl.create [
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
                           TabItem.header "WrapPanel Demo"
                           TabItem.content (ViewBuilder.Create<WrapPanelDemo.Host>([]))
                       ]
                       TabItem.create [
                           TabItem.header "ToolTip Demo"
                           TabItem.content (ViewBuilder.Create<ToolTipDemo.Host>([]))
                       ]
                   ]
               ]
               
            ]
        ]       
