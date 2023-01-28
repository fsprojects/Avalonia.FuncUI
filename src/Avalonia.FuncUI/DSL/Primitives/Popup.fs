namespace Avalonia.FuncUI.DSL


[<AutoOpen>]
module Popup =
    open Avalonia
    open Avalonia.Controls
    open Avalonia.Controls.Primitives
    open Avalonia.Controls.Primitives.PopupPositioning
    open Avalonia.FuncUI.Builder
    open Avalonia.FuncUI.Types
    open System

    let create (attrs: Attr<Popup> list): View<Popup> =
        ViewBuilder.Create<Popup>(attrs)

    type Popup with

        static member child<'t when 't :> Popup>(value: IView option) : Attr<'t> =
            AttrBuilder<'t>.CreateContentSingle(Popup.ChildProperty, value)

        static member child<'t when 't :> Popup>(value: IView) : Attr<'t> =
            value |> Some |> Popup.child

        static member isOpen<'t when 't :> Popup>(value: bool) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(Popup.IsOpenProperty, value, ValueNone)

        static member isLightDismissEnabled<'t when 't :> Popup>(value: bool) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(Popup.IsLightDismissEnabledProperty, value, ValueNone)

        static member topmost<'t when 't :> Popup>(value: bool) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(Popup.TopmostProperty, value, ValueNone)

        static member placementAnchor<'t when 't :> Popup>(value: PopupAnchor) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<PopupAnchor>(Popup.PlacementAnchorProperty, value, ValueNone)

        static member placementConstraintAdjustment<'t when 't :> Popup>(value: PopupPositionerConstraintAdjustment) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<PopupPositionerConstraintAdjustment>(Popup.PlacementConstraintAdjustmentProperty, value, ValueNone)

        static member placementGravity<'t when 't :> Popup>(value: PopupGravity) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<PopupGravity>(Popup.PlacementGravityProperty, value, ValueNone)

        static member placementRect<'t when 't :> Popup>(value: Nullable<Rect>) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<Rect Nullable>(Popup.PlacementRectProperty, value, ValueNone)

        static member placementRect<'t when 't :> Popup>(value: Rect) : Attr<'t> =
            value |> Nullable |> Popup.placementRect

        static member placementRect<'t when 't :> Popup>(value: Rect option) : Attr<'t> =
            value |> Option.toNullable |> Popup.placementRect

        static member placementMode<'t when 't :> Popup>(value: PlacementMode) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<PlacementMode>(Popup.PlacementModeProperty, value, ValueNone)

        static member placementTarget<'t when 't :> Popup>(value: Control) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<Control>(Popup.PlacementTargetProperty, value, ValueNone)

        static member verticalOffset<'t when 't :> Popup>(value: double) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<double>(Popup.VerticalOffsetProperty, value, ValueNone)

        static member horizontalOffset<'t when 't :> Popup>(value: double) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<double>(Popup.HorizontalOffsetProperty, value, ValueNone)

        static member windowManagerAddShadowHint<'t when 't :> Popup>(value: bool) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(Popup.WindowManagerAddShadowHintProperty, value, ValueNone)