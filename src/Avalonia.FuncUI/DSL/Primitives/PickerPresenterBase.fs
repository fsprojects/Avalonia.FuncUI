namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module PickerPresenterBase =
    open System
    open Avalonia.Controls.Primitives
    open Avalonia.FuncUI.Builder

    type PickerPresenterBase with
        static member onConfirmed<'t when 't :> PickerPresenterBase>(func: 't -> unit, ?subPatchOptions) =
            let name = nameof Unchecked.defaultof<'t>.Confirmed
            let factory: SubscriptionFactory<'t> =
                fun (control, func, token) ->
                    let control = control :?> 't
                    let handler = EventHandler(fun s e -> func(s :?> 't))
                    let event = control.Confirmed

                    event.AddHandler(handler)
                    token.Register(fun _ -> event.RemoveHandler(handler)) |> ignore

            AttrBuilder<'t>.CreateSubscription<'t>(name, factory, func, ?subPatchOptions = subPatchOptions)

        static member onDismissed<'t when 't :> PickerPresenterBase>(func: 't -> unit, ?subPatchOptions) =
            let name = nameof Unchecked.defaultof<'t>.Dismissed
            let factory: SubscriptionFactory<'t> =
                fun (control, func, token) ->
                    let control = control :?> 't
                    let handler = EventHandler(fun s e -> func(s :?> 't))
                    let event = control.Dismissed

                    event.AddHandler(handler)
                    token.Register(fun _ -> event.RemoveHandler(handler)) |> ignore

            AttrBuilder<'t>.CreateSubscription<'t>(name, factory, func, ?subPatchOptions = subPatchOptions)
