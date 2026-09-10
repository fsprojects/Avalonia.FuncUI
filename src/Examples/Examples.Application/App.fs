namespace Examples.Application

open System
open System.IO
open Avalonia
open Avalonia.Controls
open Avalonia.Controls.ApplicationLifetimes
open Avalonia.Styling
open Avalonia.Themes.Fluent
open Avalonia.FuncUI
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Hosts

module Views =
    let window (count: IWritable<int>) (showTray: IWritable<bool>) =
        Component(fun ctx ->
            let count = ctx.usePassed count
            let showTray = ctx.usePassed showTray
            StackPanel.create [
                StackPanel.margin 24.0
                StackPanel.spacing 12.0
                StackPanel.children [
                    TextBlock.create [ TextBlock.text $"Count: {count.Current}" ]
                    Button.create [
                        Button.content "Increment"
                        Button.onClick (fun _ -> count.Set(count.Current + 1))
                    ]
                    Button.create [
                        Button.content (if showTray.Current then "Remove tray icon" else "Restore tray icon")
                        Button.onClick (fun _ -> showTray.Set(not showTray.Current))
                    ]
                    TextBlock.create [
                        TextBlock.text "The application menu and tray menu share this counter."
                        TextBlock.textWrapping Avalonia.Media.TextWrapping.Wrap
                    ]
                ]
            ])

    let application icon (count: IWritable<int>) (showTray: IWritable<bool>) showWindow quit =
        ApplicationComponent(fun ctx ->
            let count = ctx.usePassed count
            let showTray = ctx.usePassed showTray
            let dark = ctx.useState false

            // Each use of this description creates a separate native menu instance.
            // Avalonia native menus cannot have multiple parents.
            let menu () =
                NativeMenu.create [
                    NativeMenu.items [
                        NativeMenuItem.create [
                            NativeMenuItem.header $"Increment ({count.Current})"
                            NativeMenuItem.onClick (fun _ -> count.Set(count.Current + 1))
                        ]
                        NativeMenuItem.create [
                            NativeMenuItem.header "Dark theme"
                            NativeMenuItem.toggleType MenuItemToggleType.CheckBox
                            NativeMenuItem.isChecked dark.Current
                            NativeMenuItem.onClick (fun _ -> dark.Set(not dark.Current))
                        ]
                        NativeMenuItem.create [
                            NativeMenuItem.header "Show window"
                            NativeMenuItem.onClick (fun _ -> showWindow())
                        ]
                        NativeMenuItemSeparator.create []
                        NativeMenuItem.create [
                            NativeMenuItem.header "Quit"
                            NativeMenuItem.onClick (fun _ -> quit())
                        ]
                    ]
                ]

            [
                Application.name "FuncUI application example"
                Application.requestedThemeVariant (if dark.Current then ThemeVariant.Dark else ThemeVariant.Light)
                Application.nativeMenu (
                    NativeMenu.create [
                        NativeMenu.items [
                            NativeMenuItem.create [
                                NativeMenuItem.header "Counter"
                                NativeMenuItem.menu (menu())
                            ]
                        ]
                    ])
                Application.trayIcons [
                    if showTray.Current then
                        TrayIcon.create [
                            TrayIcon.icon icon
                            TrayIcon.toolTipText $"FuncUI counter: {count.Current}"
                            TrayIcon.menu (menu())
                            TrayIcon.onClicked (fun _ -> showWindow())
                        ]
                ]
            ])

type App() =
    inherit HostApplication()

    override this.Initialize() =
        this.Styles.Add(FluentTheme())

    override this.OnFrameworkInitializationCompleted() =
        match this.ApplicationLifetime with
        | :? IClassicDesktopStyleApplicationLifetime as desktop ->
            let count = new State<int>(0)
            let showTray = new State<bool>(true)
            let icon = WindowIcon(Path.Combine(AppContext.BaseDirectory, "Assets", "icon.ico"))
            let window = new HostWindow()
            window.Title <- "Stateful FuncUI application"
            window.Width <- 440.0
            window.Height <- 260.0
            window.Icon <- icon
            window.Content <- Views.window count showTray
            desktop.MainWindow <- window
            desktop.ShutdownMode <- ShutdownMode.OnMainWindowClose
            let showWindow () =
                window.Show()
                window.WindowState <- WindowState.Normal
                window.Activate()
            this.Update(Views.application icon count showTray showWindow (fun () -> desktop.Shutdown()))
        | _ -> ()
        // Registers automatic component/native-object cleanup on lifetime exit.
        base.OnFrameworkInitializationCompleted()
