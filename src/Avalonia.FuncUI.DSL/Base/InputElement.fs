namespace Avalonia.FuncUI.DSL
open Avalonia.Interactivity

[<AutoOpen>]
module InputElement =  
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
    open Avalonia.Input

    type InputElement with
        static member focusable<'t when 't :> InputElement>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(InputElement.FocusableProperty, value, ValueNone)
            
        static member isEnabled<'t when 't :> InputElement>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(InputElement.IsEnabledProperty, value, ValueNone)

        static member cursor<'t when 't :> InputElement>(cursor: Cursor) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<Cursor>(InputElement.CursorProperty, cursor, ValueNone)
      
        static member isHitTestVisible<'t when 't :> InputElement>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(InputElement.IsHitTestVisibleProperty, value, ValueNone)
            
        static member onGotFocus<'t when 't :> InputElement>(func: GotFocusEventArgs -> unit, subPatchOptions) =
            AttrBuilder<'t>.CreateSubscription<GotFocusEventArgs>(InputElement.GotFocusEvent, func, subPatchOptions)
            
        static member onLostFocus<'t when 't :> InputElement>(func: RoutedEventArgs -> unit, subPatchOptions) =
            AttrBuilder<'t>.CreateSubscription<RoutedEventArgs>(InputElement.LostFocusEvent, func, subPatchOptions)
            
        static member onKeyDown<'t when 't :> InputElement>(func: KeyEventArgs -> unit, subPatchOptions) =
            AttrBuilder<'t>.CreateSubscription<KeyEventArgs>(InputElement.KeyDownEvent, func, subPatchOptions)
            
        static member onKeyUp<'t when 't :> InputElement>(func: KeyEventArgs -> unit, subPatchOptions) =
            AttrBuilder<'t>.CreateSubscription<KeyEventArgs>(InputElement.KeyUpEvent, func, subPatchOptions)
            
        static member onTextInput<'t when 't :> InputElement>(func: TextInputEventArgs -> unit, subPatchOptions) =
            AttrBuilder<'t>.CreateSubscription<TextInputEventArgs>(InputElement.TextInputEvent, func, subPatchOptions)
            
        static member onPointerEnter<'t when 't :> InputElement>(func: PointerEventArgs -> unit, subPatchOptions) =
            AttrBuilder<'t>.CreateSubscription<PointerEventArgs>(InputElement.PointerEnterEvent, func, subPatchOptions)
            
        static member onPointerLeave<'t when 't :> InputElement>(func: PointerEventArgs -> unit, subPatchOptions) =
            AttrBuilder<'t>.CreateSubscription<PointerEventArgs>(InputElement.PointerLeaveEvent, func, subPatchOptions)
            
        static member onPointerMoved<'t when 't :> InputElement>(func: PointerEventArgs -> unit, subPatchOptions) =
            AttrBuilder<'t>.CreateSubscription<PointerEventArgs>(InputElement.PointerMovedEvent, func, subPatchOptions)
            
        static member onPointerPressed<'t when 't :> InputElement>(func: PointerPressedEventArgs -> unit, subPatchOptions) =
            AttrBuilder<'t>.CreateSubscription<PointerPressedEventArgs>(InputElement.PointerPressedEvent, func, subPatchOptions)
            
        static member onPointerReleased<'t when 't :> InputElement>(func: PointerReleasedEventArgs -> unit, subPatchOptions) =
            AttrBuilder<'t>.CreateSubscription<PointerReleasedEventArgs>(InputElement.PointerReleasedEvent, func, subPatchOptions)
            
        static member onPointerCaptureLost<'t when 't :> InputElement>(func: PointerCaptureLostEventArgs -> unit, subPatchOptions) =
            AttrBuilder<'t>.CreateSubscription<PointerCaptureLostEventArgs>(InputElement.PointerCaptureLostEvent, func, subPatchOptions)
            
        static member onPointerWheelChanged<'t when 't :> InputElement>(func: PointerWheelEventArgs -> unit, subPatchOptions) =
            AttrBuilder<'t>.CreateSubscription<PointerWheelEventArgs>(InputElement.PointerWheelChangedEvent, func, subPatchOptions)
            
        static member onTapped<'t when 't :> InputElement>(func: RoutedEventArgs -> unit, subPatchOptions) =
            AttrBuilder<'t>.CreateSubscription<RoutedEventArgs>(InputElement.TappedEvent, func, subPatchOptions)
            
        static member onDoubleTapped<'t when 't :> InputElement>(func: RoutedEventArgs -> unit, subPatchOptions) =
            AttrBuilder<'t>.CreateSubscription<RoutedEventArgs>(InputElement.DoubleTappedEvent, func, subPatchOptions)