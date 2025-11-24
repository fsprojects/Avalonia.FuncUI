namespace Examples.ThemeSwitcher

open System
open Avalonia
open Avalonia.Controls
open Avalonia.Controls.ApplicationLifetimes
open Avalonia.Input
open Avalonia.Themes.Fluent
open Avalonia.FuncUI
open Avalonia.FuncUI.Diagnostics

#if DEBUG
type FuncCommand(execute, canExecute) =
    let canExecuteChanged = new Event<EventHandler, EventArgs>()

    interface Windows.Input.ICommand with
        [<CLIEvent>]
        member _.CanExecuteChanged: IEvent<EventHandler, EventArgs> = canExecuteChanged.Publish

        member _.CanExecute(parameter: obj) : bool = Option.ofObj parameter |> canExecute

        member _.Execute(parameter: obj) : unit = Option.ofObj parameter |> execute
#endif

type MainWindow() as this =
    inherit Window()

    do
        this.Title <- "Theme Switcher Example"

        this.Icon <-
            Uri "avares://Examples.ThemeSwitcher/Assets/Icons/icon.ico"
            |> Platform.AssetLoader.Open
            |> WindowIcon

        this.Width <- 800.0
        this.MinWidth <- 600.0
        this.Height <- 680.0
        this.MinHeight <- 400.0
#if DEBUG
        this.KeyBindings.AddRange
            [ KeyBinding(
                  // F12 key opens Inspector Window for component development
                  Gesture = KeyGesture(Key.F12, KeyModifiers.None),
                  Command =
                      FuncCommand(
                          (fun _ -> InspectorWindow(this).Show this),
                          // Prevent multiple inspector windows
                          (fun _ ->
                              match Application.Current.ApplicationLifetime with
                              | :? IClassicDesktopStyleApplicationLifetime as desktopLifetime ->
                                  desktopLifetime.Windows |> Seq.exists (fun w -> w :? InspectorWindow) |> not
                              | _ -> false)
                      )
              ) ]
#endif
        this.Content <- Main.view ()

type App() =
    inherit Application()

    override this.Initialize() =
        this.Styles.Add(FluentTheme())
        this.Styles.Load "avares://Avalonia.Controls.ColorPicker/Themes/Fluent/Fluent.xaml"
        this.RequestedThemeVariant <- Styling.ThemeVariant.Dark

#if DEBUG
        this.AttachDeveloperTools() |> ignore
#endif

    override this.OnFrameworkInitializationCompleted() =
        match this.ApplicationLifetime with
        | :? IClassicDesktopStyleApplicationLifetime as desktopLifetime ->
            let mainWindow = MainWindow()
            desktopLifetime.MainWindow <- mainWindow
        | _ -> ()

module Program =

    [<EntryPoint>]
    let main (args: string[]) =
        AppBuilder.Configure<App>().UsePlatformDetect().UseSkia().StartWithClassicDesktopLifetime(args)
