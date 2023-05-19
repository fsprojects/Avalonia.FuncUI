namespace Avalonia.FuncUI.DSL

open Avalonia

[<AutoOpen>]
module RenderOptions =
    open Avalonia.Media
    open Avalonia.Media.Imaging
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder

    type Visual with

        static member bitmapInterpolationMode<'t when 't :> Visual>(mode: BitmapInterpolationMode) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<BitmapInterpolationMode>(
                name = nameof BitmapInterpolationMode,
                value = mode,
                getter = ValueSome RenderOptions.GetBitmapInterpolationMode,
                setter = ValueSome RenderOptions.SetBitmapInterpolationMode,
                comparer = ValueNone
            )

        static member edgeMode<'t when 't :> Visual>(mode: EdgeMode) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<EdgeMode>(
                name = nameof EdgeMode,
                value = mode,
                getter = ValueSome RenderOptions.GetEdgeMode,
                setter = ValueSome RenderOptions.SetEdgeMode,
                comparer = ValueNone
            )

        static member bitmapBlendingMode<'t when 't :> Visual>(mode: BitmapBlendingMode) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<BitmapBlendingMode>(
                name = nameof BitmapBlendingMode,
                value = mode,
                getter = ValueSome RenderOptions.GetBitmapBlendingMode,
                setter = ValueSome RenderOptions.SetBitmapBlendingMode,
                comparer = ValueNone
            )

        static member textRenderingMode<'t when 't :> Visual>(mode: TextRenderingMode) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<TextRenderingMode>(
                name = nameof TextRenderingMode,
                value = mode,
                getter = ValueSome RenderOptions.GetTextRenderingMode,
                setter = ValueSome RenderOptions.SetTextRenderingMode,
                comparer = ValueNone
            )


    type RenderOptions with
        end