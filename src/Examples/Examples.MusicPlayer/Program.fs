namespace Examples.MusicPlayer
open Avalonia
open Avalonia.Controls.ApplicationLifetimes
open Avalonia.FuncUI
open LibVLCSharp.Shared


type App() =
    inherit Application()

    override this.Initialize() =
        this.Styles.Load "avares://Citrus.Avalonia/Rust.xaml"
        this.Styles.Load "avares://Examples.MusicPlayer/Styles.xaml"
        Core.Initialize()

    override this.OnFrameworkInitializationCompleted() =
        match this.ApplicationLifetime with
        | :? IClassicDesktopStyleApplicationLifetime as desktopLifetime -> desktopLifetime.MainWindow <- Shell.ShellWindow()
        | _ -> ()

module Program =

    [<EntryPoint>]
    let main (args: string []) =
        AppBuilder.Configure<App>().UsePlatformDetect().UseSkia().StartWithClassicDesktopLifetime(args)
