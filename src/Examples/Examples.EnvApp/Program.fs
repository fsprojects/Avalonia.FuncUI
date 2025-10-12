﻿namespace Examples.EnvApp

open Avalonia
open Avalonia.Controls
open Avalonia.Controls.ApplicationLifetimes
open Avalonia.Controls.Shapes
open Avalonia.FuncUI.Experimental
open Avalonia.FuncUI.Hosts
open Avalonia.FuncUI.Types
open Avalonia.Layout
open Avalonia.Media
open Avalonia.Themes.Fluent
open Avalonia.FuncUI
open Avalonia.FuncUI.DSL

#nowarn "57"

[<RequireQualifiedAccess>]
module SharedState =

    let brush = EnvironmentState<IWritable<IBrush>>.Create "brush"
    let size = EnvironmentState<IWritable<int>>.Create "size"

[<AbstractClass; Sealed>]
type Views =

    static member canvas () =
        Component.create ("canvas", fun ctx ->
            let canvasOutlet = ctx.useState (null, renderOnChange = false)
            let isPressed = ctx.useState (false, renderOnChange = false)
            let lastPoint = ctx.useState (None, renderOnChange = false)

            let brush = ctx.useEnvState (SharedState.brush, renderOnChange = false)
            let size = ctx.useEnvState (SharedState.size, renderOnChange = false)

            ctx.attrs [
                Component.dock Dock.Top
            ]

            View.createWithOutlet canvasOutlet.Set Canvas.create [
                Canvas.verticalAlignment VerticalAlignment.Stretch
                Canvas.horizontalAlignment HorizontalAlignment.Stretch
                Canvas.background Brushes.White
                Canvas.onPointerPressed (fun _ ->
                    isPressed.Set true
                )
                Canvas.onPointerReleased (fun _ ->
                    isPressed.Set false
                    lastPoint.Set None
                )
                Canvas.onPointerMoved (fun args ->
                    let point = args.GetPosition(canvasOutlet.Current)

                    if isPressed.Current then
                        match lastPoint.Current with
                        | Some lastPoint ->
                            let line = Line(
                                StartPoint = lastPoint,
                                EndPoint = point,
                                Stroke = brush.Current,
                                StrokeThickness = float size.Current,
                                StrokeLineCap = PenLineCap.Round
                            )

                            if canvasOutlet.Current <> null then
                                canvasOutlet.Current.Children.Add line

                        | None ->
                            ()

                        lastPoint.Set (Some point)
                    else
                        ()
                )
            ] :> IView
        )

    static member colorPicker (brush: IWritable<IBrush>) =
        Component.create ("color_picker", fun ctx ->
            let brush = ctx.usePassed (brush, renderOnChange = true)

            let brushes: IBrush list = [
                Brushes.Black
                Brushes.Red
                Brushes.Green
                Brushes.Blue
                Brushes.Yellow
            ]

            ctx.attrs [
                Component.dock Dock.Left
            ]

            StackPanel.create [
                StackPanel.orientation Orientation.Horizontal
                StackPanel.spacing 5.0
                StackPanel.children [
                    for item in brushes do
                        Border.create [
                            Border.width 32.0
                            Border.height 32.0
                            Border.cornerRadius 16.0
                            Border.background item
                            Border.borderThickness 4.0
                            Border.borderBrush (
                                if item = brush.Current
                                then item
                                else Brushes.Transparent
                            )
                            Border.onPointerPressed (fun _ ->
                                brush.Set item
                            )
                        ]
                ]
            ]
            :> IView
        )

    static member sizePicker (size: IWritable<int>) =
        Component.create ("size_picker", fun ctx ->
            let size = ctx.usePassed (size, renderOnChange = true)

            let sizes: int list = [ 2; 4; 6; 8; 16; 32; ]

            ctx.attrs [
                Component.dock Dock.Right
            ]

            StackPanel.create [
                StackPanel.orientation Orientation.Horizontal
                StackPanel.spacing 5.0
                StackPanel.children [
                    for item in sizes do
                        Border.create [
                            Border.width (float item)
                            Border.height (float item)
                            Border.cornerRadius (float item / 2.0)
                            Border.background (
                                if item = size.Current
                                then Brushes.Black
                                else Brushes.Gray
                            )
                            Border.onPointerPressed (fun _ ->
                                size.Set item
                            )
                        ]
                ]
            ]
            :> IView
        )

    static member settings () =
        Component.create ("settings", fun ctx ->
            let brush = ctx.useEnvState (SharedState.brush, renderOnChange = false)
            let size = ctx.useEnvState (SharedState.size, renderOnChange = false)

            ctx.attrs [
                Component.dock Dock.Bottom
                Component.margin 5.0
                Component.padding 5.0
                Component.cornerRadius 8.0
                Component.background "#bdc3c7"
            ]

            DockPanel.create [
                DockPanel.lastChildFill false
                DockPanel.children [
                    Views.colorPicker brush
                    Views.sizePicker size
                ]
            ]
            :> IView
        )

    static member main () =
        Component (fun ctx ->
            let brush = ctx.useState (Brushes.Black :> IBrush, renderOnChange = false)
            let size = ctx.useState (2, renderOnChange = false)

            SharedState.brush.provide (
                providedValue = brush,
                content = (
                    EnvironmentStateProvider.create(
                        state = SharedState.size,
                        providedValue = size,
                        content = (
                            DockPanel.create [
                                DockPanel.lastChildFill true
                                DockPanel.background Brushes.White
                                DockPanel.children [
                                    Views.settings ()
                                    Views.canvas ()
                                ]
                            ]
                        )
                    )
                )
            )
        )


type MainWindow() as this =
    inherit HostWindow()
    do
        base.Title <- "Environment App"
        base.Width <- 500.0
        base.Height <- 500.0
        this.Content <- Views.main ()

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