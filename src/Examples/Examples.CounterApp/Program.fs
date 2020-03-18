module Examples.CounterApp

open System
open Avalonia
open Avalonia.Controls
open Elmish
open Avalonia.FuncUI.Components.Hosts
open Avalonia.FuncUI
open Avalonia.FuncUI.Elmish
open Avalonia.Controls.ApplicationLifetimes
open Avalonia.FuncUI.DSL

module Counter =
    type State = { count : int }

    let init = { count = 0 }

    type Add =
        | Add of State
        | SetCount of int

    let update (msg : Add) (state: State) : State =
        match msg with
        | SetCount number -> { state with count = number }
        | Add state' ->
            { state' with count = state'.count + 1 }
            

    let view (state : State) dispatch =
        StackPanel.create [
            StackPanel.children [
                Grid.create [
                    Grid.children [
                        Button.create [
                            Button.content "+ (with scope `state`)"
                            
                            // function is only updated if the `scope` (need to find a better name for this) changed. In
                            // this case we need to explicitly declare that this function relies on `state` and need to be
                            // updated if the `state` changed
                             
                            Button.onClick (fun _ ->
                                dispatch (Add state)
                            , state) // <- this is optional!
                        ]
                    ]
                ]

                TextBox.create [
                    TextBox.text (string state.count)
                ]
            ]
        ]

    ()

type MainWindow() as this =
    inherit HostWindow()
    do
        base.Title <- "Counter Example"
        base.Height <- 400.0
        base.Width <- 400.0

        Elmish.Program.mkSimple (fun () -> Counter.init) Counter.update Counter.view
        |> Program.withHost this
        |> Program.withConsoleTrace
        |> Program.run

type App() =
    inherit Application()

    override this.Initialize() =
        this.Styles.Load "resm:Avalonia.Themes.Default.DefaultTheme.xaml?assembly=Avalonia.Themes.Default"
        this.Styles.Load "resm:Avalonia.Themes.Default.Accents.BaseDark.xaml?assembly=Avalonia.Themes.Default"

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