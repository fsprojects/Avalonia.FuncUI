namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module ContextMenu =
    open Avalonia.Controls
    open Avalonia.FuncUI.Builder
    open Avalonia.FuncUI.Types
     
    let create (attrs: IAttr<ContextMenu> list): IView<ContextMenu> =
        ViewBuilder.Create<ContextMenu>(attrs)
     
    type ContextMenu with
        end