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

    let create (attrs: IAttr<ContentPresenter> list): IView<ContentPresenter> =
        ViewBuilder.Create<ContentPresenter>(attrs)

    type ContentPresenter with
        static member background<'t when 't :> ContentPresenter>(brush: IBrush) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<IBrush>(ContentPresenter.BackgroundProperty, brush, ValueNone)

        static member background<'t when 't :> ContentPresenter>(color: string) : IAttr<'t> =
            Color.Parse(color) |> ImmutableSolidColorBrush |> ContentPresenter.background

        static member background<'t when 't :> ContentPresenter>(color: Color) : IAttr<'t> =
            color |> ImmutableSolidColorBrush |> ContentPresenter.background

        static member borderBrush<'t when 't :> ContentPresenter>(brush: IBrush) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<IBrush>(ContentPresenter.BorderBrushProperty, brush, ValueNone)

        static member borderBrush<'t when 't :> ContentPresenter>(color: string) : IAttr<'t> =
            Color.Parse(color) |> ImmutableSolidColorBrush |> ContentPresenter.borderBrush

        static member borderBrush<'t when 't :> ContentPresenter>(color: Color) : IAttr<'t> =
            color |> ImmutableSolidColorBrush |> ContentPresenter.borderBrush

        static member borderThickness<'t when 't :> ContentPresenter>(value: Thickness) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<Thickness>(ContentPresenter.BorderThicknessProperty, value, ValueNone)

        static member borderThickness<'t when 't :> ContentPresenter>(value: float) : IAttr<'t> =
            Thickness(value) |> ContentPresenter.borderThickness

        static member borderThickness<'t when 't :> ContentPresenter>(horizontal: float, vertical: float) : IAttr<'t> =
            Thickness(horizontal, vertical) |> ContentPresenter.borderThickness

        static member borderThickness<'t when 't :> ContentPresenter>(left: float, top: float, right: float, bottom: float) : IAttr<'t> =
            Thickness(left, top, right, bottom) |> ContentPresenter.borderThickness

        static member boxShadows<'t when 't :> ContentPresenter>(value: BoxShadows) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty(ContentPresenter.BoxShadowProperty, value, ValueNone)

        static member boxShadow<'t when 't :> ContentPresenter>(value: BoxShadow) : IAttr<'t> =
            value |> BoxShadows |> ContentPresenter.boxShadows

        static member foreground<'t when 't :> ContentPresenter>(brush: IBrush) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<IBrush>(ContentPresenter.ForegroundProperty, brush, ValueNone)

        static member foreground<'t when 't :> ContentPresenter>(color: string) : IAttr<'t> =
            Color.Parse(color) |> ImmutableSolidColorBrush |> ContentPresenter.foreground

        static member foreground<'t when 't :> ContentPresenter>(color: Color) : IAttr<'t> =
            color |> ImmutableSolidColorBrush |> ContentPresenter.foreground

        static member fontFamily<'t when 't :> ContentPresenter>(value: FontFamily) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<FontFamily>(ContentPresenter.FontFamilyProperty, value, ValueNone)

        static member fontSize<'t when 't :> ContentPresenter>(value: float) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<float>(ContentPresenter.FontSizeProperty, value, ValueNone)

        static member fontStyle<'t when 't :> ContentPresenter>(value: FontStyle) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<FontStyle>(ContentPresenter.FontStyleProperty, value, ValueNone)

        static member fontWeight<'t when 't :> ContentPresenter>(value: FontWeight) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<FontWeight>(ContentPresenter.FontWeightProperty, value, ValueNone)

        static member fontStretch<'t when 't :> ContentPresenter>(value: FontStretch) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<FontStretch>(ContentPresenter.FontStretchProperty, value, ValueNone)

        static member textAlignment<'t when 't :> ContentPresenter>(value: TextAlignment) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<TextAlignment>(ContentPresenter.TextAlignmentProperty, value, ValueNone)

        static member textWrapping<'t when 't :> ContentPresenter>(value: TextWrapping) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<TextWrapping>(ContentPresenter.TextWrappingProperty, value, ValueNone)

        static member textTrimming<'t when 't :> ContentPresenter>(value: TextTrimming) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<TextTrimming>(ContentPresenter.TextTrimmingProperty, value, ValueNone)

        static member lineHeight<'t when 't :> ContentPresenter>(value: float) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<float>(ContentPresenter.LineHeightProperty, value, ValueNone)

        static member maxLines<'t when 't :> ContentPresenter>(value: int) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<int>(ContentPresenter.MaxLinesProperty, value, ValueNone)

        static member cornerRadius<'t when 't :> ContentPresenter>(value: CornerRadius) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<CornerRadius>(ContentPresenter.CornerRadiusProperty, value, ValueNone)

        static member cornerRadius<'t when 't :> ContentPresenter>(value: float) : IAttr<'t> =
            CornerRadius(value) |> ContentPresenter.cornerRadius

        static member cornerRadius<'t when 't :> ContentPresenter>(horizontal: float, vertical: float) : IAttr<'t> =
            CornerRadius(horizontal, vertical) |> ContentPresenter.cornerRadius

        static member cornerRadius<'t when 't :> ContentPresenter>(left: float, top: float, right: float, bottom: float) : IAttr<'t> =
            CornerRadius(left, right, top, bottom) |> ContentPresenter.cornerRadius

        static member child<'t when 't :> ContentPresenter>(value: IView option) : IAttr<'t> =
            AttrBuilder<'t>.CreateContentSingle(ContentPresenter.ChildProperty, value)

        static member child<'t when 't :> ContentPresenter>(value: IView) : IAttr<'t> =
            value |> Some |> ContentPresenter.child

        static member content<'t when 't :> ContentPresenter>(value: IView option) : IAttr<'t> =
            AttrBuilder<'t>.CreateContentSingle(ContentPresenter.ContentProperty, value)

        static member content<'t when 't :> ContentPresenter>(value: IView) : IAttr<'t> =
            value |> Some |> ContentPresenter.content

        static member contentTemplate<'t when 't :> ContentPresenter>(template: IDataTemplate) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<IDataTemplate>(ContentPresenter.ContentTemplateProperty, template, ValueNone)

        static member horizontalContentAlignment<'t when 't :> ContentPresenter>(value: HorizontalAlignment) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<HorizontalAlignment>(ContentPresenter.HorizontalContentAlignmentProperty, value, ValueNone)

        static member verticalContentAlignment<'t when 't :> ContentPresenter>(value: VerticalAlignment) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<VerticalAlignment>(ContentPresenter.VerticalContentAlignmentProperty, value, ValueNone)

        static member padding<'t when 't :> ContentPresenter>(value: Thickness) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<Thickness>(ContentPresenter.PaddingProperty, value, ValueNone)

        static member padding<'t when 't :> ContentPresenter>(value: float) : IAttr<'t> =
            Thickness(value) |> ContentPresenter.padding

        static member padding<'t when 't :> ContentPresenter>(horizontal: float, vertical: float) : IAttr<'t> =
            Thickness(horizontal, vertical) |> ContentPresenter.padding

        static member padding<'t when 't :> ContentPresenter>(left: float, top: float, right: float, bottom: float) : IAttr<'t> =
            Thickness(left, top, right, bottom) |> ContentPresenter.padding

        static member recognizesAccessKey<'t when 't :> ContentPresenter>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(ContentPresenter.RecognizesAccessKeyProperty, value, ValueNone)
