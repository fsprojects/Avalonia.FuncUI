namespace Avalonia.FuncUI.DSL
open Avalonia
open Avalonia.Controls
open Avalonia.FuncUI.Core.Domain
open Avalonia.FuncUI.Core.Domain
open Avalonia.Media
open Avalonia.Styling

[<AutoOpen>]
module Extensions =
    open Avalonia.FuncUI
    open Avalonia.FuncUI
    
    open Avalonia    
    open Avalonia.Controls
    open Avalonia.Animation
    open Avalonia.Layout
    open Avalonia.Input

    type Animatable with
        static member transitions<'t when 't :> Animatable>(transitions: Transitions) : IAttr<'t> =
            let accessor = Accessor.Avalonia Animatable.TransitionsProperty
            let property = Property.createDirect(accessor, transitions)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member clock<'t when 't :> Animatable>(clock: IClock) : IAttr<'t> =
            let accessor = Accessor.Avalonia Animatable.ClockProperty
            let property = Property.createDirect(accessor, clock)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
    type StyledElement with
        static member dataContext<'t when 't :> StyledElement>(dataContext: obj) : IAttr<'t> =
            let accessor = Accessor.Avalonia StyledElement.DataContextProperty
            let property = Property.createDirect(accessor, dataContext)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member name<'t when 't :> StyledElement>(name: string) : IAttr<'t> =
            let accessor = Accessor.Avalonia StyledElement.NameProperty
            let property = Property.createDirect(accessor, name)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member templatedParent<'t when 't :> StyledElement>(template: #ITemplatedControl) : IAttr<'t> =
            let accessor = Accessor.Avalonia StyledElement.TemplatedParentProperty
            let property = Property.createDirect(accessor, template)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
    type Visual with
        static member clipToBounds<'t when 't :> Visual>(value: bool) : IAttr<'t> =
            let accessor = Accessor.Avalonia Visual.ClipToBoundsProperty
            let property = Property.createDirect(accessor, value)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member clip<'t when 't :> Visual>(mask: Geometry) : IAttr<'t> =
            let accessor = Accessor.Avalonia Visual.ClipProperty
            let property = Property.createDirect(accessor, mask)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member isVisible<'t when 't :> Visual>(visible: bool) : IAttr<'t> =
            let accessor = Accessor.Avalonia Visual.IsVisibleProperty
            let property = Property.createDirect(accessor, visible)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
    
        static member opacity<'t when 't :> Visual>(value: double) : IAttr<'t> =
            let accessor = Accessor.Avalonia Visual.OpacityProperty
            let property = Property.createDirect(accessor, value)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
      
        static member opacityMask<'t when 't :> Visual>(value: #IBrush) : IAttr<'t> =
            let accessor = Accessor.Avalonia Visual.OpacityMaskProperty
            let property = Property.createDirect(accessor, value)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>

        static member renderTransform<'t when 't :> Visual>(transform: Transform) : IAttr<'t> =
            let accessor = Accessor.Avalonia Visual.RenderTransformProperty
            let property = Property.createDirect(accessor, transform)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
    
        static member renderTransformOrigin<'t when 't :> Visual>(origin: RelativePoint) : IAttr<'t> =
            let accessor = Accessor.Avalonia Visual.RenderTransformProperty
            let property = Property.createDirect(accessor, origin)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member zIndex<'t when 't :> Visual>(index: int) : IAttr<'t> =
            let accessor = Accessor.Avalonia Visual.ZIndexProperty
            let property = Property.createDirect(accessor, index)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
      
    type Layoutable with
        static member width<'t when 't :> Layoutable>(value: double) : IAttr<'t> =
            let accessor = Accessor.Avalonia Layoutable.WidthProperty
            let property = Property.createDirect(accessor, value)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member height<'t when 't :> Layoutable>(value: double) : IAttr<'t> =
            let accessor = Accessor.Avalonia Layoutable.HeightProperty
            let property = Property.createDirect(accessor, value)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member minWidth<'t when 't :> Layoutable>(value: double) : IAttr<'t> =
            let accessor = Accessor.Avalonia Layoutable.MinWidthProperty
            let property = Property.createDirect(accessor, value)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member minHeight<'t when 't :> Layoutable>(value: double) : IAttr<'t> =
            let accessor = Accessor.Avalonia Layoutable.MinHeightProperty
            let property = Property.createDirect(accessor, value)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
      
        static member maxWidth<'t when 't :> Layoutable>(value: double) : IAttr<'t> =
            let accessor = Accessor.Avalonia Layoutable.MaxWidthProperty
            let property = Property.createDirect(accessor, value)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member maxHeight<'t when 't :> Layoutable>(value: double) : IAttr<'t> =
            let accessor = Accessor.Avalonia Layoutable.MaxHeightProperty
            let property = Property.createDirect(accessor, value)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member margin<'t when 't :> Layoutable>(margin: Thickness) : IAttr<'t> =
            let accessor = Accessor.Avalonia Layoutable.MarginProperty
            let property = Property.createDirect(accessor, margin)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
    
        static member horizontalAlignment<'t when 't :> Layoutable>(value: HorizontalAlignment) : IAttr<'t> =
            let accessor = Accessor.Avalonia Layoutable.HorizontalAlignmentProperty
            let property = Property.createDirect(accessor, value)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
   
        static member verticalAlignment<'t when 't :> Layoutable>(value: VerticalAlignment) : IAttr<'t> =
            let accessor = Accessor.Avalonia Layoutable.VerticalAlignmentProperty
            let property = Property.createDirect(accessor, value)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
           
        static member useLayoutRounding<'t when 't :> Layoutable>(value: bool) : IAttr<'t> =
            let accessor = Accessor.Avalonia Layoutable.UseLayoutRoundingProperty
            let property = Property.createDirect(accessor, value)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
  
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
      
    type StackPanel with
        static member create (attrs: IAttr<StackPanel> list): IView<StackPanel> =
            View.create<StackPanel>(attrs)
        
        static member orientation<'t when 't :> StackPanel>(orientation: Orientation) : IAttr<'t> =
            let accessor = Accessor.Avalonia StackPanel.OrientationProperty
            let property = Property.createDirect(accessor, orientation)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member children<'t when 't :> StackPanel>(children: IView list) : IAttr<'t> =
            let accessor = Accessor.Instance "Children"
            let content = Content.createMultiple(accessor, children)
            let attr = Attr.createContent<'t> content
            attr :> IAttr<'t>
            
    type DockPanel with
       static member create (attrs: IAttr<DockPanel> list): IView<DockPanel> =
            View.create<DockPanel>(attrs)
        
       static member lastChildFill<'t when 't :> DockPanel>(fill: bool) : IAttr<'t> =
            let accessor = Accessor.Avalonia DockPanel.LastChildFillProperty
            let property = Property.createDirect(accessor, fill)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
                 
       static member children<'t when 't :> DockPanel>(children: IView list) : IAttr<'t> =
            let accessor = Accessor.Instance "Children"
            let content = Content.createMultiple(accessor, children)
            let attr = Attr.createContent<'t> content
            attr :> IAttr<'t>
    
    type TextBlock with
       static member create (attrs: IAttr<TextBlock> list): IView<TextBlock> =
            View.create<TextBlock>(attrs)
        
       static member text<'t when 't :> TextBlock>(text: string) : IAttr<'t> =
            let accessor = Accessor.Avalonia TextBlock.TextProperty
            let property = Property.createDirect(accessor, text)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
    
    type Button with
       static member create (attrs: IAttr<Button> list): IView<Button> =
            View.create<Button>(attrs)
        
       static member content<'t when 't :> Button>(text: string) : IAttr<'t> =
            let accessor = Accessor.Avalonia Button.ContentProperty
            let property = Property.createDirect(accessor, text)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
    
       static member content<'t when 't :> Button>(content: IView) : IAttr<'t> =
            let accessor = Accessor.Avalonia Button.ContentProperty
            let property = Property.createDirect(accessor, content)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
module Playground =
     let view =
          StackPanel.create [
               StackPanel.orientation Orientation.Horizontal
               StackPanel.children [
                    Button.create [
                         Button.content "click me"
                    ]
               ]
          ]