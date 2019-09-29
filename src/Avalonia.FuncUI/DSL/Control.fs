namespace Avalonia.FuncUI.DSL
open Avalonia.Media
open Avalonia.Visuals.Media.Imaging

[<AutoOpen>]
module Control =  
    open Avalonia
    open Avalonia.Controls
    open Avalonia.FuncUI.Core.Domain
    open Avalonia.Styling
    
    type Control with
        static member focusAdorner<'t when 't :> Control>(value: #ITemplate<IControl>) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty Control.FocusAdornerProperty
            let property = Property.createDirect(accessor, value)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member tag<'t when 't :> Control>(value: obj) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty Control.TagProperty
            let property = Property.createDirect(accessor, value)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>

        static member contextMenu<'t when 't :> Control>(menu: ContextMenu) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty Control.ContextMenuProperty
            let property = Property.createDirect(accessor, menu)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member dock<'t when 't :> Control>(dock: Dock) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty DockPanel.DockProperty
            let property = Property.createAttached(accessor, dock)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member bitmapInterpolationMode<'t when 't :> Control>(mode: BitmapInterpolationMode) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty RenderOptions.BitmapInterpolationModeProperty
            let property = Property.createAttached(accessor, mode)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>