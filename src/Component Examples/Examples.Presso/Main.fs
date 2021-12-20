namespace Examples.Presso

open Avalonia.Controls.Shapes
open Avalonia.FuncUI
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

        static member Init =
            { water = 500<ml>
              method = General
              strength = Regular }

    let waterAmountView (state: IWritable<State>) =
        Component.create ("water", fun ctx ->
            let state = ctx.usePassedState state

            DockPanel.create [
                DockPanel.dock Dock.Top
                DockPanel.children [
                    TextBlock.create [
                        TextBlock.dock Dock.Top
                        TextBlock.fontSize 32.0
                        TextBlock.verticalAlignment VerticalAlignment.Center
                        TextBlock.horizontalAlignment HorizontalAlignment.Center
                        TextBlock.text (sprintf "%i ml" state.Current.water)
                    ]
                    UniformGrid.create [
                        UniformGrid.dock Dock.Top
                        UniformGrid.columns 4
                        UniformGrid.children [
                            for i in [ 250<ml> .. 250<ml> .. 1000<ml> ] do
                                RadioButton.create [
                                    RadioButton.horizontalAlignment HorizontalAlignment.Stretch
                                    RadioButton.onChecked (fun _ ->
                                        state.Set { state.Current with water = i }
                                    )
                                    RadioButton.content (sprintf "%i ml" i)
                                    RadioButton.classes [ "water" ]
                                    RadioButton.groupName "water"
                                    RadioButton.isChecked (state.Current.water.Equals i)
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
                        Slider.value (float state.Current.water)
                        Slider.onValueChanged (fun i ->
                            (int i)
                            |> LanguagePrimitives.Int32WithMeasure<ml>
                            |> fun i -> { state.Current with water = i }
                            |> state.Set
                        )
                    ]
                ]
            ] :> _
        )


    let brewingMethodView (state: IWritable<State>) =
        Component.create ("method", fun ctx ->
            let state = ctx.usePassedState state

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
                                    let brush = "avares://Examples.Presso/Assets/general.jpeg" |> Bitmap.Create |> ImageBrush
                                    brush.Stretch <- Stretch.UniformToFill
                                    brush
                                )
                                RadioButton.horizontalAlignment HorizontalAlignment.Stretch
                                RadioButton.onChecked (fun _ ->
                                    state.Set { state.Current with method = BrewingMethod.General }
                                )
                                RadioButton.groupName "brewing_method"
                                RadioButton.content "General"
                                RadioButton.classes [ "brewing_method" ]
                                RadioButton.isChecked (state.Current.method.Equals BrewingMethod.General)
                            ]
                            RadioButton.create [
                                RadioButton.background (
                                    let brush = "avares://Examples.Presso/Assets/cold-brew.jpeg" |> Bitmap.Create |> ImageBrush
                                    brush.Stretch <- Stretch.UniformToFill
                                    brush
                                )
                                RadioButton.horizontalAlignment HorizontalAlignment.Stretch
                                RadioButton.onChecked (fun _ ->
                                    state.Set { state.Current with method = BrewingMethod.ColdBrew }
                                )
                                RadioButton.content "Cold Brew"
                                RadioButton.groupName "brewing_method"
                                RadioButton.classes [ "brewing_method" ]
                                RadioButton.isChecked (state.Current.method.Equals BrewingMethod.ColdBrew)
                            ]
                            RadioButton.create [
                                RadioButton.background (
                                    let brush = "avares://Examples.Presso/Assets/french-press.jpeg" |> Bitmap.Create |> ImageBrush
                                    brush.Stretch <- Stretch.UniformToFill
                                    brush
                                )
                                RadioButton.horizontalAlignment HorizontalAlignment.Stretch
                                RadioButton.onChecked (fun _ ->
                                    state.Set { state.Current with method = BrewingMethod.FrenchPress }
                                )
                                RadioButton.content "French Press"
                                RadioButton.groupName "brewing_method"
                                RadioButton.classes [ "brewing_method" ]
                                RadioButton.isChecked (state.Current.method.Equals BrewingMethod.FrenchPress)
                            ]
                            RadioButton.create [
                                RadioButton.background (
                                    let brush = "avares://Examples.Presso/Assets/pour-over.jpeg" |> Bitmap.Create |> ImageBrush
                                    brush.Stretch <- Stretch.UniformToFill
                                    brush
                                )
                                RadioButton.horizontalAlignment HorizontalAlignment.Stretch
                                RadioButton.onChecked (fun _ ->
                                    state.Set { state.Current with method = BrewingMethod.PourOver }
                                )
                                RadioButton.content "Pour Over"
                                RadioButton.groupName "brewing_method"
                                RadioButton.classes [ "brewing_method" ]
                                RadioButton.isChecked (state.Current.method.Equals BrewingMethod.PourOver)
                            ]
                        ]
                    ]
                ]
            ] :> _
        )


    let strengthView (state: IWritable<State>) =
        Component.create ("strength", fun ctx ->
            let state = ctx.usePassedState state

            Border.create [
                Border.dock Dock.Top
                Border.background (
                    let brush = "avares://Examples.Presso/Assets/coffee-beans.jpeg" |> Bitmap.Create |> ImageBrush
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
                                        RadioButton.onChecked (fun _ ->
                                            state.Set { state.Current with strength = Strength.Regular }
                                        )
                                        RadioButton.content "Regular"
                                        RadioButton.groupName "strength"
                                        RadioButton.classes [ "strength" ]
                                        RadioButton.isChecked (state.Current.strength.Equals Strength.Regular)
                                        RadioButton.horizontalAlignment HorizontalAlignment.Stretch
                                    ]
                                    RadioButton.create [
                                        RadioButton.onChecked (fun _ ->
                                            state.Set { state.Current with strength = Strength.Strong }
                                        )
                                        RadioButton.content "Strong"
                                        RadioButton.groupName "strength"
                                        RadioButton.classes [ "strength" ]
                                        RadioButton.isChecked (state.Current.strength.Equals Strength.Strong)
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
                                TextBlock.text (sprintf "%.1f g" (calculateCoffeeAmount state.Current.method state.Current.strength state.Current.water))
                            ]
                        ]
                    ]
                )
            ] :> _
        )


    let view =
        Component (fun ctx ->
            let state = ctx.useState State.Init

            StackPanel.create [
                StackPanel.orientation Orientation.Vertical
                StackPanel.children [
                    waterAmountView state
                    brewingMethodView state
                    strengthView state
                ]
            ] :> _
        )
