namespace Examples.WebAssemblyPlayground

open System
open System.Net.Http
open Avalonia.Web.Blazor
open Bolero
open Bolero.Html
open Microsoft.AspNetCore.Components
open Microsoft.AspNetCore.Components.WebAssembly.Hosting
open Bolero.Remoting.Client
open Avalonia.ReactiveUI
open Microsoft.Extensions.DependencyInjection

type MainView () =
    inherit Component()

    override this.SetParametersAsync (parameters) =
        base.SetParametersAsync parameters

    override this.OnParametersSet () =
        base.OnParametersSet()

        WebAppBuilder.Configure<App>()
            .UseReactiveUI()
            .SetupWithSingleViewLifetime()
        ()

    override this.Render () =
        comp<AvaloniaView> [] []

module Program =

    [<EntryPoint>]
    let Main args =

        let builder = WebAssemblyHostBuilder.CreateDefault(args)
        builder.RootComponents.Add<MainView>("#app")
        builder.Services.AddScoped<HttpClient>(fun sp -> new HttpClient(BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)))
        builder.Build().RunAsync() |> ignore
        0
