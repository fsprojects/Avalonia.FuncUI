namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module LineBreak =
    open Avalonia.FuncUI.Builder
    open Avalonia.FuncUI.Types
    open Avalonia.Controls.Documents

    let create (attrs: Attr<LineBreak> list): IView<LineBreak> =
        ViewBuilder.Create(attrs)

    /// Creates a simple line-break with no attributes.
    let simple : IView<LineBreak> =
        create([])
