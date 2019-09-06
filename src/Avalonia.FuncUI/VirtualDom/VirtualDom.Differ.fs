namespace Avalonia.FuncUI.VirtualDom

open Avalonia.FuncUI.Library
open Avalonia.FuncUI.Core.Domain
open Delta
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
    
    let private reset (last: IAttr) : AttrDelta =
        AttrDelta.From last
        
    let private diffContentSingle (last: IView option) (next: IView option) : ViewDelta option =
        match next with
        | Some next ->
            match last with
            | Some last -> Some (Differ.diff last next)
            | None -> Some (ViewDelta.From next)
        | None -> None
                
    let private diffContentMultiple (lastList: IView list) (nextList: IView list) : ViewDelta list =
        nextList |> List.mapi (fun index next ->
            if index + 1 <= lastList.Length then
                Differ.diff lastList.[index] nextList.[index]
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
    
    let private diffAttributes (lastAttrs: IAttr list) (nextAttrs: IAttr list) : AttrDelta list =
        let delta = new ResizeArray<AttrDelta>()
        
        for lastAttr in lastAttrs do
            let nextAttr = nextAttrs |> List.tryFind (fun attr -> attr.UniqueName = lastAttr.UniqueName)
            
            match nextAttr with
            // update if changed
            | Some nextAttr ->
                if not (nextAttr.Equals lastAttr) then
                    delta.Add(update lastAttr nextAttr)
            
            // reset  
            | None ->
                delta.Add(reset lastAttr)
        
        for nextAttr in nextAttrs do
            let exists = lastAttrs |> List.exists (fun attr -> attr.UniqueName = nextAttr.UniqueName)
            // add if not there
            if exists then
                delta.Add (AttrDelta.From nextAttr)
                
        List.ofSeq delta
    
    let diff (last: IView) (next: IView) : ViewDelta =
        {
            ViewDelta.viewType = next.ViewType
            ViewDelta.attrs = diffAttributes last.Attrs next.Attrs
        }

