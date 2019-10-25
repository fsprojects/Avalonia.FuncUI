namespace Avalonia.FuncUI.DSL

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