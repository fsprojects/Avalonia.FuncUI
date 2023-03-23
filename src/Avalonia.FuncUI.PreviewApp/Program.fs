namespace Avalonia.FuncUI.PreviewApp

open System
open System.IO
open System.Reflection
open System.Runtime.Loader
open Avalonia
open Avalonia.Controls
open Avalonia.Controls.ApplicationLifetimes
open Avalonia.FuncUI.Hosts
open Avalonia.FuncUI.Types
open Avalonia.Platform.Storage
open Avalonia.Styling
open Avalonia.Themes.Fluent
open Avalonia.Threading

module Main =
    open Avalonia.Controls
    open Avalonia.FuncUI
    open Avalonia.FuncUI.DSL
    open Avalonia.Layout

    type PreviewItem =
        { Method: MethodInfo
          Component: Component }

    let mutable asm: Assembly option = None

    let previewVersion (file: IReadable<string>) =
        Component.create ("version", fun ctx ->
            let asmCtx = ctx.useState<AssemblyLoadContext>(null, false)
            let asm = ctx.useState<Assembly option>(None, false)
            let sp = ctx.useState<StackPanel>(null, false)
            let previewItems = ctx.useState<PreviewItem list>([], true)

            ctx.useEffect (
                handler = (fun () ->
                    asmCtx.Set (AssemblyLoadContext("preview", true))
                    let bytes = File.ReadAllBytes file.Current
                    let memory = new MemoryStream(bytes)
                    asm.Set (Some (asmCtx.Current.LoadFromStream memory))
                    ()
                ),
                triggers = [ EffectTrigger.AfterInit ]
            )

            ctx.useEffect (
                handler = (fun () ->
                    asmCtx.Current.Unload ()
                    asmCtx.Set (AssemblyLoadContext("preview", true))
                    let bytes = File.ReadAllBytes file.Current
                    let memory = new MemoryStream(bytes)
                    asm.Set (Some (asmCtx.Current.LoadFromStream memory))
                    ()
                ),
                triggers = [ EffectTrigger.AfterChange file ]
            )

            ctx.useEffect (
                handler = (fun () ->
                    match asm.Current with
                    | Some asm ->
                        let methods: MethodInfo[] =
                            asm.GetTypes()
                            |> Array.collect (fun t -> t.GetMethods())
                            |> Array.filter (fun m -> m.GetCustomAttributes(typeof<PreviewAttribute>, false).Length > 0)

                        let previewMethods =
                            methods
                            |> Array.filter (fun m -> m.ReturnParameter.ParameterType = typeof<Component>)

                        let items =
                            previewMethods
                            |> Array.map (fun m ->
                                let previewAttribute = m.GetCustomAttribute(typeof<PreviewAttribute>) :?> PreviewAttribute
                                m.Invoke(null, previewAttribute.Args) :?> Component

                                { PreviewItem.Method = m
                                  PreviewItem.Component = m.Invoke(null, previewAttribute.Args) :?> Component }
                            )
                            |> Array.toList

                        previewItems.Set items

                    | None ->
                        ()
                ),
                triggers = [ EffectTrigger.AfterChange asm ]
            )

            View.createWithOutlet sp.Set StackPanel.create [
                StackPanel.children [

                    for item in previewItems.Current do
                        Border.create [
                            Border.margin 10
                            Border.child (
                                DockPanel.create [
                                    DockPanel.children [

                                        (* Header *)
                                        DockPanel.create [
                                            DockPanel.dock Dock.Top
                                            DockPanel.children [
                                                TextBlock.create [
                                                    TextBlock.foreground "#3498db"
                                                    TextBlock.text item.Method.Name
                                                ]

                                                TextBlock.create [
                                                    TextBlock.foreground "#3498db"
                                                ]
                                            ]
                                        ]

                                        (* content *)
                                        Border.create [
                                            Border.borderBrush "#3498db"
                                            Border.borderThickness 2
                                            Border.dock Dock.Bottom
                                            Border.child item.Component
                                        ]
                                    ]
                                ]
                            )
                        ]

                ]
            ]
        )

    let preview (file: string) =
        Component.create (file, fun ctx ->
            let watcher = ctx.useStateLazy (fun () ->
                new FileSystemWatcher(
                    path = Path.GetDirectoryName file,
                    Filter = Path.GetFileName file
                )
            )
            let file = ctx.useState(file, false)

            ctx.useEffect (
                handler = (fun () ->
                    watcher.Current.EnableRaisingEvents <- true
                    ctx.trackDisposable (
                        watcher.Current.Created.Subscribe (fun args ->
                            (* just trigger a signal *)
                            file.Set file.Current
                        )

                    )

                    ctx.trackDisposable (
                        watcher.Current.Changed.Subscribe (fun args ->
                            (* just trigger a signal *)
                            file.Set file.Current
                        )
                    )
                ),
                triggers = [ EffectTrigger.AfterInit ]
            )

            previewVersion file
        )

    let main () =
        Component (fun ctx ->
            let file = ctx.useState<string option>(Some "/Users/josuajaeger/sources/Avalonia.FuncUI/src/Examples/Component Examples/Examples.CounterApp/bin/Debug/net6.0/Examples.CounterApp.dll")

            DockPanel.create [
                DockPanel.children [
                    //Button.create [
                    //    Button.dock Dock.Bottom
                    //    Button.onClick (fun _ ->
                    //        Async.Start (
                    //            async {
                    //                let! path = Helpers.openDllPicker ()
//
                    //                file.Set path
                    //            }
                    //        )
                    //    )
                    //    Button.content "choose file"
                    //]

                    match file.Current with
                    | Some file -> preview file
                    | None -> TextBlock.create [ TextBlock.text "no file selected" ]
                ]
            ]
        )

type MainWindow() =
    inherit HostWindow()
    do
        base.Title <- "Previewer"
        base.Content <- Main.main ()

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
