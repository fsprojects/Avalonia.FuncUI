namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module ListBoxItem =
    open Avalonia.Controls
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
    
    let create (attrs: IAttr<ListBoxItem> list): IView<ListBoxItem> =
        View.create<ListBoxItem>(attrs)
     
    type ListBoxItem with

        static member isSelected<'t when 't :> ListBoxItem>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(ListBoxItem.IsSelectedProperty, value, ValueNone)

        static member onIsSelectedChanged<'t when 't :> ListBoxItem>(func: bool -> unit) =
            AttrBuilder<'t>.CreateSubscription<bool>(ListBoxItem.IsSelectedProperty, func)