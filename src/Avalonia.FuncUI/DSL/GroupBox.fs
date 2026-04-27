namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module GroupBox =
    open Avalonia.FuncUI.Types
    open Avalonia.Controls

    let create (attrs: IAttr<GroupBox> list): IView<GroupBox> =
        ViewBuilder.Create<GroupBox>(attrs)
