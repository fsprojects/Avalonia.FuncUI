namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module DockPanel =  
    open Avalonia.Controls
    open Avalonia.FuncUI.Types
    
    let create (attrs: IAttr<DockPanel> list): IView<DockPanel> =
        View.create<DockPanel>(attrs)
            
    type DockPanel with
        
        static member lastChildFill<'t when 't :> DockPanel>(fill: bool) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty DockPanel.LastChildFillProperty
            let property = Property.createDirect(accessor, fill)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>