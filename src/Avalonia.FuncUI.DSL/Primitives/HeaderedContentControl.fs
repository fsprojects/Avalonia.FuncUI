namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module HeaderedContentControl =
    open Avalonia.Controls.Primitives
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
    open Avalonia.Controls.Templates
     
    let create (attrs: IAttr<HeaderedContentControl> list): IView<HeaderedContentControl> =
        ViewBuilder.Create<HeaderedContentControl>(attrs)
     
    type HeaderedContentControl with
        static member header<'t when 't :> HeaderedContentControl>(text: string) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, string>(HeaderedContentControl.HeaderProperty, text, ValueNone)
            
        static member header<'t when 't :> HeaderedContentControl>(value: obj) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, obj>(HeaderedContentControl.HeaderProperty, value, ValueNone)
            
        static member header<'t when 't :> HeaderedContentControl>(value: IView option) : IAttr<'t> =
            AttrBuilder.CreateContentSingle(HeaderedContentControl.HeaderProperty, value)
            
        static member header<'t when 't :> HeaderedContentControl>(value: IView) : IAttr<'t> =
            value |> Some |> HeaderedContentControl.header
            
        static member headerTemplate<'t when 't :> HeaderedContentControl>(value: IDataTemplate) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, IDataTemplate>(HeaderedContentControl.HeaderTemplateProperty, value, ValueNone)