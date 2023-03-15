
open System.Runtime.Versioning
open Avalonia
open Avalonia.Browser

module public Program =
    [<assembly: SupportedOSPlatform("browser")>]
    do ()

    [<CompiledName "BuildAvaloniaApp">] 
    let public buildAvaloniaApp () = 
        AppBuilder
            .Configure<Avalonia.FuncUI.ControlCatalog.App>()

    [<EntryPoint>]
    let main argv =
        buildAvaloniaApp().
            StartBrowserAppAsync("out")
            |> ignore

        0