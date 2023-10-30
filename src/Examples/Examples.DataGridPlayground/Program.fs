namespace Examples.EnvApp

open System.Collections.ObjectModel
open Avalonia
open Avalonia.Controls
open Avalonia.Controls.ApplicationLifetimes
open Avalonia.Data
open Avalonia.FuncUI.Hosts
open Avalonia.Themes.Fluent
open Avalonia.FuncUI
open Avalonia.FuncUI.DSL
#nowarn "57"

type Person (name, age, male) =
    member val Name = name with get, set
    member val Age = age with get, set
    member val IsMale = male with get, set

[<AbstractClass; Sealed>]
type Views =

    static member main () =
        Component (fun ctx ->
            let data = ctx.useState (
                ObservableCollection [
                    Person("John", 20, true)
                    Person("Jane", 21, false)
                    Person("Bob", 22, true)
                ]
            )

            DockPanel.create [
                DockPanel.children [

                    DataGrid.create [
                        DataGrid.dock Dock.Top
                        DataGrid.isReadOnly false
                        DataGrid.items data.Current

                        DataGrid.columns [
                            DataGridTextColumn.create [
                                DataGridTextColumn.header "Name"
                                DataGridTextColumn.binding (Binding ("Name", BindingMode.TwoWay))
                                DataGridTextColumn.width (DataGridLength(2, DataGridLengthUnitType.Star))
                            ]
                            DataGridTemplateColumn.create [
                                DataGridTemplateColumn.header "Name"
                                DataGridTemplateColumn.cellTemplate (
                                    DataTemplateView<_>.create (fun (data: Person) ->
                                        TextBlock.create [
                                            TextBlock.text data.Name
                                        ]
                                    )
                                )
                                DataGridTemplateColumn.cellEditingTemplate (
                                    DataTemplateView<_>.create (fun (data: Person) ->
                                        TextBox.create [
                                            TextBox.init (fun t ->
                                                t.Bind(TextBox.TextProperty, Binding("Name", BindingMode.TwoWay)) |> ignore
                                            )
                                        ]
                                    )
                                )
                            ]
                            DataGridTextColumn.create [
                                DataGridTextColumn.header "Age"
                                DataGridTextColumn.binding (Binding "Age")
                            ]
                            DataGridCheckBoxColumn.create [
                                DataGridCheckBoxColumn.header "IsMale"
                                DataGridCheckBoxColumn.binding (Binding "IsMale")
                            ]
                        ]
                    ]
                ]
            ]
        )

type MainWindow() as this =
    inherit HostWindow()
    do
        base.Title <- "DataGrid Playground"
        base.Width <- 500.0
        base.Height <- 500.0
        this.Content <- Views.main ()

type App() =
    inherit Application()

    override this.Initialize() =
        this.Styles.Add (FluentTheme())
        this.RequestedThemeVariant <- Styling.ThemeVariant.Dark
        this.Styles.Load "avares://Avalonia.Controls.DataGrid/Themes/Fluent.xaml"

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