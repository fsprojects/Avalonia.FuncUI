namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module Visual =  
    open Avalonia
    open Avalonia.Media
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
            
    type Visual with
    
        static member clipToBounds<'t when 't :> Visual>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(Visual.ClipToBoundsProperty, value, ValueNone)
        
        static member clip<'t when 't :> Visual>(mask: Geometry) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<Geometry>(Visual.ClipProperty, mask, ValueNone)
            
        static member isVisible<'t when 't :> Visual>(visible: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(Visual.IsVisibleProperty, visible, ValueNone)
    
        static member opacity<'t when 't :> Visual>(value: float) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<float>(Visual.IsVisibleProperty, value, ValueNone)
      
        static member opacityMask<'t when 't :> Visual>(value: IBrush) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<IBrush>(Visual.OpacityMaskProperty, value, ValueNone)

        static member renderTransform<'t when 't :> Visual>(transform: Transform) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<Transform>(Visual.RenderTransformProperty, transform, ValueNone)
    
        static member renderTransformOrigin<'t when 't :> Visual>(origin: RelativePoint) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<RelativePoint>(Visual.RenderTransformOriginProperty, origin, ValueNone)
            
        static member zIndex<'t when 't :> Visual>(index: int) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<int>(Visual.ZIndexProperty, index, ValueNone)