namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module ListBoxItem =
    open Avalonia.Controls
    open Avalonia.FuncUI.Types
    
    let create (attrs: IAttr<ListBoxItem> list): IView<ListBoxItem> =
        View.create<ListBoxItem>(attrs)
     
    type ListBoxItem with

        static member isSelected<'t when 't :> ListBoxItem>(value: bool) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty ListBoxItem.IsSelectedProperty
            let property = Property.createDirect(accessor, value)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>

        static member onIsSelectedChanged<'t when 't :> ListBoxItem>(func: bool -> unit) =
            let subscription = Subscription.createFromProperty(ListBoxItem.IsSelectedProperty, func)
            let attr = Attr.createSubscription<'t>(subscription)
            attr :> IAttr<'t>