namespace CounterElmishSample

open Avalonia.FuncUI.Hosts
open Avalonia
open Avalonia.FuncUI.Elmish
open Elmish

//The main window of your application. You can style the window by changing properties of its base (As shown below with the window title).
type MainWindow() =
    inherit HostWindow()

    do
        base.Title <- "Avalonia.FuncUI Counter Elmish template"
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

        //Create an elmish MVU program using an initial model, update function and view
        Elmish.Program.mkSimple (fun () -> Counter.initialState) Counter.update Counter.view
        |> Program.withHost mainWindow
        |> Program.withConsoleTrace
        |> Program.run

        app.Run(mainWindow)
        |> ignore

    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [<EntryPoint>]
    [<CompiledName "Main">]
    let main(args: string[]) =
        buildAvaloniaApp().Start(appMain, args)
        0
