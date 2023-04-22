namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module InlineUIContainer =
    open Avalonia.FuncUI.Builder
    open Avalonia.FuncUI.Types
    open Avalonia.Controls.Documents

    let create (attrs : IAttr<InlineUIContainer> list) : IView<InlineUIContainer> =
        ViewBuilder.Create(attrs)

    type InlineUIContainer with
        static member child<'t when 't :> InlineUIContainer>(child : IView option) : IAttr<'t> =
            AttrBuilder<'t>.CreateContentSingle(InlineUIContainer.ChildProperty, child)

        static member child<'t when 't :> InlineUIContainer>(child : IView) : IAttr<'t> =
            InlineUIContainer.child(Some child)