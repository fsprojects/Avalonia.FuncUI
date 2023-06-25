namespace Avalonia.FuncUI.Benchmarks.Scenarios.Diffing

open System

open Avalonia.Controls
open Avalonia.Controls.Primitives
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Types
open Avalonia.Layout
open Avalonia.Media

open BenchmarkDotNet.Attributes
open BenchmarkDotNet.Engines

[<RequireQualifiedAccess>]
module GameOfLife =

    let random = Random(Seed = 42)

    let randomView (cells: int, extendedAttrs: bool) =
        UniformGrid.create [
            UniformGrid.columns cells
            UniformGrid.rows cells
            UniformGrid.children [
                for x = 0 to cells - 1 do
                    for y = 0 to cells - 1 do
                        let alive = random.Next(100) < 50

                        Button.create [
                            Button.row y
                            Button.column x
                            Button.cornerRadius 5.0
                            Button.borderThickness 1.0
                            Button.onClick ignore

                            if extendedAttrs then
                                Button.verticalAlignment VerticalAlignment.Center
                                Button.horizontalAlignment HorizontalAlignment.Center
                                Button.padding 0.0
                                Button.margin 0.0
                                Button.borderBrush Brushes.Black
                                Button.foreground Brushes.Black
                                Button.content (if alive then "X" else " ")

                            if alive then
                                Button.background "green"
                            else
                                Button.background "gray"
                        ]
            ]
        ]

open Avalonia.FuncUI.VirtualDom.Delta

module internal rec DifferChampionA =

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
                  Content = diffContent (last, next) }

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
            | Some last -> Some (diff(last, next))
            | None -> Some (ViewDelta.From next)
        | None -> None

    let private diffContentMultiple (lastList: IView list, nextList: IView list) : ViewDelta list =
        let lastListLength = lastList.Length

        nextList |> List.mapi (fun index next ->
            if index + 1 <= lastListLength then
                diff(lastList.[index], next)
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

                | _ -> invalidOp "'last' must be of type content"

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

module internal rec DifferChampionB =

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
                  Content = diffContent (last, next) }

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
            | Some last -> Some (diff(last, next))
            | None -> Some (ViewDelta.From next)
        | None -> None

    let private diffContentMultiple (lastList: IView list, nextList: IView list) : ViewDelta list =
        let lastListLength = lastList.Length

        let mutable lastTail: IView list = lastList

        nextList |> List.mapi (fun index next ->
            if index + 1 <= lastListLength then
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

                | _ -> invalidOp "'last' must be of type content"

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

module internal rec DifferChampionC =

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
                  Content = diffContent (last, next) }

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
            | Some last -> Some (diff(last, next))
            | None -> Some (ViewDelta.From next)
        | None -> None

    let private diffContentMultiple (lastList: IView list, nextList: IView list) : ViewDelta list =
        let lastListLength = lastList.Length

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

                | _ -> invalidOp "'last' must be of type content"

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

[<SimpleJob(RunStrategy.Throughput, warmupCount = 5, iterationCount = 5)>]
[<MemoryDiagnoser; >]
type GameOfLifeBench () =

    let mutable viewLast: IView option = None

    let mutable viewNext: IView option = None

    [<Params(10, 100)>]
    member val Cells: int = 0 with get, set

    [<Params(true, false)>]
    member val ExtendedAttrs: bool = false with get, set

    [<GlobalSetup>]
    member this.GlobalSetup () =
        viewLast <- Some (GameOfLife.randomView (this.Cells, this.ExtendedAttrs))
        viewNext <- Some (GameOfLife.randomView (this.Cells, this.ExtendedAttrs))

    [<Benchmark(Baseline = true)>]
    member this.Baseline () : unit =
        let _ = Avalonia.FuncUI.VirtualDom.Differ.diff (viewLast.Value, viewNext.Value)
        ()

    [<Benchmark>]
    member this.ChampionA () : unit =
        let _ = DifferChampionA.diff (viewLast.Value, viewNext.Value)
        ()

    [<Benchmark>]
    member this.ChampionB () : unit =
        let _ = DifferChampionB.diff (viewLast.Value, viewNext.Value)
        ()

    [<Benchmark>]
    member this.ChampionC () : unit =
        let _ = DifferChampionC.diff (viewLast.Value, viewNext.Value)
        ()