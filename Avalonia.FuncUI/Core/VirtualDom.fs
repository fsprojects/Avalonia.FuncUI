namespace Avalonia.FuncUI.Core

open System.Collections.Generic
open System.Linq
open Avalonia
open Avalonia.FuncUI.Core.Model
open Avalonia.Controls
open System

module VirtualDom =

    (* compute the diff and new state without using the actual UI objects *)
    module rec Diff = 

        let diffAttrInfos (lastAttrs: Attr list) (nextAttrs: Attr list) : Attr list = 
            let nextDict = Dictionary<string, Attr>()
            let lastDict = Dictionary<string, Attr>()

            for item in nextAttrs do
                nextDict.Add(item.Id, item)

            for item in lastAttrs do
                lastDict.Add(item.Id, item)

            let merged = Dictionary<string, Attr>()

            for last in lastAttrs do
                if nextDict.ContainsKey(last.Id) then
                    let next = nextDict.[last.Id]
                    if next <> last then
                        match next with
                        | Content content -> 
                            ()
                        | _ -> 
                            merged.Add(next.Id, next)
                    else ()                    
                else
                    let attr =
                        match last with
                        | Property property ->
                            Property { property with Value = AvaloniaProperty.UnsetValue }
                        | Content content ->
                            let emptyContent = match content.Content with
                            | ViewContent.Single single -> ViewContent.Single None
                            | ViewContent.Multiple mult -> ViewContent.Multiple []
                            Content { content with Content = emptyContent }
                        | Event event ->
                            Event { event with Value = null }

                    merged.Add(last.Id, attr)

            for next in nextAttrs do
                if lastDict.ContainsKey(next.Id) |> not then
                    merged.Add(next.Id, next)
                else ()

            merged.ToArray()
            |> Seq.map (fun pair -> pair.Value)
            |> Seq.toList

        let diff (last: ViewElement) (next: ViewElement) : ViewElement =
            ViewElement.create(next.ViewType, diffAttrInfos last.Attrs next.Attrs)

    (* patch UI elements to match their model (after diffing) *)
    module Patcher =

        let patchProperty (view: IControl) (attr: PropertyAttr) : unit =
            view.SetValue(attr.Property, attr.Value)

        let patch (view: IControl) (viewElement: ViewElement) : unit =
            for attr in viewElement.Attrs do
                match attr with 
                | Property property -> patchProperty view property
                | _ -> () // TODO: patch event

