namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module DockPanel =  
    open Avalonia.Controls
    open Avalonia.FuncUI.Types
    
    let create (attrs: IAttr<DockPanel> list): IView<DockPanel> =
        View.create<DockPanel>(attrs)
    
    type Control with
        static member dock<'t when 't :> Control>(dock: Dock) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty DockPanel.DockProperty
            let property = Property.createAttached(accessor, dock)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
    
    type DockPanel with
        
        static member lastChildFill<'t when 't :> DockPanel>(fill: bool) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty DockPanel.LastChildFillProperty
            let property = Property.createDirect(accessor, fill)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>