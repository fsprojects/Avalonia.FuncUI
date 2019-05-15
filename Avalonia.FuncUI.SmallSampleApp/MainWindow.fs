namespace Avalonia.FuncUI.SmallSampleApp

open Avalonia.Controls
open Avalonia.FuncUI.Hosts

type MainWindow() as this =
    inherit HostWindow()

    do
        base.Title <- "FuncUI Sample"
        base.Height <- 800.0
        base.Width <- 1000.0     
        
        //this.VisualRoot.VisualRoot.Renderer.DrawFps <- true
        //this.VisualRoot.VisualRoot.Renderer.DrawDirtyRects <- true
        ()
