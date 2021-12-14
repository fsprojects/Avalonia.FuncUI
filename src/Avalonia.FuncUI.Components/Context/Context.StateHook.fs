namespace Avalonia.FuncUI

open System
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

[<RequireQualifiedAccess; Struct>]
type StateHookValueResolver =
    | Const of constant: IConnectable
    | Lazy of factory: (unit -> IConnectable)

type StateHookValue (resolver: StateHookValueResolver) =
    let mutable resolved: IConnectable voption = ValueNone

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

    static member Lazy (factory: unit -> IConnectable) : StateHookValue =
        factory
        |> StateHookValueResolver.Lazy
        |> StateHookValue

    static member Const (constant:  IConnectable) : StateHookValue =
        constant
        |> StateHookValueResolver.Const
        |> StateHookValue


[<Struct>]
type StateHook =
    { Identity: string
      State: StateHookValue
      RenderOnChange: bool }

    static member Create (identity, state, renderOnChange) =
        { StateHook.Identity = identity
          StateHook.State = state
          StateHook.RenderOnChange = renderOnChange }