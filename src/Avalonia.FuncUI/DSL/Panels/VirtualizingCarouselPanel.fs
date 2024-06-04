namespace Avalonia.FuncUI.DSL

open Avalonia.Controls

[<AutoOpen>]
module VirtualizingCarouselPanel =
    open Avalonia.FuncUI.Types

    let create (attrs: IAttr<VirtualizingCarouselPanel> list): IView<VirtualizingCarouselPanel> =
        ViewBuilder.Create<VirtualizingCarouselPanel>(attrs)

    type VirtualizingCarouselPanel with
        end
