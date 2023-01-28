namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module TabStripItem =
    open Avalonia.Controls.Primitives
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder

    let create (attrs: Attr<TabStripItem> list): View<TabStripItem> =
        ViewBuilder.Create<TabStripItem>(attrs)

    type TabStripItem with
        end