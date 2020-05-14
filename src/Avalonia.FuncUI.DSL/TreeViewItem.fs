namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module TreeViewItem =
    open Avalonia.Controls
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
       
    let create (attrs: IAttr<TreeViewItem> list): IView<TreeViewItem> =
        ViewBuilder.Create<TreeViewItem>(attrs)

    type TreeViewItem with

        /// <summary>
        /// Sets a value indicating whether the item is expanded to show its children.
        /// </summary>
        static member isExpanded<'t when 't :> TreeViewItem>(value: bool) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, bool>(TreeViewItem.IsExpandedProperty, value, ValueNone)
        
        /// <summary>
        /// Sets the selection state of the item.
        /// </summary>
        static member isSelected<'t when 't :> TreeViewItem>(value: bool ) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, bool >(TreeViewItem.IsSelectedProperty, value, ValueNone)
         
        /// <summary>
        /// Sets the level/indentation of the item.
        /// </summary>
        static member level<'t when 't :> TreeViewItem>(value: int) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, int>(TreeViewItem.LevelProperty, value, ValueNone)