namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module Separator =
    open Avalonia.Controls
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder

    let create (attrs: Attr<Separator> list): View<Separator> =
        ViewBuilder.Create<Separator>(attrs)

    type Separator with
        end