namespace Avalonia.FuncUI

open System
open Avalonia.Controls
open Avalonia.FuncUI.Types
open Avalonia.FuncUI.DSL
open Avalonia.LogicalTree
open Avalonia.Styling
#nowarn "57"

type EnvironmentState<'value> =
    internal {
        Name: string
        DefaultValue: IWritable<'value> option
    }

    [<Experimental "this feature is experimental. The API might change.">]
    static member Create (name: string, ?defaultValue: IWritable<'value>) : EnvironmentState<'value> =
        { Name = name
          DefaultValue = defaultValue }

[<Experimental "this feature is experimental. The API might change.">]
[<AllowNullLiteral>]
type EnvironmentStateProvider<'value>
  ( state: EnvironmentState<'value>,
    providedState: IWritable<'value>) as this =

    inherit ContentControl ()

    member this.EnvironmentState with get () = state

    member this.ProvidedState with get () = providedState

    override this.StyleKeyOverride = typeof<ContentControl>

type EnvironmentStateProvider<'value> with

    [<Experimental "this feature is experimental. The API might change.">]
    static member create (state: EnvironmentState<'value>, providedValue: IWritable<'value>, content: IView) =
        { View.ViewType = typeof<EnvironmentStateProvider<'value>>
          View.ViewKey = ValueNone
          View.Attrs = [ ContentControl.content content ]
          View.Outlet = ValueNone
          View.ConstructorArgs = [| state :> obj; providedValue :> obj |] }
        :> IView<EnvironmentStateProvider<'value>>

type EnvironmentState<'value> with

    [<Experimental "this feature is experimental. The API might change.">]
    member this.provide (providedValue: IWritable<'value>, content: IView) =
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

        [<Experimental "this feature is experimental. The API might change.">]
        member this.useEnvState (state: EnvironmentState<'value>, ?renderOnChange: bool) =
           let obtainValue () =
               match EnvironmentStateConsumer.tryFind (this.control, state), state.DefaultValue with
               | ValueSome value, _ -> value
               | ValueNone, Some defaultValue -> defaultValue
               | ValueNone, None -> failwithf "No value provided for environment state '%s'" state.Name

           this.usePassedLazy (
               obtainValue = obtainValue,
               ?renderOnChange = renderOnChange
           )
