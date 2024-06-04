namespace Avalonia.FuncUI.DSL

open Avalonia.Controls
open Avalonia.Interactivity
open Avalonia.Layout

[<AutoOpen>]
module VirtualizingStackPanel =
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder

    let create (attrs: IAttr<VirtualizingStackPanel> list): IView<VirtualizingStackPanel> =
        ViewBuilder.Create<VirtualizingStackPanel>(attrs)

    type VirtualizingStackPanel with
        /// <summary>
        /// Occurs when the measurements for horizontal snap points change.
        /// </summary>
        static member onHorizontalSnapPointsChanged<'t when 't :> VirtualizingStackPanel>(func: RoutedEventArgs -> unit, ?subPatchOptions) : IAttr<'t> =
            AttrBuilder<'t>.CreateSubscription<RoutedEventArgs>(VirtualizingStackPanel.HorizontalSnapPointsChangedEvent, func, ?subPatchOptions = subPatchOptions)

        /// <summary>
        /// Occurs when the measurements for vertical snap points change.
        /// </summary>
        static member onVerticalSnapPointsChanged<'t when 't :> VirtualizingStackPanel>(func: RoutedEventArgs -> unit, ?subPatchOptions) : IAttr<'t> =
            AttrBuilder<'t>.CreateSubscription<RoutedEventArgs>(VirtualizingStackPanel.VerticalSnapPointsChangedEvent, func, ?subPatchOptions = subPatchOptions)

        /// <summary>
        ///  Gets or sets the axis along which items are laid out.
        /// </summary>
        static member orientation<'t when 't :> VirtualizingStackPanel>(orientation: Orientation) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<Orientation>(VirtualizingStackPanel.OrientationProperty, orientation, ValueNone)

        /// <summary>
        /// Gets or sets whether the horizontal snap points for the ``Avalonia.Controls.VirtualizingStackPanel`` are equidistant from each other.
        /// </summary>
        static member areHorizontalSnapPointsRegular<'t when 't :> VirtualizingStackPanel>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(VirtualizingStackPanel.AreHorizontalSnapPointsRegularProperty, value, ValueNone)

        /// <summary>
        /// Gets or sets whether the vertical snap points for the ``Avalonia.Controls.VirtualizingStackPanel`` are equidistant from each other.
        /// </summary>
        static member areVerticalSnapPointsRegular<'t when 't :> VirtualizingStackPanel>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(VirtualizingStackPanel.AreVerticalSnapPointsRegularProperty, value, ValueNone)
