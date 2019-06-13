namespace Inspector

open Avalonia.FuncUI.Hosts
open Avalonia
open Avalonia.FuncUI.Elmish
open Elmish

type MainWindow() =
    inherit HostWindow()

    do
        base.Title <- "Counter Elmish"
        base.Height <- 400.0
        base.Width <- 400.0    
      
        //this.VisualRoot.VisualRoot.Renderer.DrawFps <- true
        //this.VisualRoot.VisualRoot.Renderer.DrawDirtyRects <- true
        ()

type App() =
    inherit Application()

    override this.Initialize() =
        this.Styles.Load "resm:Avalonia.Themes.Default.DefaultTheme.xaml?assembly=Avalonia.Themes.Default"
        this.Styles.Load "resm:Avalonia.Themes.Default.Accents.BaseDark.xaml?assembly=Avalonia.Themes.Default"

module Program =
    open Avalonia
    open Avalonia.Logging.Serilog

    // Avalonia configuration, don't remove; also used by visual designer.
    [<CompiledName "BuildAvaloniaApp">]
    let buildAvaloniaApp() =
        AppBuilder
            .Configure<App>()
            .UsePlatformDetect()
            .LogToDebug()

    // Your application's entry point.
    [<CompiledName "AppMain">]
    let appMain (app: Application) (args: string[]) =
        let mainWindow = MainWindow()

        Elmish.Program.mkSimple (fun () -> InspectorView.init) InspectorView.update InspectorView.view
        |> Program.withHost mainWindow
        |> Program.withConsoleTrace
        |> Program.run

        app.Run(mainWindow)

    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [<EntryPoint>]
    [<CompiledName "Main">]
    let main(args: string[]) =
        buildAvaloniaApp().Start(appMain, args)
        0
