namespace CounterElmishSample

open Avalonia.FuncUI.Components.Hosts
open Avalonia
open Avalonia.FuncUI.Elmish
open Elmish
open Avalonia.Controls.ApplicationLifetimes

type MainWindow() as this =
    inherit HostWindow()

    do
        base.Title <- "Counter Elmish"
        base.Height <- 400.0
        base.Width <- 400.0    
      
        this.VisualRoot.VisualRoot.Renderer.DrawFps <- true
        //this.VisualRoot.VisualRoot.Renderer.DrawDirtyRects <- true
        ()
        
type App() =
    inherit Application()

    override this.Initialize() =
        this.Styles.Load "resm:Avalonia.Themes.Default.DefaultTheme.xaml?assembly=Avalonia.Themes.Default"
        this.Styles.Load "resm:Avalonia.Themes.Default.Accents.BaseDark.xaml?assembly=Avalonia.Themes.Default"

    override this.OnFrameworkInitializationCompleted() =
        match this.ApplicationLifetime with
        | :? IClassicDesktopStyleApplicationLifetime as desktopLifetime ->
            let mainWindow = MainWindow()
            desktopLifetime.MainWindow <- mainWindow
            
            Elmish.Program.mkSimple (fun () -> Counter.init) Counter.update Counter.view
            |> Program.withHost mainWindow
            |> Program.withConsoleTrace
            |> Program.run
        | _ -> ()

module Program =
    open Avalonia
    open Avalonia.Logging.Serilog

    [<EntryPoint>]
    [<CompiledName "Main">]
    let main(args: string[]) =
        AppBuilder
            .Configure<App>()
            .UsePlatformDetect()
            .UseSkia()
            .LogToDebug()
            .StartWithClassicDesktopLifetime(args)
