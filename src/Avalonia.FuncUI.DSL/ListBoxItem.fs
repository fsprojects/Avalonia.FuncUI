namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module ListBoxItem =
    open Avalonia.Controls
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
    
    let create (attrs: IAttr<ListBoxItem> list): IView<ListBoxItem> =
        ViewBuilder.Create<ListBoxItem>(attrs)
     
    type ListBoxItem with

        static member isSelected<'t when 't :> ListBoxItem>(value: bool) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, bool>(ListBoxItem.IsSelectedProperty, value, ValueNone)

        static member onIsSelectedChanged<'t when 't :> ListBoxItem>(func: bool -> unit, ?subPatchOptions) =
            AttrBuilder.CreateSubscription<'t, bool>(ListBoxItem.IsSelectedProperty, func, ?subPatchOptions = subPatchOptions)