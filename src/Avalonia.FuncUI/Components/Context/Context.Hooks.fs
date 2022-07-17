namespace Avalonia.FuncUI

open System
open System.Runtime.CompilerServices
open Avalonia.FuncUI

[<AutoOpen>]
module IComponentContextExtensions =

    type IComponentContextCore with

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
                    state = StateHookValue.Lazy (fun _ -> new State<'value>(initialValue) :> IAnyReadable),
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

        /// <summary>
        /// Attaches a effect to the component context.
        /// <example>
        /// <code>
        /// let view () =
        ///     Component (fun ctx ->
        ///         let count = ctx.useState 0
        ///         ctx.useEffect (
        ///             handler = (fun _ ->
        ///                 // do something
        ///                 ..
        ///                 // return a cleanup function
        ///                 { new IDisposable with
        ///                     method Dispose() =
        ///                         // do something
        ///                 }
        ///             ),
        ///             triggers = [
        ///                 EffectTrigger.AfterChange count
        ///                 EffectTrigger.AfterRender
        ///                 EffectTrigger.AfterInit
        ///             ]
        ///         )
        ///         ..
        ///     )
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="handler">handler is called when a trigger fires.</param>
        /// <param name="triggers">list of effect triggers</param>
        /// <param name="callerLineNumber">Usually provided by the compiler. Needs to be unique in the components context.</param>
        member this.useEffect (handler: unit -> IDisposable, triggers: EffectTrigger list, [<CallerLineNumber>] ?callerLineNumber: int) =
            this.useEffectHook (EffectHook.Create(HookIdentity.CallerCodeLocation callerLineNumber.Value, handler, triggers))

        /// <summary>
        /// Attaches a effect to the component context.
        /// <example>
        /// <code>
        /// let view () =
        ///     Component (fun ctx ->
        ///         let count = ctx.useState 0
        ///         ctx.useEffect (
        ///             handler = (fun _ ->
        ///                 // do something
        ///             ),
        ///             triggers = [
        ///                 EffectTrigger.AfterChange count
        ///                 EffectTrigger.AfterRender
        ///                 EffectTrigger.AfterInit
        ///             ]
        ///         )
        ///         ..
        ///     )
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="handler">handler is called when a trigger fires.</param>
        /// <param name="triggers">list of effect triggers</param>
        /// <param name="callerLineNumber">Usually provided by the compiler. Needs to be unique in the components context.</param>
        member this.useEffect (handler: unit -> unit, triggers: EffectTrigger list, [<CallerLineNumber>] ?callerLineNumber: int) =
            this.useEffectHook (EffectHook.Create(HookIdentity.CallerCodeLocation callerLineNumber.Value, (fun _ -> handler(); null), triggers))


    type IComponentContextCore with


        /// <summary>
        ///
        /// </summary>
        /// <example>
        /// <code>
        ///
        /// type IComponentContextCore with
        ///     (* custom hook *)
        ///     member this.randomNumberHook ([&lt;CallerLineNumber&gt;] ?callerLineNumber: int) : IStateHook&lt;int&gt; =
        ///         this.useGrouped (callerLineNumber.Value, fun ctx ->
        ///             let a = ctx.useState ("a", ?callerLineNumber: callerLineNumber)
        ///             let b = ctx.useState ("b", ?callerLineNumber: callerLineNumber)
        ///             ctx.useState (randomInt.Next(), ?callerLineNumber: callerLineNumber)
        ///         )
        ///
        /// type Views () =
        ///     (* example component *)
        ///     static member ExampleView () =
        ///         Component ("example", fun ctx ->
        ///             let randomA = ctx.useRandomNumberHook ()
        ///             let randomB = ctx.useRandomNumberHook ()
        ///
        ///             TextBlock.create [
        ///                 // 'randomA' and 'randomB' can be different as they have different hook identities.
        ///                 TextBlock.text $"A: {randomA.current} B: {randomB.current}"
        ///             ]
        ///         )
        /// </code>
        /// </example>
        /// <remarks>
        /// Below an example of why grouping is needed / what problem this solves.
        /// <code>
        ///
        /// type IComponentContextCore with
        ///     (* custom hook *)
        ///     member this.randomNumberHook () : IStateHook&lt;int&gt; =
        ///         this.useState (randomInt.Next())
        ///
        /// type Views () =
        ///     (* example component *)
        ///     static member ExampleView () =
        ///         Component ("example", fun ctx ->
        ///             let randomA = ctx.useRandomNumberHook ()
        ///             let randomB = ctx.useRandomNumberHook ()
        ///
        ///             TextBlock.create [
        ///                 TextBlock.text $"A: {randomA.current} B: {randomB.current}"
        ///             ]
        ///         )
        /// </code>
        /// <para>
        /// The problem is that 'randomA' and 'randomB' both point to the same value, because the end up getting the same
        /// hook identity.
        /// This is the case because the Hook identity is implicitly provided by the caller line number. So the
        /// 'randomNumberHook' actually looks like this:
        /// </para>
        /// <code>
        /// type IComponentContextCore with
        ///     (* custom hook *)
        ///     member this.randomNumberHook () : IStateHook&lt;int&gt; =
        ///         this.useState (randomInt.Next(), callerLineNumber: 42)
        /// </code>
        /// <para>
        /// This problem can be solved by threading through the caller line number:
        /// </para>
        /// <code>
        /// type IComponentContextCore with
        ///     (* custom hook *)
        ///     member this.randomNumberHook ([&lt;CallerLineNumber&gt;] ?callerLineNumber: int) : IStateHook&lt;int&gt; =
        ///         this.useState (randomInt.Next(), ?callerLineNumber: callerLineNumber)
        /// </code>
        /// <para>
        /// This solves the initial problem, but what if the custom hook function needs to call multiple other hooks?
        /// This would require custom code to give each call a unique caller line number / or more generally a hook
        /// identity.
        /// </para>
        /// <code>
        /// type IComponentContextCore with
        ///     (* custom hook *)
        ///     member this.randomNumberHook ([&lt;CallerLineNumber&gt;] ?callerLineNumber: int) : IStateHook&lt;int&gt; =
        ///         let a = this.useState ("a", ?callerLineNumber: callerLineNumber)       // -|
        ///         let b = this.useState ("b", ?callerLineNumber: callerLineNumber)       // -|- same line number
        ///         this.useState (randomInt.Next(), ?callerLineNumber: callerLineNumber)  // -|
        /// </code>
        /// <para>
        /// It would be possible to solve this by manually threading through the caller line number and providing an
        /// offset / postfix for each call.
        /// This would be quite verbose and error prone, but is exactly what the 'group' function does implicitly.
        /// </para>
        /// </remarks>
        member this.useGrouped (callerLineNumber: int, render: IComponentContextCore -> 'state) : 'state =
            let ctxGroup = ContextGroup(this, callerLineNumber)
            render ctxGroup