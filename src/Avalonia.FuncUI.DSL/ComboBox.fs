namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module ComboBox =
    open Avalonia.Controls
    open Avalonia.Layout
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
    
    let create (attrs: IAttr<ComboBox> list): IView<ComboBox> =
        ViewBuilder.Create<ComboBox>(attrs)
    
    type ComboBox with
    
        static member isDropDownOpen<'t when 't :> ComboBox>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(ComboBox.IsDropDownOpenProperty, value, ValueNone)
            
        static member horizontalContentAlignment<'t when 't :> ComboBox>(alignment: HorizontalAlignment) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<HorizontalAlignment>(ComboBox.HorizontalContentAlignmentProperty, alignment, ValueNone)
            
        static member maxDropDownHeight<'t when 't :> ComboBox>(height: float) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<float>(ComboBox.MaxDropDownHeightProperty, height, ValueNone)
            
        static member verticalContentAlignment<'t when 't :> ComboBox>(alignment: VerticalAlignment) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<VerticalAlignment>(ComboBox.VerticalContentAlignmentProperty, alignment, ValueNone)
            
        static member virtualizationMode<'t when 't :> ComboBox>(mode: ItemVirtualizationMode) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<ItemVirtualizationMode>(ComboBox.VirtualizationModeProperty, mode, ValueNone)