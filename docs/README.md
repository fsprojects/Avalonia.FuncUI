# Getting Started

<div align="center">

<figure><img src=".gitbook/assets/Screenshot 2023-02-17 at 21.16.10.png" alt=""><figcaption><p>Screenshot of the example app. (Simple counter app with a text block and two buttons)</p></figcaption></figure>

</div>

## Step 1: Empty Console App

Create a new F# console application targeting .net 8 or higher.&#x20;

## Step 2: Packages

&#x20;Reference the following packages [Avalonia.Desktop](https://www.nuget.org/packages/Avalonia.Desktop/11.1.0), [Avalonia.Themes.Fluent](https://www.nuget.org/packages/Avalonia.Themes.Fluent/11.1.0) and [Avalonia.FuncUI](https://www.nuget.org/packages/Avalonia.FuncUI/1.5.1).

{% tabs %}
{% tab title="dotnet CLI" %}
Run the following commands in your project directory:

```bash
dotnet add package Avalonia.Desktop --version 11.1.0
dotnet add package Avalonia.Themes.Fluent --version 11.1.0
dotnet add package Avalonia.FuncUI --version 1.5.1
```
{% endtab %}

{% tab title="edit Project file" %}
Paste the following package references to your fsproject file:

```html
<PackageReference Include="Avalonia.Desktop" Version="11.1.0" />
<PackageReference Include="Avalonia.Themes.Fluent" Version="11.1.0" />
<PackageReference Include="Avalonia.FuncUI" Version="1.5.1" />
```
{% endtab %}
{% endtabs %}

## Step 3: Add code to `Program.fs`

```fsharp
namespace CounterApp

open Avalonia
open Avalonia.Controls.ApplicationLifetimes
open Avalonia.Themes.Fluent
open Avalonia.FuncUI.Hosts
open Avalonia.Controls
open Avalonia.FuncUI
open Avalonia.FuncUI.DSL
open Avalonia.Layout

module Main =

    let view () =
        Component(fun ctx ->
            let state = ctx.useState 0

            DockPanel.create [
                DockPanel.children [
                    Button.create [
                        Button.dock Dock.Bottom
                        Button.onClick (fun _ -> state.Set(state.Current - 1))
                        Button.content "-"
                        Button.horizontalAlignment HorizontalAlignment.Stretch
                        Button.horizontalContentAlignment HorizontalAlignment.Center
                    ]
                    Button.create [
                        Button.dock Dock.Bottom
                        Button.onClick (fun _ -> state.Set(state.Current + 1))
                        Button.content "+"
                        Button.horizontalAlignment HorizontalAlignment.Stretch
                        Button.horizontalContentAlignment HorizontalAlignment.Center
                    ]
                    TextBlock.create [
                        TextBlock.dock Dock.Top
                        TextBlock.fontSize 48.0
                        TextBlock.verticalAlignment VerticalAlignment.Center
                        TextBlock.horizontalAlignment HorizontalAlignment.Center
                        TextBlock.text (string state.Current)
                    ]
                ]
            ]
        )

type MainWindow() =
    inherit HostWindow()
    do
        base.Title <- "Counter Example"
        base.Content <- Main.view ()

type App() =
    inherit Application()

    override this.Initialize() =
        this.Styles.Add (FluentTheme())
        this.RequestedThemeVariant <- Styling.ThemeVariant.Dark

    override this.OnFrameworkInitializationCompleted() =
        match this.ApplicationLifetime with
        | :? IClassicDesktopStyleApplicationLifetime as desktopLifetime ->
            desktopLifetime.MainWindow <- MainWindow()
        | _ -> ()

module Program =

    [<EntryPoint>]
    let main(args: string[]) =
        AppBuilder
            .Configure<App>()
            .UsePlatformDetect()
            .UseSkia()
            .StartWithClassicDesktopLifetime(args)

```

## Step 4: build and run 🎉

```bash
dotnet run
```
