namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module ProgressBar =
    open Avalonia.Layout
    open Avalonia.Controls
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
   
    let create (attrs: IAttr<ProgressBar> list): IView<ProgressBar> =
        ViewBuilder.Create<ProgressBar>(attrs)

    type ProgressBar with

        static member isIndeterminate<'t when 't :> ProgressBar>(value: bool) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, bool>(ProgressBar.IsIndeterminateProperty , value, ValueNone)

        static member orientation<'t when 't :> ProgressBar>(value: Orientation) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, Orientation>(ProgressBar.OrientationProperty , value, ValueNone)