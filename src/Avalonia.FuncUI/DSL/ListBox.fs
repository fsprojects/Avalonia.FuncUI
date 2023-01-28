namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module ListBox =
    open System.Collections
    open Avalonia.Controls
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder

    let create (attrs: Attr<ListBox> list): IView<ListBox> =
        ViewBuilder.Create<ListBox>(attrs)

    type ListBox with

        static member selectedItems<'t when 't :> ListBox>(items: IList) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<IList>(ListBox.SelectedItemsProperty, items, ValueNone)

        static member onSelectedItemsChanged<'t when 't :> ListBox>(func: IList -> unit, ?subPatchOptions) =
            AttrBuilder<'t>.CreateSubscription<IList>(ListBox.SelectedItemsProperty, func, ?subPatchOptions = subPatchOptions)

        static member selectionMode<'t when 't :> ListBox>(mode: SelectionMode) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<SelectionMode>(ListBox.SelectionModeProperty, mode, ValueNone)

        static member virtualizationMode<'t when 't :> ListBox>(mode: ItemVirtualizationMode) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<ItemVirtualizationMode>(ListBox.VirtualizationModeProperty, mode, ValueNone)