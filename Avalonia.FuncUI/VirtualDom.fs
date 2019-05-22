namespace Avalonia.FuncUI

open System.Collections.Generic
open System.Linq
open Avalonia
open Avalonia.FuncUI.Core
open Avalonia.Controls
open System

module VirtualDom =

    module Diff = 

        let diffAttrInfos (lastAttrs: AttrInfo list) (nextAttrs: AttrInfo list) : AttrInfo list = 
            let nextDict = Dictionary<string, AttrInfo>()
            let lastDict = Dictionary<string, AttrInfo>()

            for item in nextAttrs do
                nextDict.Add(item.Attr.Id, item)

            for item in lastAttrs do
                lastDict.Add(item.Attr.Id, item)

            let merged = Dictionary<string, AttrInfo>()

            for last in lastAttrs do
                if nextDict.ContainsKey(last.Attr.Id) then
                    let next = nextDict.[last.Attr.Id]
                    if next.Equals(last) |> not then
                        merged.Add(next.Attr.Id, next)
                    else ()                    
                else
                    let attr =
                        match last.Attr with
                        | Property property ->
                            Property { property with Value = AvaloniaProperty.UnsetValue }
                        | Event event ->
                            Event { event with Value = null }

                    merged.Add(last.Attr.Id, { last with Attr = attr })

            for next in nextAttrs do
                if lastDict.ContainsKey(next.Attr.Id) |> not then
                    merged.Add(next.Attr.Id, next)
                else ()

            merged.ToArray()
            |> Seq.map (fun pair -> pair.Value)
            |> Seq.toList

        let diff (last: ViewElement) (next: ViewElement) : ViewElement =
            ViewElement.create(next.ViewType, diffAttrInfos last.Attrs next.Attrs)

    module Patcher =

        let patchProperty (view: IControl) (attr: PropertyAttr) : unit =
            view.SetValue(attr.Property, attr.Value)

        let patch (view: IControl) (viewElement: ViewElement) : unit =
            for attr in viewElement.Attrs do
                match attr.Attr with 
                | Property property -> patchProperty view property
                | _ -> () // TODO: patch event
        
    module View =

        let create (e: ViewElement) : IControl =
            let view = Activator.CreateInstance(e.ViewType) :?> IControl
            Patcher.patch view e
            view

        let update (view: IControl) (last: ViewElement option) (next: ViewElement) =
            ()

