namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module ScrollBar =
    open Avalonia.Layout
    open Avalonia.Controls.Primitives
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder

    let create (attrs: Attr<ScrollBar> list): View<ScrollBar> =
        ViewBuilder.Create<ScrollBar>(attrs)

    type ScrollBar with

        static member allowAutoHide<'t when 't :> ScrollBar>(value: bool) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(ScrollBar.AllowAutoHideProperty, value, ValueNone)

        static member isExpanded<'t when 't :> ScrollBar>(value: bool) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(ScrollBar.IsExpandedProperty, value, ValueNone)

        /// <summary>
        /// Sets the amount of the scrollable content that is currently visible.
        /// </summary>
        static member viewportSize<'t when 't :> ScrollBar>(value: double) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<double>(ScrollBar.ViewportSizeProperty, value, ValueNone)

        /// <summary>
        /// Sets a value that indicates whether the scrollbar should hide itself when it is not needed.
        /// </summary>
        static member visibility<'t when 't :> ScrollBar>(visibility: ScrollBarVisibility) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<ScrollBarVisibility>(ScrollBar.VisibilityProperty, visibility, ValueNone)

        /// <summary>
        /// Sets the orientation of the scrollbar.
        /// </summary>
        static member orientation<'t when 't :> ScrollBar>(orientation: Orientation) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<Orientation>(ScrollBar.OrientationProperty, orientation, ValueNone)