namespace Examples.WebAssemblyPlayground

open System
open Avalonia
open Avalonia.Browser.Blazor
open Bolero
open Bolero.Html
open Microsoft.AspNetCore.Components.WebAssembly.Hosting
open Avalonia.ReactiveUI

type MainView () =
    inherit Component()

    override this.SetParametersAsync (parameters) =
        base.SetParametersAsync parameters

    override this.Render () =
        comp<AvaloniaView> { attr.empty() }

module Program =

    [<EntryPoint>]
    let Main args =

        let builder = WebAssemblyHostBuilder.CreateDefault(args)
        builder.RootComponents.Add<MainView>("#app")
        let host = builder.Build()

        task {
            do! AppBuilder.Configure<App>()
                      .UseReactiveUI()
                      .StartBlazorAppAsync()

            do! host.RunAsync()
        } |> ignore

        0
