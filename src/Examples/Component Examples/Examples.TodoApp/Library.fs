namespace BasicTemplate

open System
open System.Reflection.PortableExecutable
open Avalonia
open Avalonia.Animation
open Avalonia.Controls.ApplicationLifetimes
open Avalonia.FuncUI.Types
open Avalonia.Input
open Avalonia.Media
open Avalonia.Themes.Fluent
open Avalonia.FuncUI.Hosts
open Avalonia.Controls
open Avalonia.FuncUI
open Avalonia.FuncUI.DSL
open Avalonia.Layout
open Avalonia.Threading

type TodoItem =
    { ItemId: Guid
      Title: string
      Done: bool }

[<RequireQualifiedAccess>]
module TodoItem =

    let create (title: string) =
        { TodoItem.ItemId = Guid.NewGuid()
          TodoItem.Title = title
          TodoItem.Done = false }

    let demoItems: TodoItem list =
        [
            create "Item 1"
            create "Item 2"
            create "Item 3"
        ]

[<RequireQualifiedAccess>]
module AppState =

    let items: IWritable<TodoItem list> = new State<_>(TodoItem.demoItems)

    let activeItemId: IWritable<Guid option> = new State<_>(None)

    let hideDoneItems: IWritable<bool> = new State<_>(false)

module Views =

    let listItemView (item: IWritable<TodoItem>) =
        Component.create ($"item-%O{item.Current.ItemId}", fun ctx ->
            let activeItemId = ctx.usePassed AppState.activeItemId
            let item = ctx.usePassed item
            let title = ctx.useState item.Current.Title
            let animation = ctx.useStateLazy(fun () ->
                let animation = new Animation();
                animation.Duration <- TimeSpan.FromMilliseconds(1000);
                //animation.IterationCount <- new IterationCount(2uL);

                let key0 = new KeyFrame();
                key0.KeyTime <- TimeSpan.FromMilliseconds(0);
                key0.Setters.Add(new Avalonia.Styling.Setter(Border.OpacityProperty, 1.0))
                key0.Setters.Add(new Avalonia.Styling.Setter(Border.RenderTransformProperty, Rotate3DTransform()))

                let key1 = new KeyFrame();
                key1.KeyTime <- TimeSpan.FromMilliseconds(1000);
                key1.Setters.Add(new Avalonia.Styling.Setter(Border.OpacityProperty, 0.0))
                key0.Setters.Add(new Avalonia.Styling.Setter(Border.RenderTransformProperty, Rotate3DTransform(AngleX = 90)))
                animation.Children.Add(key1);

                animation;


            )

            let isActive = Some item.Current.ItemId = activeItemId.Current

            if isActive then
                DockPanel.create [
                    DockPanel.children [

                        Button.create [
                            Button.dock Dock.Right
                            Button.content "save"
                            Button.onClick (fun _ ->
                                item.Set { item.Current with Title = title.Current }
                                activeItemId.Set None
                            )
                        ]

                        TextBox.create [
                            TextBox.dock Dock.Left
                            TextBox.text title.Current
                            TextBox.onTextChanged (fun text -> title.Set text)
                        ]
                    ]
                ]
            else
                DockPanel.create [
                    DockPanel.children [

                        Button.create [
                            Button.dock Dock.Right
                            Button.content "delete"


                            Button.onClick (fun args ->
                                ignore (
                                    task {
                                        do! animation.Current.RunAsync (ctx.control)

                                        Dispatcher.UIThread.Post (fun _ ->
                                            AppState.items.Current
                                            |> List.filter (fun i -> i.ItemId <> item.Current.ItemId)
                                            |> AppState.items.Set
                                        )

                                        return ()
                                    }
                                )
                            )
                        ]

                        Button.create [
                            Button.dock Dock.Right
                            Button.content "edit"
                            Button.onClick (fun _ -> activeItemId.Set (Some item.Current.ItemId))
                        ]

                        CheckBox.create [
                            CheckBox.dock Dock.Left
                            CheckBox.isChecked item.Current.Done
                            CheckBox.onChecked (fun _ -> item.Set { item.Current with Done = true })
                            CheckBox.onUnchecked (fun _ -> item.Set { item.Current with Done = false })
                            CheckBox.content (
                                TextBlock.create [
                                    TextBlock.text item.Current.Title
                                ]
                            )
                        ]
                    ]
                ]
        )

    let listView () =
        Component.create ("listView", fun ctx ->
            let items = ctx.usePassed AppState.items
            let hideDoneItems = ctx.usePassed AppState.hideDoneItems

            StackPanel.create [
                StackPanel.orientation Orientation.Vertical
                StackPanel.children (
                    items
                    |> State.sequenceBy (fun item -> item.ItemId)
                    |> List.filter (fun item ->
                        if hideDoneItems.Current then
                            not item.Current.Done
                         else
                             true
                    )
                    |> List.map (fun item -> listItemView item)
                )
            ]
        )


    let mainView () =
        Component(fun ctx ->

            let hideDoneItems = ctx.usePassed AppState.hideDoneItems

            DockPanel.create [
                DockPanel.children [
                    StackPanel.create [
                        StackPanel.dock Dock.Bottom
                        StackPanel.children [
                            Button.create [
                                Button.content "add item"
                                Button.onClick (fun _ ->
                                    let newItem = TodoItem.create ""
                                    AppState.items.Set (AppState.items.Current @ [newItem])
                                    AppState.activeItemId.Set (Some newItem.ItemId))
                            ]

                            Button.create [
                                Button.content (
                                    if hideDoneItems.Current
                                    then "show done items"
                                    else "hide done items"
                                )
                                Button.onClick (fun _ ->
                                    hideDoneItems.Set (not hideDoneItems.Current)
                                )
                            ]

                        ]
                    ]

                    ContentControl.create [
                        ContentControl.dock Dock.Top
                        ContentControl.content (listView())
                    ]
                ]
            ]
        )

type MainWindow() as this =
    inherit HostWindow()
    do
        base.Title <- "BasicTemplate"
        base.Width <- 400.0
        base.Height <- 400.0
        this.Content <- Views.mainView ()

type App() =
    inherit Application()

    override this.Initialize() =
        this.Styles.Add (FluentTheme())

    override this.OnFrameworkInitializationCompleted() =
        match this.ApplicationLifetime with
        | :? IClassicDesktopStyleApplicationLifetime as desktopLifetime ->
            desktopLifetime.MainWindow <- MainWindow()
        | _ -> ()

[<RequireQualifiedAccess>]
module Program =

    [<EntryPoint>]
    let main(args: string[]) =
        AppBuilder
            .Configure<App>()
            .UsePlatformDetect()
            .UseSkia()
            .StartWithClassicDesktopLifetime(args)