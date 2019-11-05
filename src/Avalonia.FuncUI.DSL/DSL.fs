namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module Helpers =
    open Avalonia.FuncUI.Types
    
    let generalize (view: IView<'t>) : IView =
        view :> IView
        
        
[<AutoOpen>]
module AvaloniaExtensions =
    open Avalonia.Markup.Xaml.Styling
    open System
    open Avalonia.Styling

    type Styles with
        member this.Load (source: string) = 
            let style = new StyleInclude(baseUri = null)
            style.Source <- new Uri(source)
            this.Add(style)