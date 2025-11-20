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
            ResourceDictionary.keyValue ("DefaultPadding", Thickness (16.0))
            ResourceDictionary.keyValue ("ButtonPadding", Thickness (12.0, 8.0))

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

    let themeVariantScopeView (themeVariant: ThemeVariant) (attrs) =
        ThemeVariantScope.create [
            yield! attrs
            ThemeVariantScope.requestedThemeVariant themeVariant
            ThemeVariantScope.child (
                DockPanel.create [
                    DockPanel.init (fun dp ->
                        dp.Styles.AddRange [
                            Style (fun x -> x.Is<Panel> ())
                            |> addProp _.Setters [
                                Setter (
                                    Panel.BackgroundProperty,
                                    dp.GetResourceObservable("SurfaceBrush").ToBinding ()
                                )
                            ]
                            Style (fun x -> x.Is<TextBlock> ())
                            |> addProp _.Setters [
                                Setter (
                                    TextBlock.ForegroundProperty,
                                    dp.GetResourceObservable("TextBrush").ToBinding ()
                                )
                                Setter (
                                    TextBlock.PaddingProperty,
                                    dp.GetResourceObservable("DefaultPadding").ToBinding ()
                                )
                            ]
                            Style (fun x -> x.Is<Button> ())
                            |> addProp _.Setters [
                                Setter (
                                    Button.BackgroundProperty,
                                    dp.GetResourceObservable("PrimaryBrush").ToBinding ()
                                )
                                Setter (Button.ForegroundProperty, dp.GetResourceObservable("TextBrush").ToBinding ())
                                Setter (Button.PaddingProperty, dp.GetResourceObservable("ButtonPadding").ToBinding ())
                            ]
                            |> addProp _.Children [
                                Style (fun x -> x.Nesting().Class (":pointerover"))
                                |> addProp _.Setters [
                                    Setter (
                                        TextBlock.BackgroundProperty,
                                        dp.GetResourceObservable("AccentBrush").ToBinding ()
                                    )
                                    Setter (
                                        TextBlock.ForegroundProperty,
                                        dp.GetResourceObservable("TextBrush").ToBinding ()
                                    )
                                ]
                            ]
                        ])
                    DockPanel.lastChildFill false
                    DockPanel.children [
                        TextBlock.create [
                            TextBlock.text (sprintf "Current Theme: %A" themeVariant)
                            TextBlock.margin (Thickness (0.0, 0.0, 0.0, 16.0))
                            TextBlock.fontSize 18.0
                        ]
                        Button.create [ Button.content "Sample Button" ]
                    ]
                ]
            )
        ]

    let colorPickerView (label: string) (color: Color) =
        ColorPicker.create [ ColorPicker.color color ]

    let view () =
        Component (fun ctx ->
            let state = ctx.useStateLazy init
            ctx.attrs [ Component.resources (createThemeResources state.Current) ]

            Grid.create [
                Grid.rowDefinitions "Auto,Auto,Auto"
                Grid.columnDefinitions "*"
                Grid.children [
                    themeVariantScopeView ThemeVariant.Dark [ Grid.row 0; Grid.column 0 ]
                    themeVariantScopeView ThemeVariant.Light [ Grid.row 1; Grid.column 0 ]
                    themeVariantScopeView ThemeVariant.Custom [ Grid.row 2; Grid.column 0 ]
                ]
            ])
