namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module TreeViewItem =
    open Avalonia.Controls
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder

    let create (attrs: Attr<TreeViewItem> list): IView<TreeViewItem> =
        ViewBuilder.Create<TreeViewItem>(attrs)

    type TreeViewItem with

        /// <summary>
        /// Sets a value indicating whether the item is expanded to show its children.
        /// </summary>
        static member isExpanded<'t when 't :> TreeViewItem>(value: bool) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(TreeViewItem.IsExpandedProperty, value, ValueNone)

        /// <summary>
        /// Sets the selection state of the item.
        /// </summary>
        static member isSelected<'t when 't :> TreeViewItem>(value: bool ) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<bool >(TreeViewItem.IsSelectedProperty, value, ValueNone)

        /// <summary>
        /// Sets the level/indentation of the item.
        /// </summary>
        static member level<'t when 't :> TreeViewItem>(value: int) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<int>(TreeViewItem.LevelProperty, value, ValueNone)