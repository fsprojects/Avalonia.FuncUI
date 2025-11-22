namespace Examples.ThemeSwitcher.DSL

open Avalonia.Controls
open Avalonia.FuncUI.Types

[<AutoOpen>]
module ColorViewDSL =
    open Avalonia.FuncUI.Builder
    open Avalonia.Media

    type ColorView with

        static member color<'t when 't :> ColorView>(color: Color) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<Color>(ColorView.ColorProperty, color, ValueNone)

        static member colorModel<'t when 't :> ColorView>(colorModel: ColorModel) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<ColorModel>(ColorView.ColorModelProperty, colorModel, ValueNone)

        static member colorSpectrumComponents<'t when 't :> ColorView>
            (components: ColorSpectrumComponents)
            : IAttr<'t> =
            AttrBuilder<'t>
                .CreateProperty<ColorSpectrumComponents>(
                    ColorView.ColorSpectrumComponentsProperty,
                    components,
                    ValueNone
                )

        static member colorSpectrumShape<'t when 't :> ColorView>(shape: ColorSpectrumShape) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<ColorSpectrumShape>(ColorView.ColorSpectrumShapeProperty, shape, ValueNone)

        static member hexInputAlphaPosition<'t when 't :> ColorView>(position: AlphaComponentPosition) : IAttr<'t> =
            AttrBuilder<'t>
                .CreateProperty<AlphaComponentPosition>(ColorView.HexInputAlphaPositionProperty, position, ValueNone)

        static member hsvColor<'t when 't :> ColorView>(hsvColor: HsvColor) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<HsvColor>(ColorView.HsvColorProperty, hsvColor, ValueNone)

        static member isAccentColorsVisible<'t when 't :> ColorView>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(ColorView.IsAccentColorsVisibleProperty, value, ValueNone)

        static member isAlphaEnabled<'t when 't :> ColorView>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(ColorView.IsAlphaEnabledProperty, value, ValueNone)

        static member isAlphaVisible<'t when 't :> ColorView>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(ColorView.IsAlphaVisibleProperty, value, ValueNone)

        static member isColorComponentsVisible<'t when 't :> ColorView>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(ColorView.IsColorComponentsVisibleProperty, value, ValueNone)

        static member isColorModelVisible<'t when 't :> ColorView>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(ColorView.IsColorModelVisibleProperty, value, ValueNone)

        static member isColorPaletteVisible<'t when 't :> ColorView>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(ColorView.IsColorPaletteVisibleProperty, value, ValueNone)

        static member isColorPreviewVisible<'t when 't :> ColorView>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(ColorView.IsColorPreviewVisibleProperty, value, ValueNone)

        static member isColorSpectrumVisible<'t when 't :> ColorView>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(ColorView.IsColorSpectrumVisibleProperty, value, ValueNone)

        static member isColorSpectrumSliderVisible<'t when 't :> ColorView>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(ColorView.IsColorSpectrumSliderVisibleProperty, value, ValueNone)

        static member isComponentSliderVisible<'t when 't :> ColorView>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(ColorView.IsComponentSliderVisibleProperty, value, ValueNone)

        static member isComponentTextInputVisible<'t when 't :> ColorView>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(ColorView.IsComponentTextInputVisibleProperty, value, ValueNone)

        static member isHexInputVisible<'t when 't :> ColorView>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(ColorView.IsHexInputVisibleProperty, value, ValueNone)

        static member maxHue<'t when 't :> ColorView>(value: int) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<int>(ColorView.MaxHueProperty, value, ValueNone)

        static member maxSaturation<'t when 't :> ColorView>(value: int) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<int>(ColorView.MaxSaturationProperty, value, ValueNone)

        static member maxValue<'t when 't :> ColorView>(value: int) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<int>(ColorView.MaxValueProperty, value, ValueNone)

        static member minHue<'t when 't :> ColorView>(value: int) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<int>(ColorView.MinHueProperty, value, ValueNone)

        static member minSaturation<'t when 't :> ColorView>(value: int) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<int>(ColorView.MinSaturationProperty, value, ValueNone)

        static member minValue<'t when 't :> ColorView>(value: int) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<int>(ColorView.MinValueProperty, value, ValueNone)

        static member paletteColors<'t when 't :> ColorView>(value: seq<Color>) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<seq<Color>>(ColorView.PaletteColorsProperty, value, ValueNone)

        static member paletteColumnCount<'t when 't :> ColorView>(value: int) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<int>(ColorView.PaletteColumnCountProperty, value, ValueNone)

        static member palette<'t when 't :> ColorView>(value: IColorPalette) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<IColorPalette>(ColorView.PaletteProperty, value, ValueNone)

        static member selectedIndex<'t when 't :> ColorView>(value: int) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<int>(ColorView.SelectedIndexProperty, value, ValueNone)
