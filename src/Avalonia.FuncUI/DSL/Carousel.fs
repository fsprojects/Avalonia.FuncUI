namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module Carousel =
    open Avalonia.Animation
    open Avalonia.Controls
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder

    let create (attrs: Attr<Carousel> list): View<Carousel> =
        ViewBuilder.Create<Carousel>(attrs)

    type Carousel with

        static member isVirtualized<'t when 't :> Carousel>(value: bool) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(Carousel.IsVirtualizedProperty, value, ValueNone)

        static member pageTransition<'t when 't :> Carousel>(transition: IPageTransition) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<IPageTransition>(Carousel.PageTransitionProperty, transition, ValueNone)