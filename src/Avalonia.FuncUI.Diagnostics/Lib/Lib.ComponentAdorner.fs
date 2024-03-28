namespace Avalonia.FuncUI.Diagnostics

open Avalonia.Controls
open Avalonia.Controls.Primitives
open Avalonia.FuncUI
open Avalonia.Media
open Avalonia.Media.Immutable

type internal ComponentHighlightAdorner (adornedElement: Component) =
    inherit Control ()

    override this.Render (ctx: DrawingContext) =
        base.Render ctx
        ctx.FillRectangle (ImmutableSolidColorBrush(Colors.Blue, 0.2), adornedElement.Bounds.WithX(0.0).WithY(0.0))

    member this.Component = adornedElement

    static member Attache (adornedElement: Component) =
        let adorner = ComponentHighlightAdorner adornedElement
        adorner.IsHitTestVisible <- false

        let layer = AdornerLayer.GetAdornerLayer adornedElement

        if not (isNull layer) then
            let alreadyAttached =
                layer.Children
                |> Seq.toList // copy to be safe
                |> Seq.filter (fun c -> c.GetType() = typeof<ComponentHighlightAdorner>)
                |> Seq.cast<ComponentHighlightAdorner>
                |> Seq.exists (fun c -> c.Component.ComponentId = adornedElement.ComponentId)

            if not alreadyAttached then
                layer.Children.Add adorner

                AdornerLayer.SetAdornedElement (adorner, adornedElement)
                AdornerLayer.SetLeft (adorner, 0.0)
                AdornerLayer.SetIsClipEnabled (adorner, false)

    static member Remove (adornedElement: Component) =
        let layer = AdornerLayer.GetAdornerLayer adornedElement

        if not (isNull layer) then
            layer.Children
            |> Seq.toList // copy to be safe
            |> Seq.filter (fun c -> c.GetType() = typeof<ComponentHighlightAdorner>)
            |> Seq.cast<ComponentHighlightAdorner>
            |> Seq.filter (fun c -> c.Component.ComponentId = adornedElement.ComponentId)
            |> Seq.cast<Control>
            |> layer.Children.RemoveAll