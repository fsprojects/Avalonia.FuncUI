namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module NativeMenu =
    open Avalonia.Controls
    open Avalonia.FuncUI.Builder
    open Avalonia.FuncUI.Types
     
    let create (attrs: IAttr<NativeMenu> list): IView<NativeMenu> =
        ViewBuilder.Create<NativeMenu>(attrs)
     
    type NativeMenu with
        static member onNeedsUpdate<'t when 't :> NativeMenu>(func: System.EventArgs -> unit, ?subPatchOptions: SubPatchOptions) : IAttr<'t> =
            let factory: SubscriptionFactory<System.EventArgs> = fun (target, handler, token) ->
                let subscription = (target :?> NativeMenu).NeedsUpdate.Subscribe handler
                token.Register(fun () -> subscription.Dispose()) |> ignore
            let name = nameof Unchecked.defaultof<'t>.NeedsUpdate
            AttrBuilder<'t>.CreateSubscription(name, factory, func, ?subPatchOptions = subPatchOptions)

        static member onOpening<'t when 't :> NativeMenu>(func: System.EventArgs -> unit, ?subPatchOptions: SubPatchOptions) : IAttr<'t> =
            let factory: SubscriptionFactory<System.EventArgs> = fun (target, handler, token) ->
                let subscription = (target :?> NativeMenu).Opening.Subscribe handler
                token.Register(fun () -> subscription.Dispose()) |> ignore
            let name = nameof Unchecked.defaultof<'t>.Opening
            AttrBuilder<'t>.CreateSubscription(name, factory, func, ?subPatchOptions = subPatchOptions)

        static member onClosed<'t when 't :> NativeMenu>(func: System.EventArgs -> unit, ?subPatchOptions: SubPatchOptions) : IAttr<'t> =
            let factory: SubscriptionFactory<System.EventArgs> = fun (target, handler, token) ->
                let subscription = (target :?> NativeMenu).Closed.Subscribe handler
                token.Register(fun () -> subscription.Dispose()) |> ignore
            let name = nameof Unchecked.defaultof<'t>.Closed
            AttrBuilder<'t>.CreateSubscription(name, factory, func, ?subPatchOptions = subPatchOptions)

        static member items<'t when 't :> NativeMenu>(value: IView list) : IAttr<'t> =
            let getter : ('t -> obj) = (fun control -> control.Items :> obj)

            AttrBuilder<'t>.CreateContentMultiple("Items", ValueSome getter, ValueNone, value)