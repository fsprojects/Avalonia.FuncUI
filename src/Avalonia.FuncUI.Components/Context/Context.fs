namespace Avalonia.FuncUI

open System
open System.Collections.Generic
open System.Collections.ObjectModel
open Avalonia.FuncUI
open Avalonia.Threading

type IComponentContext =

    /// <summary>
    /// Forces the component to be re-rendered.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Components actually only get re-rendered if the component content changed - and
    /// even then only the content that is actually different from the previous render
    /// will get patched.
    /// </para>
    /// <para>
    /// To ensure a full re-render the component key needs to be changed.
    /// </para>
    /// </remarks>
    abstract forceRender: unit -> unit

    /// <summary>
    /// <para>
    /// Adds the passed disposable to the component context's disposal list.
    /// </para>
    /// <para>
    /// Disposables that are tracked by the component context will be disposed
    /// when the component is destroyed.
    /// </para>
    /// </summary>
    abstract trackDisposable: IDisposable -> unit

    /// <summary>
    /// Extension point to register state hooks to the component context.
    /// (used by extension methods that add the default state hooks)
    /// </summary>
    abstract useStateHook: StateHook -> IReadable<'value>

    /// <summary>
    /// Extension point to register effect hooks to the component context.
    /// (used by extension methods that add the default effect hooks)
    /// </summary>
    abstract useEffectHook: EffectHook -> unit

type Context () =
    let disposables = new DisposableBag ()
    let hooks = Dictionary<HookIdentity, StateHook>()
    let effects = Dictionary<HookIdentity, EffectHook>()
    let effectQueue =
        let effectQueue = new EffectQueue()
        disposables.Add effectQueue
        effectQueue

    let render = Event<unit>()

    member internal this.EffectQueue = effectQueue

    member internal this.OnRender = render.Publish

    member internal this.Hooks with get () = Map.ofDict hooks


    interface IComponentContext with
        member this.forceRender () =
            render.Trigger ()

        member this.trackDisposable (item: IDisposable) : unit =
            disposables.Add item

        member this.useStateHook<'value>(stateHook: StateHook) : IReadable<'value> =
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
                                action = (fun _ -> (this :> IComponentContext).forceRender()),
                                priority = DispatcherPriority.Background
                            )
                        )
                    )

                state

        member this.useEffectHook (effect: EffectHook) : unit =
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
