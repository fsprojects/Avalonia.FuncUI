namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module SplitView =
    open Avalonia.Controls
    open Avalonia.Media
    open Avalonia.Media.Immutable
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder

    let create (attrs: Attr<SplitView> list): View<SplitView> =
        ViewBuilder.Create<SplitView>(attrs)

    type SplitView with

        static member content<'t when 't :> SplitView>(value: IView voption) : Attr<'t> =
            AttrBuilder<'t>.CreateContentSingle(SplitView.ContentProperty, value)

        static member content<'t when 't :> SplitView>(value: IView) : Attr<'t> =
            value |> ValueSome |> SplitView.content

        static member compactPaneLengthProperty<'t when 't :> SplitView>(value: float) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<float>(SplitView.CompactPaneLengthProperty, value, ValueNone)

        static member displayMode<'t when 't :> SplitView>(value: SplitViewDisplayMode) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<SplitViewDisplayMode>(SplitView.DisplayModeProperty, value, ValueNone)

        static member isPaneOpen<'t when 't :> SplitView>(value: bool) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(SplitView.IsPaneOpenProperty, value, ValueNone)

        static member openPaneLength<'t when 't :> SplitView>(value: float) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<float>(SplitView.OpenPaneLengthProperty, value, ValueNone)

        static member paneBackground<'t when 't :> SplitView>(value: IBrush) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<IBrush>(SplitView.PaneBackgroundProperty, value, ValueNone)

        static member paneBackground<'t when 't :> SplitView>(color: string) : Attr<'t> =
            color |> Color.Parse |> ImmutableSolidColorBrush |> SplitView.paneBackground

        static member paneBackground<'t when 't :> SplitView>(value: Color) : Attr<'t> =
            value |> ImmutableSolidColorBrush |> SplitView.paneBackground

        static member panePlacement<'t when 't :> SplitView>(value: SplitViewPanePlacement) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<SplitViewPanePlacement>(SplitView.PanePlacementProperty, value, ValueNone)

        static member pane<'t when 't :> SplitView>(value: IView voption) : Attr<'t> =
            AttrBuilder<'t>.CreateContentSingle(SplitView.PaneProperty, value)

        static member pane<'t when 't :> SplitView>(value: IView) : Attr<'t> =
            value |> ValueSome |> SplitView.pane

        static member useLightDismissOverlayMode<'t when 't :> SplitView>(value: bool) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(SplitView.UseLightDismissOverlayModeProperty, value, ValueNone)

        static member templateSettings<'t when 't :> SplitView>(value: SplitViewTemplateSettings) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<SplitViewTemplateSettings>(SplitView.TemplateSettingsProperty, value, ValueNone)
