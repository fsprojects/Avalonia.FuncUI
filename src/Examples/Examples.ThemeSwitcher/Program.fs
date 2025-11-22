namespace Examples.ThemeSwitcher

open Avalonia
open Avalonia.Controls
open Avalonia.Controls.ApplicationLifetimes
open Avalonia.Themes.Fluent
open Avalonia.FuncUI
open Avalonia.FuncUI.Hosts

type MainWindow() =
    inherit HostWindow()

    do
        base.Title <- "Theme Switcher Example"

        base.Icon <-
            System.Uri "avares://Examples.ThemeSwitcher/Assets/Icons/icon.ico"
            |> Platform.AssetLoader.Open
            |> WindowIcon

        base.Width <- 800.0
        base.MinWidth <- 600.0
        base.Height <- 680.0
        base.MinHeight <- 400.0
        base.Content <- Main.view ()

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
