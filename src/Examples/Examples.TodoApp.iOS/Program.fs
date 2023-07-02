namespace Examples.TodoApp


open Avalonia.iOS

type AppDelegate () =
    inherit AvaloniaAppDelegate<App>()

[<RequireQualifiedAccess>]
module Program =

    [<EntryPoint>]
    let main (args: string array) : int =
        UIKit.UIApplication.Main(args, null, typeof<AppDelegate>)
        0