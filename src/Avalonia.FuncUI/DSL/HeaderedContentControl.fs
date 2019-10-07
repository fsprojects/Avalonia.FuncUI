namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module HeaderedContentControl =
    open Avalonia.Controls.Primitives
    open Avalonia.FuncUI.Types
    open Avalonia.Controls.Templates
     
    type HeaderedContentControl with
        static member header<'t when 't :> HeaderedContentControl>(text: string) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty HeaderedContentControl.HeaderProperty
            let property = Property.createDirect(accessor, text)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member header<'t when 't :> HeaderedContentControl>(value: IView) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty HeaderedContentControl.HeaderProperty
            let content = Content.createSingle(accessor, Some value)
            let attr = Attr.createContent<'t> content
            attr :> IAttr<'t>
            
        static member header<'t when 't :> HeaderedContentControl>(value: IView option) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty HeaderedContentControl.HeaderProperty
            let content = Content.createSingle(accessor, value)
            let attr = Attr.createContent<'t> content
            attr :> IAttr<'t>
            
        static member header<'t when 't :> HeaderedContentControl>(value: obj) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty HeaderedContentControl.HeaderProperty
            let property = Property.createDirect(accessor, value)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member headerTemplate<'t when 't :> HeaderedContentControl>(value: IDataTemplate) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty HeaderedContentControl.HeaderTemplateProperty
            let property = Property.createDirect(accessor, value)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>