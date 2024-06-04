namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module SelectingItemsControl =
    open Avalonia.Controls.Primitives
    open Avalonia.Controls
    open Avalonia.Data
    open Avalonia.Interactivity
    open Avalonia.FuncUI.Builder
    open Avalonia.FuncUI.Types

    let create (attrs: IAttr<SelectingItemsControl> list): IView<SelectingItemsControl> =
        ViewBuilder.Create<SelectingItemsControl>(attrs)

    type Control with
        static member isSelected<'t when 't :> Control>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(SelectingItemsControl.IsSelectedProperty, value, ValueNone)

    type SelectingItemsControl with

        static member autoScrollToSelectedItem<'t when 't :> SelectingItemsControl>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(SelectingItemsControl.AutoScrollToSelectedItemProperty, value, ValueNone)

        static member selectedIndex<'t when 't :> SelectingItemsControl>(index: int) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<int>(SelectingItemsControl.SelectedIndexProperty, index, ValueNone)

        static member onSelectedIndexChanged<'t when 't :> SelectingItemsControl>(func: int -> unit, ?subPatchOptions) =
            AttrBuilder<'t>.CreateSubscription<int>(SelectingItemsControl.SelectedIndexProperty, func, ?subPatchOptions = subPatchOptions)

        static member selectedItem<'t when 't :> SelectingItemsControl>(item: obj) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<obj>(SelectingItemsControl.SelectedItemProperty, item, ValueNone)

        static member selectedValueBinding<'t when 't  :> SelectingItemsControl>(binding: IBinding) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<IBinding>(SelectingItemsControl.SelectedValueBindingProperty, binding, ValueNone)

        static member selectedValue<'t when 't :> SelectingItemsControl>(value: obj) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<obj>(SelectingItemsControl.SelectedValueProperty, value, ValueNone)

        static member isTextSearchEnabled<'t when 't :> SelectingItemsControl>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(SelectingItemsControl.IsTextSearchEnabledProperty, value, ValueNone)

        static member wrapSelection<'t when 't :> SelectingItemsControl>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(SelectingItemsControl.WrapSelectionProperty, value, ValueNone)

        static member selection<'t when 't :> SelectingItemsControl>(model: Selection.ISelectionModel) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<Selection.ISelectionModel>(ListBox.SelectionProperty, model, ValueNone)

        static member onSelectedItemChanged<'t when 't :> SelectingItemsControl>(func: obj -> unit, ?subPatchOptions) =
            AttrBuilder<'t>.CreateSubscription<obj>(SelectingItemsControl.SelectedItemProperty, func, ?subPatchOptions = subPatchOptions)

        static member onIsSelectedChanged<'t when 't :> SelectingItemsControl>(func: RoutedEventArgs -> unit, ?subPatchOptions) =
            AttrBuilder<'t>.CreateSubscription<RoutedEventArgs>(SelectingItemsControl.IsSelectedChangedEvent, func, ?subPatchOptions = subPatchOptions)

        static member onSelectionChanged<'t when 't :> SelectingItemsControl>(func: SelectionChangedEventArgs -> unit, ?subPatchOptions) =
            AttrBuilder<'t>.CreateSubscription<SelectionChangedEventArgs>(SelectingItemsControl.SelectionChangedEvent, func, ?subPatchOptions = subPatchOptions)
