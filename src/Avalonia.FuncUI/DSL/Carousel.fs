namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module Carousel =
    open Avalonia.Animation
    open Avalonia.Controls
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
    
    let create (attrs: IAttr<Carousel> list): IView<Carousel> =
        ViewBuilder.Create<Carousel>(attrs)
     
    type Carousel with

        static member pageTransition<'t when 't :> Carousel>(transition: IPageTransition) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<IPageTransition>(Carousel.PageTransitionProperty, transition, ValueNone)