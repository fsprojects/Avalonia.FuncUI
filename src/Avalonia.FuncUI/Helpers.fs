namespace Avalonia.FuncUI

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
            let style = StyleInclude(baseUri = null)
            style.Source <- Uri(source)
            this.Add(style)
            
module internal EqualityComparers =
    open Avalonia.Media

    let compareTransforms (t1: obj, t2: obj) =
        match t1, t2 with
        | :? ITransform as t1, (:? ITransform as t2) when t1.GetType() = t2.GetType() ->
            t1.Value.Equals(t2.Value)
        | _ -> false
