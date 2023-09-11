namespace Example.Tetris

open System
open Elmish
open Avalonia
open Avalonia.FuncUI.Hosts
open Avalonia.FuncUI.Elmish
open Avalonia.Controls.ApplicationLifetimes
open Avalonia.Themes.Fluent
open Avalonia.Threading
open Avalonia.Input

type MainWindow() as this =
    inherit HostWindow()

    do
        base.Title <- "Tetris"
        base.Width <- 450.0
        base.Height <- 600.0

        let subscriptions (_state: Game.State) =
            let timerSub (dispatch: Game.Msg -> unit) =
                let invoke () =
                    Game.Update |> dispatch
                    true

                DispatcherTimer.Run(Func<bool>(invoke), TimeSpan.FromMilliseconds 10.0)

            let keyDownSub (dispatch: Game.Msg -> unit) =
                this.KeyDown.Subscribe(fun eventArgs ->
                    match eventArgs.Key with
                    | Key.RightShift
                    | Key.LeftShift -> Game.Msg.RotL
                    | Key.Space -> Game.Msg.RotR
                    | Key.S -> Game.Msg.Down
                    | Key.A -> Game.Msg.Left
                    | Key.D -> Game.Msg.Right
                    | Key.E -> Game.Msg.Hold
                    | _ -> Game.Msg.Empty
                    |> dispatch)

            [ [ nameof timerSub ], timerSub; [ nameof keyDownSub ], keyDownSub ]

        Program.mkSimple Game.init Game.update View.view
        |> Program.withHost this
        |> Program.withSubscription subscriptions
        |> Program.run


type App() =
    inherit Application()

    override this.Initialize() =
        this.Styles.Add(FluentTheme())
        this.RequestedThemeVariant <- Styling.ThemeVariant.Dark

    override this.OnFrameworkInitializationCompleted() =
        match this.ApplicationLifetime with
        | :? IClassicDesktopStyleApplicationLifetime as desktopLifetime -> desktopLifetime.MainWindow <- MainWindow()
        | _ -> ()


module Program =
    [<EntryPoint>]
    let main argv =
        AppBuilder
            .Configure<App>()
            .UsePlatformDetect()
            .UseSkia()
            .StartWithClassicDesktopLifetime(argv)
