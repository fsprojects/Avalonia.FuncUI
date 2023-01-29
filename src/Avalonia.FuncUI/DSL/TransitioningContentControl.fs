namespace Avalonia.FuncUI.DSL

open Avalonia.Animation

[<AutoOpen>]
module TransitioningContentControl =
    open Avalonia.Controls
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder

    let create (attrs : Attr<TransitioningContentControl> list) : View<TransitioningContentControl> =
        ViewBuilder.Create<TransitioningContentControl>(attrs)

    type TransitioningContentControl with

        static member currentContent<'t when 't :> TransitioningContentControl>(value: obj) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<obj>(TransitioningContentControl.CurrentContentProperty, value, ValueNone)

        static member pageTransition<'t when 't :> TransitioningContentControl>(transition: IPageTransition) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<IPageTransition>(TransitioningContentControl.PageTransitionProperty, transition, ValueNone)