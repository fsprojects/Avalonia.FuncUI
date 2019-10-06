namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module Canvas =  
    open Avalonia.Controls
    open Avalonia.FuncUI.Types
    
    let create (attrs: IAttr<Canvas> list): IView<Canvas> =
        View.create<Canvas>(attrs)
    
    type Control with
        static member left<'t when 't :> Control>(value: double) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty Canvas.LeftProperty
            let property = Property.createAttached(accessor, value)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member top<'t when 't :> Control>(value: double) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty Canvas.TopProperty
            let property = Property.createAttached(accessor, value)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member right<'t when 't :> Control>(value: double) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty Canvas.RightProperty
            let property = Property.createAttached(accessor, value)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member bottom<'t when 't :> Control>(value: double) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty Canvas.BottomProperty
            let property = Property.createAttached(accessor, value)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
    
    type Canvas with
        end