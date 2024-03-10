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
                                MenuItem.onClick (fun args ->
                                    Async.StartImmediate (
                                        async {
                                            let json = System.Text.Json.JsonSerializer.Serialize value.Current

                                            do! Async.AwaitTask (TopLevel.GetTopLevel(args.Source :?> Visual).Clipboard.SetTextAsync json)
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
        Component.create ($"state_hook_detail_view_{hook.Identity}", fun _ctx ->
            StackPanel.create [
                StackPanel.spacing 10.0
                StackPanel.children [
                    StateHookViews.stateHookHeader hook
                ]
            ]
            :> IView
        )

    static member stateHookView (hook: StateHook) =
        Component.create ($"state_hook_detail_view_{hook.Identity}", fun _ctx ->
            Border.create [
                Border.padding 5.0
                Border.child (
                    StackPanel.create [
                        StackPanel.spacing 10.0
                        StackPanel.children [
                            StateHookViews.stateHookHeader hook
                        ]
                    ]
                )
            ]
            :> IView
        )