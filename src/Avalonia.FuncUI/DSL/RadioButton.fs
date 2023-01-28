namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module RadioButton =
    open Avalonia.Controls
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder

    let create (attrs: Attr<RadioButton> list): IView<RadioButton> =
        ViewBuilder.Create<RadioButton>(attrs)

    type RadioButton with

        static member groupName<'t when 't :> RadioButton>(value: string) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<string>(RadioButton.GroupNameProperty, value, ValueNone)
