﻿namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module WrapPanel =
    open Avalonia.Controls
    open Avalonia.Layout
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder

    let create (attrs: Attr<WrapPanel> list): IView<WrapPanel> =
        ViewBuilder.Create<WrapPanel>(attrs)

    type WrapPanel with
        static member itemHeight<'t when 't :> WrapPanel>(value: float) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<float>(WrapPanel.ItemHeightProperty, value, ValueNone)

        static member itemWidth<'t when 't :> WrapPanel>(value: float) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<float>(WrapPanel.ItemWidthProperty, value, ValueNone)

        static member orientation<'t when 't :> WrapPanel>(orientation: Orientation) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<Orientation>(WrapPanel.OrientationProperty, orientation, ValueNone)