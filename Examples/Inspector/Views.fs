namespace Inspector

open Avalonia.Controls
open Avalonia.Media
open Avalonia.FuncUI.Types
open Avalonia.FuncUI
open System

module PropertyView =

    type State = {
        Name : string
        PropertyValueType : Type
        HasGet : bool
        HasSet : bool
        Parent : Type list
    }

    let view (state: State) (dispatch): View =
        Views.stackpanel [
            Attrs.background ((SolidColorBrush.Parse "#332980b9").ToImmutable())
            Attrs.children [
                // Name
                Views.textblock [
                    Attrs.text state.Name
                    Attrs.fontsize 14.0
                    Attrs.foreground ((SolidColorBrush.Parse "#ecf0f1").ToImmutable())
                ]
                // Get, Set, Type
                Views.stackpanel [
                    Attrs.orientation Orientation.Horizontal
                    Attrs.children [

                        if state.HasGet then
                            yield Views.textblock [
                                Attrs.text "GET"
                                Attrs.foreground ((SolidColorBrush.Parse "#27ae60").ToImmutable())
                            ]

                        if state.HasSet then
                            yield Views.textblock [
                                Attrs.text "SET"
                                Attrs.foreground ((SolidColorBrush.Parse "#f39c12").ToImmutable())
                            ]

                        yield Views.textblock [
                            Attrs.text state.PropertyValueType.Name
                            Attrs.foreground ((SolidColorBrush.Parse "#2980b9").ToImmutable())
                        ]              
                    ]
                ]
                // Type
                Views.textblock [
                    Attrs.text 
                        (
                            let names = state.Parent |> List.map (fun i -> i.Name)
                            String.Join ("; ", names)
                        )

                    Attrs.foreground ((SolidColorBrush.Parse "#bdc3c7").ToImmutable())
                ]              
            ]
        ]


module ControlView =

    type State = {
        Name : string
        Type : Type
    }

    let view (state: State) (dispatch): View =
        Views.stackpanel [
            Attrs.background ((SolidColorBrush.Parse "#338e44ad").ToImmutable())
            Attrs.children [
                // Name
                Views.textblock [
                    Attrs.text state.Name
                    Attrs.foreground ((SolidColorBrush.Parse "#8e44ad").ToImmutable())
                ]
                // Type
                Views.textblock [
                    Attrs.text state.Name
                    Attrs.foreground ((SolidColorBrush.Parse "#8e44ad").ToImmutable())
                ]              
            ]
        ]


module InspectorView =

    [<RequireQualifiedAccess>]
    type Perspective = Controls | Properties

    type InspectorState = {
        Properties: PropertyView.State list
        Controls: ControlView.State list
        Perspective: Perspective
    }

    let init = {
        Properties = []
        Controls = []
        Perspective = Perspective.Controls
    }

    type Msg =
    | ShowControls
    | ShowProperties

    let update (msg: Msg) (state: InspectorState) : InspectorState =
        let loadControls() : ControlView.State list =
            Analyzer.findAllControls()
            |> List.map (fun t -> { Name = t.Name; Type = t })

        let loadProperties() : PropertyView.State list =
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
            |> List.sortByDescending (fun t -> (t.Parent.Length, t.Name))

        match msg with
        | ShowControls ->
            {
                state with
                    Controls = loadControls();
                    Perspective = Perspective.Controls
            }
        | ShowProperties ->
            {
                state with
                    Properties = loadProperties();
                    Perspective = Perspective.Properties
            }
            
    
    let view (state: InspectorState) (dispatch): View =
        Views.dockpanel [
            Attrs.children [

                Views.button [
                    Attrs.click (fun sender args -> dispatch ShowControls)
                    Attrs.content (
                        Views.textblock [
                            Attrs.text "show controls"
                        ]
                    )
                ]

                Views.button [
                    Attrs.click (fun sender args -> dispatch ShowProperties)
                    Attrs.content (
                        Views.textblock [
                            Attrs.text "show properties"
                        ]
                    )
                ]

                Views.textblock [
                    match state.Perspective with 
                    | Perspective.Properties ->
                        yield Attrs.text (sprintf "%i Properties" state.Properties.Length)
                        yield Attrs.foreground ((SolidColorBrush.Parse "#2980b9").ToImmutable())
                    | Perspective.Controls ->
                        yield Attrs.text (sprintf "%i Controls" state.Controls.Length)
                        yield Attrs.foreground ((SolidColorBrush.Parse "#8e44ad").ToImmutable())

                ]
                Views.scrollviewer [
                    Attrs.content (Views.stackpanel [
                        Attrs.children [
                            match state.Perspective with
                            | Perspective.Controls -> 
                                for control in state.Controls do
                                    yield ControlView.view control dispatch

                            | Perspective.Properties ->
                                for control in state.Properties do
                                    yield PropertyView.view control dispatch                       
                        ]
                    ])
                ] 
            ]
        ]       
