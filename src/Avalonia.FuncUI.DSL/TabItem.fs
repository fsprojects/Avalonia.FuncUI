namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module TabItem =
    open Avalonia.Controls
    open Avalonia.FuncUI.Types
    
    let create (attrs: IAttr<TabItem> list): IView<TabItem> =
        View.create<TabItem>(attrs)
     
    type TabItem with

        static member tabStripPlacement<'t when 't :> TabItem>(placement: Dock) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty TabItem.TabStripPlacementProperty
            let property = Property.createDirect(accessor, placement)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
        
        static member isSelected<'t when 't :> TabItem>(value: bool) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty TabItem.IsSelectedProperty
            let property = Property.createDirect(accessor, value)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member onIsSelectedChanged<'t when 't :> TabItem>(func: bool -> unit) =
            let subscription = Subscription.createFromProperty(TabItem.IsSelectedProperty, func)
            let attr = Attr.createSubscription<'t>(subscription)
            attr :> IAttr<'t>