namespace Avalonia.FuncUI.DSL

open Avalonia.Animation

[<AutoOpen>]
module TransitioningContentControl =
    open Avalonia.Controls
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder

    let create (attrs : IAttr<TransitioningContentControl> list) : IView<TransitioningContentControl> =
        ViewBuilder.Create<TransitioningContentControl>(attrs)

    type TransitioningContentControl with

        static member pageTransition<'t when 't :> TransitioningContentControl>(transition: IPageTransition) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<IPageTransition>(TransitioningContentControl.PageTransitionProperty, transition, ValueNone)