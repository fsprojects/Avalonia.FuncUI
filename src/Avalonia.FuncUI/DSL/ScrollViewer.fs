namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module ScrollViewer  =
    open Avalonia
    open Avalonia.Controls.Primitives
    open Avalonia.Controls
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder

    let create (attrs: IAttr<ScrollViewer> list): IView<ScrollViewer> =
        ViewBuilder.Create<ScrollViewer>(attrs)

    type Control with

        /// <summary>
        /// Gets or sets a value that determines whether the <see cref="ScrollViewer"/> uses a
        /// bring-into-view scroll behavior when an item in the view gets focus.
        /// </summary>
        /// 
        /// <param name="value">
        /// true to use a behavior that brings focused items into view. false to use a behavior
        /// that focused items do not automatically scroll into view. The default is true.
        /// </param>
        static member bringIntoViewOnFocusChange<'t when 't :> Control>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(ScrollViewer.BringIntoViewOnFocusChangeProperty, value, ValueNone)

        /// <summary>
        /// Sets the vertical scrollbar visibility.
        /// </summary>
        static member verticalScrollBarVisibility<'t when 't :> Control>(value: ScrollBarVisibility) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<ScrollBarVisibility>(ScrollViewer.VerticalScrollBarVisibilityProperty, value, ValueNone)

        /// <summary>
        /// Gets or sets how scroll gesture reacts to the snap points along the vertical axis.
        /// </summary>
        static member verticalSnapPointsType<'t when 't :> Control>(value: SnapPointsType) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<SnapPointsType>(ScrollViewer.VerticalSnapPointsTypeProperty, value, ValueNone)

        /// <summary>
        /// Gets or sets how the existing snap points are vertically aligned versus the initial viewport.
        /// </summary>
        static member verticalSnapPointsAlignment<'t when 't :> Control>(value: SnapPointsAlignment) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<SnapPointsAlignment>(ScrollViewer.VerticalSnapPointsAlignmentProperty, value, ValueNone)

        /// <summary>
        /// Sets the horizontal scrollbar visibility.
        /// </summary>
        static member horizontalScrollBarVisibility<'t when 't :> Control>(value: ScrollBarVisibility) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<ScrollBarVisibility>(ScrollViewer.HorizontalScrollBarVisibilityProperty, value, ValueNone)

        /// <summary>
        /// Gets or sets how scroll gesture reacts to the snap points along the horizontal axis.
        /// </summary>
        static member horizontalSnapPointsType<'t when 't :> Control>(value: SnapPointsType) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<SnapPointsType>(ScrollViewer.HorizontalSnapPointsTypeProperty, value, ValueNone)

        /// <summary>
        /// Gets or sets how the existing snap points are horizontally aligned versus the initial viewport.
        /// </summary>
        static member horizontalSnapPointsAlignment<'t when 't :> Control>(value: SnapPointsAlignment) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<SnapPointsAlignment>(ScrollViewer.HorizontalSnapPointsAlignmentProperty, value, ValueNone)

        /// <summary>
        /// Gets a value that indicates whether scrollbars can hide itself when user is not interacting with it.
        /// </summary>
        static member allowAutoHide<'t when 't :> Control>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(ScrollViewer.AllowAutoHideProperty, value, ValueNone)

        /// <summary>
        ///  Gets or sets if scroll chaining is enabled. The default value is true.
        /// </summary>
        /// <remarks>
        ///  After a user hits a scroll limit on an element that has been nested within another scrollable element,
        /// you can specify whether that parent element should continue the scrolling operation begun in its child element.
        /// This is called scroll chaining.
        /// </remarks>
        static member isScrollChainingEnabled<'t when 't :> Control>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(ScrollViewer.IsScrollChainingEnabledProperty, value, ValueNone)

        /// <summary>
        /// Gets or sets whether scroll gestures should include inertia in their behavior and value.
        /// </summary>
        static member isScrollInertiaEnabled<'t when 't :> Control>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(ScrollViewer.IsScrollInertiaEnabledProperty, value, ValueNone)

    type ScrollViewer with

        /// <summary>
        /// Sets the current scroll offset.
        /// </summary>
        static member offset<'t when 't :> ScrollViewer>(value: Vector) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<Vector>(ScrollViewer.OffsetProperty, value, ValueNone)

        /// <summary>
        /// Occurs when changes are detected to the scroll position, extent, or viewport size.
        /// </summary>
        static member onScrollChanged<'t when 't :> ScrollViewer>(func: ScrollChangedEventArgs -> unit, ?subPatchOptions) =
            AttrBuilder<'t>.CreateSubscription<ScrollChangedEventArgs>(ScrollViewer.ScrollChangedEvent, func, ?subPatchOptions = subPatchOptions)
