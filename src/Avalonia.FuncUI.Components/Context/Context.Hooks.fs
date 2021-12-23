namespace Avalonia.FuncUI

open System
open System.Runtime.CompilerServices
open Avalonia.FuncUI

[<AutoOpen>]
module IComponentContextExtensions =

    type IComponentContext with

        /// <summary>
        /// Attaches a value to the component context.
        /// <example>
        /// <code>
        /// Component (fun ctx ->
        ///     let count = ctx.useState 42
        ///     ..
        ///     // id will have the same value during the whole component lifetime. (unless changed via 'id.Set ..')
        ///     let id = ctx.useState (Guid.NewGuid())
        ///     ..
        /// )
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="initialValue">value should not change during the component lifetime.</param>
        /// <param name="renderOnChange">re-render component on change (default: true).</param>
        /// <param name="callerLineNumber">Usually provided by the compiler. Needs to be unique in the components context.</param>
        member this.useState<'value>(initialValue: 'value, ?renderOnChange: bool, [<CallerLineNumber>] ?callerLineNumber: int ) : IWritable<'value> =
            this.useStateHook<'value>(
                StateHook.Create (
                    identity = HookIdentity.CallerCodeLocation callerLineNumber.Value,
                    state = StateHookValue.Lazy (fun _ -> new State<'value>(initialValue) :> IConnectable),
                    renderOnChange = defaultArg renderOnChange true
                )
            ) :?> IWritable<'value>

        /// <summary>
        /// Attaches a passed <c>IWritable&lt;'value&gt;</c> value to the component context.
        /// <example>
        /// <code>
        /// let countView (count: IWritable&lt;int&gt;) =
        ///     Component (fun ctx ->
        ///         // attach passed writable value to the component context
        ///         let count = ctx.usePassed count
        ///         ..
        ///     )
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="value">value should not change during the component lifetime.</param>
        /// <param name="renderOnChange">re-render component on change (default: true).</param>
        /// <param name="callerLineNumber">Usually provided by the compiler. Needs to be unique in the components context.</param>
        member this.usePassed<'value>(value: IWritable<'value>, ?renderOnChange: bool, [<CallerLineNumber>] ?callerLineNumber: int) : IWritable<'value> =
            this.useStateHook<'value>(
                StateHook.Create (
                    identity = HookIdentity.CallerCodeLocation callerLineNumber.Value,
                    state = StateHookValue.Const value,
                    renderOnChange = defaultArg renderOnChange true
                )
            ) :?> IWritable<'value>

        /// <summary>
        /// Attaches a passed <c>IReadable&lt;'value&gt;</c> value to the component context.
        /// <example>
        /// <code>
        /// let countView (count: IReadable&lt;int&gt;) =
        ///     Component (fun ctx ->
        ///         // attach passed readable value to the component context
        ///         let count = ctx.usePassedRead count
        ///         ..
        ///     )
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="value">value should not change during the component lifetime.</param>
        /// <param name="renderOnChange">re-render component on change (default: true).</param>
        /// <param name="callerLineNumber">Usually provided by the compiler. Needs to be unique in the components context.</param>
        member this.usePassedRead<'value>(value: IReadable<'value>, ?renderOnChange: bool, [<CallerLineNumber>] ?callerLineNumber: int) : IReadable<'value> =
            this.useStateHook<'value>(
                StateHook.Create (
                    identity = HookIdentity.CallerCodeLocation callerLineNumber.Value,
                    state = StateHookValue.Const value,
                    renderOnChange = defaultArg renderOnChange true
                )
            )

        member this.useEffect (handler: unit -> IDisposable, triggers: EffectTrigger list, [<CallerLineNumber>] ?callerLineNumber: int) =
            this.useEffectHook (EffectHook.Create(HookIdentity.CallerCodeLocation callerLineNumber.Value, handler, triggers))

        member this.useEffect (handler: unit -> unit, triggers: EffectTrigger list, [<CallerLineNumber>] ?callerLineNumber: int) =
            this.useEffectHook (EffectHook.Create(HookIdentity.CallerCodeLocation callerLineNumber.Value, (fun _ -> handler(); null), triggers))