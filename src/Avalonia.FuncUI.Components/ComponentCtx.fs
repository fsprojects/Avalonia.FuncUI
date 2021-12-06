namespace Avalonia.FuncUI

open System
open System.Collections.Generic
open System.Reactive.PlatformServices
open Avalonia
open Avalonia.Controls
open Avalonia.FuncUI.Components.Hosts
open Avalonia.FuncUI.Types
open Avalonia.Styling
open Avalonia.FuncUI.VirtualDom

(*
Things I want to support out of the box:
- global set/get state
- local state that triggers a rerender on change
- local state that does not trigger a rerender on change
- passed state that triggers a rerender on change
- passed state that does not trigger a rerender on change

- run arbitrary code on change of certain global, local or passed state
*)

type CtxDependency =
    { Connectable: IConnectable
      (* caller location! for debugging *) }

    static member Create (conn) =
        { CtxDependency.Connectable = conn }

type CtxDependencies () =
    let mutable idx = 0
    let lookup = Dictionary<int, CtxDependency>()

    member this.NextIndex () =
        idx <- idx + 1

    member this.ResetIndex () =
        idx <- 0

    member this.ItemExists with get () =
        lookup.ContainsKey(idx)

    member this.Item
        with get () = lookup.[idx]
        and set v = lookup.[idx] <- v


type ComponentCtx () =
    let bag = new DisposableBag ()
    let deps = CtxDependencies()
    let onSignal = Event<unit>()

    member internal this.Deps = deps
    member internal this.OnSignal = onSignal.Publish

    member this.triggerRender () =
        onSignal.Trigger ()

    member this.useWire<'signal>(wire: IWire<'signal>, ?renderOnChange: bool) : IWire<'signal> =
        let result =
            if deps.ItemExists then
                deps.Item.Connectable :?> IWire<'signal>
            else
                deps.Item <- CtxDependency.Create wire
                if defaultArg renderOnChange true then
                    bag.Add(wire.Subscribe(ignore >> onSignal.Trigger))
                wire

        deps.NextIndex ()

        printfn $"wire (used: {result.InstanceId}, passed: {wire.InstanceId}"

        result

    member this.useTap<'signal>(tap: ITap<'signal>, ?renderOnChange: bool) : ITap<'signal> =
        let result =
            if deps.ItemExists then
                deps.Item.Connectable :?> ITap<'signal>
            else
                deps.Item <- CtxDependency.Create tap
                if defaultArg renderOnChange true then
                    bag.Add(tap.Subscribe(ignore >> onSignal.Trigger))
                tap

        deps.NextIndex ()

        printfn $"tap (used: {result.InstanceId}, passed: {tap.InstanceId}"

        result

    member this.usePort<'signal>(init: 'signal, ?renderOnChange: bool) : IWire<'signal> =
        let result =
            if deps.ItemExists then
                deps.Item.Connectable :?> IWire<'signal>
            else
                let port = new Port<'signal>(init)
                bag.Add port
                deps.Item <- CtxDependency.Create port
                if defaultArg renderOnChange true then
                    bag.Add((port :> IWire<_>).Subscribe (ignore >> onSignal.Trigger))
                port :> IWire<_>

        deps.NextIndex ()

        result

    member this.useAsync<'signal>(init: Async<'signal>) : IWire<Deferred<'signal>> =
        let state = this.usePort Deferred.NotStartedYet

        match  state.CurrentSignal with
        | Deferred.NotStartedYet ->
            state.Send Deferred.Pending

            Async.StartImmediate (
                async {
                    let! result = Async.Catch init

                    match result with
                    | Choice1Of2 value -> state.Send (Deferred.Resolved value)
                    | Choice2Of2 exn -> state.Send (Deferred.Failed exn)
                }
            )

        | _ ->
            ()

        state

    member internal this.ResetIndex () =
        deps.ResetIndex ()