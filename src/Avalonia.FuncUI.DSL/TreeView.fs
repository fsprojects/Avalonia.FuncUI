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
        /// Gets or sets a value indicating whether to automatically scroll to newly selected items.
        /// </summary>
        static member autoScrollToSelectedItem<'t when 't :> TreeView>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(TreeView.AutoScrollToSelectedItemProperty, value, ValueNone)
        
        /// <summary>
        /// Gets the selected items.
        /// </summary>
        static member selectedItem<'t when 't :> TreeView>(value: obj) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<obj>(TreeView.SelectedItemProperty, value, ValueNone)
         
        /// <summary>
        /// Gets or sets the selected item.
        /// </summary>
        static member selectedItems<'t when 't :> TreeView>(value: IList) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<IList>(TreeView.SelectedItemsProperty, value, ValueNone)

        /// <summary>
        /// Gets or sets the selection mode.
        /// </summary>
        static member selectionMode<'t when 't :> TreeView>(value: SelectionMode) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<SelectionMode>(TreeView.SelectionModeProperty, value, ValueNone)
            

[<AutoOpen>]
module TreeViewItem =
    open Avalonia.Controls
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
       
    let create (attrs: IAttr<TreeViewItem> list): IView<TreeViewItem> =
        ViewBuilder.Create<TreeViewItem>(attrs)

    type TreeViewItem with

        /// <summary>
        /// Gets or sets a value indicating whether the item is expanded to show its children.
        /// </summary>
        static member isExpanded<'t when 't :> TreeViewItem>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(TreeViewItem.IsExpandedProperty, value, ValueNone)
        
        /// <summary>
        /// Gets or sets the selection state of the item.
        /// </summary>
        static member isSelected<'t when 't :> TreeViewItem>(value: bool ) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool >(TreeViewItem.IsSelectedProperty, value, ValueNone)
         
        /// <summary>
        /// Gets the level/indentation of the item.
        /// </summary>
        static member level<'t when 't :> TreeViewItem>(value: int) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<int>(TreeViewItem.LevelProperty, value, ValueNone)