namespace Avalonia.FuncUI.DSL

open Avalonia

[<AutoOpen>]
module TextOptions =
    open Avalonia.Media
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder

    type Visual with

        static member baselinePixelAlignment<'t when 't :> Visual>(alignnment: BaselinePixelAlignment) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<BaselinePixelAlignment>(
                name = nameof BaselinePixelAlignment,
                value = alignnment,
                getter = ValueSome TextOptions.GetBaselinePixelAlignment,
                setter = ValueSome TextOptions.SetBaselinePixelAlignment,
                comparer = ValueNone
            )

        static member textHintingMode<'t when 't :> Visual>(mode: TextHintingMode) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<TextHintingMode>(
                name = nameof TextHintingMode,
                value = mode,
                getter = ValueSome TextOptions.GetTextHintingMode,
                setter = ValueSome TextOptions.SetTextHintingMode,
                comparer = ValueNone
            )

        static member textOptions<'t when 't :> Visual>(options: TextOptions) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<TextOptions>(
                name = nameof TextOptions,
                value = options,
                getter = ValueSome TextOptions.GetTextOptions,
                setter = ValueSome TextOptions.SetTextOptions,
                comparer = ValueNone
            )

        static member textRenderingMode<'t when 't :> Visual>(mode: TextRenderingMode) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<TextRenderingMode>(
                name = nameof TextRenderingMode,
                value = mode,
                getter = ValueSome TextOptions.GetTextRenderingMode,
                setter = ValueSome TextOptions.SetTextRenderingMode,
                comparer = ValueNone
            )
