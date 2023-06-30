﻿namespace BasicTemplate

open System
open Avalonia
open Avalonia.Controls.ApplicationLifetimes
open Avalonia.Input
open Avalonia.Themes.Fluent
open Avalonia.FuncUI.Hosts
open Avalonia.Controls
open Avalonia.FuncUI
open Avalonia.FuncUI.DSL
open Avalonia.Layout

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

module Views =

    let listItemView (item: IWritable<TodoItem>) =
        Component.create ($"item-{item.Current.ItemId}", fun ctx ->
            let activeItemId = ctx.usePassed AppState.activeItemId
            let isActive = Some item.Current.ItemId = activeItemId.Current
            let title = ctx.useState item.Current.Title

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
                            Button.content "edit"
                            Button.onClick (fun _ -> activeItemId.Set (Some item.Current.ItemId))
                        ]

                        CheckBox.create [
                            CheckBox.dock Dock.Left
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

            printf $"%A{items.Current}"


            StackPanel.create [
                StackPanel.orientation Orientation.Vertical
                StackPanel.children (
                    items
                    |> State.sequenceBy (fun item -> item.ItemId)
                    |> List.map (fun item -> listItemView item)
                )
            ]
        )


    let mainView () =
        Component(fun ctx ->


            DockPanel.create [
                DockPanel.children [
                    listView ()
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

        #if DEBUG
        this.AttachDevTools (KeyGesture Key.F12)
        #endif

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