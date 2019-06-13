namespace Inspector

open Avalonia.Controls
open Avalonia.Media
open Avalonia.FuncUI.Types
open Avalonia.FuncUI
open System

module PropertyView =

    type State = {
        Name : string
        Type : Type
        HasGet : bool
        HasSet : bool
        Parent : Type list
    }

    let view (state: State) (dispatch): View =
        Views.stackpanel [
            Attrs.children [
                // Name
                Views.textblock [
                    Attrs.text state.Name
                ]
                // Type
                Views.textblock [
                    Attrs.text state.Name
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
            Attrs.children [
                // Name
                Views.textblock [
                    Attrs.text state.Name
                ]
                // Type
                Views.textblock [
                    Attrs.text state.Name
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

    let update (msg: Msg) (state: InspectorState) : InspectorState =
        let loadControls() : ControlView.State list =
            Analyzer.findAllControls()
            |> List.map (fun t -> { Name = t.Name; Type = t })

        match msg with
        | ShowControls -> { state with Controls = loadControls() }
            
    
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
                Views.textblock [
                    match state.Perspective with
                    | Perspective.Controls ->
                        yield Attrs.text (sprintf "%i Controls" state.Controls.Length)
                    | Perspective.Properties ->
                        yield Attrs.text (sprintf "%i Properties" state.Properties.Length)
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
