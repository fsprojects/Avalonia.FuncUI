namespace Avalonia.FuncUI.DSL

open System.Collections

[<AutoOpen>]
module SelectingItemsControl =
    open Avalonia.Controls.Primitives
    open Avalonia.Controls
    open Avalonia.FuncUI.Builder
    open Avalonia.FuncUI.Types

    let create (attrs: Attr<SelectingItemsControl> list): View<SelectingItemsControl> =
        ViewBuilder.Create<SelectingItemsControl>(attrs)

    type SelectingItemsControl with

        static member autoScrollToSelectedItem<'t when 't :> SelectingItemsControl>(value: bool) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(SelectingItemsControl.AutoScrollToSelectedItemProperty, value, ValueNone)

        static member selectedIndex<'t when 't :> SelectingItemsControl>(index: int) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<int>(SelectingItemsControl.SelectedIndexProperty, index, ValueNone)

        static member onSelectedIndexChanged<'t when 't :> SelectingItemsControl>(func: int -> unit, ?subPatchOptions) =
            AttrBuilder<'t>.CreateSubscription<int>(SelectingItemsControl.SelectedIndexProperty, func, ?subPatchOptions = subPatchOptions)

        static member selectedItem<'t when 't :> SelectingItemsControl>(item: obj) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<obj>(SelectingItemsControl.SelectedItemProperty, item, ValueNone)

        static member selection<'t when 't :> SelectingItemsControl>(model: Selection.ISelectionModel) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<Selection.ISelectionModel>(ListBox.SelectionProperty, model, ValueNone)

        static member onSelectedItemChanged<'t when 't :> SelectingItemsControl>(func: obj -> unit, ?subPatchOptions) =
            AttrBuilder<'t>.CreateSubscription<obj>(SelectingItemsControl.SelectedItemProperty, func, ?subPatchOptions = subPatchOptions)

        static member onSelectionChanged<'t when 't :> SelectingItemsControl>(func: SelectionChangedEventArgs -> unit, ?subPatchOptions) =
            AttrBuilder<'t>.CreateSubscription<SelectionChangedEventArgs>(SelectingItemsControl.SelectionChangedEvent, func, ?subPatchOptions = subPatchOptions)
