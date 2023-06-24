open System
open System.Diagnostics
open Avalonia
open AvaApp

// Initialization code. Don't use any Avalonia, third-party APIs or any
// SynchronizationContext-reliant code before AppMain is called: things aren't initialized
// yet and stuff might break.
[<STAThread; EntryPoint>]
let Main(args: string array) =
    About.urlOpen <- fun url ->
        if OperatingSystem.IsWindows() then
            Process.Start(ProcessStartInfo("cmd", $"/c start %s{url}")) |> ignore
        elif OperatingSystem.IsLinux() || OperatingSystem.IsFreeBSD() then
            Process.Start("xdg-open", url) |> ignore
        elif OperatingSystem.IsMacOS() then
            Process.Start("open", url) |> ignore
    AppBuilder.Configure<App>()
        .UsePlatformDetect()
        .LogToTrace(?level = None)
        .StartWithClassicDesktopLifetime(args)