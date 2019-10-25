namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module HeaderedContentControl =
    open Avalonia.Controls.Primitives
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
    open Avalonia.Controls.Templates
     
    type HeaderedContentControl with
        static member header<'t when 't :> HeaderedContentControl>(text: string) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<string>(HeaderedContentControl.HeaderProperty, text, ValueNone)
            
        static member header<'t when 't :> HeaderedContentControl>(value: obj) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<obj>(HeaderedContentControl.HeaderProperty, value, ValueNone)
            
        static member header<'t when 't :> HeaderedContentControl>(value: IView option) : IAttr<'t> =
            AttrBuilder<'t>.CreateContentSingle(HeaderedContentControl.HeaderProperty, value)
            
        static member header<'t when 't :> HeaderedContentControl>(value: IView) : IAttr<'t> =
            value |> Some |> HeaderedContentControl.header
            
        static member headerTemplate<'t when 't :> HeaderedContentControl>(value: IDataTemplate) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<IDataTemplate>(HeaderedContentControl.HeaderTemplateProperty, value, ValueNone)