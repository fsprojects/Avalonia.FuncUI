namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module ContentControl =  
    open Avalonia.Controls
    open Avalonia.FuncUI.Types
    open Avalonia.Controls.Templates
    open Avalonia.Layout
     
    type ContentControl with
        static member content<'t when 't :> ContentControl>(text: string) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty ContentControl.ContentProperty
            let property = Property.createDirect(accessor, text)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member content<'t when 't :> ContentControl>(value: IView) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty ContentControl.ContentProperty
            let content = Content.createSingle(accessor, Some value)
            let attr = Attr.createContent<'t> content
            attr :> IAttr<'t>
            
        static member content<'t when 't :> ContentControl>(value: IView option) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty ContentControl.ContentProperty
            let content = Content.createSingle(accessor, value)
            let attr = Attr.createContent<'t> content
            attr :> IAttr<'t>
            
        static member content<'t when 't :> ContentControl>(value: obj) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty ContentControl.BorderBrushProperty
            let property = Property.createDirect(accessor, value)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member contentTemplate<'t when 't :> ContentControl>(value: #IDataTemplate) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty ContentControl.ContentTemplateProperty
            let property = Property.createDirect(accessor, value)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>

        static member horizontalAlignment<'t when 't :> ContentControl>(value: HorizontalAlignment) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty ContentControl.HorizontalAlignmentProperty
            let property = Property.createDirect(accessor, value)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member verticalAlignment<'t when 't :> ContentControl>(value: VerticalAlignment) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty ContentControl.VerticalAlignmentProperty
            let property = Property.createDirect(accessor, value)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>