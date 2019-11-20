namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module ToggleButton =
    open Avalonia.Controls.Primitives
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
    
    let create (attrs: IAttr<ToggleButton> list): IView<ToggleButton> =
        ViewBuilder.Create<ToggleButton>(attrs)
     
    type ToggleButton with
        static member isThreeState<'t when 't :> ToggleButton>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(ToggleButton.IsThreeStateProperty, value, ValueNone)
            
        static member isChecked<'t when 't :> ToggleButton>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(ToggleButton.IsCheckedProperty, value, ValueNone)

       

