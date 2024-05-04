namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module HeaderedSelectingItemsControl =
    open Avalonia.Controls.Primitives
    open Avalonia.Controls.Templates
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
    
    let create (attrs: IAttr<HeaderedSelectingItemsControl> list): IView<HeaderedSelectingItemsControl> =
        ViewBuilder.Create<HeaderedSelectingItemsControl>(attrs)
    
    type HeaderedSelectingItemsControl with
        static member header<'t when 't :> HeaderedSelectingItemsControl>(text: string) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<string>(HeaderedSelectingItemsControl.HeaderProperty, text, ValueNone)
            
        static member header<'t when 't :> HeaderedSelectingItemsControl>(value: obj) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<obj>(HeaderedSelectingItemsControl.HeaderProperty, value, ValueNone)
            
        static member header<'t when 't :> HeaderedSelectingItemsControl>(value: IView option) : IAttr<'t> =
            AttrBuilder<'t>.CreateContentSingle(HeaderedSelectingItemsControl.HeaderProperty, value)
            
        static member header<'t when 't :> HeaderedSelectingItemsControl>(value: IView) : IAttr<'t> =
            value |> Some |> HeaderedSelectingItemsControl.header

        static member headerTemplate<'t when 't :> HeaderedSelectingItemsControl>(value: IDataTemplate) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<IDataTemplate>(HeaderedSelectingItemsControl.HeaderTemplateProperty, value, ValueNone)
