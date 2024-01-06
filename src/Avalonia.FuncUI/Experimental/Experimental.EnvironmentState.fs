namespace Avalonia.FuncUI.Experimental

open System
open Avalonia.Controls
open Avalonia.FuncUI
open Avalonia.FuncUI.Types
open Avalonia.FuncUI.DSL
open Avalonia.LogicalTree

#nowarn "57"

type EnvironmentState<'value> =
    internal {
        Name: string
        DefaultValue: 'value option
    }

    static member Create (name: string, ?defaultValue: 'value) : EnvironmentState<'value> =
        { Name = name
          DefaultValue = defaultValue }

[<AllowNullLiteral>]
type EnvironmentStateProvider<'value>
  ( state: EnvironmentState<'value>,
    providedState: 'value) =

    inherit ContentControl ()

    member this.EnvironmentState with get () = state

    member this.ProvidedState with get () = providedState

    override this.StyleKeyOverride = typeof<ContentControl>

type EnvironmentStateProvider<'value> with

    static member create (state: EnvironmentState<'value>, providedValue: 'value, content: IView) =
        { View.ViewType = typeof<EnvironmentStateProvider<'value>>
          View.ViewKey = ValueNone
          View.Attrs = [ ContentControl.content content ]
          View.Outlet = ValueNone
          View.ConstructorArgs = [| state :> obj; providedValue :> obj |] }
        :> IView<EnvironmentStateProvider<'value>>

type EnvironmentState<'value> with

    member this.provide (providedValue: 'value, content: IView) =
        EnvironmentStateProvider<'value>.create(this, providedValue, content)

[<RequireQualifiedAccess>]
module private EnvironmentStateConsumer =

    let rec private tryFindNext (control: EnvironmentStateProvider<'value>, state: EnvironmentState<'value>) =
        match control.FindLogicalAncestorOfType<EnvironmentStateProvider<'value>>(includeSelf = false) with
        | null -> ValueNone
        | ancestor ->
            if ancestor.EnvironmentState.Name.Equals(state.Name, StringComparison.Ordinal) then
                ValueSome ancestor.ProvidedState
            else
                tryFindNext (ancestor, state)

    let tryFind (control: Control, state: EnvironmentState<'value>) =
        match control.FindLogicalAncestorOfType<EnvironmentStateProvider<'value>>(includeSelf = false) with
        | null -> ValueNone
        | ancestor ->
            if ancestor.EnvironmentState.Name.Equals(state.Name, StringComparison.Ordinal) then
                ValueSome ancestor.ProvidedState
            else
                tryFindNext (ancestor, state)

[<AutoOpen>]
module __ContextExtensions_useEnvHook =

    type IComponentContext with

        member this.readEnvValue(state: EnvironmentState<'value>) : 'value =
            match EnvironmentStateConsumer.tryFind (this.control, state), state.DefaultValue with
            | ValueSome value, _ -> value
            | ValueNone, Some defaultValue -> defaultValue
            | ValueNone, None -> failwithf "No value provided for environment value '%s'" state.Name

        member this.useEnvState(state: EnvironmentState<IWritable<'value>>, ?renderOnChange: bool) : IWritable<'value> =
           this.usePassedLazy (
               obtainValue = (fun () -> this.readEnvValue(state)),
               ?renderOnChange = renderOnChange
           )

