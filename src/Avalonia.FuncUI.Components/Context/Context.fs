namespace Avalonia.FuncUI

open System
open System.Collections.Generic
open System.Reactive.PlatformServices
open System.Runtime.CompilerServices
open System.Threading
open Avalonia
open Avalonia.Controls
open Avalonia.FuncUI
open Avalonia.FuncUI.Components.Hosts
open Avalonia.FuncUI.Types
open Avalonia.Platform
open Avalonia.Styling
open Avalonia.FuncUI.VirtualDom
open Avalonia.Threading

type Context () =
    let disposables = new DisposableBag ()
    let hooks = Dictionary<string, StateHook>()
    let effects = Dictionary<string, EffectHook>()
    let effectQueue =
        let effectQueue = new EffectQueue()
        disposables.Add effectQueue
        effectQueue

    let render = Event<unit>()

    member internal this.EffectQueue = effectQueue

    member internal this.OnRender = render.Publish

    member this.forceRender () =
        render.Trigger ()

    member this.useDisposable (item: IDisposable) : unit =
        disposables.Add item

    member this.useHook<'value>(stateHook: StateHook) : IReadable<'value> =
        match hooks.TryGetValue stateHook.Identity with
        | true, known ->
            known.State.Value :?> IReadable<'value>

        | false, _ ->
            let state = stateHook.State.Value :?> IReadable<'value>

            hooks.Add (stateHook.Identity, stateHook)
            disposables.Add state

            if stateHook.RenderOnChange then
                disposables.Add (
                    state.Subscribe (fun _ ->
                        (* render the component when a hook's state changed. *)
                        Dispatcher.UIThread.Post(
                            action = (fun _ -> this.forceRender()),
                            priority = DispatcherPriority.Background
                        )
                    )
                )

            state

    member this.useEffect (effect: EffectHook) : unit =
        match effects.TryGetValue effect.Identity with
        | true, _ ->
            for trigger in effect.Triggers do
                match trigger with
                | EffectTrigger.AfterRender ->
                    effectQueue.Enqueue effect

                | _ ->
                    ()

        | false, _ ->
            effects.Add (effect.Identity, effect)

            for trigger in effect.Triggers do
                match trigger with
                | EffectTrigger.AfterChange dep ->
                    (fun _ -> effectQueue.Enqueue effect)
                    |> dep.SubscribeAny
                    |> disposables.Add

                | EffectTrigger.AfterRender ->
                    effectQueue.Enqueue effect

                | EffectTrigger.AfterInit ->
                    effectQueue.Enqueue effect

    interface IDisposable with
        member this.Dispose () =
            (disposables :> IDisposable).Dispose()

type Context with

    member this.useState<'value>(init: 'value, [<CallerLineNumber>] ?identity: int) : IWritable<'value> =
        this.useHook<'value>(
            StateHook.Create (
                identity = $"Line: {identity.Value}",
                state = StateHookValue.Lazy (fun _ -> new State<'value>(init) :> IConnectable),
                renderOnChange = true
            )
        ) :?> IWritable<'value>

    member this.usePassedState<'value>(value: IWritable<'value>, [<CallerLineNumber>] ?identity: int) : IWritable<'value> =
        this.useHook<'value>(
            StateHook.Create (
                identity = $"Line: {identity.Value}",
                state = StateHookValue.Const value,
                renderOnChange = true
            )
        ) :?> IWritable<'value>

    member this.usePassedStateReadOnly<'value>(value: IReadable<'value>, [<CallerLineNumber>] ?identity: int) : IReadable<'value> =
        this.useHook<'value>(
            StateHook.Create (
                identity = $"Line: {identity.Value}",
                state = StateHookValue.Const value,
                renderOnChange = true
            )
        )

    member this.useEffect (handler: unit -> IDisposable, triggers: EffectTrigger list, [<CallerLineNumber>] ?identity: int) =
        this.useEffect (EffectHook.Create($"{identity.Value}", handler, triggers))

    member this.useEffect (handler: unit -> unit, triggers: EffectTrigger list, [<CallerLineNumber>] ?identity: int) =
        this.useEffect (EffectHook.Create($"{identity.Value}", (fun _ -> handler(); null), triggers))