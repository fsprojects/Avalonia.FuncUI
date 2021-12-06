namespace Avalonia.FuncUI

open Avalonia.Controls
open Avalonia.FuncUI.Types
open Avalonia.Styling
open Avalonia.FuncUI.VirtualDom

type Component (render: ComponentCtx -> IView) =
    inherit ContentControl ()
    let mutable ctx = ComponentCtx ()
    let mutable lastViewElement : IView option = None

    member private this.Update (nextViewElement : IView option) : unit =
        VirtualDom.updateRoot (this, lastViewElement, nextViewElement)
        lastViewElement <- nextViewElement

    member private this.Update () : unit =
        ctx.ResetIndex ()

        ctx
        |> render
        |> Some
        |> this.Update

    override this.OnInitialized () =
        base.OnInitialized ()

        this.Update ()

        ctx.OnSignal.Add (fun _ ->
            this.Update ()
        )

    interface IStyleable with
        member this.StyleKey = typeof<ContentControl>

type Component with

    static member create(key: string, render: ComponentCtx -> IView) : IView<Component> =
        { View.ViewType = typeof<Component>
          View.ViewKey = ValueSome key
          View.Attrs = list.Empty
          View.ConstructorArgs = [| render :> obj |] }
        :> IView<Component>