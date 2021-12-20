namespace Avalonia.FuncUI

open System
open Avalonia.Controls
open Avalonia.FuncUI.Types
open Avalonia.Styling
open Avalonia.FuncUI.VirtualDom

type Component (render: Context -> IView) =
    inherit ContentControl ()
    let context = new Context()
    let mutable lastViewElement : IView option = None

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

        context.useDisposable (
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

    static member create(key: string, render: Context -> IView) : IView<Component> =
        { View.ViewType = typeof<Component>
          View.ViewKey = ValueSome key
          View.Attrs = list.Empty
          View.ConstructorArgs = [| render :> obj |] }
        :> IView<Component>