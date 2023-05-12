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

    type RenderOptions with
        end