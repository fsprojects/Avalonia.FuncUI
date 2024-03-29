﻿namespace Avalonia.FuncUI.ControlCatalog

open Avalonia
open Avalonia.Controls.ApplicationLifetimes
open Avalonia.FuncUI.Hosts
open Elmish
open Avalonia.FuncUI.Elmish
open Avalonia.FuncUI.ControlCatalog.Views
open Avalonia.Themes.Fluent
open Avalonia.FuncUI
open Avalonia.Controls

type MainWindow() as this =
    inherit HostWindow()

    do
        base.Title <- "Control Catalog"
        base.Height <- 600.0
        base.Width <- 800.0

        TopLevel.GetTopLevel(this).RendererDiagnostics.DebugOverlays <- Avalonia.Rendering.RendererDebugOverlays.Fps
        //this.VisualRoot.VisualRoot.Renderer.DrawDirtyRects <- true
        ()

type App() =
    inherit Application()

    override this.Initialize() =
        this.Styles.Add (FluentTheme())
        this.Styles.Load "avares://Avalonia.FuncUI.ControlCatalog/Styles/TabControl.xaml"

    override this.OnFrameworkInitializationCompleted() =
        let host =
            match this.ApplicationLifetime with
            | :? ISingleViewApplicationLifetime as single ->
                let mainView = HostControl()
                single.MainView <-mainView
                Some (mainView :> IViewHost)
               
            | :? IClassicDesktopStyleApplicationLifetime as desktopLifetime ->
                let mainWindow = MainWindow()
                desktopLifetime.MainWindow <- mainWindow
                Some mainWindow
            
            | _ -> None

        match host with
        | Some hostControl ->
            Elmish.Program.mkSimple MainView.init MainView.update MainView.view
            |> Program.withHost hostControl
            |> Program.withConsoleTrace
            |> Program.run
        | _ -> ()

        base.OnFrameworkInitializationCompleted()
