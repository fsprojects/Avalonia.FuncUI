namespace BasicMvuTemplate

open Elmish
open Avalonia
open Avalonia.Controls.ApplicationLifetimes
open Avalonia.Diagnostics
open Avalonia.Input
open Avalonia.FuncUI
open Avalonia.FuncUI.Elmish
open Avalonia.Themes.Fluent
open Avalonia.FuncUI.Hosts

type MainWindow() as this =
    inherit HostWindow()
    do
        base.Title <- "BasicMvuTemplate"
        base.Width <- 400.0
        base.Height <- 400.0
//-:cnd
#if DEBUG
        this.AttachDevTools(KeyGesture(Key.F12))
#endif
//+:cnd
        Elmish.Program.mkSimple Counter.init Counter.update Counter.view
        |> Program.withHost this
//-:cnd
#if DEBUG
        |> Program.withConsoleTrace
#endif
//+:cnd
        |> Program.run

        
type App() =
    inherit Application()

    override this.Initialize() =
        this.Styles.Add (FluentTheme())

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
            .StartWithClassicDesktopLifetime(args)