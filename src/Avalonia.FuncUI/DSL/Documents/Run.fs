namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module Run =
    open Avalonia.FuncUI.Builder
    open Avalonia.FuncUI.Types
    open Avalonia.Controls.Documents

    let create (attrs: Attr<Run> list) : IView<Run> = ViewBuilder.Create(attrs)

    type Run with
        static member text<'t when 't :> Run>(value: string) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<string>(Run.TextProperty, value, ValueNone)

    let createText (text: string) : IView<Run> = ViewBuilder.Create([ Run.text text ])
