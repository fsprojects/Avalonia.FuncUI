namespace Avalonia.FuncUI

open System
open System.Runtime.CompilerServices
open Avalonia.FuncUI

[<AutoOpen>]
module IComponentContextExtensions =

    type IComponentContext with

        member this.useState<'value>(init: 'value, [<CallerLineNumber>] ?identity: int) : IWritable<'value> =
            this.useStateHook<'value>(
                StateHook.Create (
                    identity = $"Line: {identity.Value}",
                    state = StateHookValue.Lazy (fun _ -> new State<'value>(init) :> IConnectable),
                    renderOnChange = true
                )
            ) :?> IWritable<'value>

        member this.usePassedState<'value>(value: IWritable<'value>, [<CallerLineNumber>] ?identity: int) : IWritable<'value> =
            this.useStateHook<'value>(
                StateHook.Create (
                    identity = $"Line: {identity.Value}",
                    state = StateHookValue.Const value,
                    renderOnChange = true
                )
            ) :?> IWritable<'value>

        member this.usePassedStateReadOnly<'value>(value: IReadable<'value>, [<CallerLineNumber>] ?identity: int) : IReadable<'value> =
            this.useStateHook<'value>(
                StateHook.Create (
                    identity = $"Line: {identity.Value}",
                    state = StateHookValue.Const value,
                    renderOnChange = true
                )
            )

        member this.useEffect (handler: unit -> IDisposable, triggers: EffectTrigger list, [<CallerLineNumber>] ?identity: int) =
            this.useEffectHook (EffectHook.Create($"{identity.Value}", handler, triggers))

        member this.useEffect (handler: unit -> unit, triggers: EffectTrigger list, [<CallerLineNumber>] ?identity: int) =
            this.useEffectHook (EffectHook.Create($"{identity.Value}", (fun _ -> handler(); null), triggers))