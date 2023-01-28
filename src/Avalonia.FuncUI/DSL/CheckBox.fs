namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module CheckBox =
    open Avalonia.Controls
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder

    let create (attrs: Attr<CheckBox> list): IView<CheckBox> =
        ViewBuilder.Create<CheckBox>(attrs)

    type CheckBox with end

