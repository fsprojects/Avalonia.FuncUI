namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module ContextMenu =
    open Avalonia.Controls
    open Avalonia.FuncUI.Builder
    open Avalonia.FuncUI.Types

    let create (attrs: Attr<ContextMenu> list): View<ContextMenu> =
        ViewBuilder.Create<ContextMenu>(attrs)

    type ContextMenu with
        end