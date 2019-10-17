namespace Avalonia.FuncUI.VirtualDom

open Avalonia.FuncUI.Library
open Avalonia.FuncUI.Types
open Delta

module internal rec Differ =
 
    let private update (last: IAttr) (next: IAttr) : AttrDelta =
        match next with
        | Property' property ->
            AttrDelta.Property {
                accessor = property.accessor;
                value = Some property.value;
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
                value = None;
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
        | _ -> failwithf "can't reset attribute '%A'. There is no reset operation implemented." last
            
        | Subscription' subscription ->
            AttrDelta.Subscription {
                name = subscription.name
                subscribe = subscription.subscribe
                funcType = subscription.funcType
                funcCapturesState = subscription.funcCapturesState
                func = None
            }
        
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
        let delta = new ResizeArray<AttrDelta>()
        
        for lastAttr in lastAttrs do
            let nextAttr = nextAttrs |> List.tryFind (fun attr -> attr.UniqueName = lastAttr.UniqueName)
            
            match nextAttr with
            // update if changed
            | Some nextAttr ->
                let eq = nextAttr.Equals lastAttr
                if not eq || nextAttr.ForcePatch then
                    delta.Add(update lastAttr nextAttr)
                else
                    ()
            // reset  
            | None ->
                delta.Add(reset lastAttr)
        
        for nextAttr in nextAttrs do
            let exists = lastAttrs |> List.exists (fun attr -> attr.UniqueName = nextAttr.UniqueName)
            // add if not there
            if not exists then
                delta.Add (AttrDelta.From nextAttr)
                
        List.ofSeq delta
    
    let diff (last: IView, next: IView) : ViewDelta =
        // only diff attributes if viewType matches
        if last.ViewType = next.ViewType then
            {
                ViewDelta.viewType = next.ViewType
                ViewDelta.attrs = diffAttributes last.Attrs next.Attrs
            }
        else
            ViewDelta.From next

