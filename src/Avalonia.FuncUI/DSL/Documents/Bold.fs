namespace Avalonia.FuncUI.DSL

open Avalonia.FuncUI.Types

[<AutoOpen>]
module Bold =  
    open Avalonia.FuncUI.Builder
    open Avalonia.Controls.Documents

    let create (attrs: IAttr<Bold> list): IView<Bold> =
        ViewBuilder.Create(attrs)
        
    let simple (text: string): IView<Bold> =
        ViewBuilder.Create([
            Bold.inlines [
                Run.create [
                    Run.text text
                ] :> IView
            ]
        ])
