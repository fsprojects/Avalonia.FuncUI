namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module ItemsRepeater =
    open System.Collections
    open Avalonia.Controls
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
    open Avalonia.Controls.Templates
    
    let create (attrs : IAttr<ItemsRepeater> list): IView<ItemsRepeater> =
        ViewBuilder.Create<ItemsRepeater>(attrs)
        
    type ItemsRepeater with        
        static member viewItems<'t when 't :> ItemsRepeater>(views: List<IView>): IAttr<'t> =
            AttrBuilder<'t>.CreateContentMultiple(ItemsRepeater.ItemsProperty, views)
        
        static member dataItems<'t when 't :> ItemsRepeater>(data : IEnumerable): IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<IEnumerable>(ItemsRepeater.ItemsProperty, data, ValueNone)
            
        static member itemTemplate<'t when 't :> ItemsRepeater>(value : IDataTemplate): IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<IDataTemplate>(ItemsRepeater.ItemTemplateProperty, value, ValueNone)
