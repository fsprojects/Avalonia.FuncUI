namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module ListBoxItem =
    open Avalonia.Controls
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder

    let create (attrs: Attr<ListBoxItem> list): View<ListBoxItem> =
        ViewBuilder.Create<ListBoxItem>(attrs)

    type ListBoxItem with

        static member isSelected<'t when 't :> ListBoxItem>(value: bool) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(ListBoxItem.IsSelectedProperty, value, ValueNone)

        static member onIsSelectedChanged<'t when 't :> ListBoxItem>(func: bool -> unit, ?subPatchOptions) =
            AttrBuilder<'t>.CreateSubscription<bool>(ListBoxItem.IsSelectedProperty, func, ?subPatchOptions = subPatchOptions)