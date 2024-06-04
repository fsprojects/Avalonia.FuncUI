namespace Avalonia.FuncUI.DSL

open Avalonia.Controls
open Avalonia.Styling

[<AutoOpen>]
module ThemeVariantScope =
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder

    let create (attrs: IAttr<ThemeVariantScope> list): IView<ThemeVariantScope> =
        ViewBuilder.Create<ThemeVariantScope>(attrs)

    type ThemeVariantScope with
        static member requestedThemeVariant<'t when 't :> ThemeVariantScope>(value: ThemeVariant) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<ThemeVariant>(ThemeVariantScope.RequestedThemeVariantProperty, value, ValueNone)
