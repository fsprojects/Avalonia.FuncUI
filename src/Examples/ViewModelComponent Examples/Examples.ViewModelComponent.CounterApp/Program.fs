namespace Examples.CounterApp

open Avalonia
open Avalonia.Controls
open Avalonia.Controls.ApplicationLifetimes
open Avalonia.Interactivity
open Avalonia.Layout
open Avalonia.Themes.Fluent
open Avalonia.FuncUI.Hosts
open Avalonia.FuncUI
open Avalonia.FuncUI.DSL

type CounterComponent_CodeBehindFlavoured () =
    inherit ViewModelComponentBase ()

    //[<Observe>]
    member val Counter: int = 0 with get, set

    member this.Decrement (args: RoutedEventArgs) =
        this.Counter <- this.Counter - 1

    member this.Increment (args: RoutedEventArgs) =
        this.Counter <- this.Counter + 1

    override this.Build () =
        DockPanel.create [
            DockPanel.children [
                Button.create [
                    Button.dock Dock.Bottom
                    Button.onClick this.Decrement
                    Button.content "-"
                    Button.horizontalAlignment HorizontalAlignment.Stretch
                ]
                Button.create [
                    Button.dock Dock.Bottom
                    Button.onClick this.Increment
                    Button.content "+"
                    Button.horizontalAlignment HorizontalAlignment.Stretch
                ]
                TextBox.create [
                    TextBox.dock Dock.Bottom
                    //TextBox.onTextChanged (
                    //    (fun text ->
                    //        let isNumber, number = System.Int32.TryParse text
                    //        if isNumber then number |> state.Set)
                    //)
                    //TextBox.text (string state.Current)
                    TextBox.horizontalAlignment HorizontalAlignment.Stretch
                ]
                TextBlock.create [
                    TextBlock.dock Dock.Top
                    TextBlock.fontSize 48.0
                    TextBlock.verticalAlignment VerticalAlignment.Center
                    TextBlock.horizontalAlignment HorizontalAlignment.Center
                    TextBlock.text "0"
                ]
            ]
        ]


type MainWindow() =
    inherit HostWindow()
    do
        base.Title <- "Counter Example"
        base.Height <- 400.0
        base.Width <- 400.0
        base.Content <- CounterComponent_CodeBehindFlavoured()

type App() =
    inherit Application()

    override this.Initialize() =
        this.Styles.Add (FluentTheme(baseUri = null, Mode = FluentThemeMode.Dark))

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
