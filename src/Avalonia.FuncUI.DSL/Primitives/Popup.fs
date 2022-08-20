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

    let create (attrs: IAttr<Popup> list): IView<Popup> =
        ViewBuilder.Create<Popup>(attrs)
    
    type Popup with
        
        static member child<'t when 't :> Popup>(value: IView option) : IAttr<'t> =
            AttrBuilder<'t>.CreateContentSingle(Popup.ChildProperty, value)
        
        static member child<'t when 't :> Popup>(value: IView) : IAttr<'t> =
            value |> Some |> Popup.child
            
        static member isOpen<'t when 't :> Popup>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(Popup.IsOpenProperty, value, ValueNone)
            
        static member isLightDismissEnabled<'t when 't :> Popup>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(Popup.IsLightDismissEnabledProperty, value, ValueNone)
            
        static member topmost<'t when 't :> Popup>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(Popup.TopmostProperty, value, ValueNone)

        static member placementAnchor<'t when 't :> Popup>(value: PopupAnchor) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<PopupAnchor>(Popup.PlacementAnchorProperty, value, ValueNone)
            
        static member placementConstraintAdjustment<'t when 't :> Popup>(value: PopupPositionerConstraintAdjustment) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<PopupPositionerConstraintAdjustment>(Popup.PlacementConstraintAdjustmentProperty, value, ValueNone)

        static member placementGravity<'t when 't :> Popup>(value: PopupGravity) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<PopupGravity>(Popup.PlacementGravityProperty, value, ValueNone)

        static member placementRect<'t when 't :> Popup>(value: Nullable<Rect>) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<Rect Nullable>(Popup.PlacementRectProperty, value, ValueNone)

        static member placementRect<'t when 't :> Popup>(value: Rect) : IAttr<'t> =
            value |> Nullable |> Popup.placementRect

        static member placementRect<'t when 't :> Popup>(value: Rect option) : IAttr<'t> =
            value |> Option.toNullable |> Popup.placementRect

        static member placementMode<'t when 't :> Popup>(value: PlacementMode) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<PlacementMode>(Popup.PlacementModeProperty, value, ValueNone)
            
        static member placementTarget<'t when 't :> Popup>(value: Control) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<Control>(Popup.PlacementTargetProperty, value, ValueNone)
            
        static member verticalOffset<'t when 't :> Popup>(value: double) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<double>(Popup.VerticalOffsetProperty, value, ValueNone)
            
        static member horizontalOffset<'t when 't :> Popup>(value: double) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<double>(Popup.HorizontalOffsetProperty, value, ValueNone)

        static member windowManagerAddShadowHint<'t when 't :> Popup>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(Popup.WindowManagerAddShadowHintProperty, value, ValueNone)