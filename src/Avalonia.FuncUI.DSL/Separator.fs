namespace Avalonia.FuncUI.DSL

open Avalonia.Media

[<AutoOpen>]
module Separator =
    open Avalonia.Controls.Primitives
    open Avalonia.Controls
    open Avalonia.Layout
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
   
    let create (attrs: IAttr<Separator> list): IView<Separator> =
        ViewBuilder.Create<Separator>(attrs)

    type Separator with            
