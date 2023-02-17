namespace Avalonia.FuncUI.ControlCatalog

open Avalonia

module Program =

    [<EntryPoint>]
    let main(args: string[]) =
        AppBuilder
            .Configure<Avalonia.FuncUI.ControlCatalog.App>()
            .UsePlatformDetect()
            .UseSkia()
            .StartWithClassicDesktopLifetime(args)

