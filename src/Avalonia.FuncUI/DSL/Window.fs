namespace Avalonia.FuncUI.DSL

open Avalonia.Controls
open Avalonia.FuncUI.Types
open Avalonia.FuncUI.Builder
open Avalonia.Platform

[<AutoOpen>]
module Window =

    let create (attrs: IAttr<Window> list) : IView<Window> =
        ViewBuilder.Create<Window>(attrs)

    type Window with

        static member canResize<'t when 't :> Window>(value) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty(Window.CanResizeProperty, value, ValueNone)

        static member child(value: IView) : IAttr<Window> =
            AttrBuilder<Window>.CreateContentSingle(Window.ContentProperty, Some value)

        static member closingBehavior<'t when 't :> Window>(value) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty(Window.ClosingBehaviorProperty, value, ValueNone)

        static member extendClientAreaChromeHints<'t when 't :> Window>(value) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty(Window.ExtendClientAreaChromeHintsProperty, value, ValueNone)

        static member extendClientAreaTitleBarHeightHint<'t when 't :> Window>(value) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty(Window.ExtendClientAreaTitleBarHeightHintProperty, value, ValueNone)

        static member extendClientAreaToDecorationsHint<'t when 't :> Window>(value) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty(Window.ExtendClientAreaToDecorationsHintProperty, value, ValueNone)

        static member isExtendedIntoWindowDecorationsProperty<'t when 't :> Window>(func, ?subPatchOptions) : IAttr<'t> =
            AttrBuilder<'t>.CreateSubscription(Window.IsExtendedIntoWindowDecorationsProperty, func, ?subPatchOptions = subPatchOptions)

        static member icon<'t when 't :> Window>(value) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty(Window.IconProperty, value, ValueNone)

        static member sizeToContent<'t when 't :> Window>(value) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty(Window.SizeToContentProperty, value, ValueNone)

        static member title<'t when 't :> Window>(value) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty(Window.TitleProperty, value, ValueNone)

        static member windowState<'t when 't :> Window>(value) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty(Window.WindowStateProperty, value, ValueNone)
