open UIKit
// This is the main entry point of the application.
let [<EntryPoint>] Main(args: string array) =
    AvaApp.About.urlOpen <- fun url -> new Foundation.NSUrl(url) |> UIApplication.SharedApplication.OpenUrl |> ignore
    // if you want to use a different Application Delegate class from "AppDelegate"
    // you can specify it here.
    UIApplication.Main(args, null, typeof<AvaApp.iOS.AppDelegate>)
    0