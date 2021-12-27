namespace Avalonia.FuncUI.Diagnostics

open Avalonia.Controls
open Avalonia.FuncUI

type internal ChildWindow(title: string, comp: Component) =
    inherit Window ()

    do
        base.Width <- 400.0
        base.Height <- 600.0
        base.Title <- title
        base.Tag <- "inspector"
        base.Content <- comp
