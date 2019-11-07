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
            
        static member onGotFocus<'t when 't :> InputElement>(func: GotFocusEventArgs -> unit) =
            AttrBuilder<'t>.CreateSubscription<GotFocusEventArgs>(InputElement.GotFocusEvent, func)
            
        static member onLostFocus<'t when 't :> InputElement>(func: RoutedEventArgs -> unit) =
            AttrBuilder<'t>.CreateSubscription<RoutedEventArgs>(InputElement.LostFocusEvent, func)
            
        static member onKeyDown<'t when 't :> InputElement>(func: KeyEventArgs -> unit) =
            AttrBuilder<'t>.CreateSubscription<KeyEventArgs>(InputElement.KeyDownEvent, func)
            
        static member onKeyUp<'t when 't :> InputElement>(func: KeyEventArgs -> unit) =
            AttrBuilder<'t>.CreateSubscription<KeyEventArgs>(InputElement.KeyUpEvent, func)
            
        static member onTextInput<'t when 't :> InputElement>(func: TextInputEventArgs -> unit) =
            AttrBuilder<'t>.CreateSubscription<TextInputEventArgs>(InputElement.TextInputEvent, func)
            
        static member onPointerEnter<'t when 't :> InputElement>(func: PointerEventArgs -> unit) =
            AttrBuilder<'t>.CreateSubscription<PointerEventArgs>(InputElement.PointerEnterEvent, func)
            
        static member onPointerLeave<'t when 't :> InputElement>(func: PointerEventArgs -> unit) =
            AttrBuilder<'t>.CreateSubscription<PointerEventArgs>(InputElement.PointerLeaveEvent, func)
            
        static member onPointerMoved<'t when 't :> InputElement>(func: PointerEventArgs -> unit) =
            AttrBuilder<'t>.CreateSubscription<PointerEventArgs>(InputElement.PointerMovedEvent, func)
            
        static member onPointerPressed<'t when 't :> InputElement>(func: PointerPressedEventArgs -> unit) =
            AttrBuilder<'t>.CreateSubscription<PointerPressedEventArgs>(InputElement.PointerPressedEvent, func)
            
        static member onPointerReleased<'t when 't :> InputElement>(func: PointerReleasedEventArgs -> unit) =
            AttrBuilder<'t>.CreateSubscription<PointerReleasedEventArgs>(InputElement.PointerReleasedEvent, func)
            
        static member onPointerCaptureLost<'t when 't :> InputElement>(func: PointerCaptureLostEventArgs -> unit) =
            AttrBuilder<'t>.CreateSubscription<PointerCaptureLostEventArgs>(InputElement.PointerCaptureLostEvent, func)
            
        static member onPointerWheelChanged<'t when 't :> InputElement>(func: PointerWheelEventArgs -> unit) =
            AttrBuilder<'t>.CreateSubscription<PointerWheelEventArgs>(InputElement.PointerWheelChangedEvent, func)
            
        static member onTapped<'t when 't :> InputElement>(func: RoutedEventArgs -> unit) =
            AttrBuilder<'t>.CreateSubscription<RoutedEventArgs>(InputElement.TappedEvent, func)
            
        static member onDoubleTapped<'t when 't :> InputElement>(func: RoutedEventArgs -> unit) =
            AttrBuilder<'t>.CreateSubscription<RoutedEventArgs>(InputElement.DoubleTappedEvent, func)