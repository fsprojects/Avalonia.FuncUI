namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module ToggleButton =
    open Avalonia.Controls.Primitives
    open Avalonia.FuncUI.Types
    
    let create (attrs: IAttr<ToggleButton> list): IView<ToggleButton> =
        View.create<ToggleButton>(attrs)
     
    type ToggleButton with
        static member isThreeState<'t when 't :> ToggleButton>(value: bool) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty ToggleButton.IsThreeStateProperty
            let property = Property.createDirect(accessor, value)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member isChecked<'t when 't :> ToggleButton>(value: bool) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty ToggleButton.IsCheckedProperty
            let property = Property.createDirect(accessor, value)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>

       

