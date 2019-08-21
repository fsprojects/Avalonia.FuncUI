namespace Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Core.Domain
open Avalonia.FuncUI.Core.Domain

[<AutoOpen>]
module Extensions =
    open Avalonia.FuncUI
    open Avalonia.FuncUI
    
    open Avalonia.Controls
    
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