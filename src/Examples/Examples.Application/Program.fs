namespace Examples.Application

open Avalonia

module Program =
    [<EntryPoint>]
    let main args =
        AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .StartWithClassicDesktopLifetime(args)
