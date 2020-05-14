namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module TabItem =
    open Avalonia.Controls
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
    
    let create (attrs: IAttr<TabItem> list): IView<TabItem> =
        ViewBuilder.Create<TabItem>(attrs)
     
    type TabItem with

        static member tabStripPlacement<'t when 't :> TabItem>(placement: Dock) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, Dock>(TabItem.TabStripPlacementProperty, placement, ValueNone)
        
        static member isSelected<'t when 't :> TabItem>(value: bool) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, bool>(TabItem.IsSelectedProperty, value, ValueNone)
            
        static member onIsSelectedChanged<'t when 't :> TabItem>(func: bool -> unit, ?subPatchOptions) =
            AttrBuilder.CreateSubscription<'t, bool>(TabItem.IsSelectedProperty, func, ?subPatchOptions = subPatchOptions)