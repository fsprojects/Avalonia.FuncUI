namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module ResourceProvider =
    open Avalonia.Controls
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
    open Avalonia

    type ResourceProvider with

        /// <summary>
        /// Create subscription from `'view.GetResourceObservable`.
        /// </summary>
        /// <remarks>
        /// - If `'view.Orner` is `null`, `func` is never called.
        /// - Observation result is wrapped in `option` type: `None` for `null` or `AvaloniaProperty.UnsetValue`, `Some value` otherwise.
        /// </remarks>
        static member onResourceObservable<'t when 't :> ResourceProvider>
            (key: obj, func: obj option -> unit, ?defaultThemeVariant, ?subPatchOptions)
            : IAttr<'t> =
            let factory: SubscriptionFactory<obj option> =
                fun (control, func, token) ->
                    let callback (v: obj) =
                        match v with
                        | null -> None
                        | v when v = AvaloniaProperty.UnsetValue -> None
                        | _ -> Some v
                        |> func
                    let subscription =
                        let control = control :?> 't

                        match defaultThemeVariant with
                        | Some themeVariant -> control.GetResourceObservable(key, defaultThemeVariant=themeVariant)
                        | None -> control.GetResourceObservable(key)
                        |> _.Subscribe(callback)

                    token.Register(fun () -> subscription.Dispose()) |> ignore

            let name =
                let viewType = typeof<'t>.Name
                let defaultThemeVariantStr =
                    match defaultThemeVariant with
                    | Some v -> $".{v}"
                    | None -> ""
                $"{viewType}.ResourceObservable.{key}{defaultThemeVariantStr}"

            AttrBuilder<'t>
                .CreateSubscription<obj option>(name, factory, func, ?subPatchOptions = subPatchOptions)
