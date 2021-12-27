namespace Avalonia.FuncUI.Diagnostics

open Avalonia.FuncUI
open Avalonia.FuncUI.Diagnostics
open System
open Avalonia.Controls
open Avalonia.FuncUI.Components
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Types
open Avalonia.Layout

type internal InspectorViews =

    static member componentListPicker () =
        Component.create ("component_list_picker", fun ctx ->
            let components = ctx.usePassed (InspectorState.shared.Components, renderOnChange = false)
            let selectedId = ctx.usePassed InspectorState.shared.SelectedComponentId

            let sortedComponents = ctx.useState List.empty

            ctx.useEffect (
                handler = (fun () ->
                    let next =
                        components.Current
                        |> Seq.map (fun c -> Some c.ComponentId)
                        |> Seq.sort
                        |> Seq.toList

                    if sortedComponents.Current <> next then
                        sortedComponents.Set next
                ),
                triggers = [
                    EffectTrigger.AfterChange components
                ]
            )

            ComboBox.create [
                ComboBox.horizontalAlignment HorizontalAlignment.Stretch
                ComboBox.dataItems sortedComponents.Current
                ComboBox.itemTemplate (
                    DataTemplateView.create<_, _>(fun (data: Guid option) ->
                        TextBlock.create [
                            TextBlock.text (
                                match data with
                                | Some value -> $"Component {value.HumanReadable}"
                                | None -> "No component selected"
                            )
                        ]
                    )
                )
                ComboBox.selectedItem selectedId.Current
                ComboBox.onSelectedItemChanged (fun args ->
                    match args with
                    | :? Option<Guid> as value -> selectedId.Set value
                    | _ -> ()
                )
            ]
            :> IView
        )

    static member componentDetailsView (selected: IReadable<ComponentRef option>) =
        Component.create ("details_view", fun ctx ->
            let selected = ctx.usePassedRead selected

            ScrollViewer.create [
                ScrollViewer.padding 5.0
                ScrollViewer.content (
                    DockPanel.create [
                        DockPanel.lastChildFill true
                        DockPanel.children [
                            match selected.Current with
                            | Some comp ->
                                StackPanel.create [
                                    StackPanel.orientation Orientation.Vertical
                                    StackPanel.spacing 5.0
                                    StackPanel.children [
                                        for hook in comp.Ref.Context.Hooks do
                                           StateHookViews.stateHookView hook.Value
                                    ]
                                ]
                            | None ->
                                TextBlock.create [
                                    TextBlock.text "No component selected"
                                ]
                        ]
                    ]
                )
            ]
            :> IView
        )

    static member componentDetailsView () =
        Component.create ("details_view", fun ctx ->
            let selected = ctx.usePassedRead InspectorState.shared.SelectedComponent

            InspectorViews.componentDetailsView selected :> IView
        )

    static member modeButton () =
        Component.create ("mode_button_view", fun ctx ->
            let mode = ctx.usePassed InspectorState.shared.InspectorWindowPosition

            match mode.Current with
            | InspectorWindowPosition.Pinned ->
                Button.create [
                    Button.content "Unpin"
                    Button.margin (0.0, 0.0, 5.0, 0.0)
                    Button.onClick (fun _args ->
                        InspectorState.shared.InspectorWindowPosition.Set InspectorWindowPosition.Normal
                    )
                ]
                :> IView
            | InspectorWindowPosition.Normal ->
                Button.create [
                    Button.content "Pin"
                    Button.margin (0.0, 0.0, 5.0, 0.0)
                    Button.onClick (fun _args ->
                        InspectorState.shared.InspectorWindowPosition.Set InspectorWindowPosition.Pinned
                    )
                ]
                :> IView
        )

    static member menu () =
        Component.create ("menu_view", fun ctx ->
            let selected = ctx.usePassedRead InspectorState.shared.SelectedComponent

            Border.create [
                Border.padding 5.0
                Border.background "#006e59"
                Border.child (
                    DockPanel.create [
                        DockPanel.lastChildFill true
                        DockPanel.children [
                            InspectorViews.modeButton ()

                            Button.create [
                                Button.dock Dock.Right
                                Button.margin (5.0, 0.0, 0.0, 0.0)
                                Button.isVisible selected.Current.IsSome
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
                                    let humanReadable =
                                        selected.Current
                                        |> Option.map (fun s -> s.ComponentId.HumanReadable)
                                        |> Option.defaultValue "-"

                                    let childWindow =
                                        ChildWindow (
                                            title = $"component {humanReadable}",
                                            comp = (
                                                Component (fun _ctx ->
                                                    InspectorViews.componentDetailsView (new ReadOnlyState<ComponentRef option>(selected.Current)) :> IView
                                                )
                                            )
                                        )

                                    childWindow.Show ()
                                )
                            ]

                            InspectorViews.componentListPicker ()
                        ]
                    ]
                )
            ]
            :> IView
        )

    static member view =
        Component (fun ctx ->
            let selectedComponent = ctx.usePassedRead (InspectorState.shared.SelectedComponent, false)
            let components = ctx.usePassedRead (InspectorState.shared.Components, false)

            ctx.useEffect (
                handler = (fun _ ->
                    for comp in components.Current do
                        ComponentHighlightAdorner.Remove comp.Ref

                    match selectedComponent.Current with
                    | Some comp ->
                        ComponentHighlightAdorner.Attache comp.Ref
                    | None ->
                        ()
                ),
                triggers = [
                    EffectTrigger.AfterChange selectedComponent
                ]
            )

            DockPanel.create [
                DockPanel.lastChildFill true
                DockPanel.children [
                    ContentControl.create [
                        ContentControl.dock Dock.Top
                        ContentControl.content (
                            InspectorViews.menu ()
                        )
                    ]
                    InspectorViews.componentDetailsView ()
                ]
            ] :> IView
        )
