namespace Examples.GeneticAlgorithm

open System.Windows.Input
open Avalonia
open Avalonia.Controls
open Avalonia.Themes.Fluent
open Avalonia.FuncUI.Hosts
open Avalonia.Controls.ApplicationLifetimes

type internal SimpleCommand (action) =
    let event = Event<_, _>()

    interface ICommand with
        member this.CanExecute(obj) = true
        member this.Execute(obj) = action(obj)
        member this.add_CanExecuteChanged(handler) = event.Publish.AddHandler(handler)
        member this.remove_CanExecuteChanged(handler) = event.Publish.AddHandler(handler)



type MainWindow() as this =
    inherit HostWindow()

    do
        let nextGeneration = NativeMenuItem "compute next generation"
        nextGeneration.Command <-
            SimpleCommand (fun _ ->
                StateStore.shared.NextGeneration ()
            )

        let next10Generation = NativeMenuItem "compute 10 next generations"
        next10Generation.Command <-
            SimpleCommand (fun _ ->
                StateStore.shared.NextGenerations 10
            )

        let generationItem = NativeMenuItem "Generations"
        let generationMenu =  NativeMenu()
        generationItem.Menu <- generationMenu
        generationMenu.Add nextGeneration
        generationMenu.Add next10Generation

        let nativeMenu = NativeMenu()
        nativeMenu.Add generationItem

        NativeMenu.SetMenu(this, nativeMenu)

    do
        base.Title <- "Genetic Algorithm Example"
        base.Content <- Views.mainView ()

type App() =
    inherit Application()


    override this.Initialize() =
        this.Styles.Add (FluentTheme(Mode = FluentThemeMode.Dark))

    override this.OnFrameworkInitializationCompleted() =
        this.Name <- "Genetic Algorithm Example"
        match this.ApplicationLifetime with
        | :? IClassicDesktopStyleApplicationLifetime as desktopLifetime ->
            let mainWindow = MainWindow()
            mainWindow.MinWidth <- 850.0
            mainWindow.MinHeight <- 600.0

            //mainWindow.Renderer.DrawDirtyRects <- true
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