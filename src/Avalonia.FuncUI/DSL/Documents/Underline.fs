namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module Underline =
    open Avalonia.FuncUI.Builder
    open Avalonia.FuncUI.Types
    open Avalonia.Controls.Documents

    let create (attrs: Attr<Underline> list): View<Underline> =
        ViewBuilder.Create(attrs)

    let createText (text: string): View<Underline> =
        ViewBuilder.Create([
            Underline.inlines [
                Run.create [
                    Run.text text
                ] :> IView
            ]
        ])
