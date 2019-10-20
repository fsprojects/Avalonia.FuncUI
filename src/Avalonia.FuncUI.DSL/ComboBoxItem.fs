namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module ComboBox =
    open Avalonia.Controls
    open Avalonia.FuncUI.Types
    
    let create (attrs: IAttr<ComboBox> list): IView<ComboBox> =
        View.create<ComboBox>(attrs)
    
    type ComboBox with
    
        static member isDropDownOpen<'t when 't :> ComboBox>(value: bool) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty ComboBox.IsDropDownOpenProperty
            let property = Property.createDirect(accessor, value)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member maxDropDownHeight<'t when 't :> ComboBox>(height: float) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty ComboBox.MaxDropDownHeightProperty
            let property = Property.createDirect(accessor, height)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member virtualizationMode<'t when 't :> ComboBox>(mode: ItemVirtualizationMode) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty ComboBox.VirtualizationModeProperty
            let property = Property.createDirect(accessor, mode)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>