namespace Avalonia.FuncUI.VirtualDom

open Avalonia.FuncUI.Types
open Avalonia.FuncUI.VirtualDom.Delta

module internal rec Differ =

    let private update (last: IAttr, next: IAttr) : AttrDelta =
        match next with
        | Property' property ->
            AttrDelta.Property
                { Accessor = property.Accessor
                  Value = Some property.Value
                  DefaultValueFactory = property.DefaultValueFactory }

        | Content' content ->
            AttrDelta.Content
                { Accessor = content.Accessor;
                  Content = Differ.diffContent (last, next) }

        | Subscription' subscription ->
            AttrDelta.Subscription (SubscriptionDelta.From subscription)

        | InitFunction init ->
            AttrDelta.SetupFunction init

        | _ -> failwithf "no update operation is defined for '%A' next" next

    let private reset (last: IAttr) : AttrDelta =
        match last with
        | Property' property ->
            AttrDelta.Property
                { Accessor = property.Accessor;
                  Value = None
                  DefaultValueFactory = property.DefaultValueFactory }

        | Content' content ->
            let empty =
                match content.Content with
                | ViewContent.Single _ -> ViewContentDelta.Single None
                | ViewContent.Multiple _ -> ViewContentDelta.Multiple []

            AttrDelta.Content
                { Accessor = content.Accessor;
                  Content = empty }

        | Subscription' subscription ->
            AttrDelta.Subscription
                { Name = subscription.Name
                  Subscribe = subscription.Subscribe
                  Func = None }

        | InitFunction init ->
            AttrDelta.SetupFunction init

        | _ -> failwithf "no reset operation is defined for last '%A'" last

    let private diffContentSingle (last: IView option, next: IView option) : ViewDelta option =
        match next with
        | Some next ->
            match last with
            | Some last -> Some (Differ.diff(last, next))
            | None -> Some (ViewDelta.From next)
        | None -> None

    let private diffContentMultiple (lastList: IView list, nextList: IView list) : ViewDelta list =
        let lastListLength = lastList.Length

        (* implementation that avoids indexed access to lists *)
        (* more details: https://github.com/fsprojects/Avalonia.FuncUI/pull/317 *)

        let mutable lastTail: IView list = lastList

        nextList |> List.mapi (fun index next ->
            if index < lastListLength then
                let result = diff(lastTail.Head, next)
                lastTail <- lastTail.Tail
                result
            else
                ViewDelta.From next
        )

    let private diffContent (last: IAttr, next: IAttr) : ViewContentDelta =
            match next with
            | Content' nextContent ->
                match last with
                | Content' lastContent ->
                    match nextContent.Content with

                    // Single Content
                    | ViewContent.Single nextSingleContent ->
                        match lastContent.Content with
                        | ViewContent.Single lastSingleContent ->
                            ViewContentDelta.Single (diffContentSingle (lastSingleContent, nextSingleContent))
                        | _ ->
                            ViewContentDelta.Single None

                    // Multiple Content
                    | ViewContent.Multiple nextMultipleContent ->
                        match lastContent.Content with
                        | ViewContent.Multiple lastMultipleContent ->
                            ViewContentDelta.Multiple (diffContentMultiple (lastMultipleContent, nextMultipleContent))
                        | _ ->
                            ViewContentDelta.Multiple (diffContentMultiple (List.empty, nextMultipleContent))

                | _ -> ViewContentDelta.From nextContent.Content

            | _ -> invalidOp "'next' must be of type content"

    let internal diffAttributes (lastAttrs: IAttr list, nextAttrs: IAttr list) : AttrDelta list =

        let delta = ResizeArray<AttrDelta>()

        (* check if there is a corresponding new attribute for all old attributes *)
        for lastAttr in lastAttrs do

            let isAttrStillPresent =
                nextAttrs
                |> List.tryFind (fun nextAttr -> nextAttr.UniqueName = lastAttr.UniqueName)

            (* check if attribute is still present *)
            match isAttrStillPresent with

            (* attribute is still there, but it's value changed. -> update value *)
            | Some nextAttr when not (nextAttr.Equals lastAttr) ->
                let attrDelta = update (lastAttr, nextAttr)
                delta.Add attrDelta

            (* attribute is still there. It hasn't changed -> do nothing *)
            | Some _ -> ()

            (* attribute is no longer present. -> reset value *)
            | None ->
                let attrDelta = reset lastAttr
                delta.Add attrDelta

        (* check if new attributes need to be added  *)
        for nextAttr in nextAttrs do

            let attrIsAlreadyKnown =
                lastAttrs
                |> List.exists (fun lastAttr -> lastAttr.UniqueName = nextAttr.UniqueName)

            (* attribute is new - create delta from it -> set value  *)
            if not attrIsAlreadyKnown then
                delta.Add (AttrDelta.From nextAttr)

        List.ofSeq delta

    let diff (last: IView, next: IView) : ViewDelta =
        // only diff attributes if viewType/viewKey matches
        if last.ViewKey <> next.ViewKey then
            ViewDelta.From (next, true)
        elif last.ViewType <> next.ViewType then
            ViewDelta.From next
        else
            { ViewType = next.ViewType
              Attrs = diffAttributes (last.Attrs, next.Attrs)
              ConstructorArgs = next.ConstructorArgs
              KeyDidChange = false
              Outlet = next.Outlet }
