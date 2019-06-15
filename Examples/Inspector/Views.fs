namespace Inspector

open Avalonia.Controls
open Avalonia.Media
open Avalonia.FuncUI.Types
open Avalonia.FuncUI
open System
open System.Text

module CustomViews =
    
    let typeView (t: Type) : View =
        let sb = StringBuilder()
        sb.Append t.Name |> ignore

        if t.IsGenericType then
            sb.Append "<" |> ignore
            t.GenericTypeArguments |> Seq.iteri (fun index item -> 
                if index = 0 then
                    sb.Append item.Name |> ignore
                else
                    sb.Append ", "  |> ignore
                    sb.Append item.Name  |> ignore
                    ()
            ) 
            sb.Append ">" |> ignore

        Views.textblock [
            Attrs.text (sb.ToString())
            Attrs.foreground ((SolidColorBrush.Parse "#2980b9").ToImmutable())
        ]

    let typeListView (t: Type list): View =
        Views.stackpanel [
            Attrs.orientation Orientation.Horizontal
            Attrs.children [
                for item in t do
                    yield typeView item
            ]
        ]

    let accessView (hasGet: bool, hasSet: bool) : View =
        Views.stackpanel [
            Attrs.orientation Orientation.Horizontal
            Attrs.margin 2.0
            Attrs.children [

                if hasGet then
                    yield Views.border [
                        Attrs.margin 1.0
                        Attrs.padding (2.0, 0.0)
                        Attrs.cornerRadius 5.0
                        Attrs.background "#27ae60"
                        Attrs.child (
                            Views.textblock [
                                Attrs.text "GET"
                            ]
                        )
                    ]

                if hasSet then
                    yield Views.border [
                        Attrs.margin 1.0
                        Attrs.padding (2.0, 0.0)
                        Attrs.cornerRadius 5.0
                        Attrs.background "#16a085"
                        Attrs.child (
                            Views.textblock [
                                Attrs.text "SET"
                            ]
                        )
                    ]
            ]
        ]

    let containerView (title: string) (view: View) : View =
        Views.border [
            Attrs.background "#332980b9"
            Attrs.margin 2.0
            Attrs.padding 5.0
            Attrs.cornerRadius 5.0
            Attrs.child (
                Views.stackpanel [
                    Attrs.children [
                        Views.textblock [
                            Attrs.text title
                            Attrs.fontSize 14.0
                            Attrs.foreground "#ecf0f1"
                        ]
                        view
                    ]
                ]
            )

        ]

module PropertyView =

    type State = {
        Name : string
        PropertyValueType : Type
        HasGet : bool
        HasSet : bool
        Parent : Type list
    }

    let view (state: State) (dispatch): View =
        CustomViews.containerView state.Name (
            Views.stackpanel [
                Attrs.children [
                    Views.stackpanel [
                        Attrs.orientation Orientation.Horizontal
                        Attrs.children [
                            CustomViews.accessView (state.HasGet, state.HasSet)
                            CustomViews.typeView state.PropertyValueType
                        ]
                    ]
                    CustomViews.typeListView state.Parent      
                ]
            ]
        )

module ControlView =

    type State = {
        Name : string
        DefiningType : Type
    }

    let view (state: State) (dispatch): View =
        CustomViews.containerView state.Name (
            Views.stackpanel [
                Attrs.children [
                    CustomViews.typeView state.DefiningType          
                ]
            ]
        )

module ElementView =
    
    type State =
    | Property of PropertyView.State
    | Control of ControlView.State

    let view (state: State) dispatch : View = 
        match state with
        | Property state -> PropertyView.view state dispatch
        | Control state -> ControlView.view state dispatch

module ElementsView =

    type State = ElementView.State list

    let view (state: State) dispatch : View =
        Views.scrollviewer [
            Attrs.content (
                Views.stackpanel [
                    Attrs.children [
                        for element in state do
                            yield ElementView.view element dispatch
                    ]
                ]
            )
        ]

module InspectorView =

    type InspectorState = {
        Elements : ElementsView.State
    }

    let init () =
        let controls : ElementView.State list =
            Analyzer.findAllControls()
            |> List.map (fun t ->
                {
                    ControlView.State.Name = t.Name;
                    ControlView.State.DefiningType = t
                }
            )
            |> List.map (fun t -> ElementView.State.Control t)

        let properties : ElementView.State list =
            Analyzer.findAllProperties()
            |> List.map (fun t ->
                {
                    PropertyView.State.Name = t.Name;
                    PropertyView.State.PropertyValueType = t.ValueType;
                    PropertyView.State.HasGet = t.HasGet;
                    PropertyView.State.HasSet = t.HasSet;
                    PropertyView.State.Parent = t.Parent;
                }
            )
            |> List.map (fun t -> ElementView.State.Property t)

        {
            Elements = properties @ controls 
        }

    type Msg =
    | ShowControls
    | ShowProperties

    let update (msg: Msg) (state: InspectorState) : InspectorState =
        state
            
    let view (state: InspectorState) (dispatch): View =
        Views.tabControl [
            Attrs.tabStripPlacement Dock.Left
            Attrs.items [
                Views.tabItem [
                    Attrs.header "All"
                    Attrs.content (
                        Views.dockpanel [
                            Attrs.children [
                                ElementsView.view state.Elements dispatch
                            
                            ]
                        ]
                    )
                ]
            ]      
        ]