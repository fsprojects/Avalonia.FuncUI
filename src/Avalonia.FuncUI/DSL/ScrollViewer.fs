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
        /// Sets the vertical scrollbar visibility.
        /// </summary>
        static member verticalScrollBarVisibility<'t when 't :> Control>(value: ScrollBarVisibility) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<ScrollBarVisibility>(ScrollViewer.VerticalScrollBarVisibilityProperty, value, ValueNone)

        /// <summary>
        /// Sets the horizontal scrollbar visibility.
        /// </summary>
        static member horizontalScrollBarVisibility<'t when 't :> Control>(value: ScrollBarVisibility) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<ScrollBarVisibility>(ScrollViewer.HorizontalScrollBarVisibilityProperty, value, ValueNone)

    type ScrollViewer with

        static member allowAutoHide<'t when 't :> ScrollViewer>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(ScrollViewer.AllowAutoHideProperty, value, ValueNone)

        /// <summary>
        /// Sets the extent of the scrollable content.
        /// </summary>
        static member extent<'t when 't :> ScrollViewer>(value: Size) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<Size>(ScrollViewer.ExtentProperty, value, ValueNone)

        static member isExpanded<'t when 't :> ScrollViewer>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(ScrollViewer.IsExpandedProperty, value, ValueNone)

        static member largeChange<'t when 't :> ScrollViewer>(value: Size) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<Size>(ScrollViewer.LargeChangeProperty, value, ValueNone)

        /// <summary>
        /// Sets the current scroll offset.
        /// </summary>
        static member offset<'t when 't :> ScrollViewer>(value: Vector) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<Vector>(ScrollViewer.OffsetProperty, value, ValueNone)

        static member onScrollChanged<'t when 't :> ScrollViewer>(func: ScrollChangedEventArgs -> unit, ?subPatchOptions) =
            AttrBuilder<'t>.CreateSubscription<ScrollChangedEventArgs>(ScrollViewer.ScrollChangedEvent, func, ?subPatchOptions = subPatchOptions)

        static member smallChange<'t when 't :> ScrollViewer>(value: Size) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<Size>(ScrollViewer.SmallChangeProperty, value, ValueNone)

        /// <summary>
        /// Sets the size of the viewport on the scrollable content.
        /// </summary>
        static member viewport<'t when 't :> ScrollViewer>(value: Size) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<Size>(ScrollViewer.ViewportProperty, value, ValueNone)