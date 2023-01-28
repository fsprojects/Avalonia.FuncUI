namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module Span =
    open Avalonia.FuncUI.Builder
    open Avalonia.FuncUI.Types
    open Avalonia.Controls.Documents

    let create (attrs: Attr<Span> list): View<Span> =
        ViewBuilder.Create(attrs)

    type Span with
        static member inlines<'t when 't :> Span>(value: InlineCollection) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<InlineCollection>(Span.InlinesProperty, value, ValueNone)

        static member inlines<'t when 't :> Span>(values: IView list (* TODO: Change to View<Inline> *)) : Attr<'t> =
            let getter : ('t -> obj) = (fun control -> control.Inlines :> obj)
            AttrBuilder<'t>.CreateContentMultiple("Inlines", ValueSome getter, ValueNone, values)
