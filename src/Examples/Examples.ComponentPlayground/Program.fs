namespace Examples.CounterApp
open System
open Avalonia
open Avalonia.Controls.ApplicationLifetimes
open Avalonia.Media
open Avalonia.Themes.Fluent
open Avalonia.FuncUI.Hosts
open Avalonia.Controls

module Helpers =

    let randomColor () : string =
        String.Format("#{0:X6}", Random.Shared.Next(0x1000000));

module Main =
    open Avalonia.Controls
    open Avalonia.FuncUI
    open Avalonia.FuncUI.DSL
    open Avalonia.Layout

    let counterViewNegativeIndicator () =
        Component.create (fun _ ->
            TextBlock.create [
                TextBlock.fontSize 24.0
                TextBlock.verticalAlignment VerticalAlignment.Center
                TextBlock.horizontalAlignment HorizontalAlignment.Center
                TextBlock.background (SolidColorBrush.Parse (Helpers.randomColor()))
                TextBlock.text "Negative"
            ]
        )

    let counterView (count: int) =
        Component.create (fun _ ->
            StackPanel.create [
                StackPanel.dock Dock.Top
                StackPanel.spacing 5
                StackPanel.orientation Orientation.Horizontal
                StackPanel.horizontalAlignment HorizontalAlignment.Center
                StackPanel.children [

                    TextBlock.create [
                        TextBlock.fontSize 48.0
                        TextBlock.verticalAlignment VerticalAlignment.Center
                        TextBlock.horizontalAlignment HorizontalAlignment.Center
                        TextBlock.text (string count)
                    ]

                    if count < 0 then
                        counterViewNegativeIndicator ()
                ]
            ]
        )

    let view () =
        Component (fun ctx ->
            let state = ctx.useState 0

            DockPanel.create [
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
                    counterView state.Current
                ]
            ]
        )


type MainWindow() =
    inherit HostWindow()
    do
        base.Title <- "Counter Example"
        base.Height <- 400.0
        base.Width <- 400.0
        base.Content <- Main.view ()

type App() =
    inherit Application()

    override this.Initialize() =
        this.Styles.Add (FluentTheme())
        this.RequestedThemeVariant <- Styling.ThemeVariant.Dark

    override this.OnFrameworkInitializationCompleted() =
        match this.ApplicationLifetime with
        | :? IClassicDesktopStyleApplicationLifetime as desktopLifetime ->
            let mainWindow = MainWindow()
            desktopLifetime.MainWindow <- mainWindow
        | _ -> ()

module Program =

    [<EntryPoint>]
    let main(args: string[]) =
        AppBuilder
            .Configure<App>()
            .UsePlatformDetect()
            .UseSkia()
            .StartWithClassicDesktopLifetime(args)
