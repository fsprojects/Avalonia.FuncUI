namespace Avalonia.FuncUI

open System.Collections.Generic
open System.Linq
open Avalonia
open Avalonia.FuncUI.Core

module VirtualDom =

    module Diff = 

         
        let diffAttrInfos (lastAttrs: AttrInfo list) (nextAttrs: AttrInfo list) : AttrInfo list = 
            let nextDict = Dictionary<string, AttrInfo>()
            nextAttrs |> List.iter (fun i -> nextDict.Add(i.Attr.Id, i))

            let merged = Dictionary<string, AttrInfo>()
            lastAttrs |> List.iter (fun i -> 
                if nextDict.ContainsKey i.Attr.Id then
                    let next = nextDict.[i.Attr.Id]
                    if next.Equals(i) then
                        ()
                    else
                        merged.Add(next.Attr.Id, next)
                else
                    let n = { i with Attr = 
                        match i.Attr with
                        | Property property -> Property { property with Value = AvaloniaProperty.UnsetValue }
                        | Event event -> Event { event with Value = null }
                    }

                    merged.Add(i.Attr.Id, n)
            )

            nextAttrs |> List.iter (fun i ->
                if merged.ContainsKey(i.Attr.Id) |> not then
                    merged.Add(i.Attr.Id, i)
                else ()           
            )

            merged.ToArray()
            |> Seq.map (fun pair -> pair.Value)
            |> Seq.toList

        let diff (last: ViewElement) (next: ViewElement) =
            ViewElement.Create(next.ViewType, diffAttrInfos last.Attrs next.Attrs)
        
    module View =
        open Avalonia.Controls
        open System
        open Avalonia

        let create (e: ViewElement) : IControl =
            Activator.CreateInstance(e.ViewType) :?> IControl

        let update (view: IControl) (last: ViewElement option) (next: ViewElement) =
            ()

