namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module StackPanel =
    open Avalonia.Controls
    open Avalonia.Interactivity
    open Avalonia.Layout
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder

    let create (attrs: IAttr<StackPanel> list): IView<StackPanel> =
        ViewBuilder.Create<StackPanel>(attrs)

    type StackPanel with
        /// <summary>
        /// Occurs when the measurements for horizontal snap points change.
        /// </summary>
        static member onHorizontalSnapPointsChanged<'t when 't :> StackPanel>(func: RoutedEventArgs -> unit, ?subPatchOptions) : IAttr<'t> =
            AttrBuilder<'t>.CreateSubscription<RoutedEventArgs>(StackPanel.HorizontalSnapPointsChangedEvent, func, ?subPatchOptions = subPatchOptions)

        /// <summary>
        /// Occurs when the measurements for vertical snap points change.
        /// </summary>
        static member onVerticalSnapPointsChanged<'t when 't :> StackPanel>(func: RoutedEventArgs -> unit, ?subPatchOptions) : IAttr<'t> =
            AttrBuilder<'t>.CreateSubscription<RoutedEventArgs>(StackPanel.VerticalSnapPointsChangedEvent, func, ?subPatchOptions = subPatchOptions)

        /// <summary>
        /// Gets or sets the size of the spacing to place between child controls.
        /// </summary>
        static member spacing<'t when 't :> StackPanel>(value: double) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<double>(StackPanel.SpacingProperty, value, ValueNone)

        /// <summary>
        /// Gets or sets the orientation in which child controls will be layed out.
        /// </summary>
        static member orientation<'t when 't :> StackPanel>(orientation: Orientation) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<Orientation>(StackPanel.OrientationProperty, orientation, ValueNone)

        static member areHorizontalSnapPointsRegular<'t when 't :> StackPanel>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(StackPanel.AreHorizontalSnapPointsRegularProperty, value, ValueNone)

        static member areVerticalSnapPointsRegular<'t when 't :> StackPanel>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(StackPanel.AreVerticalSnapPointsRegularProperty, value, ValueNone)
