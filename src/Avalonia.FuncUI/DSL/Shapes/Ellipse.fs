namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module Ellipse =
    open Avalonia.Controls.Shapes
    open Avalonia.FuncUI.Builder
    open Avalonia.FuncUI.Types

    let create (attrs: Attr<Ellipse> list): IView<Ellipse> =
        ViewBuilder.Create<Ellipse>(attrs)