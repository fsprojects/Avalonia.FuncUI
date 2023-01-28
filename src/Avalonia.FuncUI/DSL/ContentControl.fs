namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module ContentControl =
    open Avalonia.Controls
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
    open Avalonia.Controls.Templates
    open Avalonia.Layout

    let create (attrs : Attr<ContentControl> list) : View<ContentControl> =
        ViewBuilder.Create<ContentControl>(attrs)

    type ContentControl with
        static member content<'t when 't :> ContentControl>(text: string) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<string>(ContentControl.ContentProperty, text, ValueNone)

        static member content<'t when 't :> ContentControl>(value: obj) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<obj>(ContentControl.ContentProperty, value, ValueNone)

        static member content<'t when 't :> ContentControl>(value: IView voption) : Attr<'t> =
            AttrBuilder<'t>.CreateContentSingle(ContentControl.ContentProperty, value)

        static member content<'t when 't :> ContentControl>(value: IView) : Attr<'t> =
            value
            |> Some
            |> ContentControl.content

        static member contentTemplate<'t when 't :> ContentControl>(value: IDataTemplate) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<IDataTemplate>(ContentControl.ContentTemplateProperty, value, ValueNone)

        static member horizontalAlignment<'t when 't :> ContentControl>(value: HorizontalAlignment) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<HorizontalAlignment>(ContentControl.HorizontalAlignmentProperty, value, ValueNone)

        static member verticalAlignment<'t when 't :> ContentControl>(value: VerticalAlignment) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<VerticalAlignment>(ContentControl.VerticalAlignmentProperty, value, ValueNone)

        static member horizontalContentAlignment<'t when 't :> ContentControl>(value: HorizontalAlignment) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<HorizontalAlignment>(ContentControl.HorizontalContentAlignmentProperty, value, ValueNone)

        static member verticalContentAlignment<'t when 't :> ContentControl>(value: VerticalAlignment) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<VerticalAlignment>(ContentControl.VerticalContentAlignmentProperty, value, ValueNone)
