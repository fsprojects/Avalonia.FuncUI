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

module internal Setters =
    open Avalonia.Collections
    open Avalonia.Controls

    /// Update list with minimal CollectionChanged.
    let avaloniaList<'t when 't : equality> (list: IAvaloniaList<'t>) (newItems: seq<'t>) =
        if Seq.isEmpty newItems then
            list.Clear()
        else if list.Count = 0 then
            list.AddRange(newItems)
        else
            list |> Seq.except newItems |> list.RemoveAll
            
            for newIndex, newItem in Seq.indexed newItems do
                let oldIndex = list |> Seq.tryFindIndex ((=) newItem)
                
                match oldIndex with
                | Some oldIndex when oldIndex = newIndex -> ()
                | Some oldIndex -> list.Move(oldIndex, newIndex)
                | None -> list.Insert(newIndex, newItem)


module internal EqualityComparers =
    open System.Linq
    open Avalonia.Media

    let compareTransforms (t1: obj, t2: obj) =
        match t1, t2 with
        | :? ITransform as t1, (:? ITransform as t2) when t1.GetType() = t2.GetType() ->
            t1.Value.Equals(t2.Value)
        | _ -> false

    let compareSeq<'e,'t when 'e :> 't seq> (a: obj, b: obj) =
        Enumerable.SequenceEqual(a :?> 'e, b :?> 'e)
