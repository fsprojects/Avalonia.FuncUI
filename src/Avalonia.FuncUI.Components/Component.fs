namespace Avalonia.FuncUI

open System
open Avalonia.Controls
open Avalonia.Controls.Primitives
open Avalonia.FuncUI
open Avalonia.FuncUI.Types
open Avalonia.Media
open Avalonia.Styling
open Avalonia.FuncUI.VirtualDom

[<AllowNullLiteral>]
type Component (render: IComponentContext -> IView) =
    inherit ContentControl ()
    let context = new Context()
    let componentId = Guid.Unique

    let mutable lastViewElement : IView option = None

    member internal this.Context with get () = context
    member internal this.ComponentId with get () = componentId

    member private this.Update (nextViewElement : IView option) : unit =
        VirtualDom.updateRoot (this, lastViewElement, nextViewElement)
        lastViewElement <- nextViewElement

    member private this.Update () : unit =
        //printfn "Component.Update called"

        context
        |> render
        |> Some
        |> this.Update

        context.EffectQueue.ProcessAfterRender ()

    override this.OnInitialized () =
        base.OnInitialized ()

        (context :> IComponentContext).trackDisposable (
            context.OnRender.Subscribe (fun _ ->
                this.Update ()
            )
        )

        this.Update ()

    override this.Finalize () =
        base.Finalize ()
        (context :> IDisposable).Dispose ()

    interface IStyleable with
        member this.StyleKey = typeof<ContentControl>

type Component with

    static member create(key: string, render: IComponentContext -> IView) : IView<Component> =
        { View.ViewType = typeof<Component>
          View.ViewKey = ValueSome key
          View.Attrs = list.Empty
          View.ConstructorArgs = [| render :> obj |] }
        :> IView<Component>