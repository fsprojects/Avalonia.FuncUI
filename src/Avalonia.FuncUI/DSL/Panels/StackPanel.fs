namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module StackPanel =
    open Avalonia.Controls
    open Avalonia.Layout
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder

    let create (attrs: Attr<StackPanel> list): View<StackPanel> =
        ViewBuilder.Create<StackPanel>(attrs)

    type StackPanel with
        /// <summary>
        /// Gets or sets the size of the spacing to place between child controls.
        /// </summary>
        static member spacing<'t when 't :> StackPanel>(value: double) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<double>(StackPanel.SpacingProperty, value, ValueNone)

        /// <summary>
        /// Gets or sets the orientation in which child controls will be layed out.
        /// </summary>
        static member orientation<'t when 't :> StackPanel>(orientation: Orientation) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<Orientation>(StackPanel.OrientationProperty, orientation, ValueNone)