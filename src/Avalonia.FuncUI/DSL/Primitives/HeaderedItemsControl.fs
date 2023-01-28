namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module HeaderedItemsControl =
    open Avalonia.Controls.Primitives
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder

    let create (attrs: Attr<HeaderedItemsControl> list): View<HeaderedItemsControl> =
        ViewBuilder.Create<HeaderedItemsControl>(attrs)

    type HeaderedItemsControl with

        static member header<'t when 't :> HeaderedItemsControl>(text: string) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<string>(HeaderedItemsControl.HeaderProperty, text, ValueNone)

        static member header<'t when 't :> HeaderedItemsControl>(value: obj) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<obj>(HeaderedItemsControl.HeaderProperty, value, ValueNone)

        static member header<'t when 't :> HeaderedItemsControl>(value: IView option) : Attr<'t> =
            AttrBuilder<'t>.CreateContentSingle(HeaderedItemsControl.HeaderProperty, value)

        static member header<'t when 't :> HeaderedItemsControl>(value: IView) : Attr<'t> =
            value |> Some |> HeaderedItemsControl.header