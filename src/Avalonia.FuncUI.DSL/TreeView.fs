namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module TreeView =
    open System.Collections
    open Avalonia.Controls
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
   
    let create (attrs: IAttr<TreeView> list): IView<TreeView> =
        ViewBuilder.Create<TreeView>(attrs)

    type TreeView with

        /// <summary>
        /// Sets a value indicating whether to automatically scroll to newly selected items.
        /// </summary>
        static member autoScrollToSelectedItem<'t when 't :> TreeView>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(TreeView.AutoScrollToSelectedItemProperty, value, ValueNone)
        
        /// <summary>
        /// Subscribes to changes in the SelectedItem property.
        /// </summary>
        static member onSelectedItemChanged<'t when 't :> TreeView>(func: obj -> unit, ?subPatchOptions) =
            AttrBuilder<'t>.CreateSubscription<obj>(TreeView.SelectedItemProperty, func, ?subPatchOptions = subPatchOptions)

        /// <summary>
        /// Sets the selected item.
        /// </summary>
        static member selectedItem<'t when 't :> TreeView>(value: obj) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<obj>(TreeView.SelectedItemProperty, value, ValueNone)
         
        /// <summary>
        /// Sets the selected items.
        /// </summary>
        static member selectedItems<'t when 't :> TreeView>(value: IList) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<IList>(TreeView.SelectedItemsProperty, value, ValueNone)

        /// <summary>
        /// Sets the selection mode.
        /// </summary>
        static member selectionMode<'t when 't :> TreeView>(value: SelectionMode) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<SelectionMode>(TreeView.SelectionModeProperty, value, ValueNone)