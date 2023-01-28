namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module TreeView =
    open System.Collections
    open Avalonia.Controls
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder

    let create (attrs: Attr<TreeView> list): View<TreeView> =
        ViewBuilder.Create<TreeView>(attrs)

    type TreeView with

        /// <summary>
        /// Sets a value indicating whether to automatically scroll to newly selected items.
        /// </summary>
        static member autoScrollToSelectedItem<'t when 't :> TreeView>(value: bool) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(TreeView.AutoScrollToSelectedItemProperty, value, ValueNone)

        /// <summary>
        /// Sets the selected items.
        /// </summary>
        static member selectedItem<'t when 't :> TreeView>(value: obj) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<obj>(TreeView.SelectedItemProperty, value, ValueNone)

        /// <summary>
        /// Subscribes to changes in the SelectedItem property.
        /// </summary>
        static member onSelectedItemChanged<'t when 't :> TreeView>(func: obj -> unit, ?subPatchOptions) =
            AttrBuilder<'t>.CreateSubscription<obj>(TreeView.SelectedItemProperty, func, ?subPatchOptions = subPatchOptions)

        /// <summary>
        /// Sets the selected items.
        /// </summary>
        static member selectedItems<'t when 't :> TreeView>(value: IList) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<IList>(TreeView.SelectedItemsProperty, value, ValueNone)

        /// <summary>
        /// Subscribes to changes in the SelectedItems property.
        /// </summary>
        static member onSelectedItemsChanged<'t when 't :> TreeView>(func: IList -> unit, ?subPatchOptions) =
            AttrBuilder<'t>.CreateSubscription<IList>(TreeView.SelectedItemsProperty, func, ?subPatchOptions = subPatchOptions)

        /// <summary>
        /// Sets the selection mode.
        /// </summary>
        static member selectionMode<'t when 't :> TreeView>(value: SelectionMode) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<SelectionMode>(TreeView.SelectionModeProperty, value, ValueNone)