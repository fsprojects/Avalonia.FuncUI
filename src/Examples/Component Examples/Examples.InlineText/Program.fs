namespace Examples.InlineText

open Avalonia
open Avalonia.Controls.ApplicationLifetimes
open Avalonia.Themes.Fluent
open Avalonia.FuncUI.Hosts

type MainWindow() as this =
    inherit HostWindow()
    do
        base.Title <- "Examples.InlineText"
        base.Width <- 1200.0
        base.Height <- 400.0
        this.Content <- View.view

        //this.VisualRoot.VisualRoot.Renderer.DrawFps <- true
        //this.VisualRoot.VisualRoot.Renderer.DrawDirtyRects <- true

        
type App() =
    inherit Application()

    override this.Initialize() =
        this.Styles.Add (FluentTheme(baseUri = null, Mode = FluentThemeMode.Dark))

    override this.OnFrameworkInitializationCompleted() =
        match this.ApplicationLifetime with
        | :? IClassicDesktopStyleApplicationLifetime as desktopLifetime ->
            desktopLifetime.MainWindow <- MainWindow()
        | _ -> ()

module Program =

    [<EntryPoint>]
    let main(args: string[]) =
        AppBuilder
            .Configure<App>()
            .UsePlatformDetect()
            .UseSkia()
            .StartWithClassicDesktopLifetime(args)