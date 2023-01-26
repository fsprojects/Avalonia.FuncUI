namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module LayoutTransformControl =
    open Avalonia.Media
    open Avalonia.Controls
    open Avalonia.FuncUI
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
   
    let create (attrs: IAttr<LayoutTransformControl> list): IView<LayoutTransformControl> =
        ViewBuilder.Create<LayoutTransformControl>(attrs)

    type LayoutTransformControl with
        static member layoutTransform<'t when 't :> LayoutTransformControl>(value: ITransform) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<ITransform>(LayoutTransformControl.LayoutTransformProperty, value, ValueSome EqualityComparers.compareTransforms)
            
        static member useRenderTransform<'t when 't :> LayoutTransformControl>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(LayoutTransformControl.UseRenderTransformProperty, value, ValueNone)