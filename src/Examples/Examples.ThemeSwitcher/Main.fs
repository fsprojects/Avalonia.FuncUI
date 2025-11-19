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


    module ThemeVariant =
        let Custom = ThemeVariant ("Custom", ThemeVariant.Dark)

    /// Define Custom ThemeVariant's Colors.
    type State =
        { PrimaryBrushColor: Color
          SecondaryBrushColor: Color
          BackgroundBrushColor: Color
          SurfaceBrushColor: Color
          TextBrushColor: Color
          AccentBrushColor: Color }

    /// Initial state with default colors.
    let init =
        { PrimaryBrushColor = Colors.RoyalBlue
          SecondaryBrushColor = Colors.SteelBlue
          BackgroundBrushColor = Colors.White
          SurfaceBrushColor = Color.Parse ("#F3F3F3")
          TextBrushColor = Colors.Black
          AccentBrushColor = Colors.Blue }

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
            ResourceDictionary.themeDictionariesKeyValue (
                ThemeVariant.Dark,
                ResourceDictionary.create [
                    ResourceDictionary.keyValue ("PrimaryBrush", SolidColorBrush (Colors.CornflowerBlue))
                    ResourceDictionary.keyValue ("SecondaryBrush", SolidColorBrush (Colors.LightSteelBlue))
                    ResourceDictionary.keyValue ("BackgroundBrush", SolidColorBrush (Color.Parse ("#1E1E1E")))
                    ResourceDictionary.keyValue ("SurfaceBrush", SolidColorBrush (Color.Parse ("#252526")))
                    ResourceDictionary.keyValue ("TextBrush", SolidColorBrush (Colors.White))
                    ResourceDictionary.keyValue ("AccentBrush", SolidColorBrush (Colors.DodgerBlue))
                ]
            )

            ResourceDictionary.themeDictionariesKeyValue (
                ThemeVariant.Light,
                ResourceDictionary.create [
                    ResourceDictionary.keyValue ("PrimaryBrush", SolidColorBrush (Colors.RoyalBlue))
                    ResourceDictionary.keyValue ("SecondaryBrush", SolidColorBrush (Colors.SteelBlue))
                    ResourceDictionary.keyValue ("BackgroundBrush", SolidColorBrush (Colors.White))
                    ResourceDictionary.keyValue ("SurfaceBrush", SolidColorBrush (Color.Parse ("#F3F3F3")))
                    ResourceDictionary.keyValue ("TextBrush", SolidColorBrush (Colors.Black))
                    ResourceDictionary.keyValue ("AccentBrush", SolidColorBrush (Colors.Blue))
                ]
            )

            ResourceDictionary.themeDictionariesKeyValue (
                ThemeVariant.Custom,
                ResourceDictionary.create [
                    ResourceDictionary.keyValue ("PrimaryBrush", SolidColorBrush (state.PrimaryBrushColor))
                    ResourceDictionary.keyValue ("SecondaryBrush", SolidColorBrush (state.SecondaryBrushColor))
                    ResourceDictionary.keyValue ("BackgroundBrush", SolidColorBrush (state.BackgroundBrushColor))
                    ResourceDictionary.keyValue ("SurfaceBrush", SolidColorBrush (state.SurfaceBrushColor))
                    ResourceDictionary.keyValue ("TextBrush", SolidColorBrush (state.TextBrushColor))
                    ResourceDictionary.keyValue ("AccentBrush", SolidColorBrush (state.AccentBrushColor))
                ]
            )
        ]
    
    let colorPickerView (color:Color) =
        ColorPicker.create []

    let view () =
        Component (fun ctx ->

            Grid.create [
                Grid.rowDefinitions "Auto,Auto"
                Grid.columnDefinitions "*,*"
                Grid.children [ ColorPicker.create [] ]
            ])
