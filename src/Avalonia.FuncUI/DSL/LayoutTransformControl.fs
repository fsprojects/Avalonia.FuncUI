namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module LayoutTransformControl =
    open Avalonia.Media
    open Avalonia.Controls
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder

    let create (attrs: Attr<LayoutTransformControl> list): IView<LayoutTransformControl> =
        ViewBuilder.Create<LayoutTransformControl>(attrs)

    type LayoutTransformControl with
        static member layoutTransform<'t when 't :> LayoutTransformControl>(value: ITransform) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<ITransform>(LayoutTransformControl.LayoutTransformProperty, value, ValueNone)

        static member useRenderTransform<'t when 't :> LayoutTransformControl>(value: bool) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(LayoutTransformControl.UseRenderTransformProperty, value, ValueNone)