namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module Visual =  
    open Avalonia
    open Avalonia.Media
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
            
    type Visual with
    
        static member clipToBounds<'t when 't :> Visual>(value: bool) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, bool>(Visual.ClipToBoundsProperty, value, ValueNone)
        
        static member clip<'t when 't :> Visual>(mask: Geometry) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, Geometry>(Visual.ClipProperty, mask, ValueNone)
            
        static member isVisible<'t when 't :> Visual>(visible: bool) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, bool>(Visual.IsVisibleProperty, visible, ValueNone)
    
        static member opacity<'t when 't :> Visual>(value: float) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, float>(Visual.IsVisibleProperty, value, ValueNone)
      
        static member opacityMask<'t when 't :> Visual>(value: IBrush) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, IBrush>(Visual.OpacityMaskProperty, value, ValueNone)

        static member renderTransform<'t when 't :> Visual>(transform: Transform) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, Transform>(Visual.RenderTransformProperty, transform, ValueNone)
    
        static member renderTransformOrigin<'t when 't :> Visual>(origin: RelativePoint) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, RelativePoint>(Visual.RenderTransformOriginProperty, origin, ValueNone)
            
        static member zIndex<'t when 't :> Visual>(index: int) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, int>(Visual.ZIndexProperty, index, ValueNone)