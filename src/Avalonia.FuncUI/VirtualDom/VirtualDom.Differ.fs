namespace Avalonia.FuncUI.VirtualDom

open System.Collections.Generic
open Avalonia.FuncUI.Types
open Delta

module internal rec Differ =
 
    let private update (last: IAttr) (next: IAttr) : AttrDelta =
        match next with
        | Property' property ->
            AttrDelta.Property {
                accessor = property.accessor
                value = Some property.value
                defaultValueFactory = property.defaultValueFactory
            }
        | Content' content ->
            AttrDelta.Content {
                accessor = content.accessor;
                content = Differ.diffContent last next
            }
        | Subscription' subscription ->
            AttrDelta.Subscription (SubscriptionDelta.From subscription)
        | _ -> failwithf "no update operation is defined for '%A' next" next
    
    let private reset (last: IAttr) : AttrDelta =
        match last with
        | Property' property ->
            AttrDelta.Property {
                accessor = property.accessor;
                value = None
                defaultValueFactory = property.defaultValueFactory
            }
        | Content' content ->
            let empty =
                match content.content with
                | ViewContent.Single _ -> ViewContentDelta.Single None
                | ViewContent.Multiple _ -> ViewContentDelta.Multiple []
            
            AttrDelta.Content {
                accessor = content.accessor;
                content = empty
            }
        | Subscription' subscription ->
            AttrDelta.Subscription {
                name = subscription.name
                subscribe = subscription.subscribe
                func = None
            }
        | _ -> failwithf "no reset operation is defined for last '%A'" last
        
    let private diffContentSingle (last: IView option) (next: IView option) : ViewDelta option =
        match next with
        | Some next ->
            match last with
            | Some last -> Some (Differ.diff(last, next))
            | None -> Some (ViewDelta.From next)
        | None -> None
                
    let private diffContentMultiple (lastList: IView list) (nextList: IView list) : ViewDelta list =
        nextList |> List.mapi (fun index next ->
            if index + 1 <= lastList.Length then
                Differ.diff(lastList.[index], nextList.[index])
            else
                ViewDelta.From next
        )

    let private diffContent (last: IAttr) (next: IAttr) : ViewContentDelta =
            match next with
            | Content' nextContent ->
                match last with
                | Content' lastContent ->
                    match nextContent.content with
                    
                    // Single Content
                    | ViewContent.Single nextSingleContent ->
                        match lastContent.content with
                        | ViewContent.Single lastSingleContent ->
                            ViewContentDelta.Single (diffContentSingle lastSingleContent nextSingleContent)
                        | _ ->
                            ViewContentDelta.Single None
                
                    // Multiple Content
                    | ViewContent.Multiple nextMultipleContent ->
                        match lastContent.content with
                        | ViewContent.Multiple lastMultipleContent ->
                            ViewContentDelta.Multiple (diffContentMultiple lastMultipleContent nextMultipleContent)
                        | _ ->
                            ViewContentDelta.Multiple (diffContentMultiple [] nextMultipleContent)
                            
                | _ -> invalidOp "'last' must be of type content"
                
            | _ -> invalidOp "'next' must be of type content"
    
    let private diffAttributes (lastAttrs: IAttr list) (nextAttrs: IAttr list) : AttrDelta list =
        (* TODO: optimize. *)

        (* NOTE: using a map here might be actually slower
        than iterating over a short list. views usually don't
        have a lot of attributes set. needs benchmarking  *)
        let nextAttrsMap : Map<string, IAttr> =
            nextAttrs
            |> List.map (fun i -> i.UniqueName, i)
            |> Map.ofList
            
        let lastAttrsMap : Map<string, IAttr> =
            lastAttrs
            |> List.map (fun i -> i.UniqueName, i)
            |> Map.ofList
            
        let delta = ResizeArray<AttrDelta>()            
            
        (* check if there is a corresponding new attribute for all old attributes *)
        for lastAttr in lastAttrs do
            
            (* check if attribute is still present *)
            match Map.tryFind lastAttr.UniqueName nextAttrsMap with
            
            (* attribute is still there, but it's value changed. -> update value *)
            | Some nextAttr when not (nextAttr.Equals lastAttr) ->
                let attrDelta = update lastAttr nextAttr
                delta.Add attrDelta
                
            (* attribute is still there. It hasn't changed -> do nothing *)
            | Some _ -> ()

            (* attribute is no longer present. -> reset value *)
            | None ->
                let attrDelta = reset lastAttr
                delta.Add attrDelta
                    
        (* check if new attributes need to be added  *)
        for nextAttr in nextAttrs do
            
            (* attribute is new - create delta from it -> set value  *)
            if not (Map.containsKey nextAttr.UniqueName lastAttrsMap) then
                delta.Add (AttrDelta.From nextAttr)
                
        List.ofSeq delta
    
    let diff (last: IView, next: IView) : ViewDelta =
        // only diff attributes if viewType matches
        if last.ViewType = next.ViewType then
            
            (* patch existing *)
            if last.ViewConstructorParams = next.ViewConstructorParams then
                {
                    ViewDelta.viewType = next.ViewType
                    ViewDelta.attrs = diffAttributes last.Attrs next.Attrs
                    ViewDelta.viewConstructorParams = next.ViewConstructorParams
                    ViewDelta.reinstantiate = false
                }
                
            (* reinstantiate *)
            else
                {
                    ViewDelta.viewType = next.ViewType
                    ViewDelta.attrs = List.map AttrDelta.From next.Attrs
                    ViewDelta.viewConstructorParams = next.ViewConstructorParams
                    ViewDelta.reinstantiate = last.ViewConstructorParams <> next.ViewConstructorParams
                }
        else
            ViewDelta.From next

