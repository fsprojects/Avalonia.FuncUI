namespace Avalonia.FuncUI.Experimental

open System
open System.Runtime.CompilerServices
open System.Threading
open Avalonia.Controls
open Avalonia.FuncUI
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Types
open Avalonia.Media

(*
    Ported from https://github.com/Zaid-Ajaj/Feliz/tree/master/Feliz.UseDeferred by Zaid Ajaj
*)

[<RequireQualifiedAccess>]
type Deferred<'T> =
    | HasNotStartedYet
    | InProgress
    | Resolved of 'T
    | Failed of exn

[<AutoOpen>]
module ContextExtensions =

    type IComponentContext with

        member ctx.useDeferred(operation: Async<'T>, dependencies: IAnyReadable seq) =
            let deferred = ctx.useState Deferred.HasNotStartedYet

            let executeOperation = async {
                try
                    do deferred .= Deferred<'T>.InProgress
                    let! output = operation
                    do deferred .= Deferred<'T>.Resolved output
                with error ->
                    do deferred .= (Deferred<'T>.Failed error)
            }

            ctx.useEffect (
                handler = (fun () ->
                    let ct = new CancellationTokenSource()

                    Async.Start (executeOperation, ct.Token)

                    ct :> IDisposable
                ),
                triggers = [
                    yield EffectTrigger.AfterInit

                    for dependency in dependencies do
                       yield  EffectTrigger.AfterChange dependency
                ]

            )

            deferred

        member ctx.useDeferredCallback(operation: 'TIn -> Async<'TOut>, setDeferred: Deferred<'TOut> -> unit) =
            let cancellationToken = ctx.useState(new CancellationTokenSource(), renderOnChange = false)
            let executeOperation arg = async {
                try
                    do setDeferred(Deferred<'TOut>.InProgress)
                    let! output = operation arg
                    do setDeferred(Deferred<'TOut>.Resolved output)
                with error ->
                    do setDeferred(Deferred<'TOut>.Failed error)
            }

            ctx.useEffect (
                handler = (fun () ->
                    cancellationToken.Current :> IDisposable
                ),
                triggers = [ EffectTrigger.AfterInit ]
            )

            let start (arg: 'TIn) =
                if not cancellationToken.Current.IsCancellationRequested then
                    Async.Start(executeOperation arg, cancellationToken.Current.Token)

            start

[<AbstractClass; Sealed; Extension>]
type DeferredExtensions =

    [<Extension>]
    static member Map(data: IReadable<Deferred<'t>>, func: 't -> IView) : IView  =
        match data.Current with
        | Deferred.HasNotStartedYet ->
            ProgressBar.create [
                ProgressBar.isIndeterminate false
            ]

        | Deferred.InProgress ->
            ProgressBar.create [
                ProgressBar.isIndeterminate true
            ]

        | Deferred.Resolved data ->
            func data

        | Deferred.Failed ex ->
            TextBlock.create [
                TextBlock.text $"Error: %A{ex}"
                TextBlock.foreground Brushes.Red
            ]
            |> generalize


