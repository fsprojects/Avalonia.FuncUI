namespace Examples.GameOfLife

open System
open Avalonia
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
        base.Height <- 500.0
        base.Width <- 500.0
        
        let timer (_state: Main.State) =
            let sub (dispatch: Main.Msg -> unit) =
                let invoke() =
                    Board.Evolve |> Main.BoardMsg |> dispatch
                    true
                    
                DispatcherTimer.Run(Func<bool>(invoke), TimeSpan.FromMilliseconds 100.0) |> ignore
                
            Cmd.ofSub sub
                
        //this.VisualRoot.VisualRoot.Renderer.DrawFps <- true
        //this.VisualRoot.VisualRoot.Renderer.DrawDirtyRects <- true
        
        Elmish.Program.mkProgram Main.initialState Main.update Main.view
        |> Program.withHost this
        |> Program.withSubscription timer
        |> Program.run
        
type App() =
    inherit Application()

    override this.Initialize() =
        this.Styles.Add (FluentTheme(baseUri = null, Mode = FluentThemeMode.Dark))

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