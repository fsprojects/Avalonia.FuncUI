namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module RadioButton =
    open Avalonia.Controls
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
   
    let create (attrs: IAttr<RadioButton> list): IView<RadioButton> =
        ViewBuilder.Create<RadioButton>(attrs)

    type RadioButton with

        /// <summary>
        /// 
        /// </summary>
        static member groupName<'t when 't :> RadioButton>(value: string) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<string>(RadioButton.GroupNameProperty, value, ValueNone)
        