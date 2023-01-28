namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module ToolTip =
    open Avalonia.Controls
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder

    let create (attrs: Attr<ToolTip> list): View<ToolTip> =
        ViewBuilder.Create<ToolTip>(attrs)

    type Control with

        /// <summary>
        /// A value indicating whether the tool tip is visible.
        /// </summary>
        static member isOpen<'t when 't :> Control>(value: bool) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(ToolTip.IsOpenProperty, value, ValueNone)

        /// <summary>
        /// The content to be displayed in the control's tooltip.
        /// </summary>
        static member tip<'t when 't :> Control>(value: string) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<string>(ToolTip.TipProperty, value, ValueNone)

        /// <summary>
        /// The content to be displayed in the control's tooltip.
        /// </summary>
        static member tip<'t when 't :> Control>(value: obj) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<obj>(ToolTip.TipProperty, value, ValueNone)

        /// <summary>
        /// The content to be displayed in the control's tooltip.
        /// </summary>
        static member tip<'t when 't :> Control>(value: IView option) : Attr<'t> =
            AttrBuilder<'t>.CreateContentSingle(ToolTip.TipProperty, value)

        /// <summary>
        /// The content to be displayed in the control's tooltip.
        /// </summary>
        static member tip<'t when 't :> Control>(value: IView) : Attr<'t> =
            value |> Some |> ToolTip.tip

        /// <summary>
        /// A value indicating how the tool tip is positioned.
        /// </summary>
        static member placement<'t when 't :> Control>(value: PlacementMode) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<PlacementMode>(ToolTip.PlacementProperty, value, ValueNone)

        /// <summary>
        /// A value indicating how the tool tip is positioned.
        /// </summary>
        static member horizontalOffset<'t when 't :> Control>(value: float) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<float>(ToolTip.HorizontalOffsetProperty, value, ValueNone)

        /// <summary>
        /// A value indicating how the tool tip is positioned.
        /// </summary>
        static member verticalOffset<'t when 't :> Control>(value: float) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<float>(ToolTip.VerticalOffsetProperty, value, ValueNone)

        /// <summary>
        /// A value indicating the time, in milliseconds, before a tool tip opens.
        /// </summary>
        static member showDelay<'t when 't :> Control>(value: int) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<int>(ToolTip.ShowDelayProperty, value, ValueNone)
