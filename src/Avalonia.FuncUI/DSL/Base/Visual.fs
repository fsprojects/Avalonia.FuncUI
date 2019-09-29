namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module Visual =  
    open Avalonia
    open Avalonia.Media
    open Avalonia.FuncUI.Types
            
    type Visual with
        static member clipToBounds<'t when 't :> Visual>(value: bool) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty Visual.ClipToBoundsProperty
            let property = Property.createDirect(accessor, value)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member clip<'t when 't :> Visual>(mask: Geometry) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty Visual.ClipProperty
            let property = Property.createDirect(accessor, mask)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member isVisible<'t when 't :> Visual>(visible: bool) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty Visual.IsVisibleProperty
            let property = Property.createDirect(accessor, visible)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
    
        static member opacity<'t when 't :> Visual>(value: float) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty Visual.OpacityProperty
            let property = Property.createDirect(accessor, value)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
      
        static member opacityMask<'t when 't :> Visual>(value: #IBrush) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty Visual.OpacityMaskProperty
            let property = Property.createDirect(accessor, value)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>

        static member renderTransform<'t when 't :> Visual>(transform: Transform) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty Visual.RenderTransformProperty
            let property = Property.createDirect(accessor, transform)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
    
        static member renderTransformOrigin<'t when 't :> Visual>(origin: RelativePoint) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty Visual.RenderTransformProperty
            let property = Property.createDirect(accessor, origin)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member zIndex<'t when 't :> Visual>(index: int) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty Visual.ZIndexProperty
            let property = Property.createDirect(accessor, index)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>