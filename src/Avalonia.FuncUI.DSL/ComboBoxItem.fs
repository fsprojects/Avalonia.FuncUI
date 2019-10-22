namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module ComboBox =
    open Avalonia.Controls
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
    
    let create (attrs: IAttr<ComboBox> list): IView<ComboBox> =
        View.create<ComboBox>(attrs)
    
    type ComboBox with
    
        static member isDropDownOpen<'t when 't :> ComboBox>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(ComboBox.IsDropDownOpenProperty, value, ValueNone)
            
        static member maxDropDownHeight<'t when 't :> ComboBox>(height: float) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<float>(ComboBox.MaxDropDownHeightProperty, height, ValueNone)
            
        static member virtualizationMode<'t when 't :> ComboBox>(mode: ItemVirtualizationMode) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<ItemVirtualizationMode>(ComboBox.VirtualizationModeProperty, mode, ValueNone)