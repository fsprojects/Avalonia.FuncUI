namespace Avalonia.FuncUI.DSL

open Avalonia.Controls
open Avalonia.FuncUI.Types
open Avalonia.FuncUI.Builder

[<AutoOpen>]
module Window =

    let create (attrs: IAttr<Window> list) : IView<Window> =
        ViewBuilder.Create<Window>(attrs)

    type Window with

        static member canResize<'t when 't :> Window>(value) =
            AttrBuilder<'t>.CreateProperty(Window.CanResizeProperty, value, ValueNone)

        static member child(value: IView) =
            AttrBuilder<Window>.CreateContentSingle(Window.ContentProperty, Some value)

        static member closingBehavior<'t when 't :> Window>(value) =
            AttrBuilder<'t>.CreateProperty(Window.ClosingBehaviorProperty, value, ValueNone)

        static member extendClientAreaChromeHints<'t when 't :> Window>(value) =
            AttrBuilder<'t>.CreateProperty(Window.ExtendClientAreaChromeHintsProperty, value, ValueNone)

        static member extendClientAreaTitleBarHeightHint<'t when 't :> Window>(value) =
            AttrBuilder<'t>.CreateProperty(Window.ExtendClientAreaTitleBarHeightHintProperty, value, ValueNone)

        static member extendClientAreaToDecorationsHint<'t when 't :> Window>(value) =
            AttrBuilder<'t>.CreateProperty(Window.ExtendClientAreaToDecorationsHintProperty, value, ValueNone)

        static member isExtendedIntoWindowDecorations<'t when 't :> Window>(func, ?subPatchOptions) =
            AttrBuilder<'t>.CreateSubscription(Window.IsExtendedIntoWindowDecorationsProperty, func, ?subPatchOptions = subPatchOptions)

        static member icon<'t when 't :> Window>(value) =
            AttrBuilder<'t>.CreateProperty(Window.IconProperty, value, ValueNone)

        static member offScreenMargin<'t when 't :> Window>(func, ?subPatchOptions) =
            AttrBuilder<'t>.CreateSubscription(Window.OffScreenMarginProperty, func, ?subPatchOptions = subPatchOptions)

        static member showActivated<'t when 't :> Window>(value) =
            AttrBuilder<'t>.CreateProperty(Window.ShowActivatedProperty, value, ValueNone)

        static member showInTaskbar<'t when 't :> Window>(value) =
            AttrBuilder<'t>.CreateProperty(Window.ShowInTaskbarProperty, value, ValueNone)

        static member sizeToContent<'t when 't :> Window>(value) =
            AttrBuilder<'t>.CreateProperty(Window.SizeToContentProperty, value, ValueNone)

        static member systemDecorations<'t when 't :> Window>(value) =
            AttrBuilder<'t>.CreateProperty(Window.SystemDecorationsProperty, value, ValueNone)

        static member title<'t when 't :> Window>(value) =
            AttrBuilder<'t>.CreateProperty(Window.TitleProperty, value, ValueNone)

        static member windowDecorationMargin<'t when 't :> Window>(func, ?subPatchOptions) =
            AttrBuilder<'t>.CreateSubscription(Window.WindowDecorationMarginProperty, func, ?subPatchOptions = subPatchOptions)

        static member windowStartupLocation<'t when 't :> Window>(value) =
            AttrBuilder<'t>.CreateProperty(Window.WindowStartupLocationProperty, value, ValueNone)

        static member windowState<'t when 't :> Window>(value) =
            AttrBuilder<'t>.CreateProperty(Window.WindowStateProperty, value, ValueNone)
