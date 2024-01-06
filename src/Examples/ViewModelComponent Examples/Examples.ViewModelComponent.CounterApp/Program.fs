namespace Examples.CounterApp

open System.ComponentModel
open Avalonia
open Avalonia.Controls
open Avalonia.Controls.ApplicationLifetimes
open Avalonia.FuncUI.Experimental
open Avalonia.FuncUI.Types
open Avalonia.Interactivity
open Avalonia.Layout
open Avalonia.Media
open Avalonia.Themes.Fluent
open Avalonia.FuncUI.Hosts
open Avalonia.FuncUI
open Avalonia.FuncUI.DSL

type CounterComponent () =
    inherit StaticComponent ()

    let counter: IWritable<int> = new State<int>(0)

    member this.Decrement (_args: RoutedEventArgs) =
        counter.Set (counter.Current - 1)

    member this.Increment (_args: RoutedEventArgs) =
        counter.Set (counter.Current + 1)

    override this.Build () : IView =
        DockPanel.create [
            DockPanel.children [

                Button.create [
                    Button.dock Dock.Bottom
                    Button.onClick this.Decrement
                    Button.content "-"
                    Button.horizontalAlignment HorizontalAlignment.Stretch
                    Button.init (fun button ->
                        button.Bind(Button.IsEnabledProperty, counter.Map (fun c -> c > -10))
                    )
                ]

                Button.create [
                    Button.dock Dock.Bottom
                    Button.onClick this.Increment
                    Button.content "+"
                    Button.horizontalAlignment HorizontalAlignment.Stretch
                    Button.init (fun button ->
                        button.Bind(Button.IsEnabledProperty, counter.Map (fun c -> c < 10))
                    )
                ]

                TextBlock.create [
                    TextBlock.dock Dock.Top
                    TextBlock.fontSize 48.0
                    TextBlock.verticalAlignment VerticalAlignment.Center
                    TextBlock.horizontalAlignment HorizontalAlignment.Center
                    TextBlock.init (fun textBlock ->
                        textBlock.Bind(TextBlock.TextProperty, counter.Map string)
                        textBlock.Bind(TextBlock.ForegroundProperty, counter.Map (fun c ->
                            if c < 0
                            then Brushes.Red :> IBrush
                            else Brushes.Green :> IBrush
                        ))
                    )
                ]
            ]
        ]


type MainWindow() =
    inherit HostWindow()
    do
        base.Title <- "Counter Example"
        base.Height <- 400.0
        base.Width <- 400.0
        base.Content <- CounterComponent()

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
