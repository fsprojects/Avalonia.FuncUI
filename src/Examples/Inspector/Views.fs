namespace rec Inspector

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

        Views.textBlock [
            Attrs.text (sb.ToString())
            Attrs.foreground ((SolidColorBrush.Parse "#2980b9").ToImmutable())
        ]

    let typeListView (t: Type list): View =
        Views.stackPanel [
            Attrs.orientation Orientation.Horizontal
            Attrs.children [
                for item in t do
                    yield typeView item
            ]
        ]

    let accessView (hasGet: bool, hasSet: bool) : View =
        Views.stackPanel [
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
                            Views.textBlock [
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
                            Views.textBlock [
                                Attrs.text "SET"
                            ]
                        )
                    ]
            ]
        ]

    let elementContainerView (title: string) (color: string) (view: View) : View =
        Views.border [
            Attrs.background "#772c3e50"
            Attrs.margin 2.0
            Attrs.padding 5.0
            Attrs.cornerRadius 5.0
            Attrs.child (
                Views.stackPanel [
                    Attrs.children [
                        Views.textBlock [
                            Attrs.text title
                            Attrs.fontSize 14.0
                            Attrs.foreground color
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
        CustomViews.elementContainerView state.Name "#9b59b6" (
            Views.stackPanel [
                Attrs.children [
                    Views.stackPanel [
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

module EventView =

    type State = {
        Name : string
        EventValueType : Type
        Parent : Type list
    }

    let view (state: State) (dispatch): View =
        CustomViews.elementContainerView state.Name "#f1c40f" (
            Views.stackPanel [
                Attrs.children [
                    Views.stackPanel [
                        Attrs.orientation Orientation.Horizontal
                        Attrs.children [
                            CustomViews.typeView state.EventValueType
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
        CustomViews.elementContainerView state.Name "#ecf0f1" (
            Views.stackPanel [
                Attrs.children [
                    CustomViews.typeView state.DefiningType          
                ]
            ]
        )

module ElementView =
    
    type State =
    | Property of PropertyView.State
    | Event of EventView.State
    | Control of ControlView.State

    let view (state: State) dispatch : View = 
        match state with
        | Property state -> PropertyView.view state dispatch
        | Event state -> EventView.view state dispatch
        | Control state -> ControlView.view state dispatch

module ElementsView =

    type State = ElementView.State list

    let init () : State =
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

        let events : ElementView.State list =
            Analyzer.findAllEvents()
            |> List.map (fun t ->
                {
                    EventView.State.Name = t.Name;
                    EventView.State.EventValueType = t.ValueType;
                    EventView.State.Parent = t.Parent;
                }
            )
            |> List.map (fun t -> ElementView.State.Event t)

        properties @ events @ controls

    let filterElement (filter: FilterView.State) (element: ElementView.State) : bool =
        match element with
        | ElementView.Property property ->
            if filter.IncludeProperties then
                true
            else false
        | ElementView.Event event -> 
            if filter.IncludeEvents then
                true
            else false
        | ElementView.Control control ->
            if filter.IncludeControls then
                true
            else false


    let view (state: State) (filter: FilterView.State) dispatch : View =
        Views.scrollViewer [
            Attrs.dockPanel_dock Dock.Bottom
            Attrs.padding 2.0
            Attrs.content (
                Views.stackPanel [
                    Attrs.children [
                        for element in state do
                            if filterElement filter element then
                                yield ElementView.view element dispatch
                    ]
                ]
            )
        ]

module FilterView =

    type State = {
        IncludeProperties : bool
        IncludeEvents : bool
        IncludeControls : bool
    }

    let init () = 
        {
            IncludeProperties = true
            IncludeEvents = true
            IncludeControls = true
        }

    type Msg =
        | IncludeProperties of bool
        | IncludeEvents of bool
        | IncludeControls of bool

    let update (state: State) (msg: Msg) : State =
        match msg with
        | IncludeProperties includeProperties ->
            { state with IncludeProperties = includeProperties }
        | IncludeEvents includeEvents ->
            { state with IncludeEvents = includeEvents }
        | IncludeControls includeControls ->
            { state with IncludeControls = includeControls }

    let view (state: State) dispatch : View =
        Views.stackPanel [
            Attrs.background "#2c3e50"
            Attrs.dockPanel_dock Dock.Top
            Attrs.children [
                Views.stackPanel [
                    Attrs.margin 5.0
                    Attrs.dockPanel_dock Dock.Top
                    Attrs.orientation Orientation.Horizontal
                    Attrs.children [
                        Views.checkBox [
                            Attrs.content (sprintf "Properties [%b]" state.IncludeProperties)
                            Attrs.isChecked state.IncludeProperties
                            Attrs.click (fun obj args -> 
                                dispatch (InspectorView.FilterViewMsg (Msg.IncludeProperties (obj :?> CheckBox).IsChecked.Value))
                                args.Handled <- true
                            )
                        ]
                        Views.checkBox [
                            Attrs.content (sprintf "Events [%b]" state.IncludeEvents)
                            Attrs.isChecked state.IncludeEvents
                            Attrs.click (fun obj args -> 
                                dispatch (InspectorView.FilterViewMsg (Msg.IncludeEvents (obj :?> CheckBox).IsChecked.Value))
                                args.Handled <- true
                            )
                        ]
                        Views.checkBox [
                            Attrs.content (sprintf "Controls [%b]" state.IncludeControls)
                            Attrs.isChecked state.IncludeControls
                            Attrs.click (fun obj args -> 
                                dispatch (InspectorView.FilterViewMsg (Msg.IncludeControls (obj :?> CheckBox).IsChecked.Value))
                                args.Handled <- true
                            )
                        ]
                    ]
                ]
                Views.textBox [
                    Attrs.margin 5.0
                    Attrs.watermark "Search for Name..."
                ]
            ]
        ]

module InspectorView =

    type InspectorState = {
        Elements : ElementsView.State
        Filter : FilterView.State
    }

    let init () =
        {
            Elements = ElementsView.init()
            Filter = FilterView.init()
        }

    type Msg =
    | FilterViewMsg of FilterView.Msg

    let update (msg: Msg) (state: InspectorState) : InspectorState =
        match msg with
        | FilterViewMsg filter ->
            { state with Filter = FilterView.update state.Filter filter }
            
    let view (state: InspectorState) (dispatch): View =
        Views.dockpanel [
            Attrs.children [
                FilterView.view state.Filter dispatch
                ElementsView.view state.Elements state.Filter dispatch   
            ]
        ]