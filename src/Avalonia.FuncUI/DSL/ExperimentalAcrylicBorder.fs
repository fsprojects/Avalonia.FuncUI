namespace Avalonia.FuncUI.DSL

open Avalonia
open Avalonia.Controls
open Avalonia.Media

[<AutoOpen>]
module ExperimentalAcrylicBorder =
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder

    let create (attrs: IAttr<ExperimentalAcrylicBorder> list): IView<ExperimentalAcrylicBorder> =
        ViewBuilder.Create<ExperimentalAcrylicBorder>(attrs)

    type ExperimentalAcrylicBorder with
        static member cornerRadius<'t when 't :> ExperimentalAcrylicBorder>(value: CornerRadius) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<CornerRadius>(ExperimentalAcrylicBorder.CornerRadiusProperty, value, ValueNone)

        static member material<'t when 't :> ExperimentalAcrylicBorder>(value: ExperimentalAcrylicMaterial) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<ExperimentalAcrylicMaterial>(ExperimentalAcrylicBorder.MaterialProperty, value, ValueNone)
