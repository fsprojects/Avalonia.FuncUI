namespace Avalonia.FuncUI.Diagnostics

open Avalonia.FuncUI
open Avalonia.FuncUI.Diagnostics
open Avalonia
open Avalonia.Controls
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Types
open Avalonia.Layout
open Avalonia.Media

type internal StateHookViews =

    static member private valuePresenterView (value: IReadable<obj>) =
        Component.create ("value_presenter_view", fun ctx ->
            let value = ctx.usePassedRead value

            Border.create [
                Border.background "#333333"
                Border.cornerRadius 5.0
                Border.padding 10.0
                Border.contextMenu (
                    ContextMenu.create [
                        ContextMenu.viewItems [
                            MenuItem.create [
                                MenuItem.header "Copy"
                                MenuItem.onClick (fun _ ->
                                    Async.StartImmediate (
                                        async {
                                            let json = System.Text.Json.JsonSerializer.Serialize value.Current

                                            do! Async.AwaitTask (Application.Current.Clipboard.SetTextAsync json)
                                        }
                                    )
                                )
                            ]
                        ]
                    ]
                )
                Border.child (
                    TextBlock.create [
                        TextBlock.foreground "#dddddd"
                        TextBlock.fontSize 12.0
                        TextBlock.fontFamily (FontFamily "Fira Code, monospace")
                        TextBlock.textWrapping TextWrapping.Wrap
                        TextBlock.text $"{value.Current}"
                    ]
                )
            ]
            :> IView
        )

    static member private anyReadableView (name: string, readable: IAnyReadable) =
        Component.create ($"any_readable_view_{readable.InstanceId}", fun ctx ->
            let currentValue = ctx.useState (readable.CurrentAny, true)

            ctx.useEffect(
                handler = (fun _ ->
                    currentValue.Set readable.CurrentAny
                ),
                triggers = [
                    EffectTrigger.AfterChange readable
                ]
            )

            StackPanel.create [
                StackPanel.orientation Orientation.Vertical
                StackPanel.spacing 5.0
                StackPanel.children [
                    TextBlock.create [
                        TextBlock.foreground "#ecf0f1"
                        TextBlock.text $"{name}"
                    ]
                    StateHookViews.valuePresenterView currentValue
                ]
            ]
            :> IView
        )

    static member private stateHookSourcesView (hook: StateHook) =
        Component.create ("state_hook_sources", fun ctx ->
            TreeView.create [
                TreeView.itemTemplate (
                    DataTemplateView<_>.create (
                        (fun info -> Seq.ofList info.Source.InstanceType.Sources),
                        (fun info ->
                            StackPanel.create [
                                StackPanel.orientation Orientation.Vertical
                                StackPanel.spacing 5.0
                                StackPanel.children [

                                    TextBlock.create [
                                        TextBlock.foreground "#2ecc71"
                                        TextBlock.text $"{Reflector.fullTypeName(info.Source.GetType())}"
                                    ]

                                    StateHookViews.anyReadableView (info.Name, info.Source)
                                ]
                            ]
                        )
                    )

                )
                TreeView.dataItems hook.State.Value.InstanceType.Sources
            ]
            :> IView
        )

    static member private stateHookHeader (hook: StateHook) =
        Component.create ($"state_hook_details_header_{hook.Identity}", fun ctx ->
            let currentValue = ctx.useState (hook.State.Value.CurrentAny, true)

            ctx.useEffect(
                handler = (fun _ ->
                    currentValue.Set hook.State.Value.CurrentAny
                ),
                triggers = [
                    EffectTrigger.AfterChange hook.State.Value
                ]
            )

            StackPanel.create [
                StackPanel.orientation Orientation.Vertical
                StackPanel.spacing 5.0
                StackPanel.children [
                    TextBlock.create [
                        TextBlock.foreground "#2ecc71"
                        TextBlock.text $"{Reflector.fullTypeName(hook.State.Value.GetType())}"
                    ]
                    StackPanel.create [
                        StackPanel.orientation Orientation.Horizontal
                        StackPanel.spacing 5.0
                        StackPanel.children [
                            TextBlock.create [
                                TextBlock.opacity 0.8
                                TextBlock.text "Render on change:"
                            ]
                            TextBlock.create [
                                TextBlock.foreground (if hook.RenderOnChange then "#1abc9c" else "#f39c12")
                                TextBlock.text $"{hook.RenderOnChange}"
                            ]
                            TextBlock.create [
                                TextBlock.opacity 0.8
                                TextBlock.text "Hook Identity:"
                            ]
                            TextBlock.create [
                                TextBlock.foreground "#1abc9c"
                                TextBlock.text $"{hook.Identity}"
                            ]
                        ]
                    ]
                    StateHookViews.valuePresenterView currentValue
                ]
            ] :> IView
        )

    static member stateHookDetailsView (hook: StateHook) =
        Component.create ($"state_hook_detail_view_{hook.Identity}", fun ctx ->
            StackPanel.create [
                StackPanel.spacing 10.0
                StackPanel.children [
                    StateHookViews.stateHookHeader hook

                    match hook.State.Value.InstanceType with
                    | InstanceType.Adapter _ ->
                        TextBlock.create [
                            TextBlock.text "Adapter sources:"
                        ]

                        StackPanel.create [
                            StackPanel.orientation Orientation.Vertical
                            StackPanel.spacing 5.0
                            StackPanel.children [
                                StateHookViews.stateHookSourcesView hook
                            ]
                        ]
                    | _ ->
                        ()
                ]
            ]
            :> IView
        )

    static member stateHookView (hook: StateHook) =
        Component.create ($"state_hook_detail_view_{hook.Identity}", fun ctx ->
            Border.create [
                Border.padding 5.0
                Border.child (
                    StackPanel.create [
                        StackPanel.spacing 10.0
                        StackPanel.children [
                            StateHookViews.stateHookHeader hook

                            if not hook.State.Value.InstanceType.Sources.IsEmpty then
                                Button.create [
                                    Button.content (
                                        StackPanel.create [
                                            StackPanel.orientation Orientation.Horizontal
                                            StackPanel.spacing 5.0
                                            StackPanel.children [
                                                IconView.create (Icons.openInNewWindow, 16)
                                                TextBlock.create [
                                                    TextBlock.text "Details"
                                                ]
                                            ]
                                        ]
                                    )
                                    Button.onClick (fun _ ->
                                        let childWindow =
                                            ChildWindow (
                                                title = "hook details",
                                                comp = (
                                                    Component (fun _ctx ->
                                                        ScrollViewer.create [
                                                            ScrollViewer.padding 5.0
                                                            ScrollViewer.content (
                                                                StateHookViews.stateHookDetailsView hook
                                                            )
                                                        ]
                                                        :> IView
                                                    )
                                                )
                                            )

                                        childWindow.Show ()
                                    )
                                ]
                        ]
                    ]
                )
            ]
            :> IView
        )