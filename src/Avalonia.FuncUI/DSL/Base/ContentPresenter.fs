namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module ContentPresenter =
    open Avalonia
    open Avalonia.Controls.Templates
    open Avalonia.Layout
    open Avalonia.Controls.Presenters
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
    open Avalonia.Media
    open Avalonia.Media.Immutable

    let create (attrs: Attr<ContentPresenter> list): View<ContentPresenter> =
        ViewBuilder.Create<ContentPresenter>(attrs)

    type ContentPresenter with
        static member background<'t when 't :> ContentPresenter>(brush: IBrush) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<IBrush>(ContentPresenter.BackgroundProperty, brush, ValueNone)

        static member background<'t when 't :> ContentPresenter>(color: string) : Attr<'t> =
            Color.Parse(color) |> ImmutableSolidColorBrush |> ContentPresenter.background

        static member borderBrush<'t when 't :> ContentPresenter>(brush: IBrush) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<IBrush>(ContentPresenter.BorderBrushProperty, brush, ValueNone)

        static member borderBrush<'t when 't :> ContentPresenter>(color: string) : Attr<'t> =
            Color.Parse(color) |> ImmutableSolidColorBrush |> ContentPresenter.borderBrush

        static member borderThickness<'t when 't :> ContentPresenter>(value: Thickness) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<Thickness>(ContentPresenter.BorderThicknessProperty, value, ValueNone)

        static member borderThickness<'t when 't :> ContentPresenter>(value: float) : Attr<'t> =
            Thickness(value) |> ContentPresenter.borderThickness

        static member borderThickness<'t when 't :> ContentPresenter>(horizontal: float, vertical: float) : Attr<'t> =
            Thickness(horizontal, vertical) |> ContentPresenter.borderThickness

        static member borderThickness<'t when 't :> ContentPresenter>(left: float, top: float, right: float, bottom: float) : Attr<'t> =
            Thickness(left, top, right, bottom) |> ContentPresenter.borderThickness

        static member boxShadows<'t when 't :> ContentPresenter>(value: BoxShadows) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty(ContentPresenter.BoxShadowProperty, value, ValueNone)

        static member boxShadow<'t when 't :> ContentPresenter>(value: BoxShadow) : Attr<'t> =
            value |> BoxShadows |> ContentPresenter.boxShadows

        static member cornerRadius<'t when 't :> ContentPresenter>(value: CornerRadius) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<CornerRadius>(ContentPresenter.CornerRadiusProperty, value, ValueNone)

        static member cornerRadius<'t when 't :> ContentPresenter>(value: float) : Attr<'t> =
            CornerRadius(value) |> ContentPresenter.cornerRadius

        static member cornerRadius<'t when 't :> ContentPresenter>(horizontal: float, vertical: float) : Attr<'t> =
            CornerRadius(horizontal, vertical) |> ContentPresenter.cornerRadius

        static member cornerRadius<'t when 't :> ContentPresenter>(left: float, top: float, right: float, bottom: float) : Attr<'t> =
            CornerRadius(left, right, top, bottom) |> ContentPresenter.cornerRadius

        static member child<'t when 't :> ContentPresenter>(value: IView option) : Attr<'t> =
            AttrBuilder<'t>.CreateContentSingle(ContentPresenter.ChildProperty, value)

        static member child<'t when 't :> ContentPresenter>(value: IView) : Attr<'t> =
            value |> Some |> ContentPresenter.child

        static member content<'t when 't :> ContentPresenter>(value: IView option) : Attr<'t> =
            AttrBuilder<'t>.CreateContentSingle(ContentPresenter.ContentProperty, value)

        static member content<'t when 't :> ContentPresenter>(value: IView) : Attr<'t> =
            value |> Some |> ContentPresenter.content

        static member contentTemplate<'t when 't :> ContentPresenter>(template: IDataTemplate) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<IDataTemplate>(ContentPresenter.ContentTemplateProperty, template, ValueNone)

        static member horizontalContentAlignment<'t when 't :> ContentPresenter>(value: HorizontalAlignment) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<HorizontalAlignment>(ContentPresenter.HorizontalContentAlignmentProperty, value, ValueNone)

        static member verticalContentAlignment<'t when 't :> ContentPresenter>(value: VerticalAlignment) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<VerticalAlignment>(ContentPresenter.VerticalContentAlignmentProperty, value, ValueNone)

        static member padding<'t when 't :> ContentPresenter>(value: Thickness) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<Thickness>(ContentPresenter.PaddingProperty, value, ValueNone)

        static member padding<'t when 't :> ContentPresenter>(value: float) : Attr<'t> =
            Thickness(value) |> ContentPresenter.padding

        static member padding<'t when 't :> ContentPresenter>(horizontal: float, vertical: float) : Attr<'t> =
            Thickness(horizontal, vertical) |> ContentPresenter.padding

        static member padding<'t when 't :> ContentPresenter>(left: float, top: float, right: float, bottom: float) : Attr<'t> =
            Thickness(left, top, right, bottom) |> ContentPresenter.padding