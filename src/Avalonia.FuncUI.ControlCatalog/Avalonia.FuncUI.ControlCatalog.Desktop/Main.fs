namespace Avalonia.FuncUI.ControlCatalog

open Avalonia
open System

module Program =

    [<STAThread>]
    [<EntryPoint>]
    let main(args: string[]) =
        AppBuilder
            .Configure<Avalonia.FuncUI.ControlCatalog.App>()
            .UsePlatformDetect()
            .UseSkia()
            .StartWithClassicDesktopLifetime(args)

