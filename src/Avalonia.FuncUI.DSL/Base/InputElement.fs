namespace Avalonia.FuncUI.DSL
open Avalonia.Interactivity

[<AutoOpen>]
module InputElement =  
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
    open Avalonia.Input

    type InputElement with
        static member focusable<'t when 't :> InputElement>(value: bool) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, bool>(InputElement.FocusableProperty, value, ValueNone)
            
        static member isEnabled<'t when 't :> InputElement>(value: bool) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, bool>(InputElement.IsEnabledProperty, value, ValueNone)

        static member cursor<'t when 't :> InputElement>(cursor: Cursor) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, Cursor>(InputElement.CursorProperty, cursor, ValueNone)
      
        static member isHitTestVisible<'t when 't :> InputElement>(value: bool) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, bool>(InputElement.IsHitTestVisibleProperty, value, ValueNone)
            
        static member onGotFocus<'t when 't :> InputElement>(func: GotFocusEventArgs -> unit, ?subPatchOptions) =
            AttrBuilder.CreateSubscription<'t, GotFocusEventArgs>(InputElement.GotFocusEvent, func, ?subPatchOptions = subPatchOptions)
            
        static member onLostFocus<'t when 't :> InputElement>(func: RoutedEventArgs -> unit, ?subPatchOptions) =
            AttrBuilder.CreateSubscription<'t, RoutedEventArgs>(InputElement.LostFocusEvent, func, ?subPatchOptions = subPatchOptions)
            
        static member onKeyDown<'t when 't :> InputElement>(func: KeyEventArgs -> unit, ?subPatchOptions) =
            AttrBuilder.CreateSubscription<'t, KeyEventArgs>(InputElement.KeyDownEvent, func, ?subPatchOptions = subPatchOptions)
            
        static member onKeyUp<'t when 't :> InputElement>(func: KeyEventArgs -> unit, ?subPatchOptions) =
            AttrBuilder.CreateSubscription<'t, KeyEventArgs>(InputElement.KeyUpEvent, func, ?subPatchOptions = subPatchOptions)
            
        static member onTextInput<'t when 't :> InputElement>(func: TextInputEventArgs -> unit, ?subPatchOptions) =
            AttrBuilder.CreateSubscription<'t, TextInputEventArgs>(InputElement.TextInputEvent, func, ?subPatchOptions = subPatchOptions)
            
        static member onPointerEnter<'t when 't :> InputElement>(func: PointerEventArgs -> unit, ?subPatchOptions) =
            AttrBuilder.CreateSubscription<'t, PointerEventArgs>(InputElement.PointerEnterEvent, func, ?subPatchOptions = subPatchOptions)
            
        static member onPointerLeave<'t when 't :> InputElement>(func: PointerEventArgs -> unit, ?subPatchOptions) =
            AttrBuilder.CreateSubscription<'t, PointerEventArgs>(InputElement.PointerLeaveEvent, func, ?subPatchOptions = subPatchOptions)
            
        static member onPointerMoved<'t when 't :> InputElement>(func: PointerEventArgs -> unit, ?subPatchOptions) =
            AttrBuilder.CreateSubscription<'t, PointerEventArgs>(InputElement.PointerMovedEvent, func, ?subPatchOptions = subPatchOptions)
            
        static member onPointerPressed<'t when 't :> InputElement>(func: PointerPressedEventArgs -> unit, ?subPatchOptions) =
            AttrBuilder.CreateSubscription<'t, PointerPressedEventArgs>(InputElement.PointerPressedEvent, func, ?subPatchOptions = subPatchOptions)
            
        static member onPointerReleased<'t when 't :> InputElement>(func: PointerReleasedEventArgs -> unit, ?subPatchOptions) =
            AttrBuilder.CreateSubscription<'t, PointerReleasedEventArgs>(InputElement.PointerReleasedEvent, func, ?subPatchOptions = subPatchOptions)
            
        static member onPointerCaptureLost<'t when 't :> InputElement>(func: PointerCaptureLostEventArgs -> unit, ?subPatchOptions) =
            AttrBuilder.CreateSubscription<'t, PointerCaptureLostEventArgs>(InputElement.PointerCaptureLostEvent, func, ?subPatchOptions = subPatchOptions)
            
        static member onPointerWheelChanged<'t when 't :> InputElement>(func: PointerWheelEventArgs -> unit, ?subPatchOptions) =
            AttrBuilder.CreateSubscription<'t, PointerWheelEventArgs>(InputElement.PointerWheelChangedEvent, func, ?subPatchOptions = subPatchOptions)
            
        static member onTapped<'t when 't :> InputElement>(func: RoutedEventArgs -> unit, ?subPatchOptions) =
            AttrBuilder.CreateSubscription<'t, RoutedEventArgs>(InputElement.TappedEvent, func, ?subPatchOptions = subPatchOptions)
            
        static member onDoubleTapped<'t when 't :> InputElement>(func: RoutedEventArgs -> unit, ?subPatchOptions) =
            AttrBuilder.CreateSubscription<'t, RoutedEventArgs>(InputElement.DoubleTappedEvent, func, ?subPatchOptions = subPatchOptions)