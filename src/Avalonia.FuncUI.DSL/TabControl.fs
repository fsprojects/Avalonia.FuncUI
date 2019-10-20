namespace Avalonia.FuncUI.DSL
open Avalonia.Controls.Templates
open Avalonia.Controls.Templates
open Avalonia.Layout

[<AutoOpen>]
module TabControl =
    open Avalonia.Controls.Primitives
    open Avalonia.Controls
    
    open Avalonia.FuncUI.Types
    
    let create (attrs: IAttr<TabControl> list): IView<TabControl> =
        View.create<TabControl>(attrs)
     
    type TabControl with

        static member tabStripPlacement<'t when 't :> TabControl>(placement: Dock) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty TabControl.TabStripPlacementProperty
            let property = Property.createDirect(accessor, placement)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member horizontalContentAlignment<'t when 't :> TabControl>(alignment: HorizontalAlignment) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty TabControl.HorizontalAlignmentProperty
            let property = Property.createDirect(accessor, alignment)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member verticalContentAlignment<'t when 't :> TabControl>(alignment: VerticalAlignment) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty TabControl.VerticalAlignmentProperty
            let property = Property.createDirect(accessor, alignment)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member contentTemplate<'t when 't :> TabControl>(value: IDataTemplate) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty TabControl.ContentTemplateProperty
            let property = Property.createDirect(accessor, value)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member selectedContent<'t when 't :> TabControl>(value: obj) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty TabControl.SelectedContentProperty
            let property = Property.createDirect(accessor, value)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member onSelectedContentChanged<'t when 't :> TabControl>(func: obj -> unit) =
            let subscription = Subscription.createFromProperty(TabControl.SelectedContentProperty, func)
            let attr = Attr.createSubscription<'t>(subscription)
            attr :> IAttr<'t>
            
        static member selectedContentTemplate<'t when 't :> TabControl>(value: IDataTemplate) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty TabControl.SelectedContentTemplateProperty
            let property = Property.createDirect(accessor, value)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member onSelectedContentTemplateChanged<'t when 't :> TabControl>(func: IDataTemplate -> unit) =
            let subscription = Subscription.createFromProperty(TabControl.SelectedContentTemplateProperty, func)
            let attr = Attr.createSubscription<'t>(subscription)
            attr :> IAttr<'t>