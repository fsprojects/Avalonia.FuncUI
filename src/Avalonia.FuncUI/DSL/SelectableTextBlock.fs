namespace Avalonia.FuncUI.DSL


[<AutoOpen>]
module SelectableTextBlock =
    open Avalonia.Controls
    open Avalonia.FuncUI.Builder
    open Avalonia.FuncUI.Types

    let create (attrs: Attr<SelectableTextBlock> list): IView<SelectableTextBlock> =
        ViewBuilder.Create<SelectableTextBlock>(attrs)
