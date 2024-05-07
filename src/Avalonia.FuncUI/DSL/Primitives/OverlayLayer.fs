namespace Avalonia.FuncUI.DSL

open Avalonia.Controls.Primitives

[<AutoOpen>]
module OverlayLayer =
    open Avalonia.FuncUI.Types

    let create (attrs: IAttr<OverlayLayer> list): IView<OverlayLayer> =
        ViewBuilder.Create<OverlayLayer>(attrs)

    type OverlayLayer with
        end