namespace Avalonia.FuncUI.DSL

open Avalonia
open Avalonia.Controls
open Avalonia.Controls.Primitives

[<AutoOpen>]
module AdornerLayer =
    open Avalonia.FuncUI
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder

    let create (attrs: IAttr<AdornerLayer> list): IView<AdornerLayer> =
        ViewBuilder.Create<AdornerLayer>(attrs)

    type Visual with
        static member adornedElement<'t, 'u when 't :> Visual and 'u :> Visual>(value: 'u) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<Visual>(AdornerLayer.AdornedElementProperty, value, ValueNone)

        static member adornedElement<'t, 'u when 't :> Visual and 'u :> Visual>(view: IView<'u>) : IAttr<'t> =
            let view = generalize view
            AttrBuilder<'t>.CreateContentSingle(AdornerLayer.AdornedElementProperty, Some view)

        static member isClipEnabled<'t when 't :> Visual>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(AdornerLayer.IsClipEnabledProperty, value, ValueNone)

        static member adorner<'t, 'c when 't :> Visual and 'c :> Control>(value: 'c) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<Control>(AdornerLayer.AdornerProperty, value, ValueNone)

        static member adorner<'t, 'c when 't :> Visual and 'c :> Control>(view: IView<'c>) : IAttr<'t> =
            let view = generalize view
            AttrBuilder<'t>.CreateContentSingle(AdornerLayer.AdornerProperty, Some view)

    type AdornerLayer with
        static member fefaultFocusAdorner<'t, 'c when 't :> AdornerLayer and 'c :> Control>(value: ITemplate<'c>) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<ITemplate<'c>>(AdornerLayer.DefaultFocusAdornerProperty, value, ValueNone)
