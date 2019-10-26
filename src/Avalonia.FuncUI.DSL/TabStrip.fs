namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module TabStrip =
    open Avalonia.Controls.Primitives
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
    
    let create (attrs: IAttr<TabStrip> list): IView<TabStrip> =
        ViewBuilder.Create<TabStrip>(attrs)
    
    type TabStrip with
        end