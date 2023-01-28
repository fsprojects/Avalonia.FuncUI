namespace Avalonia.FuncUI.DSL
open Avalonia.Interactivity

[<AutoOpen>]
module InputElement =
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
    open Avalonia.Input

    type InputElement with
        static member focusable<'t when 't :> InputElement>(value: bool) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(InputElement.FocusableProperty, value, ValueNone)

        static member isEnabled<'t when 't :> InputElement>(value: bool) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(InputElement.IsEnabledProperty, value, ValueNone)

        static member cursor<'t when 't :> InputElement>(cursor: Cursor) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<Cursor>(InputElement.CursorProperty, cursor, ValueNone)

        static member isHitTestVisible<'t when 't :> InputElement>(value: bool) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(InputElement.IsHitTestVisibleProperty, value, ValueNone)

        static member onGotFocus<'t when 't :> InputElement>(func: GotFocusEventArgs -> unit, ?subPatchOptions) =
            AttrBuilder<'t>.CreateSubscription<GotFocusEventArgs>(InputElement.GotFocusEvent, func, ?subPatchOptions = subPatchOptions)

        static member onLostFocus<'t when 't :> InputElement>(func: RoutedEventArgs -> unit, ?subPatchOptions) =
            AttrBuilder<'t>.CreateSubscription<RoutedEventArgs>(InputElement.LostFocusEvent, func, ?subPatchOptions = subPatchOptions)

        static member onKeyDown<'t when 't :> InputElement>(func: KeyEventArgs -> unit, ?subPatchOptions) =
            AttrBuilder<'t>.CreateSubscription<KeyEventArgs>(InputElement.KeyDownEvent, func, ?subPatchOptions = subPatchOptions)

        static member onKeyUp<'t when 't :> InputElement>(func: KeyEventArgs -> unit, ?subPatchOptions) =
            AttrBuilder<'t>.CreateSubscription<KeyEventArgs>(InputElement.KeyUpEvent, func, ?subPatchOptions = subPatchOptions)

        static member onTextInput<'t when 't :> InputElement>(func: TextInputEventArgs -> unit, ?subPatchOptions) =
            AttrBuilder<'t>.CreateSubscription<TextInputEventArgs>(InputElement.TextInputEvent, func, ?subPatchOptions = subPatchOptions)

        static member onPointerEntered<'t when 't :> InputElement>(func: PointerEventArgs -> unit, ?subPatchOptions) =
            AttrBuilder<'t>.CreateSubscription<PointerEventArgs>(InputElement.PointerEnteredEvent, func, ?subPatchOptions = subPatchOptions)

        static member onPointerExited<'t when 't :> InputElement>(func: PointerEventArgs -> unit, ?subPatchOptions) =
            AttrBuilder<'t>.CreateSubscription<PointerEventArgs>(InputElement.PointerExitedEvent, func, ?subPatchOptions = subPatchOptions)

        static member onPointerMoved<'t when 't :> InputElement>(func: PointerEventArgs -> unit, ?subPatchOptions) =
            AttrBuilder<'t>.CreateSubscription<PointerEventArgs>(InputElement.PointerMovedEvent, func, ?subPatchOptions = subPatchOptions)

        static member onPointerPressed<'t when 't :> InputElement>(func: PointerPressedEventArgs -> unit, ?subPatchOptions) =
            AttrBuilder<'t>.CreateSubscription<PointerPressedEventArgs>(InputElement.PointerPressedEvent, func, ?subPatchOptions = subPatchOptions)

        static member onPointerReleased<'t when 't :> InputElement>(func: PointerReleasedEventArgs -> unit, ?subPatchOptions) =
            AttrBuilder<'t>.CreateSubscription<PointerReleasedEventArgs>(InputElement.PointerReleasedEvent, func, ?subPatchOptions = subPatchOptions)

        static member onPointerCaptureLost<'t when 't :> InputElement>(func: PointerCaptureLostEventArgs -> unit, ?subPatchOptions) =
            AttrBuilder<'t>.CreateSubscription<PointerCaptureLostEventArgs>(InputElement.PointerCaptureLostEvent, func, ?subPatchOptions = subPatchOptions)

        static member onPointerWheelChanged<'t when 't :> InputElement>(func: PointerWheelEventArgs -> unit, ?subPatchOptions) =
            AttrBuilder<'t>.CreateSubscription<PointerWheelEventArgs>(InputElement.PointerWheelChangedEvent, func, ?subPatchOptions = subPatchOptions)

        static member onTapped<'t when 't :> InputElement>(func: TappedEventArgs -> unit, ?subPatchOptions) =
            AttrBuilder<'t>.CreateSubscription<TappedEventArgs>(InputElement.TappedEvent, func, ?subPatchOptions = subPatchOptions)

        static member onDoubleTapped<'t when 't :> InputElement>(func: TappedEventArgs -> unit, ?subPatchOptions) =
            AttrBuilder<'t>.CreateSubscription<TappedEventArgs>(InputElement.DoubleTappedEvent, func, ?subPatchOptions = subPatchOptions)