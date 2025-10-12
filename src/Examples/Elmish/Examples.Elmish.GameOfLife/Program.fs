﻿namespace Examples.GameOfLife

open System
open Avalonia
open Avalonia.Controls
open Avalonia.Themes.Fluent
open Elmish
open Avalonia.FuncUI.Hosts
open Avalonia.FuncUI
open Avalonia.FuncUI.Elmish
open Avalonia.Controls.ApplicationLifetimes
open Avalonia.Threading

type MainWindow() as this =
    inherit HostWindow()
    do
        base.Title <- "Game of Life Example"
        base.Icon <- WindowIcon(System.IO.Path.Combine("Assets","Icons", "icon.ico"))
        base.Height <- 500.0
        base.Width <- 500.0

        let subscriptions (_state: Main.State) =
            let timerSub (dispatch: Main.Msg -> unit) =
                let invoke() =
                    Board.Evolve |> Main.BoardMsg |> dispatch
                    true

                DispatcherTimer.Run(Func<bool>(invoke), TimeSpan.FromMilliseconds 100.0)

            [
                [ nameof timerSub ], timerSub
            ]

        //this.VisualRoot.VisualRoot.Renderer.DrawFps <- true
        //this.VisualRoot.VisualRoot.Renderer.DrawDirtyRects <- true

        Elmish.Program.mkProgram Main.init Main.update Main.view
        |> Program.withHost this
        |> Program.withSubscription subscriptions
        |> Program.run

type App() =
    inherit Application()

    override this.Initialize() =
        this.Styles.Add (FluentTheme())
        this.RequestedThemeVariant <- Styling.ThemeVariant.Dark

    override this.OnFrameworkInitializationCompleted() =
        match this.ApplicationLifetime with
        | :? IClassicDesktopStyleApplicationLifetime as desktopLifetime ->
            let mainWindow = MainWindow()
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