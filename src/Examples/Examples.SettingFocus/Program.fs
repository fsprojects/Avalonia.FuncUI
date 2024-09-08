namespace Examples.SettingFocus

open System.Collections
open Avalonia
open Avalonia.Controls
open Avalonia.Controls.ApplicationLifetimes
open Avalonia.FuncUI
open Avalonia.FuncUI.DSL
open Avalonia.Layout
open Avalonia.Media
open Avalonia.Themes.Fluent
open Avalonia.FuncUI.Hosts

module Views =

    let view () =
        Component.create ("view", fun ctx ->
            let textBoxA = ctx.useState<TextBox>(null, renderOnChange = false)
            let textBoxB = ctx.useState<TextBox>(null, renderOnChange = false)

            let textBoxAFocus = ctx.useState(false, renderOnChange = true)
            let textBoxBFocus = ctx.useState(false, renderOnChange = true)

            StackPanel.create [
                StackPanel.margin 10
                StackPanel.spacing 10
                StackPanel.children [

                    TextBox.create [
                        TextBox.init textBoxA.Set
                        TextBox.onGotFocus (fun _ -> textBoxAFocus.Set true)
                        TextBox.onLostFocus (fun _ -> textBoxAFocus.Set false)
                    ]

                    TextBox.create [
                        TextBox.init textBoxB.Set
                        TextBox.onGotFocus (fun _ -> textBoxAFocus.Set true)
                        TextBox.onLostFocus (fun _ -> textBoxAFocus.Set false)
                    ]

                    StackPanel.create [
                        StackPanel.orientation Orientation.Horizontal
                        StackPanel.margin 10
                        StackPanel.spacing 10
                        StackPanel.children [

                            Button.create [
                                Button.content "Focus A"
                                Button.background (if textBoxAFocus.Current then Brushes.Green else Brushes.Red)
                                Button.onClick (fun _ ->
                                    let _ = textBoxA.Current.Focus()
                                    ()
                                )
                            ]

                            Button.create [
                                Button.content "Focus B"
                                Button.background (if textBoxBFocus.Current then Brushes.Green else Brushes.Red)
                                Button.onClick (fun _ ->
                                    let _ = textBoxB.Current.Focus()
                                    ()
                                )
                            ]
                        ]
                    ]
                ]
            ]
        )

    let mainView () =
        Component(fun ctx ->
            view ()
        )


type MainWindow() as this =
    inherit HostWindow()
    do
        base.Title <- "Examples.SettingFocus"
        base.Width <- 1200.0
        base.Height <- 400.0
        this.Content <- Views.mainView ()

        //this.VisualRoot.VisualRoot.Renderer.DrawFps <- true
        //this.VisualRoot.VisualRoot.Renderer.DrawDirtyRects <- true


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