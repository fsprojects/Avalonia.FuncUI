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
            AttrBuilder<'t>.CreateProperty<bool>(SelectingItemsControl.AutoScrollToSelectedItemProperty, value, ValueNone)
            
        static member selectedIndex<'t when 't :> SelectingItemsControl>(index: int) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<int>(SelectingItemsControl.SelectedIndexProperty, index, ValueNone)
            
        static member onSelectedIndexChanged<'t when 't :> SelectingItemsControl>(func: int -> unit, subPatchOptions) =
            AttrBuilder<'t>.CreateSubscription<int>(SelectingItemsControl.SelectedIndexProperty, func, subPatchOptions)
            
        static member selectedItem<'t when 't :> SelectingItemsControl>(item: obj) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<obj>(SelectingItemsControl.SelectedItemProperty, item, ValueNone)
            
        static member onSelectedItemChanged<'t when 't :> SelectingItemsControl>(func: obj -> unit, subPatchOptions) =
            AttrBuilder<'t>.CreateSubscription<obj>(SelectingItemsControl.SelectedItemProperty, func, subPatchOptions)
            
        static member onSelectionChanged<'t when 't :> SelectingItemsControl>(func: SelectionChangedEventArgs -> unit, subPatchOptions) =
            AttrBuilder<'t>.CreateSubscription<SelectionChangedEventArgs>(SelectingItemsControl.SelectionChangedEvent, func, subPatchOptions)