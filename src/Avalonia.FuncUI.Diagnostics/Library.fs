namespace Avalonia.FuncUI.Diagnostics

open Avalonia
open Avalonia.Controls
open Avalonia.FuncUI.Diagnostics

type InspectorWindow(attachedTo: Window) as this =
    inherit Window ()

    let onPositionOrSizeChanged () =
        match InspectorState.shared.InspectorWindowPosition.Current with
        | InspectorWindowPosition.Pinned ->
            this.Position <- PixelPoint(attachedTo.Position.X + int attachedTo.ClientSize.Width + 16, attachedTo.Position.Y)
            this.Height <- attachedTo.ClientSize.Height
        | _ -> ()

    do
        base.Title <- "FuncUI Inspector 🕵🏻‍♂️"
        base.Content <- InspectorViews.view
        base.CanResize <- false
        base.MinWidth <- 300.0
        base.MaxWidth <- 500.0
        base.Tag <- "inspector"

        onPositionOrSizeChanged ()

        attachedTo.PositionChanged.Add (ignore >> onPositionOrSizeChanged)
        attachedTo.LayoutUpdated.Add (ignore >> onPositionOrSizeChanged)

    let _ = InspectorState.shared.InspectorWindowPosition.Subscribe (ignore >> onPositionOrSizeChanged)

