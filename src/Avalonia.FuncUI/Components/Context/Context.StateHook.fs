namespace Avalonia.FuncUI

open System
open System.Runtime.CompilerServices
open Avalonia.FuncUI

[<RequireQualifiedAccess>]
type StateHookError =
    | InstanceIdMismatch of expectedInstanceId: Guid * actualInstanceId: Guid * identity: string

    member this.ErrorString with get () =
        match this with
        | StateHookError.InstanceIdMismatch (expected, actual, identity) ->
            String.ofLines [
                "🪝 Hook Error: InstanceIdMismatch"
                $"    expected: '{expected}', actual '{actual}', identity: '{identity}'"
                "    This usually means a component key should have changed, but it didn't."
            ]

[<Struct; IsReadOnly>]
type StateHookValueResolver =
    | Const of constant: IAnyReadable
    | Lazy of factory: (unit -> IAnyReadable)

type StateHookValue (resolver: StateHookValueResolver) =
    let mutable resolved: IAnyReadable voption = ValueNone

    member this.Value with get () =
        match resolved with
        | ValueNone ->
            let resolved' =
                match resolver with
                | StateHookValueResolver.Const constant -> constant
                | StateHookValueResolver.Lazy factory -> (factory ())

            resolved <- ValueSome resolved'
            resolved'

        | ValueSome resolved ->
            resolved

    /// <summary>
    /// <example>
    /// <para>
    /// Lazy values are usually used with local state.
    /// </para>
    /// <code>
    /// StateHookValue.Lazy (fun _ -> Value 42)
    /// </code>
    /// </example>
    /// </summary>
    static member Lazy (factory: unit -> IAnyReadable) : StateHookValue =
        factory
        |> StateHookValueResolver.Lazy
        |> StateHookValue

    /// <summary>
    /// <example>
    /// <para>
    /// Const values are usually used for passed/global state.
    /// </para>
    /// <code>
    /// StateHookValue.Const passedValue
    /// </code>
    /// </example>
    /// </summary>
    static member Const (constant:  IAnyReadable) : StateHookValue =
        constant
        |> StateHookValueResolver.Const
        |> StateHookValue


[<Struct>]
type StateHook =
    { /// The hooks identity is used to uniquely identify the hook in it's context.
      /// The identity is determined by the calling order of the hooks.
      Identity: int
      /// The hooks value resolver is used to cut down the cost per render by only
      /// resolving expensive (local) values once.
      State: StateHookValue
      /// Specifies if the component is re-rendered when the hook value changes.
      RenderOnChange: bool }

    static member Create (identity, state, renderOnChange) =
        { StateHook.Identity = identity
          StateHook.State = state
          StateHook.RenderOnChange = renderOnChange }