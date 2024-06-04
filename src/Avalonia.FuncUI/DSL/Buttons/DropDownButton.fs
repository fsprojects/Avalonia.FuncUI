namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module DropDownButton =
    open Avalonia.Controls
    open Avalonia.FuncUI.Types

    let create (attrs: IAttr<DropDownButton> list): IView<DropDownButton> =
        ViewBuilder.Create<DropDownButton>(attrs)

    type DropDownButton with
        end
