namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module SelectingItemsControl =
    open Avalonia.Controls.Primitives
    open Avalonia.Controls
    open Avalonia.FuncUI.Builder
    open Avalonia.FuncUI.Types
    
    let create (attrs: IAttr<SelectingItemsControl> list): IView<SelectingItemsControl> =
        ViewBuilder.Create<SelectingItemsControl>(attrs)
     
    type SelectingItemsControl with

        static member autoScrollToSelectedItem<'t when 't :> SelectingItemsControl>(value: bool) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, bool>(SelectingItemsControl.AutoScrollToSelectedItemProperty, value, ValueNone)
            
        static member selectedIndex<'t when 't :> SelectingItemsControl>(index: int) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, int>(SelectingItemsControl.SelectedIndexProperty, index, ValueNone)
            
        static member onSelectedIndexChanged<'t when 't :> SelectingItemsControl>(func: int -> unit, ?subPatchOptions) =
            AttrBuilder.CreateSubscription<'t, int>(SelectingItemsControl.SelectedIndexProperty, func, ?subPatchOptions = subPatchOptions)
            
        static member selectedItem<'t when 't :> SelectingItemsControl>(item: obj) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, obj>(SelectingItemsControl.SelectedItemProperty, item, ValueNone)
            
        static member onSelectedItemChanged<'t when 't :> SelectingItemsControl>(func: obj -> unit, ?subPatchOptions) =
            AttrBuilder.CreateSubscription<'t, obj>(SelectingItemsControl.SelectedItemProperty, func, ?subPatchOptions = subPatchOptions)
            
        static member onSelectionChanged<'t when 't :> SelectingItemsControl>(func: SelectionChangedEventArgs -> unit, ?subPatchOptions) =
            AttrBuilder.CreateSubscription<'t, SelectionChangedEventArgs>(SelectingItemsControl.SelectionChangedEvent, func, ?subPatchOptions = subPatchOptions)