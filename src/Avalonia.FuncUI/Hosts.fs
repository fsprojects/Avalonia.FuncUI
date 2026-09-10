namespace Avalonia.FuncUI.Hosts

open Avalonia.Controls
open Avalonia.Styling
open Avalonia.FuncUI.Types
open Avalonia.FuncUI.VirtualDom

/// Hosts attributes on the application instance created by AppBuilder.
/// Mount after framework initialization when using native menus or tray icons.
type HostApplication() as this =
    inherit Avalonia.Application()

    let mutable lastAttrs: IAttr list = []
    let mutable mounted: System.IDisposable option = None
    let mutable disposed = false
    let mutable lifetimeSubscription: System.IDisposable option = None

    let apply (attrs: IAttr<Avalonia.Application> list) =
        let next = attrs |> List.map (fun attr -> attr :> IAttr)
        Patcher.patch(this, {
            Delta.ViewDelta.ViewType = this.GetType()
            ConstructorArgs = null
            KeyDidChange = false
            Outlet = ValueNone
            Attrs = Differ.diffAttributes(lastAttrs, next)
        })
        lastAttrs <- next

    let unmount () =
        let previous = mounted
        mounted <- None
        previous |> Option.iter (fun mountedView -> mountedView.Dispose())

    let verify () =
        Avalonia.Threading.Dispatcher.UIThread.VerifyAccess()
        if disposed then raise (System.ObjectDisposedException(nameof HostApplication))

    /// Apply attributes directly, for example from an Elmish view function.
    member _.Update(attrs: IAttr<Avalonia.Application> list) =
        verify()
        unmount()
        apply attrs

    /// Replace the mounted component. Ordinary state changes preserve its hooks.
    member _.Update(view: Avalonia.FuncUI.ApplicationComponent) =
        verify()
        unmount()
        let context = new Avalonia.FuncUI.Context(None)
        let mutable active = true
        let mutable pending = false
        let render () =
            if active then
                let attrs = view.Render (context :> Avalonia.FuncUI.IStateContext)
                context.AfterRender()
                apply attrs
                context.EffectQueue.ProcessAfterRender()
        let subscription = context.OnRender.Subscribe(fun () ->
            if active && not pending then
                pending <- true
                Avalonia.Threading.Dispatcher.UIThread.Post(fun () ->
                    pending <- false
                    render()))
        mounted <- Some { new System.IDisposable with
            member _.Dispose() =
                active <- false
                subscription.Dispose()
                (context :> System.IDisposable).Dispose() }
        try render()
        with _ ->
            unmount()
            reraise()

    /// Release application hooks, native objects and event subscriptions.
    member _.Dispose() =
        Avalonia.Threading.Dispatcher.UIThread.VerifyAccess()
        if not disposed then
            disposed <- true
            lifetimeSubscription |> Option.iter (fun subscription -> subscription.Dispose())
            lifetimeSubscription <- None
            try unmount()
            finally apply []

    override _.OnFrameworkInitializationCompleted() =
        match this.ApplicationLifetime with
        | :? Avalonia.Controls.ApplicationLifetimes.IControlledApplicationLifetime as lifetime when not disposed ->
            lifetimeSubscription |> Option.iter (fun subscription -> subscription.Dispose())
            lifetimeSubscription <- Some (lifetime.Exit.Subscribe(fun _ -> this.Dispose()))
        | _ -> ()
        base.OnFrameworkInitializationCompleted()

    interface System.IDisposable with
        member _.Dispose() = this.Dispose()

type IViewHost =
    abstract member Update: IView option -> unit

type HostWindow() as this =
    inherit Window()

    let mutable lastViewElement : IView option = None

    let update (nextViewElement : IView option) : unit =
        VirtualDom.updateRoot (this, lastViewElement, nextViewElement)
        lastViewElement <- nextViewElement

    interface IViewHost with
        member this.Update next =
            update next

type HostControl() as this =
    inherit ContentControl()

    let mutable lastViewElement : IView option = None

    let update (nextViewElement : IView option) : unit =
        VirtualDom.updateRoot (this, lastViewElement, nextViewElement)
        lastViewElement <- nextViewElement

    override this.StyleKeyOverride = typeof<ContentControl>

    interface IViewHost with
        member this.Update next =
            update next