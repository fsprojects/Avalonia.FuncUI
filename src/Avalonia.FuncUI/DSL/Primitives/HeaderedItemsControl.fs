namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module HeaderedItemsControl =
    open Avalonia.Controls.Primitives
    open Avalonia.Controls.Templates
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
    
    let create (attrs: IAttr<HeaderedItemsControl> list): IView<HeaderedItemsControl> =
        ViewBuilder.Create<HeaderedItemsControl>(attrs)
            
    type HeaderedItemsControl with
    
        static member header<'t when 't :> HeaderedItemsControl>(text: string) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<string>(HeaderedItemsControl.HeaderProperty, text, ValueNone)
            
        static member header<'t when 't :> HeaderedItemsControl>(value: obj) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<obj>(HeaderedItemsControl.HeaderProperty, value, ValueNone)
            
        static member header<'t when 't :> HeaderedItemsControl>(value: IView option) : IAttr<'t> =
            AttrBuilder<'t>.CreateContentSingle(HeaderedItemsControl.HeaderProperty, value)
            
        static member header<'t when 't :> HeaderedItemsControl>(value: IView) : IAttr<'t> =
            value |> Some |> HeaderedItemsControl.header

        static member headerTemplate<'t when 't :> HeaderedItemsControl>(value: IDataTemplate) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<IDataTemplate>(HeaderedItemsControl.HeaderTemplateProperty, value, ValueNone)
