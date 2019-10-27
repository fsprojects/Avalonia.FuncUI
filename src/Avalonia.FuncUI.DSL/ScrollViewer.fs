namespace Avalonia.FuncUI.DSL

open Avalonia.Media
open Avalonia

[<AutoOpen>]
module ScrollViewer  =
    open Avalonia.Controls.Primitives
    open Avalonia.Controls
    open Avalonia.Layout
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
   
    let create (attrs: IAttr<ScrollViewer > list): IView<ScrollViewer> =
        ViewBuilder.Create<ScrollViewer >(attrs)

    type ScrollViewer  with            

        /// <summary>
        /// Gets or sets the vertical scrollbar visibility.
        /// </summary>
        static member verticalScrollBarVisibility<'t when 't :> ScrollViewer>(value: ScrollBarVisibility) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<ScrollBarVisibility>(ScrollViewer.VerticalScrollBarVisibilityProperty, value, ValueNone)

        /// <summary>
        /// Gets or sets the horizontal scrollbar visibility.
        /// </summary>
        static member horizontalScrollBarVisibility<'t when 't :> ScrollViewer>(value: ScrollBarVisibility) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<ScrollBarVisibility>(ScrollViewer.HorizontalScrollBarVisibilityProperty, value, ValueNone)

        /// <summary>
        /// Gets the extent of the scrollable content.
        /// </summary>
        static member extent<'t when 't :> ScrollViewer>(value: Size) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<Size>(ScrollViewer.ExtentProperty, value, ValueNone)

        /// <summary>
        /// Gets or sets the current scroll offset.
        /// </summary>
        static member offset<'t when 't :> ScrollViewer>(value: Vector) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<Vector>(ScrollViewer.OffsetProperty, value, ValueNone)

        /// <summary>
        /// Gets the size of the viewport on the scrollable content.
        /// </summary>
        static member viewport<'t when 't :> ScrollViewer>(value: Size) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<Size>(ScrollViewer.ViewportProperty, value, ValueNone)