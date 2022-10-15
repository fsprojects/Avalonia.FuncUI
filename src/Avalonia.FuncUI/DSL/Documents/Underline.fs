namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module Underline =  
    open Avalonia.FuncUI.Builder
    open Avalonia.FuncUI.Types
    open Avalonia.Controls.Documents

    let create (attrs: IAttr<Underline> list): IView<Underline> =
        ViewBuilder.Create(attrs)
        
    let simple (text: string): IView<Underline> =
        ViewBuilder.Create([
            Underline.inlines [
                Run.create [
                    Run.text text
                ] :> IView
            ]
        ])
