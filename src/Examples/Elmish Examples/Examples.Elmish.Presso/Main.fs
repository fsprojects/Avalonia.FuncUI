namespace Examples.Presso

open Avalonia.Controls.Shapes
open Avalonia.Media

module Main =
    open Avalonia.Controls
    open Avalonia.Controls.Primitives
    open Avalonia.FuncUI.DSL
    open Avalonia.Media
    open Avalonia.Media.Imaging
    open Avalonia.Layout

    [<Measure>] type ml
    [<Measure>] type g

    type BrewingMethod =
        | General
        | FrenchPress
        | PourOver
        | ColdBrew

    type Strength =
        | Regular
        | Strong

    let calculateCoffeeAmount (method: BrewingMethod) (strength: Strength) (water: int<ml>) : float<g> =
        let ratio : int =
            match method, strength with
            | General,     Regular -> 18
            | General,     Strong  -> 15
            | FrenchPress, Regular -> 17
            | FrenchPress, Strong  -> 11
            | PourOver,    Regular -> 17
            | PourOver,    Strong  -> 15
            | ColdBrew,    Regular -> 8
            | ColdBrew,    Strong  -> 5

        let coffee = (float water) * (1.0 / float ratio)

        LanguagePrimitives.FloatWithMeasure<g>(coffee)

    type State =
        { water: int<ml>
          method: BrewingMethod
          strength: Strength }
    let init =
        { water = 500<ml>
          method = General
          strength = Regular }

    type Msg =
        | SetWaterAmount of int<ml>
        | SetBrewingMethod of BrewingMethod
        | SetStrength of Strength
        | Reset

    let update (msg: Msg) (state: State) : State =
        match msg with
        | SetWaterAmount ml -> { state with water = ml }
        | SetBrewingMethod method -> { state with method = method }
        | SetStrength strength -> { state with strength = strength }
        | Reset -> init

    let waterAmountView (state: State) (dispatch: Msg -> unit) =
        DockPanel.create [
            DockPanel.dock Dock.Top
            DockPanel.children [
                TextBlock.create [
                    TextBlock.dock Dock.Top
                    TextBlock.fontSize 32.0
                    TextBlock.verticalAlignment VerticalAlignment.Center
                    TextBlock.horizontalAlignment HorizontalAlignment.Center
                    TextBlock.text (sprintf "%i ml" state.water)
                ]
                UniformGrid.create [
                    UniformGrid.dock Dock.Top
                    UniformGrid.columns 4
                    UniformGrid.children [
                        for i in [ 250<ml> .. 250<ml> .. 1000<ml> ] do
                            RadioButton.create [
                                RadioButton.horizontalAlignment HorizontalAlignment.Stretch
                                RadioButton.onChecked (fun _ -> i |> SetWaterAmount |> dispatch)
                                RadioButton.content (sprintf "%i ml" i)
                                RadioButton.classes [ "water" ]
                                RadioButton.groupName "water"
                                RadioButton.isChecked (state.water.Equals i)
                            ]
                    ]
                ]
                Slider.create [
                    Slider.dock Dock.Top
                    Slider.orientation Orientation.Horizontal
                    Slider.minimum 0.0
                    Slider.maximum 1000.0
                    Slider.largeChange 100.0
                    Slider.smallChange 100.00
                    Slider.tickFrequency 5.0
                    Slider.isSnapToTickEnabled true
                    Slider.value (float state.water)
                    Slider.onValueChanged (fun i ->
                        (int i)
                        |> LanguagePrimitives.Int32WithMeasure<ml>
                        |> SetWaterAmount
                        |> dispatch
                    )
                ]
            ]
        ]

    let brewingMethodView (state: State) (dispatch: Msg -> unit) =
        DockPanel.create [
            DockPanel.dock Dock.Top
            DockPanel.children [
                UniformGrid.create [
                    UniformGrid.dock Dock.Top
                    UniformGrid.rows 2
                    UniformGrid.columns 2
                    UniformGrid.children [
                        RadioButton.create [
                            RadioButton.background (
                                let brush = "avares://Examples.Elmish.Presso/Assets/general.jpeg" |> Bitmap.Create |> ImageBrush
                                brush.Stretch <- Stretch.UniformToFill
                                brush
                            )
                            RadioButton.horizontalAlignment HorizontalAlignment.Stretch
                            RadioButton.onChecked (fun _ -> BrewingMethod.General |> SetBrewingMethod |> dispatch)
                            RadioButton.groupName "brewing_method"
                            RadioButton.content "General"
                            RadioButton.classes [ "brewing_method" ]
                            RadioButton.isChecked (state.method.Equals BrewingMethod.General)
                        ]
                        RadioButton.create [
                            RadioButton.background (
                                let brush = "avares://Examples.Elmish.Presso/Assets/cold-brew.jpeg" |> Bitmap.Create |> ImageBrush
                                brush.Stretch <- Stretch.UniformToFill
                                brush
                            )
                            RadioButton.horizontalAlignment HorizontalAlignment.Stretch
                            RadioButton.onChecked (fun _ -> BrewingMethod.ColdBrew |> SetBrewingMethod |> dispatch)
                            RadioButton.content "Cold Brew"
                            RadioButton.groupName "brewing_method"
                            RadioButton.classes [ "brewing_method" ]
                            RadioButton.isChecked (state.method.Equals BrewingMethod.ColdBrew)
                        ]
                        RadioButton.create [
                            RadioButton.background (
                                let brush = "avares://Examples.Elmish.Presso/Assets/french-press.jpeg" |> Bitmap.Create |> ImageBrush
                                brush.Stretch <- Stretch.UniformToFill
                                brush
                            )
                            RadioButton.horizontalAlignment HorizontalAlignment.Stretch
                            RadioButton.onChecked (fun _ -> BrewingMethod.FrenchPress |> SetBrewingMethod |> dispatch)
                            RadioButton.content "French Press"
                            RadioButton.groupName "brewing_method"
                            RadioButton.classes [ "brewing_method" ]
                            RadioButton.isChecked (state.method.Equals BrewingMethod.FrenchPress)
                        ]
                        RadioButton.create [
                            RadioButton.background (
                                let brush = "avares://Examples.Elmish.Presso/Assets/pour-over.jpeg" |> Bitmap.Create |> ImageBrush
                                brush.Stretch <- Stretch.UniformToFill
                                brush
                            )
                            RadioButton.horizontalAlignment HorizontalAlignment.Stretch
                            RadioButton.onChecked (fun _ -> BrewingMethod.PourOver |> SetBrewingMethod |> dispatch)
                            RadioButton.content "Pour Over"
                            RadioButton.groupName "brewing_method"
                            RadioButton.classes [ "brewing_method" ]
                            RadioButton.isChecked (state.method.Equals BrewingMethod.PourOver)
                        ]
                    ]
                ]
            ]
        ]

    let strengthView (state: State) (dispatch: Msg -> unit) =
        Border.create [
            Border.dock Dock.Top
            Border.background (
                let brush = "avares://Examples.Elmish.Presso/Assets/coffee-beans.jpeg" |> Bitmap.Create |> ImageBrush
                brush.Stretch <- Stretch.UniformToFill
                brush
            )
            Border.cornerRadius 12.0
            Border.margin 5.0
            Border.borderBrush "#4D1E0A"
            Border.borderThickness 1.0
            Border.child (
                DockPanel.create [
                    DockPanel.dock Dock.Top
                    DockPanel.children [
                        UniformGrid.create [
                            UniformGrid.dock Dock.Top
                            UniformGrid.columns 2
                            UniformGrid.children [
                                RadioButton.create [
                                    RadioButton.onChecked (fun _ -> Strength.Regular |> SetStrength |> dispatch)
                                    RadioButton.content "Regular"
                                    RadioButton.groupName "strength"
                                    RadioButton.classes [ "strength" ]
                                    RadioButton.isChecked (state.strength.Equals Strength.Regular)
                                    RadioButton.horizontalAlignment HorizontalAlignment.Stretch
                                ]
                                RadioButton.create [
                                    RadioButton.onChecked (fun _ -> Strength.Strong |> SetStrength |> dispatch)
                                    RadioButton.content "Strong"
                                    RadioButton.groupName "strength"
                                    RadioButton.classes [ "strength" ]
                                    RadioButton.isChecked (state.strength.Equals Strength.Strong)
                                    RadioButton.horizontalAlignment HorizontalAlignment.Stretch
                                ]
                            ]
                        ]
                        TextBlock.create [
                            TextBlock.margin 5.0
                            TextBlock.dock Dock.Top
                            TextBlock.fontSize 32.0
                            TextBlock.verticalAlignment VerticalAlignment.Center
                            TextBlock.horizontalAlignment HorizontalAlignment.Center
                            TextBlock.text (sprintf "%.1f g" (calculateCoffeeAmount state.method state.strength state.water))
                        ]
                    ]
                ]
            )
        ]

    let view (state: State) (dispatch: Msg -> unit) =
        DockPanel.create [
            DockPanel.lastChildFill false
            DockPanel.children [
                waterAmountView state dispatch
                brewingMethodView state dispatch
                strengthView state dispatch
            ]
        ]