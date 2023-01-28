namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module HeaderedSelectingItemsControl =
    open Avalonia.Controls.Primitives
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder

    let create (attrs: Attr<HeaderedSelectingItemsControl> list): IView<HeaderedSelectingItemsControl> =
        ViewBuilder.Create<HeaderedSelectingItemsControl>(attrs)

    type HeaderedSelectingItemsControl with
        static member header<'t when 't :> HeaderedSelectingItemsControl>(text: string) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<string>(HeaderedSelectingItemsControl.HeaderProperty, text, ValueNone)

        static member header<'t when 't :> HeaderedSelectingItemsControl>(value: obj) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<obj>(HeaderedSelectingItemsControl.HeaderProperty, value, ValueNone)

        static member header<'t when 't :> HeaderedSelectingItemsControl>(value: IView option) : Attr<'t> =
            AttrBuilder<'t>.CreateContentSingle(HeaderedSelectingItemsControl.HeaderProperty, value)

        static member header<'t when 't :> HeaderedSelectingItemsControl>(value: IView) : Attr<'t> =
            value |> Some |> HeaderedSelectingItemsControl.header