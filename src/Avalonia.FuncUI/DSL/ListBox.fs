namespace Avalonia.FuncUI.DSL
open System.Collections

[<AutoOpen>]
module ListBox =
    open System
    open System.Threading
    open System.Windows.Input 
    
    open Avalonia.Controls
    open Avalonia.Interactivity
    open Avalonia.Input
    
    open Avalonia.FuncUI.Types
    
    let create (attrs: IAttr<ListBox> list): IView<ListBox> =
        View.create<ListBox>(attrs)
     
    type ListBox with

        static member selectedItems<'t when 't :> ListBox>(item: IList) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty ListBox.SelectedItemsProperty
            let property = Property.createDirect(accessor, item)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>

        static member onSelectedItemsChanged<'t when 't :> ListBox>(func: IList -> unit) =
            let subscription = Subscription.createFromProperty(ListBox.SelectedItemsProperty, func)
            let attr = Attr.createSubscription<'t>(subscription)
            attr :> IAttr<'t>
        
        static member selectionMode<'t when 't :> ListBox>(mode: SelectionMode) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty ListBox.SelectionModeProperty
            let property = Property.createDirect(accessor, mode)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
        
        static member virtualizationMode<'t when 't :> ListBox>(mode: ItemVirtualizationMode) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty ListBox.VirtualizationModeProperty
            let property = Property.createDirect(accessor, mode)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>