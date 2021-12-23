namespace Avalonia.FuncUI

open System
open Avalonia.FuncUI
open Avalonia.Threading

[<RequireQualifiedAccess>]
type EffectTrigger =
    /// triggers the effect to run every time after the passed dependency has changed.
    | AfterChange of state: IAnyReadable
    /// triggers the effect to run once after the component initially rendered.
    | AfterInit
    /// triggers the effect to run every time after the component is rendered.
    | AfterRender


type EffectHook =
    { Identity: HookIdentity
      Handler: unit -> IDisposable
      Triggers: EffectTrigger list }

    static member Create (identity, effect, triggers) =
        { EffectHook.Identity = identity
          EffectHook.Handler = effect
          EffectHook.Triggers = triggers }


type internal EffectQueue () =
    let sync = obj ()
    let disposables = new DisposableBag()

    let mutable queue: EffectHook list = List.empty

    member this.Enqueue (effect: EffectHook) =
        lock sync (fun _ ->
            queue <- effect :: queue
        )

        this.ProcessIncoming ()

    member private this.Process (priority: DispatcherPriority) =
        Dispatcher.UIThread.Post (
            action = (fun _ ->
                if List.isEmpty queue then
                    ()
                else
                    let mutable detached = []
                    lock sync (fun _ ->
                        detached <- queue
                        queue <- List.empty
                    )

                    let detached =
                        detached
                        |> List.distinctBy (fun effect -> effect.Identity)

                    for effect in detached do
                        disposables.Add (effect.Handler ())
            ),
            priority = priority
        )

    /// Process effects that were likely added to the queue because some state/dependency changed.
    member this.ProcessIncoming () =
        this.Process DispatcherPriority.Normal

    /// Process effects that were enqueued after render. This should happen before the next render.
    member this.ProcessAfterRender () =
        this.Process DispatcherPriority.Render

    interface IDisposable with
        member this.Dispose () =
            (disposables :> IDisposable).Dispose()