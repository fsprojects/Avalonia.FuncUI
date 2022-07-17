namespace Avalonia.FuncUI

open System
open Avalonia.FuncUI

/// Used to automatically prefix hooks with a group identifier.
type internal ContextGroup (contextCore: IComponentContextCore, callerLineNumber: int) =
    interface IComponentContextCore with

        member this.trackDisposable (item: IDisposable) : unit =
            contextCore.trackDisposable item

        member this.useStateHook<'value>(stateHook: StateHook) : IReadable<'value> =
            contextCore.useStateHook
              { stateHook with Identity = stateHook.Identity.WithPrefix $"%i{callerLineNumber}" }

        member this.useEffectHook (effect: EffectHook) : unit =
            contextCore.useEffectHook
              { effect with Identity = effect.Identity.WithPrefix $"%i{callerLineNumber}" }
