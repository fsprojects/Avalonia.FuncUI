namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module HeaderedItemsControl =
    open Avalonia.Controls.Primitives
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
    
    let create (attrs: IAttr<HeaderedItemsControl> list): IView<HeaderedItemsControl> =
        ViewBuilder.Create<HeaderedItemsControl>(attrs)
            
    type HeaderedItemsControl with
    
        static member header<'t when 't :> HeaderedItemsControl>(text: string) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, string>(HeaderedItemsControl.HeaderProperty, text, ValueNone)
            
        static member header<'t when 't :> HeaderedItemsControl>(value: obj) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, obj>(HeaderedItemsControl.HeaderProperty, value, ValueNone)
            
        static member header<'t when 't :> HeaderedItemsControl>(value: IView option) : IAttr<'t> =
            AttrBuilder.CreateContentSingle(HeaderedItemsControl.HeaderProperty, value)
            
        static member header<'t when 't :> HeaderedItemsControl>(value: IView) : IAttr<'t> =
            value |> Some |> HeaderedItemsControl.header