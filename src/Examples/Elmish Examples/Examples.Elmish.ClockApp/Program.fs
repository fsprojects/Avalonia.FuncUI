namespace Examples.ClockApp

open System
open Avalonia
open Avalonia.Themes.Fluent
open Avalonia.Threading
open Elmish
open Avalonia.FuncUI.Hosts
open Avalonia.FuncUI
open Avalonia.FuncUI.Elmish
open Avalonia.Controls.ApplicationLifetimes

type MainWindow() as this =
    inherit HostWindow()
    do
        base.Title <- "Clock Example"
        
        base.Height <- 200.0
        base.MaxHeight <- 200.0
        base.MinHeight <- 200.0
        
        base.Width <- 200.0
        base.MaxWidth <- 200.0
        base.MinWidth <- 200.0
      
        let subscriptions (_state: Clock.State) : Sub<Clock.Msg> =
            let timerSub (dispatch: Clock.Msg -> unit) =
                let invoke() =
                    DateTime.Now |> Clock.Msg.Tick |> dispatch
                    true
                    
                DispatcherTimer.Run(Func<bool>(invoke), TimeSpan.FromMilliseconds 1000.0)

            let onClosedSub (dispatch: Clock.Msg -> unit) =
                this.Closed.Subscribe(fun e ->
                    printfn "The window has been closed."
                )

            [ 
                [ nameof timerSub ], timerSub
                [ nameof onClosedSub ], onClosedSub
            ]
        
        //this.VisualRoot.VisualRoot.Renderer.DrawFps <- true
        //this.VisualRoot.VisualRoot.Renderer.DrawDirtyRects <- true
        Elmish.Program.mkSimple Clock.init Clock.update Clock.view
        |> Program.withHost this
        |> Program.withSubscription subscriptions
        |> Program.withConsoleTrace
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