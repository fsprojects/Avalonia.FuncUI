namespace Examples.TodoApp

open System
open System.Reflection.PortableExecutable
open Avalonia
open Avalonia.Animation
open Avalonia.Animation.Easings
open Avalonia.Controls.ApplicationLifetimes
open Avalonia.FuncUI.Types
open Avalonia.Input
open Avalonia.Media
open Avalonia.Media.Imaging
open Avalonia.Platform
open Avalonia.Themes.Fluent
open Avalonia.FuncUI.Hosts
open Avalonia.Controls
open Avalonia.FuncUI
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Experimental
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

module Icons =
    (* https://icons8.com/icon/set/edit/sf-regular *)

    let delete = lazy new Bitmap(AssetLoader.Open(Uri("avares://Examples.TodoApp/Assets/Icons/trash.png")))
    let edit = lazy new Bitmap(AssetLoader.Open(Uri("avares://Examples.TodoApp/Assets/Icons/edit.png")))



module Views =

    let listItemView (item: IWritable<TodoItem>) =
        Component.create ($"item-%O{item.Current.ItemId}", fun ctx ->
            let activeItemId = ctx.usePassed AppState.activeItemId
            let item = ctx.usePassed item
            let title = ctx.useState item.Current.Title
            let animation = ctx.useStateLazy(fun () ->
                Animation()
                    .WithDuration(0.3)
                    .WithEasing(CubicEaseIn())
                    .WithFillMode(FillMode.Forward)
                    .WithKeyFrames [
                        KeyFrame()
                            .WithCue(0)
                            .WithSetter(Component.OpacityProperty, 1.0)
                            .WithSetter(TranslateTransform.XProperty, 0.0)

                        KeyFrame()
                            .WithCue(1)
                            .WithSetter(Component.OpacityProperty, 0.0)
                            .WithSetter(TranslateTransform.XProperty, 500.0)
                    ]
            )

            ctx.attrs [
                Component.renderTransform (TranslateTransform())
            ]

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
                            TextBox.fontSize 18.0
                            TextBox.fontWeight FontWeight.Light
                            TextBox.onTextChanged (fun text -> title.Set text)
                        ]
                    ]
                ]
            else
                DockPanel.create [
                    DockPanel.children [

                        Button.create [
                            Button.dock Dock.Right
                            Button.content (
                                Image.create [
                                    Image.width 24
                                    Image.height 24
                                    Image.source Icons.delete.Value
                                ]
                            )
                            Button.onClick (fun args ->
                                if not (ctx.control.IsAnimating Component.OpacityProperty) then
                                    ignore (
                                        task {
                                            do! animation.Current.RunAsync ctx.control

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
                            Button.content (
                                Image.create [
                                    Image.width 24
                                    Image.height 24
                                    Image.source Icons.edit.Value
                                ]
                            )
                            Button.onClick (fun _ -> activeItemId.Set (Some item.Current.ItemId))
                        ]

                        CheckBox.create [
                            CheckBox.dock Dock.Left
                            CheckBox.isChecked item.Current.Done
                            CheckBox.horizontalAlignment HorizontalAlignment.Stretch
                            //CheckBox.background Brushes.Gainsboro
                            CheckBox.onChecked (fun _ -> item.Set { item.Current with Done = true })
                            CheckBox.onUnchecked (fun _ -> item.Set { item.Current with Done = false })
                            CheckBox.content (
                                TextBlock.create [
                                    TextBlock.fontSize 18.0
                                    TextBlock.fontWeight FontWeight.Light
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