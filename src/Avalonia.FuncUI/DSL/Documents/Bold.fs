namespace Avalonia.FuncUI.DSL

open Avalonia.FuncUI.Types

[<AutoOpen>]
module Bold =
    open Avalonia.FuncUI.Builder
    open Avalonia.Controls.Documents

    let create (attrs: Attr<Bold> list): View<Bold> =
        ViewBuilder.Create(attrs)

    let createText (text: string): View<Bold> =
        ViewBuilder.Create([
            Bold.inlines [
                Run.create [
                    Run.text text
                ] :> IView
            ]
        ])
