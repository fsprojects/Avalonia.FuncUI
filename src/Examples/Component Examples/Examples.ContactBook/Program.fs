namespace Examples.ContactBook

open Avalonia
open Avalonia.FuncUI.Diagnostics
open Avalonia.Themes.Fluent
open Avalonia.FuncUI.Hosts
open Avalonia.Controls.ApplicationLifetimes
open Avalonia.Controls

type MainWindow() =
    inherit HostWindow()
    do
        base.Title <- "Contact List Example"
        base.Icon <- WindowIcon(System.IO.Path.Combine("Assets","Icons", "icon.ico"))
        base.Content <- Views.mainView ()

type App() =
    inherit Application()

    override this.Initialize() =
        this.Styles.Add (FluentTheme())
        this.RequestedThemeVariant <- Styling.ThemeVariant.Dark

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