namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module SelectingItemsControl =
    open Avalonia.Controls.Primitives
    open Avalonia.Controls
    
    open Avalonia.FuncUI.Types
    
    let create (attrs: IAttr<SelectingItemsControl> list): IView<SelectingItemsControl> =
        View.create<SelectingItemsControl>(attrs)
     
    type SelectingItemsControl with

        static member autoScrollToSelectedItem<'t when 't :> SelectingItemsControl>(value: bool) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty SelectingItemsControl.AutoScrollToSelectedItemProperty
            let property = Property.createDirect(accessor, value)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member selectedIndex<'t when 't :> SelectingItemsControl>(index: int) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty SelectingItemsControl.SelectedIndexProperty
            let property = Property.createDirect(accessor, index)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member onSelectedIndexChanged<'t when 't :> SelectingItemsControl>(func: int -> unit) =
            let subscription = Subscription.createFromProperty(SelectingItemsControl.SelectedIndexProperty, func)
            let attr = Attr.createSubscription<'t>(subscription)
            attr :> IAttr<'t>
            
        static member selectedItem<'t when 't :> SelectingItemsControl>(item: obj) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty SelectingItemsControl.SelectedItemProperty
            let property = Property.createDirect(accessor, item)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member onSelectedItemChanged<'t when 't :> SelectingItemsControl>(func: obj -> unit) =
            let subscription = Subscription.createFromProperty(SelectingItemsControl.SelectedItemProperty, func)
            let attr = Attr.createSubscription<'t>(subscription)
            attr :> IAttr<'t>
            
        static member onSelectionChanged<'t when 't :> SelectingItemsControl>(func: SelectionChangedEventArgs -> unit) =
            let subscription = Subscription.createFromRoutedEvent(SelectingItemsControl.SelectionChangedEvent, func)
            let attr = Attr.createSubscription<'t>(subscription)
            attr :> IAttr<'t>