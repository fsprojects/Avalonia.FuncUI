namespace Avalonia.FuncUI.ControlCatalog

open Avalonia.FuncUI.Components.Hosts
open Avalonia
open Avalonia.FuncUI.Elmish
open Elmish
open Avalonia.Controls.ApplicationLifetimes
open Avalonia.FuncUI.ControlCatalog.Views

type MainWindow() as this =
    inherit HostWindow()

    do
        base.Title <- "Control Catalog"
        base.Height <- 600.0
        base.Width <- 800.0
      
        this.VisualRoot.VisualRoot.Renderer.DrawFps <- true
        //this.VisualRoot.VisualRoot.Renderer.DrawDirtyRects <- true
        ()
        
type App() =
    inherit Application()

    override this.Initialize() =
        this.Styles.Load "avares://Avalonia.Themes.Fluent/Accents/FluentDark.xaml"
        this.Styles.Load "avares://Avalonia.Themes.Default/Accents/BaseDark.xaml"
        this.Styles.Load "avares://Avalonia.FuncUI.ControlCatalog/Styles/TabControl.xaml"

    override this.OnFrameworkInitializationCompleted() =
        match this.ApplicationLifetime with
        | :? IClassicDesktopStyleApplicationLifetime as desktopLifetime ->
            let mainWindow = MainWindow()
            desktopLifetime.MainWindow <- mainWindow
            
            Elmish.Program.mkSimple (fun () -> MainView.init) MainView.update MainView.view
            |> Program.withHost mainWindow
            |> Program.withConsoleTrace
            |> Program.run
        | _ -> ()

module Program =

    [<EntryPoint>]
    let main(args: string[]) =
        AppBuilder
            .Configure<App>()
            .UsePlatformDetect()
            .UseSkia()
            .StartWithClassicDesktopLifetime(args)

