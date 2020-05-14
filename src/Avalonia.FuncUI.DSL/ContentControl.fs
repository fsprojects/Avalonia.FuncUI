namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module ContentControl =  
    open Avalonia.Controls
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
    open Avalonia.Controls.Templates
    open Avalonia.Layout

    let create (attrs : IAttr<ContentControl> list) : IView<ContentControl> =
        ViewBuilder.Create<ContentControl>(attrs)

    type ContentControl with
        static member content<'t when 't :> ContentControl>(text: string) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, string>(ContentControl.ContentProperty, text, ValueNone)
            
        static member content<'t when 't :> ContentControl>(value: obj) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, obj>(ContentControl.ContentProperty, value, ValueNone)
            
        static member content<'t when 't :> ContentControl>(value: IView option) : IAttr<'t> =
            AttrBuilder.CreateContentSingle(ContentControl.ContentProperty, value)
            
        static member content<'t when 't :> ContentControl>(value: IView) : IAttr<'t> =
            value |> Some |> ContentControl.content
            
        static member contentTemplate<'t when 't :> ContentControl>(value: IDataTemplate) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, IDataTemplate>(ContentControl.ContentTemplateProperty, value, ValueNone)

        static member horizontalAlignment<'t when 't :> ContentControl>(value: HorizontalAlignment) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, HorizontalAlignment>(ContentControl.HorizontalAlignmentProperty, value, ValueNone)
            
        static member verticalAlignment<'t when 't :> ContentControl>(value: VerticalAlignment) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, VerticalAlignment>(ContentControl.VerticalAlignmentProperty, value, ValueNone)
