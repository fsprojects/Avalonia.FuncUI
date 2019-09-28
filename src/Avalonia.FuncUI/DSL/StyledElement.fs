namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module StyledElement =  
    open Avalonia
    open Avalonia.FuncUI.Core.Domain
    open Avalonia.Styling
            
    type StyledElement with
        static member dataContext<'t when 't :> StyledElement>(dataContext: obj) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty StyledElement.DataContextProperty
            let property = Property.createDirect(accessor, dataContext)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member name<'t when 't :> StyledElement>(name: string) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty StyledElement.NameProperty
            let property = Property.createDirect(accessor, name)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member templatedParent<'t when 't :> StyledElement>(template: #ITemplatedControl) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty StyledElement.TemplatedParentProperty
            let property = Property.createDirect(accessor, template)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>