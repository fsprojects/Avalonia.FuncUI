namespace Avalonia.FuncUI.DSL

open Avalonia
open Avalonia.Controls.Primitives
open Avalonia.Controls.Presenters

[<AutoOpen>]
module ScrollContentPresenter =
    open Avalonia.FuncUI.Builder
    open Avalonia.FuncUI.Types

    let create (attrs: IAttr<ScrollContentPresenter> list): IView<ScrollContentPresenter> =
        ViewBuilder.Create<ScrollContentPresenter>(attrs)

    type ScrollContentPresenter with
        static member canHorizontallyScroll<'t when 't :> ScrollContentPresenter>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(ScrollContentPresenter.CanHorizontallyScrollProperty, value, ValueNone)

        static member canVerticallyScroll<'t when 't :> ScrollContentPresenter>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(ScrollContentPresenter.CanVerticallyScrollProperty, value, ValueNone)

        static member offset<'t when 't :> ScrollContentPresenter>(value: Vector) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<Vector>(ScrollContentPresenter.OffsetProperty, value, ValueNone)

        static member offset<'t when 't :> ScrollContentPresenter>(x: double, y: double) : IAttr<'t> =
            Vector(x, y) |> ScrollContentPresenter.offset
        static member horizontalSnapPointsType<'t when 't :> ScrollContentPresenter>(value: SnapPointsType) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<SnapPointsType>(ScrollContentPresenter.HorizontalSnapPointsTypeProperty, value, ValueNone)

        static member verticalSnapPointsType<'t when 't :> ScrollContentPresenter>(value: SnapPointsType) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<SnapPointsType>(ScrollContentPresenter.VerticalSnapPointsTypeProperty, value, ValueNone)

        static member horizontalSnapPointsAlignment<'t when 't :> ScrollContentPresenter>(value: SnapPointsAlignment) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<SnapPointsAlignment>(ScrollContentPresenter.HorizontalSnapPointsAlignmentProperty, value, ValueNone)

        static member verticalSnapPointsAlignment<'t when 't :> ScrollContentPresenter>(value: SnapPointsAlignment) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<SnapPointsAlignment>(ScrollContentPresenter.VerticalSnapPointsAlignmentProperty, value, ValueNone)

        static member isScrollChainingEnabled<'t when 't :> ScrollContentPresenter>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(ScrollContentPresenter.IsScrollChainingEnabledProperty, value, ValueNone)
