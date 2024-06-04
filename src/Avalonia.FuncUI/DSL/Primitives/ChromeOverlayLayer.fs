namespace Avalonia.FuncUI.DSL

open Avalonia.Controls.Primitives

[<AutoOpen>]
module ChromeOverlayLayer =
    open Avalonia.FuncUI.Types

    let create (attrs: IAttr<ChromeOverlayLayer> list): IView<ChromeOverlayLayer> =
        ViewBuilder.Create<ChromeOverlayLayer>(attrs)

    type ChromeOverlayLayer with
        end
