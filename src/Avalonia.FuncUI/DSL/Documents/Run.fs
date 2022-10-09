namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module Run =  
    open Avalonia.FuncUI.Builder
    open Avalonia.FuncUI.Types
    open Avalonia.Media
    open Avalonia.Controls.Documents

    let create (attrs: IAttr<Run> list): IInline =
        InlineBuilder.Create<Run>(attrs)
    
    type Run with
        static member text<'t when 't :> Run>(value: string) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<string>(Run.TextProperty, value, ValueNone)
            
        static member background<'t when 't :> Run>(value: IBrush) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<IBrush>(Run.BackgroundProperty, value, ValueNone)