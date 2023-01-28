namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module Italic =
    open Avalonia.FuncUI.Builder
    open Avalonia.FuncUI.Types
    open Avalonia.Controls.Documents

    let create (attrs: Attr<Italic> list): IView<Italic> =
        ViewBuilder.Create(attrs)

    let createText (text: string): IView<Italic> =
        ViewBuilder.Create([
            Italic.inlines [
                Run.create [
                    Run.text text
                ] :> IView
            ]
        ])
