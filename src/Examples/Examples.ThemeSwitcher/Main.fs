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
    open Avalonia.Layout
    open Avalonia.Controls.Presenters
    open Avalonia.Markup.Xaml.MarkupExtensions
    open Avalonia.Rendering


    module ThemeVariant =
        let Custom = ThemeVariant("Custom", ThemeVariant.Light)

    type ColorSet =
        { Theme: ThemeVariant
          PrimaryBrush: SolidColorBrush
          SecondaryBrush: SolidColorBrush
          BackgroundBrush: SolidColorBrush
          SurfaceBrush: SolidColorBrush
          TextBrush: SolidColorBrush
          AccentBrush: SolidColorBrush }

    module ColorSet =
        let dark =
            { Theme = ThemeVariant.Dark
              PrimaryBrush = SolidColorBrush Colors.CornflowerBlue
              SecondaryBrush = SolidColorBrush Colors.LightSteelBlue
              BackgroundBrush = SolidColorBrush(Color.Parse("#1E1E1E"))
              SurfaceBrush = SolidColorBrush(Color.Parse("#252526"))
              TextBrush = SolidColorBrush Colors.White
              AccentBrush = SolidColorBrush Colors.DodgerBlue }

        let light =
            { Theme = ThemeVariant.Light
              PrimaryBrush = SolidColorBrush Colors.RoyalBlue
              SecondaryBrush = SolidColorBrush Colors.SteelBlue
              BackgroundBrush = SolidColorBrush Colors.White
              SurfaceBrush = SolidColorBrush(Color.Parse("#F3F3F3"))
              TextBrush = SolidColorBrush Colors.Black
              AccentBrush = SolidColorBrush Colors.Blue }

        let customInit =
            { Theme = ThemeVariant.Custom
              PrimaryBrush = SolidColorBrush Colors.MediumPurple
              SecondaryBrush = SolidColorBrush Colors.Plum
              BackgroundBrush = SolidColorBrush(Color.Parse("#FFF8DC"))
              SurfaceBrush = SolidColorBrush(Color.Parse("#FAF0E6"))
              TextBrush = SolidColorBrush Colors.DarkSlateGray
              AccentBrush = SolidColorBrush Colors.MediumOrchid }

        let colorsEqual (a: ColorSet) (b: ColorSet) =
            a.PrimaryBrush.Color = b.PrimaryBrush.Color
            && a.SecondaryBrush.Color = b.SecondaryBrush.Color
            && a.BackgroundBrush.Color = b.BackgroundBrush.Color
            && a.SurfaceBrush.Color = b.SurfaceBrush.Color
            && a.TextBrush.Color = b.TextBrush.Color
            && a.AccentBrush.Color = b.AccentBrush.Color


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
            ResourceDictionary.keyValue ("DefaultPadding", Thickness(12.0))
            ResourceDictionary.keyValue ("ButtonPadding", Thickness(8.0, 4.0))

            // Theme variant-specific resources
            for colorSet in [ ColorSet.dark; ColorSet.light; state.CustomColors ] do
                ResourceDictionary.themeDictionariesKeyValue (
                    colorSet.Theme,
                    ResourceDictionary.create [
                        ResourceDictionary.keyValue ("PrimaryBrush", colorSet.PrimaryBrush)
                        ResourceDictionary.keyValue ("SecondaryBrush", colorSet.SecondaryBrush)
                        ResourceDictionary.keyValue ("BackgroundBrush", colorSet.BackgroundBrush)
                        ResourceDictionary.keyValue ("SurfaceBrush", colorSet.SurfaceBrush)
                        ResourceDictionary.keyValue ("TextBrush", colorSet.TextBrush)
                        ResourceDictionary.keyValue ("AccentBrush", colorSet.AccentBrush)
                    ]
                )
        ]

    let inline addRangeProp<'x, 'v>
        (acccsessor: 'x -> System.Collections.Generic.ICollection<'v>)
        (vs: 'v seq)
        (x: 'x)
        =
        let collection = acccsessor x
        Seq.iter collection.Add vs
        x

    let themeVariantScopeView (themeVariant: ThemeVariant) =
        ThemeVariantScope.create [
            ThemeVariantScope.requestedThemeVariant themeVariant
            ThemeVariantScope.child (
                DockPanel.create [
                    DockPanel.init
                        _.Styles.AddRange(
                            [ Style _.Is<Layoutable>()
                              |> addRangeProp _.Setters [
                                  Setter(Layoutable.MarginProperty, DynamicResourceExtension "DefaultMargin")
                              ]
                              Style _.Is<Panel>()
                              |> addRangeProp _.Setters [
                                  Setter(Panel.BackgroundProperty, DynamicResourceExtension "SurfaceBrush")
                              ]
                              Style _.Is<TextBlock>()
                              |> addRangeProp _.Setters [
                                  Setter(TextBlock.ForegroundProperty, DynamicResourceExtension "TextBrush")
                              ]
                              Style _.Is<Button>()
                              |> addRangeProp _.Setters [
                                  Setter(Button.BackgroundProperty, DynamicResourceExtension "PrimaryBrush")
                                  Setter(Button.PaddingProperty, DynamicResourceExtension "ButtonPadding")
                              ]
                              |> addRangeProp _.Children [
                                  Style _.Nesting().Class("secondary")
                                  |> addRangeProp _.Setters [
                                      Setter(Button.BackgroundProperty, DynamicResourceExtension "SecondaryBrush")
                                  ]
                                  Style // "^:pointerover /template/ ContentPresenter#PART_ContentPresenter"
                                      _.Nesting()
                                          .Class(":pointerover")
                                          .Template()
                                          .OfType<ContentPresenter>()
                                          .Name("PART_ContentPresenter")
                                  |> addRangeProp _.Setters [
                                      Setter(Button.BackgroundProperty, DynamicResourceExtension "AccentBrush")
                                  ]
                              ] ]
                        )
                    DockPanel.lastChildFill false
                    DockPanel.children [
                        TextBlock.create [
                            TextBlock.dock Dock.Top
                            TextBlock.text $"ThemeVariantScope: {themeVariant}"
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

    let (|PropertyChanged|_|) (prop: AvaloniaProperty<'value>) (e: AvaloniaPropertyChangedEventArgs) =
        if e.Property = prop then
            let struct (oldValue, newValue) = e.GetOldAndNewValue<'value>()

            if oldValue <> newValue then
                Some(oldValue, newValue)
            else
                None
        else
            None

    let colorSetsView (state: IWritable<State>) =
        Component.create (
            "colorSetsView",
            fun ctx ->
                let state = ctx.usePassedLazy (fun _ -> state)

                let setColorWith updateFn newValue =
                    { state.Current with
                        CustomColors = updateFn state.Current.CustomColors newValue }
                    |> state.Set

                let defaultMargin = ctx.useStateLazy (fun () -> Thickness 8.0)

                ctx.attrs [
                    Component.onResourceObservable (
                        "DefaultMargin",
                        function
                        | Some(:? Thickness as v) -> defaultMargin.Set v
                        | _ -> ()
                    )
                ]

                let colors = state.Current.CustomColors

                let colorSetViewParams =
                    [ nameof colors.PrimaryBrush, colors.PrimaryBrush, (fun s v -> { s with PrimaryBrush = v })
                      nameof colors.SecondaryBrush, colors.SecondaryBrush, (fun s v -> { s with SecondaryBrush = v })
                      nameof colors.BackgroundBrush, colors.BackgroundBrush, (fun s v -> { s with BackgroundBrush = v })
                      nameof colors.SurfaceBrush, colors.SurfaceBrush, (fun s v -> { s with SurfaceBrush = v })
                      nameof colors.TextBrush, colors.TextBrush, (fun s v -> { s with TextBrush = v })
                      nameof colors.AccentBrush, colors.AccentBrush, (fun s v -> { s with AccentBrush = v }) ]

                let colorSetDefinitions =
                    List.replicate colorSetViewParams.Length "Auto" |> String.concat ","

                Grid.create [
                    Grid.columnDefinitions "Auto,*"
                    Grid.rowDefinitions $"Auto,{colorSetDefinitions},Auto"
                    Grid.margin defaultMargin.Current
                    Grid.children [
                        TextBlock.create [
                            TextBlock.column 0
                            TextBlock.columnSpan 2
                            TextBlock.row 0
                            TextBlock.classes [ "large" ]
                            TextBlock.text $"Color Sets View: {colors.Theme}"
                            TextBlock.margin defaultMargin.Current
                        ]

                        for idx, (label, brush, updateFn) in List.indexed colorSetViewParams do
                            TextBlock.create [
                                TextBlock.column 0
                                TextBlock.row (idx + 1)
                                TextBlock.margin defaultMargin.Current
                                TextBlock.classes [ "small" ]
                                TextBlock.text $"{label}:"
                            ]

                            ColorPicker.create [
                                ColorPicker.column 1
                                ColorPicker.row (idx + 1)
                                ColorPicker.margin defaultMargin.Current
                                ColorPicker.horizontalAlignment HorizontalAlignment.Right
                                ColorPicker.color brush.Color
                                ColorPicker.onPropertyChanged (function
                                    | PropertyChanged ColorPicker.ColorProperty (_, newColor) ->
                                        SolidColorBrush newColor |> setColorWith updateFn
                                    | _ -> ())
                            ]

                        Button.create [
                            Button.column 0
                            Button.columnSpan 2
                            Button.row (1 + colorSetViewParams.Length)
                            Button.margin defaultMargin.Current
                            Button.horizontalAlignment HorizontalAlignment.Right
                            Button.content "Reset to Default Colors"
                            Button.onClick (fun _ ->
                                if not (ColorSet.colorsEqual state.Current.CustomColors ColorSet.customInit) then
                                    { state.Current with
                                        CustomColors = ColorSet.customInit }
                                    |> state.Set)
                        ]
                    ]
                ]
        )

#if DEBUG
    let debugOverlaysView () =
        Component.create (
            "debugOverlaysView",
            fun ctx ->
                let rendererDebugOverlays = ctx.useState (RendererDebugOverlays.None, true)

                let mapFlag (flag: RendererDebugOverlays) =
                    rendererDebugOverlays
                    |> State.map _.HasFlag(flag) (fun (flags, enabled) ->
                        if enabled then flags ||| flag else flags &&& ~~~flag)

                let showFps = mapFlag RendererDebugOverlays.Fps

                let showDirtyRects = mapFlag RendererDebugOverlays.DirtyRects

                let showLayoutTimeGraph = mapFlag RendererDebugOverlays.LayoutTimeGraph

                let showRenderTimeGraph = mapFlag RendererDebugOverlays.RenderTimeGraph

                ctx.useEffect (
                    (fun () ->
                        TopLevel.GetTopLevel(ctx.control).RendererDiagnostics.DebugOverlays <-
                            rendererDebugOverlays.Current),
                    [ EffectTrigger.AfterChange rendererDebugOverlays ]
                )

                let handleIsCheckedChanged (state: IWritable<bool>) (e: Interactivity.RoutedEventArgs) =
                    match e.Source with
                    | :? CheckBox as cb ->
                        if cb.IsChecked.HasValue && state.Current <> cb.IsChecked.Value then
                            state.Set cb.IsChecked.Value
                    | _ -> ()

                let settingViewParams =
                    [ "Show FPS", showFps
                      "Show Dirty Rects", showDirtyRects
                      "Show Layout Time Graph", showLayoutTimeGraph
                      "Show Render Time Graph", showRenderTimeGraph ]

                StackPanel.create [
                    StackPanel.orientation Orientation.Vertical
                    StackPanel.children [
                        for text, state in settingViewParams do
                            CheckBox.create [
                                CheckBox.isChecked state.Current
                                CheckBox.onIsCheckedChanged (handleIsCheckedChanged state, OnChangeOf state.Current)
                                CheckBox.content (TextBlock.create [ TextBlock.fontSize 8; TextBlock.text text ])
                            ]
                    ]
                ]
        )
#endif

    let view () =
        Component(fun ctx ->
            let state = ctx.useStateLazy init

            Grid.create [
                Grid.resources (createThemeResources state.Current)
                Grid.init
                    _.Styles.AddRange(
                        [ Style _.Is<TextBlock>()
                          |> addRangeProp _.Setters [
                              Setter(TextBlock.FontSizeProperty, DynamicResourceExtension "MediumFontSize")
                              Setter(TextBlock.PaddingProperty, DynamicResourceExtension "ButtonPadding")
                          ]
                          |> addRangeProp _.Children [
                              Style _.Nesting().Class("large")
                              |> addRangeProp _.Setters [
                                  Setter(TextBlock.FontSizeProperty, DynamicResourceExtension "LargeFontSize")
                              ]
                              Style _.Nesting().Class("small")
                              |> addRangeProp _.Setters [
                                  Setter(TextBlock.FontSizeProperty, DynamicResourceExtension "SmallFontSize")
                              ]
                          ] ]
                    )
                Grid.rowDefinitions "Auto,Auto,Auto"
                Grid.columnDefinitions "*,Auto"
                Grid.children [
                    colorSetsView state
                    |> View.withAttrs [ Grid.row 0; Grid.column 1; Grid.rowSpan 3 ]
                    themeVariantScopeView ThemeVariant.Dark
                    |> View.withAttrs [ Grid.row 0; Grid.column 0 ]
                    themeVariantScopeView ThemeVariant.Light
                    |> View.withAttrs [ Grid.row 1; Grid.column 0 ]
                    themeVariantScopeView ThemeVariant.Custom
                    |> View.withAttrs [ Grid.row 2; Grid.column 0 ]
#if DEBUG
                    debugOverlaysView ()
                    |> View.withAttrs [
                        Grid.row 3
                        Grid.column 1
                        StackPanel.margin (Thickness 8.0)
                        StackPanel.horizontalAlignment HorizontalAlignment.Right
                        StackPanel.verticalAlignment VerticalAlignment.Bottom
                    ]
#endif
                ]
            ])
