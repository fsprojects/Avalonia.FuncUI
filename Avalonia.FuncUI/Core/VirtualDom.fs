namespace Avalonia.FuncUI

open System
open System.Reflection
open Avalonia.FuncUI
open Avalonia.FuncUI.Lib
open System.Collections
open System.Linq

module internal rec VirtualDom =
    open Types

    module Delta =
        
        type PropertyAttrDelta =
            {
                Name : string
                Value : obj option
            }
            static member From (property: PropertyAttr) : PropertyAttrDelta =
                {
                    Name = property.Name
                    Value = Some property.Value
                }

        type EventAttrDelta =
            {
                Name : string
                OldValue : Delegate
                NewValue : Delegate
            }
            static member From (event: EventAttr) : EventAttrDelta =
                {
                    Name = event.Name
                    OldValue = null
                    NewValue = event.Value
                }

        type ViewContentDelta =
            | Single of ViewDelta option
            | Multiple of ViewDelta list
            static member From (viewContent: ViewContent) : ViewContentDelta =
                match viewContent with
                | ViewContent.Single single -> 
                    match single with 
                    | Some single -> ViewContentDelta.Single (Some (ViewDelta.From single))
                    | None -> ViewContentDelta.Single None
                | ViewContent.Multiple multiple ->
                    ViewContentDelta.Multiple (multiple |> List.map ViewDelta.From)

        type ContentAttrDelta =
            {
                Name : string
                Content : ViewContentDelta
            }
            static member From (content: ContentAttr) : ContentAttrDelta =
                {
                    Name = content.Name
                    Content = ViewContentDelta.From content.Content
                }

        type AttrDelta =
            | PropertyDelta of PropertyAttrDelta
            | EventDelta of EventAttrDelta
            | ContentDelta of ContentAttrDelta
            static member From (attr: Attr) : AttrDelta =
                match attr with
                | Property delta -> AttrDelta.PropertyDelta (PropertyAttrDelta.From delta)
                | Event delta -> AttrDelta.EventDelta (EventAttrDelta.From delta)
                | Content delta -> AttrDelta.ContentDelta (ContentAttrDelta.From delta)
                
        type ViewDelta =
            {
                ViewType : Type
                Attrs : AttrDelta list
            }
            static member From (view: View) : ViewDelta =
                {
                    ViewType = view.ViewType
                    Attrs = view.Attrs |> List.map AttrDelta.From
                }

    module Differ =
        open Delta

        module AttrDiffer =

            let propertyDiffer (last: Attr list, next: Attr list) =

                (* filter for properties *)
                let selectProperties (attrs: Attr list) =
                    attrs
                    |> List.map (fun attr ->
                        match attr with
                        | Attr.Property property -> Some property
                        | _ -> None
                    )
                    |> List.choose id

                (* contains *)
                let contains (attrs: PropertyAttr list, name: string) : bool =
                    attrs |> List.exists (fun attr -> attr.Name = name)

                (* find *)
                let find (attrs: PropertyAttr list, name: string) : PropertyAttr =
                    attrs |> List.find (fun attr -> attr.Name = name)

                let lastAttrs = selectProperties last
                let nextAttrs = selectProperties next
                let delta = new System.Collections.Generic.List<AttrDelta>()

                for lastAttr in lastAttrs do
                    if contains(nextAttrs, lastAttr.Name) then
                        let nextAttr = find(nextAttrs, lastAttr.Name)

                        if nextAttr <> lastAttr then
                            // update
                            delta.Add(AttrDelta.PropertyDelta {
                                Name = nextAttr.Name
                                Value = Some nextAttr.Value
                            })

                    else
                        // reset
                        delta.Add(AttrDelta.PropertyDelta {
                            Name = lastAttr.Name
                            Value = None
                        })

                for nextAttr in nextAttrs do
                    if contains(lastAttrs, nextAttr.Name) |> not then
                        delta.Add(AttrDelta.PropertyDelta (PropertyAttrDelta.From nextAttr))

                List.ofSeq delta 

            let eventDiffer (last: Attr list, next: Attr list) =

                (* filter for events *)
                let selectEvents (attrs: Attr list) =
                    attrs
                    |> List.map (fun attr ->
                        match attr with
                        | Attr.Event event -> Some event
                        | _ -> None
                    )
                    |> List.choose id

                (* contains *)
                let contains (attrs: EventAttr list, name: string) : bool =
                    attrs |> List.exists (fun attr -> attr.Name = name)

                (* find *)
                let find (attrs: EventAttr list, name: string) : EventAttr =
                    attrs |> List.find (fun attr -> attr.Name = name)

                let lastAttrs = selectEvents last
                let nextAttrs = selectEvents next
                let delta = new System.Collections.Generic.List<AttrDelta>()

                for lastAttr in lastAttrs do
                    if contains(nextAttrs, lastAttr.Name) then
                        let nextAttr = find(nextAttrs, lastAttr.Name)

                        if nextAttr <> lastAttr then
                            // update
                            delta.Add(AttrDelta.EventDelta {
                                Name = nextAttr.Name
                                OldValue = lastAttr.Value // used to unsubscribe
                                NewValue = nextAttr.Value
                            })

                    else
                        // reset
                        delta.Add(AttrDelta.EventDelta {
                            Name = lastAttr.Name
                            OldValue = lastAttr.Value // used to unsubscribe
                            NewValue = null
                        })

                for nextAttr in nextAttrs do
                    if contains(lastAttrs, nextAttr.Name) |> not then
                        delta.Add(AttrDelta.EventDelta (EventAttrDelta.From nextAttr))

                List.ofSeq delta 

            let contentDiffer (last: Attr list, next: Attr list) =

                (* filter for events *)
                let selectContent (attrs: Attr list) =
                    attrs
                    |> List.map (fun attr ->
                        match attr with
                        | Attr.Content content -> Some content
                        | _ -> None
                    )
                    |> List.choose id

                (* contains *)
                let contains (attrs: ContentAttr list, name: string) : bool =
                    attrs |> List.exists (fun attr -> attr.Name = name)

                (* find *)
                let find (attrs: ContentAttr list, name: string) : ContentAttr =
                    attrs |> List.find (fun attr -> attr.Name = name)

                
                let diffContentSingle (last: View option) (next: View option) : ViewDelta option =
                    match next with
                    | Some next -> 
                        match last with
                        | Some last -> Some (Differ.diff(last, next))
                        | None -> Some (ViewDelta.From next)
                    | None -> None

                let diffContentMultiple (lastList: View list) (nextList: View list) : ViewDelta list =
                    let merged =
                        nextList
                        |> List.mapi (fun index next -> 
                            if index + 1 < lastList.Length then
                                Differ.diff(lastList.[index], next)
                            else
                                ViewDelta.From next
                        )
                    merged

                let diffContent (last: ContentAttr, next: ContentAttr) : AttrDelta =
                    let viewContent : ViewContentDelta =
                        match next.Content with
                        | ViewContent.Single next -> 
                            match last.Content with
                            | ViewContent.Single last -> ViewContentDelta.Single (diffContentSingle last next)
                            | _ -> ViewContentDelta.Single None
                        | ViewContent.Multiple next -> 
                            match last.Content with
                            | ViewContent.Multiple last -> ViewContentDelta.Multiple (diffContentMultiple last next)
                            | _ -> ViewContentDelta.Multiple (diffContentMultiple [] next)

                    AttrDelta.ContentDelta {
                        Name = next.Name
                        Content = viewContent
                    }


                let lastAttrs = selectContent last
                let nextAttrs = selectContent next
                let delta = new System.Collections.Generic.List<AttrDelta>()

                for lastAttr in lastAttrs do
                    if contains(nextAttrs, lastAttr.Name) then
                        let nextAttr = find(nextAttrs, lastAttr.Name)

                        if nextAttr <> lastAttr then
                            // update
                            delta.Add(diffContent(nextAttr, lastAttr))

                    else
                        // reset
                        delta.Add(AttrDelta.ContentDelta {
                            Name = lastAttr.Name
                            Content =
                                match lastAttr.Content with
                                | ViewContent.Single single -> ViewContentDelta.Single None
                                | ViewContent.Multiple multiple -> ViewContentDelta.Multiple []
                        })

                for nextAttr in nextAttrs do
                    if contains(lastAttrs, nextAttr.Name) |> not then
                        delta.Add(AttrDelta.ContentDelta (ContentAttrDelta.From nextAttr))

                List.ofSeq delta 

        let diff (next: View, last: View) : ViewDelta =
            let propertyAttrs = AttrDiffer.propertyDiffer(next.Attrs, last.Attrs)
            let eventAttrs = AttrDiffer.eventDiffer(next.Attrs, last.Attrs)
            let contentAttrs = AttrDiffer.contentDiffer(next.Attrs, last.Attrs)

            {
                ViewType = next.ViewType
                Attrs = propertyAttrs @ eventAttrs @ contentAttrs
            }

    module Patcher =
        open Delta

        module AttrPatcher =
 
            let patchProperty (view: Avalonia.Controls.IControl) (attr: PropertyAttrDelta) : unit =
                let prop = Reflection.findPropertyByName view attr.Name

                match attr.Value with
                | Some value ->
                    if prop.CanWrite then
                        prop.SetValue(view, attr.Value.Value)
                    else
                        () // TODO: do we need to handle this ?
                                
                | None -> () // TODO: reset value

            let patchEvent (view: Avalonia.Controls.IControl) (attr: EventAttrDelta) : unit =
                let eventInfo = view.GetType().GetEvent(attr.Name)

                if (attr.OldValue <> null) then
                    eventInfo.RemoveEventHandler(view, attr.OldValue)

                if (attr.NewValue <> null) then
                    eventInfo.AddEventHandler(view, attr.NewValue)
                    
            let patchContentSingle (view: Avalonia.Controls.IControl) (prop: PropertyInfo) (viewElement: ViewDelta option) =
                // TODO: handle all possible cases
                // no setter / no getter
                match viewElement with
                | Some viewElement -> 
                    let value = prop.GetValue(view)
                    if value <> null then
                        Patcher.patch((value :?> Avalonia.Controls.IControl), viewElement)
                    else
                        let contentView = createView viewElement
                        Patcher.patch(contentView, viewElement)
                        prop.SetValue(view, contentView)
                | None ->
                    prop.SetValue(view, null)

            let patchContentMultiple (view: Avalonia.Controls.IControl) (prop: PropertyInfo) (delta: ViewDelta list) =
                let propertyValue = prop.GetValue(view)

                (* often lists only have a get accessor *)
                let patch_IList (collection: IList) : unit =
                    if List.isEmpty delta then
                        collection.Clear()
                    else
                        let mutable index = 0
                        for viewElement in delta do  
                            // try patch / reuse
                            if index + 1 <= collection.Count then
                                let item = collection.[index]

                                if item.GetType() = viewElement.ViewType then
                                    // patch
                                    match item with
                                    | :? Avalonia.Controls.IControl as control -> patch(control, viewElement)
                                    | _ ->
                                        // replace
                                        let newItem = createView viewElement
                                        patch(newItem, viewElement)
                                        collection.[index] <- newItem
                                else
                                    // replace
                                    let newItem = createView viewElement
                                    patch(newItem, viewElement)
                                    collection.[index] <- newItem
                            else
                                // create
                                let newItem = createView viewElement
                                patch(newItem, viewElement)
                                collection.Add(newItem) |> ignore

                            index <- index + 1

                        // remove elements if list is to long
                        if (index + 1) < collection.Count then
                            while collection.Count <> index + 1 do
                                collection.RemoveAt (collection.Count - 1)

                (* read only, so there must be a get accessor *)
                let patch_IEnumerable (collection: IEnumerable) : unit =
                    let newList = System.Collections.Generic.List<obj>()

                    if List.isEmpty delta then
                        () // list is empty by default
                    else
                        let mutable index = 0
                        for item in collection do  
                            if index + 1 <= delta.Length then
                                if item.GetType() = delta.[index].GetType() then
                                    newList.Add delta.[index]
                                else
                                    let newItem = createView delta.[index]
                                    patch(newItem, delta.[index])
                                    newList.Add delta.[index]
                            else ()

                        if index + 1 < delta.Length then
                            let _, remaining = delta |> List.splitAt index

                            for item in remaining do
                                let newItem = createView delta.[index]
                                patch(newItem, delta.[index])
                                newList.Add delta.[index]

                    prop.SetValue(view, newList :> IEnumerable)

                match propertyValue with
                | :? IList as collection -> patch_IList collection
                | :? IEnumerable as collection -> patch_IEnumerable collection
                | _ -> raise (Exception("type does not implement EEnumerable or IList whci his required for diffing"))

                

            let patchContent (view: Avalonia.Controls.IControl) (attr: ContentAttrDelta) : unit =
                let prop = Reflection.findPropertyByName view attr.Name
                match attr.Content with
                | ViewContentDelta.Single single -> patchContentSingle view prop single
                | ViewContentDelta.Multiple multiple -> patchContentMultiple view prop multiple

        let createView (view: ViewDelta) : Avalonia.Controls.IControl =
            Activator.CreateInstance(view.ViewType) :?> Avalonia.Controls.IControl

        let patch (view: Avalonia.Controls.IControl, viewElement: ViewDelta) : unit =
            for attr in viewElement.Attrs do
                match attr with 
                | PropertyDelta property -> AttrPatcher.patchProperty view property
                | ContentDelta content -> AttrPatcher.patchContent view content
                | EventDelta event -> AttrPatcher.patchEvent view event

    let createView (view: View) : Avalonia.Controls.IControl =
        Activator.CreateInstance(view.ViewType) :?> Avalonia.Controls.IControl