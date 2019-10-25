namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module StackPanel =
    open Avalonia.Controls
    open Avalonia.Layout
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
   
    let create (attrs: IAttr<StackPanel> list): IView<StackPanel> =
        ViewBuilder.Create<StackPanel>(attrs)

    type StackPanel with
           
        static member spacing<'t when 't :> StackPanel>(value: double) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<double>(StackPanel.SpacingProperty, value, ValueNone)
           
        static member orientation<'t when 't :> StackPanel>(orientation: Orientation) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<Orientation>(StackPanel.OrientationProperty, orientation, ValueNone)