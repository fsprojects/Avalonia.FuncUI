namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module Menu =
    open Avalonia.Controls
    open Avalonia.FuncUI.Builder
    open Avalonia.FuncUI.Types

    let create (attrs: Attr<Menu> list): IView<Menu> =
        ViewBuilder.Create<Menu>(attrs)

    type Menu with
        end