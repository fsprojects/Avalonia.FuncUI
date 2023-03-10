namespace Examples.Mobile.iOS

open Avalonia
open Avalonia.Controls.ApplicationLifetimes
open Avalonia.Themes.Simple
open Avalonia.FuncUI.Hosts
open Avalonia.Controls
open Avalonia.FuncUI
open Avalonia.FuncUI.DSL
open Avalonia.Layout
open Avalonia.iOS

[<RequireQualifiedAccess>]
module Views =

    let view () =
        Component (fun ctx ->
            let state = ctx.useState 0

            DockPanel.create [
                DockPanel.margin 20.0
                DockPanel.children [
                    Button.create [
                        Button.dock Dock.Bottom
                        Button.onClick (fun _ -> state.Current - 1 |> state.Set)
                        Button.content "-"
                        Button.horizontalAlignment HorizontalAlignment.Stretch
                    ]
                    Button.create [
                        Button.dock Dock.Bottom
                        Button.onClick (fun _ -> state.Current + 1 |> state.Set)
                        Button.content "+"
                        Button.horizontalAlignment HorizontalAlignment.Stretch
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

type App() =
    inherit Application()

    override this.Initialize() =
        this.Styles.Add (SimpleTheme())
        this.RequestedThemeVariant <- Styling.ThemeVariant.Dark

    override this.OnFrameworkInitializationCompleted() =
        match this.ApplicationLifetime with
        | :? IClassicDesktopStyleApplicationLifetime as desktopLifetime ->
            desktopLifetime.MainWindow <- HostWindow(Content = Views.view())

        | :? ISingleViewApplicationLifetime as singleViewLifetime ->
            singleViewLifetime.MainView <- Views.view()

        | _ -> ()

type AppDelegate () =
    inherit AvaloniaAppDelegate<App>()

[<RequireQualifiedAccess>]
module Program =

    [<EntryPoint>]
    let main (args: string array) : int =
        UIKit.UIApplication.Main(args, null, typeof<AppDelegate>)
        0