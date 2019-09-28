namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module InputElement =  
    open Avalonia.Controls
    open Avalonia.Media.Immutable
    open System    
    open System.Windows.Input
    open Avalonia
    open Avalonia.FuncUI.Core.Domain
    open Avalonia.Media
    open Avalonia.Styling
    open Avalonia.Controls.Primitives
    open Avalonia.Controls.Templates
    open Avalonia.Animation
    open Avalonia.Layout
    open Avalonia.Interactivity
    open Avalonia.Input

    type InputElement with
        static member focusable<'t when 't :> InputElement>(value: bool) : IAttr<'t> =
            let accessor = Accessor.Avalonia InputElement.FocusableProperty
            let property = Property.createDirect(accessor, value)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member isEnabled<'t when 't :> InputElement>(value: bool) : IAttr<'t> =
            let accessor = Accessor.Avalonia InputElement.IsEnabledProperty
            let property = Property.createDirect(accessor, value)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>

        static member cursor<'t when 't :> InputElement>(cursor: Cursor) : IAttr<'t> =
            let accessor = Accessor.Avalonia InputElement.CursorProperty
            let property = Property.createDirect(accessor, cursor)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
      
        static member isHitTestVisible<'t when 't :> InputElement>(value: bool) : IAttr<'t> =
            let accessor = Accessor.Avalonia InputElement.IsHitTestVisibleProperty
            let property = Property.createDirect(accessor, value)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>