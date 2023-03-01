namespace Avalonia.FuncUI

open System
open System.Collections.Generic
open Avalonia.FuncUI
open Avalonia.FuncUI.Types
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
    abstract useState<'value> : initialValue: 'value * ?renderOnChange: bool -> IWritable<'value>

    /// <summary>
    /// Attaches a value from a function to the component context.
    /// <example>
    /// <code>
    /// Component (fun ctx ->
    ///     let count = ctx.useStateLazy (fun () -> 42)
    ///     ..
    ///     // id will have the same value during the whole component lifetime. (unless changed via 'id.Set ..')
    ///     let id = ctx.useState ((fun () -> Guid.NewGuid()), renderOnChange=false)
    ///     ..
    /// )
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="buildInitialValue">function will be called once during the component lifetime.</param>
    /// <param name="renderOnChange">re-render component on change (default: true).</param>
    abstract useStateLazy<'value> : buildInitialValue: (unit -> 'value) * ?renderOnChange: bool -> IWritable<'value>

    /// <summary>
    /// Attaches a passed <c>IWritable&lt;'value&gt;</c> value to the component context.
    /// <example>
    /// <code>
    /// let countView (count: IWritable&lt;int&gt;) =
    ///     Component (fun ctx ->
    ///         // attach passed writable value to the component context
    ///         let count = ctx.usePassed (count)
    ///         ..
    ///     )
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="value">value should not change during the component lifetime.</param>
    /// <param name="renderOnChange">re-render component on change (default: true).</param>
    abstract usePassed<'value> : value: IWritable<'value> * ?renderOnChange: bool -> IWritable<'value>

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
    abstract usePassedRead<'value> : value: IReadable<'value> * ?renderOnChange: bool -> IReadable<'value>

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
    abstract useEffect: handler: (unit -> IDisposable) * triggers: EffectTrigger list -> unit

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
    abstract useEffect: handler: (unit ->  unit) * triggers: EffectTrigger list -> unit

    /// <summary>
    /// <para>
    /// Specify attributes for the component.
    /// </para>
    /// <para>
    /// Calling <c>ctx.attrs [..]</c> does not trigger a render and only is applied when
    /// the component is rendered.
    /// </para>
    /// <para>
    /// This is useful when attributes need to be set directly on the component, for example
    /// when using a component in a grid/dock panel.
    ///
    /// <example>
    /// <code>
    /// ctx.attrs [
    ///     Border.dock Dock.Top
    ///     Border.row 0
    /// ]
    /// </code>
    /// </example>
    /// </para>
    /// </summary>
    abstract attrs: IAttr<Avalonia.Controls.Border> list -> unit

    /// <summary>
    /// The underlying Avalonia control.
    /// </summary>
    abstract control: Avalonia.Controls.Border

type Context (componentControl: Avalonia.Controls.Border) =
    let disposables = new DisposableBag ()
    let hooks = Dictionary<int, StateHook>()
    let effects = Dictionary<int, EffectHook>()
    let effectQueue =
        let effectQueue = new EffectQueue()
        disposables.Add effectQueue
        effectQueue

    let mutable componentAttrs: IAttr<Avalonia.Controls.Border> list = List.empty

    let mutable callingIndex = 0

    let incrementCallingIndex (func: unit -> 't) : 't =
        let result = func ()
        callingIndex <- callingIndex + 1
        result

    let render = Event<unit>()

    member internal this.EffectQueue = effectQueue

    member internal this.OnRender = render.Publish

    member internal this.Hooks with get () = Map.ofDict hooks

    member internal this.ComponentAttrs with get () : IAttr list =
        componentAttrs
        |> Seq.cast<IAttr>
        |> Seq.toList

    member internal this.AfterRender () =
        callingIndex <- 0

    member private this.useStateHook<'value>(stateHook: StateHook) : IReadable<'value> =
        incrementCallingIndex (fun () ->
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
        )

    member private this.useEffectHook (effect: EffectHook) : unit =
        incrementCallingIndex (fun () ->
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
        )

    interface IComponentContext with
        member this.forceRender () =
            render.Trigger ()

        member this.trackDisposable (item: IDisposable) : unit =
            disposables.Add item

        member this.attrs (attrs: IAttr<Avalonia.Controls.Border> list) : unit =
            componentAttrs <- attrs

        member this.useEffect (effect: unit -> IDisposable, triggers: EffectTrigger list) : unit =
            this.useEffectHook (
                EffectHook.Create(
                    identity = callingIndex,
                    effect = effect,
                    triggers = triggers
                )
            )

        member this.useEffect (effect: unit -> unit, triggers: EffectTrigger list) : unit =
            this.useEffectHook (
                EffectHook.Create(
                    identity = callingIndex,
                    effect = (fun _ -> effect(); null),
                    triggers = triggers
                )
            )

        member this.usePassed(value: IWritable<'t>, ?renderOnChange: bool) =
            this.useStateHook<'value>(
                StateHook.Create (
                    identity = callingIndex,
                    state = StateHookValue.Const value,
                    renderOnChange = defaultArg renderOnChange true
                )
            ) :?> IWritable<'value>

        member this.usePassedRead(value: IReadable<'t>, ?renderOnChange: bool) =
            this.useStateHook<'value>(
                StateHook.Create (
                    identity = callingIndex,
                    state = StateHookValue.Const value,
                    renderOnChange = defaultArg renderOnChange true
                )
            )

        member this.useState(initialValue: 't, ?renderOnChange: bool) =
            this.useStateHook<'value>(
                StateHook.Create (
                    identity = callingIndex,
                    state = StateHookValue.Lazy (fun _ -> new State<'value>(initialValue) :> IAnyReadable),
                    renderOnChange = defaultArg renderOnChange true
                )
            ) :?> IWritable<'value>

        member this.useStateLazy(buildInitialValue: unit -> 'value, ?renderOnChange: bool) =
            this.useStateHook<'value>(
                StateHook.Create (
                    identity = callingIndex,
                    state = StateHookValue.Lazy (fun _ -> new State<'value>(buildInitialValue()) :> IAnyReadable),
                    renderOnChange = defaultArg renderOnChange true
                )
            ) :?> IWritable<'value>


        member this.control = componentControl

    interface IDisposable with
        member this.Dispose () =
            (disposables :> IDisposable).Dispose()
