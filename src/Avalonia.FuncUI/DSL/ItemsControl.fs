namespace Avalonia.FuncUI.DSL
open Avalonia.Controls.Templates
open System.Collections

[<AutoOpen>]
module ItemsControl =
    open System
    open System.Threading
    open System.Windows.Input 
    
    open Avalonia.Controls
    open Avalonia.Interactivity
    open Avalonia.Input
    
    open Avalonia.FuncUI.Types
    
    let create (attrs: IAttr<ItemsControl> list): IView<ItemsControl> =
        View.create<ItemsControl>(attrs)
     
    type ItemsControl with

        static member items<'t when 't :> ItemsControl>(value: IEnumerable) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty ItemsControl.ItemsProperty
            let property = Property.createDirect(accessor, value)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member itemsPanel<'t when 't :> ItemsControl>(value: ITemplate<IPanel>) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty ItemsControl.ItemsPanelProperty
            let property = Property.createDirect(accessor, value)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member itemTemplate<'t when 't :> ItemsControl>(value: IDataTemplate) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty ItemsControl.ItemTemplateProperty
            let property = Property.createDirect(accessor, value)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>