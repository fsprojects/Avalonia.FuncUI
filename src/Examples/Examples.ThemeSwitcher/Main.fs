namespace Examples.ThemeSwitcher

module Main =
    open Avalonia
    open Avalonia.Controls
    open Avalonia.Media
    open Avalonia.FuncUI
    open Avalonia.FuncUI.DSL
    open Avalonia.FuncUI.Types
    open Avalonia.Styling
    open Examples.ThemeSwitcher.DSL
    open Avalonia.Data
    open Avalonia.Layout
    open Avalonia.Controls.Presenters


    module ThemeVariant =
        let Custom = ThemeVariant ("Custom", ThemeVariant.Dark)

    type ColorSet =
        { Theme: ThemeVariant
          Primary: Color
          Secondary: Color
          Background: Color
          Surface: Color
          Text: Color
          Accent: Color }

    module ColorSet =
        let dark =
            { Theme = ThemeVariant.Dark
              Primary = Colors.CornflowerBlue
              Secondary = Colors.LightSteelBlue
              Background = Color.Parse ("#1E1E1E")
              Surface = Color.Parse ("#252526")
              Text = Colors.White
              Accent = Colors.DodgerBlue }

        let light =
            { Theme = ThemeVariant.Light
              Primary = Colors.RoyalBlue
              Secondary = Colors.SteelBlue
              Background = Colors.White
              Surface = Color.Parse ("#F3F3F3")
              Text = Colors.Black
              Accent = Colors.Blue }

        let customInit =
            { Theme = ThemeVariant.Custom
              Primary = Colors.MediumPurple
              Secondary = Colors.Plum
              Background = Color.Parse ("#FFF8DC")
              Surface = Color.Parse ("#FAF0E6")
              Text = Colors.DarkSlateGray
              Accent = Colors.MediumOrchid }

    /// Define Custom ThemeVariant's Colors.
    type State = { CustomColors: ColorSet }

    /// Initial state with default colors.
    let init () = { CustomColors = ColorSet.customInit }

    // Create theme-specific resources
    let createThemeResources (state: State) : IView =
        ResourceDictionary.create [
            // Common resources used by all themes
            ResourceDictionary.keyValue ("LargeFontSize", 24.0)
            ResourceDictionary.keyValue ("MediumFontSize", 16.0)
            ResourceDictionary.keyValue ("SmallFontSize", 12.0)
            ResourceDictionary.keyValue ("DefaultMargin", Thickness 8.0)
            ResourceDictionary.keyValue ("DefaultPadding", Thickness (12.0))
            ResourceDictionary.keyValue ("ButtonPadding", Thickness (8.0, 4.0))

            // Theme variant-specific resources
            for colorSet in [ ColorSet.dark; ColorSet.light; state.CustomColors ] do
                ResourceDictionary.themeDictionariesKeyValue (
                    colorSet.Theme,
                    ResourceDictionary.create [
                        ResourceDictionary.keyValue ("PrimaryBrush", SolidColorBrush colorSet.Primary)
                        ResourceDictionary.keyValue ("SecondaryBrush", SolidColorBrush colorSet.Secondary)
                        ResourceDictionary.keyValue ("BackgroundBrush", SolidColorBrush colorSet.Background)
                        ResourceDictionary.keyValue ("SurfaceBrush", SolidColorBrush colorSet.Surface)
                        ResourceDictionary.keyValue ("TextBrush", SolidColorBrush colorSet.Text)
                        ResourceDictionary.keyValue ("AccentBrush", SolidColorBrush colorSet.Accent)
                    ]
                )
        ]

    let inline (|>!) x ([<InlineIfLambda>] f) =
        f x
        x

    let inline addProp<'x, 'v> (acccsessor: 'x -> System.Collections.Generic.ICollection<'v>) (vs: 'v seq) (x: 'x) =
        let collection = acccsessor x
        Seq.iter collection.Add vs
        x

    let inline getResourceBinding<'t when 't :> IResourceHost> (x: 't) (key: string) =
        x.GetResourceObservable(key).ToBinding ()

    let themeVariantScopeView (themeVariant: ThemeVariant) (attrs) =
        ThemeVariantScope.create [
            yield! attrs
            ThemeVariantScope.requestedThemeVariant themeVariant
            ThemeVariantScope.child (
                DockPanel.create [
                    DockPanel.init (fun dp ->
                        let getResourceBinding = getResourceBinding dp

                        dp.Styles.AddRange [
                            Style (fun x -> x.Is<Layoutable> ())
                            |> addProp _.Setters [
                                Setter (Layoutable.MarginProperty, getResourceBinding "DefaultMargin")
                            ]
                            Style (fun x -> x.Is<Panel> ())
                            |> addProp _.Setters [
                                Setter (Panel.BackgroundProperty, getResourceBinding "SurfaceBrush")
                            ]
                            Style (fun x -> x.Is<TextBlock> ())
                            |> addProp _.Setters [
                                Setter (TextBlock.ForegroundProperty, getResourceBinding "TextBrush")
                            ]
                            Style (fun x -> x.Is<Button> ())
                            |> addProp _.Setters [
                                Setter (Button.BackgroundProperty, getResourceBinding "PrimaryBrush")
                                Setter (Button.ForegroundProperty, getResourceBinding "TextBrush")
                                Setter (Button.PaddingProperty, getResourceBinding "ButtonPadding")
                            ]
                            |> addProp _.Children [
                                Style (fun x -> x.Nesting().Class ("secondary"))
                                |> addProp _.Setters [
                                    Setter (Button.BackgroundProperty, getResourceBinding "SecondaryBrush")
                                    Setter (Button.ForegroundProperty, getResourceBinding "TextBrush")
                                ]
                                // "^:pointerover /template/ ContentPresenter#PART_ContentPresenter"
                                Style (fun x ->
                                    x
                                        .Nesting()
                                        .Class(":pointerover")
                                        .Template()
                                        .OfType<ContentPresenter>()
                                        .Name ("PART_ContentPresenter"))
                                |> addProp _.Setters [
                                    Setter (Button.BackgroundProperty, getResourceBinding "AccentBrush")
                                    Setter (Button.ForegroundProperty, getResourceBinding "TextBrush")
                                ]
                            ]
                        ])
                    DockPanel.lastChildFill false
                    DockPanel.children [
                        TextBlock.create [
                            TextBlock.dock Dock.Top
                            TextBlock.text (sprintf "Current Theme: %A" themeVariant)
                            TextBlock.classes [ "large" ]
                        ]
                        TextBlock.create [ TextBlock.dock Dock.Top; TextBlock.text "This is a sample text block." ]
                        StackPanel.create [
                            StackPanel.dock Dock.Top
                            StackPanel.orientation Orientation.Horizontal
                            StackPanel.children [
                                Button.create [ Button.content "Sample Button" ]
                                Button.create [ Button.classes [ "secondary" ]; Button.content "Sample Button" ]
                            ]
                        ]
                    ]
                ]
            )
        ]

    let colorPickerView (color: Color) onColorChanged attrs =
        ColorPicker.create [
            yield! attrs
            ColorPicker.color color
            ColorPicker.onPropertyChanged (fun e ->
                if e.Property = ColorPicker.ColorProperty then
                    let newColor = e.GetNewValue<Color> ()
                    onColorChanged newColor)
        ]

    let colorView (label: string) (color: Color) onColorChanged attrs =
        StackPanel.create [
            yield! attrs
            StackPanel.orientation Orientation.Horizontal
            StackPanel.children [
                TextBlock.create [ TextBlock.width 120.0; TextBlock.text label; TextBlock.classes [ "small" ] ]
                colorPickerView color onColorChanged []
            ]
        ]

    let colorSetsView (state: IWritable<State>) (attrs) =
        Component.create (
            "colorSetsView",
            fun ctx ->
                let state = ctx.usePassedLazy (fun _ -> state)
                let defaultMargin = ctx.useStateLazy (fun () -> Thickness 0.0)

                ctx.attrs [
                    Component.onResourceObservable (
                        "DefaultMargin",
                        function
                        | Some (:? Thickness as v) -> defaultMargin.Set v
                        | _ -> ()
                    )
                    yield! attrs
                ]

                DockPanel.create [
                    DockPanel.lastChildFill false
                    DockPanel.classes [ "colorsetsview" ]
                    DockPanel.children [
                        TextBlock.create [
                            TextBlock.dock Dock.Top
                            TextBlock.classes [ "large" ]
                            TextBlock.text $"Color Sets View: {state.Current.CustomColors.Theme}"
                        ]
                        colorView
                            "Primary Color"
                            state.Current.CustomColors.Primary
                            (fun newColor ->
                                state.Set
                                    { state.Current with
                                        CustomColors.Primary = newColor })
                            [ DockPanel.dock Dock.Top; StackPanel.margin defaultMargin.Current ]
                        colorView
                            "Secondary Color"
                            state.Current.CustomColors.Secondary
                            (fun newColor ->
                                state.Set
                                    { state.Current with
                                        CustomColors.Secondary = newColor })
                            [ DockPanel.dock Dock.Top; StackPanel.margin defaultMargin.Current ]
                        colorView
                            "Background Color"
                            state.Current.CustomColors.Background
                            (fun newColor ->
                                state.Set
                                    { state.Current with
                                        CustomColors.Background = newColor })
                            [ DockPanel.dock Dock.Top; StackPanel.margin defaultMargin.Current ]
                        colorView
                            "Surface Color"
                            state.Current.CustomColors.Surface
                            (fun newColor ->
                                state.Set
                                    { state.Current with
                                        CustomColors.Surface = newColor })
                            [ DockPanel.dock Dock.Top; StackPanel.margin defaultMargin.Current ]
                        colorView
                            "Text Color"
                            state.Current.CustomColors.Text
                            (fun newColor ->
                                state.Set
                                    { state.Current with
                                        CustomColors.Text = newColor })
                            [ DockPanel.dock Dock.Top; StackPanel.margin defaultMargin.Current ]
                        colorView
                            "Accent Color"
                            state.Current.CustomColors.Accent
                            (fun newColor ->
                                state.Set
                                    { state.Current with
                                        CustomColors.Accent = newColor })
                            [ DockPanel.dock Dock.Top; StackPanel.margin defaultMargin.Current ]
                    ]
                ]
        )

    let view () =
        Component (fun ctx ->
            let state = ctx.useStateLazy init
            ctx.attrs [ Component.resources (createThemeResources state.Current) ]

            Grid.create [
                Grid.init (fun grid ->
                    let getResourceBinding = getResourceBinding grid

                    grid.Styles.AddRange [
                        Style (fun x -> x.Is<TextBlock> ())
                        |> addProp _.Setters [
                            Setter (TextBlock.FontSizeProperty, getResourceBinding "MediumFontSize")
                            Setter (TextBlock.PaddingProperty, getResourceBinding "ButtonPadding")
                        ]
                        |> addProp _.Children [
                            Style (fun x -> x.Nesting().Class ("large"))
                            |> addProp _.Setters [
                                Setter (TextBlock.FontSizeProperty, getResourceBinding "LargeFontSize")
                            ]
                            Style (fun x -> x.Nesting().Class ("small"))
                            |> addProp _.Setters [
                                Setter (TextBlock.FontSizeProperty, getResourceBinding "SmallFontSize")
                            ]
                        ]
                    ])
                Grid.rowDefinitions "Auto,Auto,Auto"
                Grid.columnDefinitions "*,Auto"
                Grid.children [
                    colorSetsView state [ Grid.row 0; Grid.column 1; Grid.rowSpan 3 ]
                    themeVariantScopeView ThemeVariant.Dark [ Grid.row 0; Grid.column 0 ]
                    themeVariantScopeView ThemeVariant.Light [ Grid.row 1; Grid.column 0 ]
                    themeVariantScopeView ThemeVariant.Custom [ Grid.row 2; Grid.column 0 ]
                ]
            ])
