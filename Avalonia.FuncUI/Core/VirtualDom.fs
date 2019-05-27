namespace Avalonia.FuncUI.Core

open System.Collections.Generic
open System.Linq
open Avalonia
open Avalonia.FuncUI.Core.Model
open Avalonia.FuncUI.Core.Lib
open Avalonia.Controls
open System
open System.Reflection

module rec VirtualDom =

    (* compute the diff and new state without using the actual UI objects *)
    module Differ = 

        module AttrDiffer =

            let diffContentSingle (last: ViewElement option) (next: ViewElement option) : ViewElement option =
                match next with
                | Some next -> 
                    match last with
                    | Some last -> Some (Differ.diff last next)
                    | None -> Some next
                | None -> None

            let diffContentMultiple (lastList: ViewElement list) (nextList: ViewElement list) : ViewElement list =
                let merged =
                    nextList
                    |> List.mapi (fun index next -> 
                        if index + 1 <= lastList.Length then
                            Differ.diff lastList.[index] next
                        else next
                    )
                merged
                    

            let diffContent (last: ContentAttr) (next: ContentAttr) : Attr =
                let viewContent : ViewContent =
                    match next.Content with
                    | ViewContent.Single next -> 
                        match last.Content with
                        | ViewContent.Single last -> ViewContent.Single (diffContentSingle last next)
                        | _ -> ViewContent.Single (diffContentSingle None next)
                    | ViewContent.Multiple next -> 
                        match last.Content with
                        | ViewContent.Multiple last -> ViewContent.Multiple (diffContentMultiple last next)
                        | _ -> ViewContent.Multiple (diffContentMultiple [] next)

                Attr.createContent (next.PropertyName, viewContent)
                
                        

        let diffAttrs (lastAttrs: Attr list) (nextAttrs: Attr list) : Attr list = 
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
                            match last with
                            | Content last -> 
                                merged.Add(next.Id, AttrDiffer.diffContent last content)
                            | _ -> 
                                merged.Add(next.Id, next)
                        | _ -> 
                            merged.Add(next.Id, next)
                    else ()                    
                else
                    let attr =
                        match last with
                        | Property property ->
                            Property { property with Value = AvaloniaProperty.UnsetValue }
                        | Content content ->
                            let emptyContent =
                                match content.Content with
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
            ViewElement.create(next.ViewType, diffAttrs last.Attrs next.Attrs)

    (* patch UI elements to match their model (after diffing) *)
    module Patcher =

        module AttrPatcher =
         
            let patchProperty (view: IControl) (attr: PropertyAttr) : unit =
                view.SetValue(attr.Property, attr.Value)

            let patchContentSingle (view: IControl) (prop: PropertyInfo) (viewElement: ViewElement option) =
                // TODO: handle all possible cases
                // no setter / no getter
                match viewElement with
                | Some viewElement -> 
                    let value = prop.GetValue(view)
                    if value <> null then
                        Patcher.patch (value :?> IControl) viewElement
                    else
                        let contentView = create viewElement
                        Patcher.patch contentView viewElement
                        prop.SetValue(view, contentView)
                | None ->
                    prop.SetValue(view, null)

            let patchContentMultiple (view: IControl) (prop: PropertyInfo) (viewElementList: ViewElement list) =
                let controls = prop.GetValue(view) :?> Controls

                if List.isEmpty viewElementList then
                    controls.Clear()
                else
                    let mutable index = 0
                    for viewElement in viewElementList do  
                        // try patch / reuse
                        if index + 1 <= controls.Count then
                            let item = controls.[index]

                            if item.GetType() = viewElement.ViewType then
                                // patch
                                patch item viewElement
                            else
                                // replace
                                let newItem = create viewElement
                                patch newItem viewElement
                                controls.[index] <- newItem
                        else
                            // create
                            let newItem = create viewElement
                            patch newItem viewElement
                            controls.Add(newItem)

                        index <- index + 1

                    // remove elements if list is to long
                    if (index + 1) < controls.Count then
                        controls.RemoveRange(index + 1, controls.Count - index + 1)

            let patchContent (view: IControl) (attr: ContentAttr) : unit =
                let prop = Reflection.findPropertyByName view attr.PropertyName
                match attr.Content with
                | ViewContent.Single single -> patchContentSingle view prop single
                | ViewContent.Multiple multiple -> patchContentMultiple view prop multiple

        let patch (view: IControl) (viewElement: ViewElement) : unit =
            for attr in viewElement.Attrs do
                match attr with 
                | Property property -> AttrPatcher.patchProperty view property
                | Content content -> AttrPatcher.patchContent view content
                | _ -> () // TODO: patch event

    let create (viewElement: ViewElement): IControl =
         Activator.CreateInstance(viewElement.ViewType) :?> IControl

