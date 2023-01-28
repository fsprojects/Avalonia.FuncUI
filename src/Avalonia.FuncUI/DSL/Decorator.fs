namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module Decorator =
    open Avalonia
    open Avalonia.Controls
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder

    let create (attrs: Attr<Decorator> list): View<Decorator> =
        ViewBuilder.Create<Decorator>(attrs)

    type Decorator with

        static member child<'t when 't :> Decorator>(value: IControl) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty(Decorator.ChildProperty, value, ValueNone)

        static member child<'t when 't :> Decorator>(value: IView option) : Attr<'t> =
            AttrBuilder<'t>.CreateContentSingle(Decorator.ChildProperty, value)

        static member child<'t when 't :> Decorator>(value: IView) : Attr<'t> =
            value |> Some |> Decorator.child

        static member padding<'t when 't :> Decorator>(value: Thickness) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<Thickness>(Decorator.PaddingProperty, value, ValueNone)

        static member padding<'t when 't :> Decorator>(value: float) : Attr<'t> =
            Thickness(value) |> Decorator.padding

        static member padding<'t when 't :> Decorator>(horizontal: float, vertical: float) : Attr<'t> =
            Thickness(horizontal, vertical) |> Decorator.padding

        static member padding<'t when 't :> Decorator>(left: float, top: float, right: float, bottom: float) : Attr<'t> =
            Thickness(left, top, right, bottom) |> Decorator.padding