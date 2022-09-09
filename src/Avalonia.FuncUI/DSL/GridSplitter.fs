namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module GridSplitter =
    open Avalonia.Controls
    open Avalonia.FuncUI.Builder
    open Avalonia.FuncUI.Types
     
    let create (attrs: IAttr<GridSplitter> list): IView<GridSplitter> =
        ViewBuilder.Create<GridSplitter>(attrs)
     
    type GridSplitter with
        end