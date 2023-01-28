namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module HeaderedContentControl =
    open Avalonia.Controls.Primitives
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
    open Avalonia.Controls.Templates

    let create (attrs: Attr<HeaderedContentControl> list): View<HeaderedContentControl> =
        ViewBuilder.Create<HeaderedContentControl>(attrs)

    type HeaderedContentControl with
        static member header<'t when 't :> HeaderedContentControl>(text: string) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<string>(HeaderedContentControl.HeaderProperty, text, ValueNone)

        static member header<'t when 't :> HeaderedContentControl>(value: obj) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<obj>(HeaderedContentControl.HeaderProperty, value, ValueNone)

        static member header<'t when 't :> HeaderedContentControl>(value: IView option) : Attr<'t> =
            AttrBuilder<'t>.CreateContentSingle(HeaderedContentControl.HeaderProperty, value)

        static member header<'t when 't :> HeaderedContentControl>(value: IView) : Attr<'t> =
            value |> Some |> HeaderedContentControl.header

        static member headerTemplate<'t when 't :> HeaderedContentControl>(value: IDataTemplate) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<IDataTemplate>(HeaderedContentControl.HeaderTemplateProperty, value, ValueNone)