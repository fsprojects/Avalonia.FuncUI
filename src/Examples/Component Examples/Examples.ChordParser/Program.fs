﻿namespace Examples.ChordParser

open Avalonia
open Avalonia.FuncUI.Diagnostics
open Avalonia.Themes.Fluent
open Avalonia.FuncUI.Components.Hosts
open Avalonia.Controls.ApplicationLifetimes

type MainWindow() =
    inherit HostWindow()
    do
        base.Title <- "Chord Parser"
        base.Content <- ChordParserView.cmp ()

type App() =
    inherit Application()

    override this.Initialize() =
        this.Styles.Add (FluentTheme(baseUri = null, Mode = FluentThemeMode.Dark))

    override this.OnFrameworkInitializationCompleted() =
        match this.ApplicationLifetime with
        | :? IClassicDesktopStyleApplicationLifetime as desktopLifetime ->
            let mainWindow = MainWindow()
            InspectorWindow(mainWindow).Show()
            //mainWindow.Renderer.DrawDirtyRects <- true
            desktopLifetime.MainWindow <- mainWindow
        | _ -> ()

module Program =

    [<EntryPoint>]
    let main(args: string[]) =
        AppBuilder
            .Configure<App>()
            .UsePlatformDetect()
            .UseSkia()
            .StartWithClassicDesktopLifetime(args)