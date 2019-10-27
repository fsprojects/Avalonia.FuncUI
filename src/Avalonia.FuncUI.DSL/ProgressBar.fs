namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module ProgressBar =
    open Avalonia.Layout
    open Avalonia.Controls
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
   
    let create (attrs: IAttr<ProgressBar> list): IView<ProgressBar> =
        ViewBuilder.Create<ProgressBar>(attrs)

    type RadioButton with

        static member isIndeterminate<'t when 't :> ProgressBar>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(ProgressBar.IsIndeterminateProperty , value, ValueNone)

        static member orientation<'t when 't :> ProgressBar>(value: Orientation) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<Orientation>(ProgressBar.OrientationProperty , value, ValueNone)