namespace Examples.TreeDataGridPlaygrounf

open System.Collections.ObjectModel
open Avalonia
open Avalonia.Controls
open Avalonia.Controls.ApplicationLifetimes
open Avalonia.FuncUI.Hosts
open Avalonia.Themes.Fluent
open Avalonia.FuncUI
open Avalonia.FuncUI.DSL
open Avalonia.Controls.Models.TreeDataGrid

type Person (name, age, male, children) =
    member val Name = name with get, set
    member val Age = age with get, set
    member val IsMale = male with get, set
    member val Children = children with get, set

[<AbstractClass; Sealed>]
type Views =

    static member main () =
        Component (fun ctx ->
            let data = ctx.useState (
                ObservableCollection [
                    Person("John", 63, true, [
                        Person("Bob", 32, true, [])
                        Person("Jill", 30, false, [
                            Person("Peter", 12, true, [])
                        ])
                    ])
                    Person("Jane", 25, false, [ 
                        Person("Tim", 6, true, []) 
                    ])
                    Person("Bob", 16, true, [])
                ]
            )
            
            let selectedItem = ctx.useState None

            DockPanel.create [
                DockPanel.children [
                    TextBlock.create [
                        TextBlock.dock Dock.Top
                        TextBlock.margin 10
                        TextBlock.text $"""Selected: {(selectedItem.Current |> Option.defaultValue (Person("", 0, false, []))).Name}"""
                    ]
                    TreeDataGrid.create [
                        TreeDataGrid.dock Dock.Top
                        
                        TreeDataGrid.init (fun ctrl ->
                            let dataSource = new HierarchicalTreeDataGridSource<Person>(data.Current)

                            dataSource.Columns.Add(
                                HierarchicalExpanderColumn<Person>(
                                    TextColumn<Person, string>("Name", _.Name), 
                                    _.Children
                                )
                            )

                            dataSource.Columns.Add (TextColumn<Person, int>("Age", _.Age))
                            dataSource.Columns.Add (CheckBoxColumn<Person>("IsMale", _.IsMale, fun o v -> o.IsMale <- v))
                            
                            dataSource.RowSelection.SelectionChanged.Add (fun (args) ->
                                    (if args.SelectedItems.Count = 0 then 
                                        None
                                    else
                                        Some args.SelectedItems[0])
                                    |> selectedItem.Set
                            )

                            ctrl.Source <- dataSource
                        )
                    ]
                ]
            ]
        )

type MainWindow() as this =
    inherit HostWindow()
    do
        base.Title <- "TreeDataGrid Playground"
        base.Width <- 500.0
        base.Height <- 500.0
        this.Content <- Views.main ()

type App() =
    inherit Application()

    override this.Initialize() =
        this.Styles.Add (FluentTheme())
        this.RequestedThemeVariant <- Styling.ThemeVariant.Dark
        this.Styles.Load "avares://Avalonia.Controls.TreeDataGrid/Themes/Fluent.axaml"

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
