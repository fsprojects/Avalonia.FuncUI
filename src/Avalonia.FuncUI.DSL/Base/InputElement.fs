namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module InputElement =  
    open Avalonia.FuncUI.Types
    open Avalonia.Input

    type InputElement with
        static member focusable<'t when 't :> InputElement>(value: bool) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty InputElement.FocusableProperty
            let property = Property.createDirect(accessor, value)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member isEnabled<'t when 't :> InputElement>(value: bool) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty InputElement.IsEnabledProperty
            let property = Property.createDirect(accessor, value)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>

        static member cursor<'t when 't :> InputElement>(cursor: Cursor) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty InputElement.CursorProperty
            let property = Property.createDirect(accessor, cursor)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
      
        static member isHitTestVisible<'t when 't :> InputElement>(value: bool) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty InputElement.IsHitTestVisibleProperty
            let property = Property.createDirect(accessor, value)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>