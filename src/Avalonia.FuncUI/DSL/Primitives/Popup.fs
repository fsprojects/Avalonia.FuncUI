namespace Avalonia.FuncUI.DSL


[<AutoOpen>]
module Popup =
    open System
    open Avalonia
    open Avalonia.Controls
    open Avalonia.Controls.Primitives
    open Avalonia.Controls.Primitives.PopupPositioning
    open Avalonia.Input
    open Avalonia.FuncUI.Builder
    open Avalonia.FuncUI.Types

    let create (attrs: IAttr<Popup> list): IView<Popup> =
        ViewBuilder.Create<Popup>(attrs)

    type Popup with

        static member onClosed<'t when 't :> Popup>(func: 't -> unit, ?subPatchOptions) : IAttr<'t> =
            let name = nameof Unchecked.defaultof<'t>.Closed
            let factory: SubscriptionFactory<'t> =
                fun (control, func, token) ->
                    let control = control :?> 't
                    let handler = EventHandler<EventArgs>(fun s e -> func(s :?> 't))
                    let event = control.Closed

                    event.AddHandler(handler)
                    token.Register(fun _ -> event.RemoveHandler(handler)) |> ignore

            AttrBuilder<'t>.CreateSubscription<'t>(name, factory, func, ?subPatchOptions = subPatchOptions)

        static member onOpened<'t when 't :> Popup>(func: 't -> unit, ?subPatchOptions) : IAttr<'t> =
            let name = nameof Unchecked.defaultof<'t>.Opened
            let factory: SubscriptionFactory<'t> =
                fun (control, func, token) ->
                    let control = control :?> 't
                    let handler = EventHandler(fun s e -> func(s :?> 't))
                    let event = control.Opened

                    event.AddHandler(handler)
                    token.Register(fun _ -> event.RemoveHandler(handler)) |> ignore

            AttrBuilder<'t>.CreateSubscription<'t>(name, factory, func, ?subPatchOptions = subPatchOptions)

        static member child<'t when 't :> Popup>(value: IView option) : IAttr<'t> =
            AttrBuilder<'t>.CreateContentSingle(Popup.ChildProperty, value)

        static member child<'t when 't :> Popup>(value: IView) : IAttr<'t> =
            value |> Some |> Popup.child

        static member dependencyResolver<'t when 't :> Popup>(value: IAvaloniaDependencyResolver) : IAttr<'t> =
            let name = nameof Unchecked.defaultof<'t>.DependencyResolver
            let getter: 't -> IAvaloniaDependencyResolver = (fun control -> control.DependencyResolver)
            let setter: 't * IAvaloniaDependencyResolver -> unit = (fun (control, value) -> control.DependencyResolver <- value)

            AttrBuilder<'t>.CreateProperty<IAvaloniaDependencyResolver>(name, value, ValueSome getter, ValueSome setter, ValueNone)

        static member inheritsTransform<'t when 't :> Popup>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(Popup.InheritsTransformProperty, value, ValueNone)

        static member isOpen<'t when 't :> Popup>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(Popup.IsOpenProperty, value, ValueNone)

        static member isLightDismissEnabled<'t when 't :> Popup>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(Popup.IsLightDismissEnabledProperty, value, ValueNone)

        static member topmost<'t when 't :> Popup>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(Popup.TopmostProperty, value, ValueNone)

        static member placement<'t when 't :> Popup>(value: PlacementMode) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<PlacementMode>(Popup.PlacementProperty, value, ValueNone)

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

        [<Obsolete "use 'placement' instead">]
        static member placementMode<'t when 't :> Popup>(value: PlacementMode) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<PlacementMode>(Popup.PlacementModeProperty, value, ValueNone)

        static member placementTarget<'t when 't :> Popup>(value: Control) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<Control>(Popup.PlacementTargetProperty, value, ValueNone)

        static member overlayDismissEventPassThrough<'t when 't :> Popup>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(Popup.OverlayDismissEventPassThroughProperty, value, ValueNone)

        static member overlayInputPassThroughElement<'t when 't :> Popup>(value: IInputElement) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<IInputElement>(Popup.OverlayInputPassThroughElementProperty, value, ValueNone)

        static member verticalOffset<'t when 't :> Popup>(value: double) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<double>(Popup.VerticalOffsetProperty, value, ValueNone)

        static member horizontalOffset<'t when 't :> Popup>(value: double) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<double>(Popup.HorizontalOffsetProperty, value, ValueNone)

        static member windowManagerAddShadowHint<'t when 't :> Popup>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(Popup.WindowManagerAddShadowHintProperty, value, ValueNone)

        static member shouldUseOverlayLayer<'t when 't :> Popup>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(Popup.ShouldUseOverlayLayerProperty, value, ValueNone)

        static member isUsingOverlayLayer<'t when 't :> Popup>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(Popup.IsUsingOverlayLayerProperty, value, ValueNone)
