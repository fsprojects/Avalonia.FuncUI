namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module Rectangle =
    open Avalonia.Controls.Shapes
    open Avalonia.FuncUI.Builder
    open Avalonia.FuncUI.Types
    
    let create (attrs: IAttr<Rectangle> list): IView<Rectangle> =
        ViewBuilder.Create<Rectangle>(attrs)