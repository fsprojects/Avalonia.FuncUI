namespace Avalonia.FuncUI

open System
open System.Diagnostics.CodeAnalysis
open Avalonia.Controls
open Avalonia.FuncUI
open Avalonia.FuncUI.Types
open Avalonia.FuncUI.VirtualDom
open Avalonia.Threading

[<AllowNullLiteral>]
[<AbstractClass>]
[<DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)>]
type ComponentBase() as this =
    inherit Border ()
    let context = new Context(this)
    let componentId = Guid.Unique

    let mutable lastViewElement : IView option = None
    let mutable lastViewAttrs: IAttr list = List.empty
    let mutable pending = false
    let mutable disposed = false

    member internal this.Context with get () = context
    member internal this.ComponentId with get () = componentId

    abstract member Render : IComponentContext -> IView

    member private this.UIThreadUpdate() : unit =
        let nextViewElement = Some (this.Render context)

        // reset internal context counter
        context.AfterRender ()

        // update view
        VirtualDom.updateBorderRoot (this, lastViewElement, nextViewElement)
        lastViewElement <- nextViewElement

        let nextViewAttrs = context.ComponentAttrs

        // update attrs
        Patcher.patch (
            this,
            { Delta.ViewDelta.ViewType = typeof<Border>
              Delta.ViewDelta.ConstructorArgs = null
              Delta.ViewDelta.KeyDidChange = false
              Delta.ViewDelta.Outlet = ValueNone
              Delta.ViewDelta.Attrs = Differ.diffAttributes (lastViewAttrs, nextViewAttrs) }
        )

        lastViewAttrs <- nextViewAttrs

        context.EffectQueue.ProcessAfterRender ()

    member private this.Update () : unit =
        if not disposed && not pending then
            pending <- true
            Dispatcher.UIThread.Post (fun _ ->
                // Allow requests made during rendering to schedule the next pass.
                pending <- false
                if not disposed then this.UIThreadUpdate ())

    /// Queue a render. Requests made while a render is pending share that pass.
    /// AfterRender effects run for actual renders, not for every request.
    member this.ForceRender () =
        this.Update ()

    override this.OnInitialized () =
        base.OnInitialized ()

        (context :> IComponentContext).trackDisposable (
            context.OnRender.Subscribe (fun _ ->
                this.Update ()
            )
        )

        this.UIThreadUpdate ()

    override this.OnDetachedFromLogicalTree (eventArgs: Avalonia.LogicalTree.LogicalTreeAttachmentEventArgs) =
        base.OnDetachedFromLogicalTree eventArgs
        disposed <- true
        (context :> IDisposable).Dispose ()

    override this.StyleKeyOverride = typeof<Border>