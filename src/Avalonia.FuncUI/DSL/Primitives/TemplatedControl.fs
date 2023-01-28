namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module TemplatedControl =
    open Avalonia.Media.Immutable
    open Avalonia
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
    open Avalonia.Media
    open Avalonia.Controls.Primitives
    open Avalonia.Controls.Templates

    let create (attrs: Attr<TemplatedControl> list): View<TemplatedControl> =
        ViewBuilder.Create<TemplatedControl>(attrs)

    type TemplatedControl with
        static member background<'t when 't :> TemplatedControl>(value: IBrush) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<IBrush>(TemplatedControl.BackgroundProperty, value, ValueNone)

        static member background<'t when 't :> TemplatedControl>(color: string) : Attr<'t> =
            Color.Parse(color) |> ImmutableSolidColorBrush |> TemplatedControl.background

        static member borderBrush<'t when 't :> TemplatedControl>(value: IBrush) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<IBrush>(TemplatedControl.BorderBrushProperty, value, ValueNone)

        static member borderBrush<'t when 't :> TemplatedControl>(color: string) : Attr<'t> =
            Color.Parse(color) |> ImmutableSolidColorBrush |> TemplatedControl.borderBrush

        static member borderThickness<'t when 't :> TemplatedControl>(value: Thickness) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<Thickness>(TemplatedControl.BorderThicknessProperty, value, ValueNone)

        static member borderThickness<'t when 't :> TemplatedControl>(value: float) : Attr<'t> =
            value |> Thickness |> TemplatedControl.borderThickness

        static member borderThickness<'t when 't :> TemplatedControl>(horizontal: float, vertical: float) : Attr<'t> =
            (horizontal, vertical) |> Thickness |> TemplatedControl.borderThickness

        static member borderThickness<'t when 't :> TemplatedControl>(left: float, top: float, right: float, bottom: float) : Attr<'t> =
            (left, top, right, bottom) |> Thickness |> TemplatedControl.borderThickness

        static member fontFamily<'t when 't :> TemplatedControl>(value: FontFamily) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<FontFamily>(TemplatedControl.FontFamilyProperty, value, ValueNone)

        static member fontSize<'t when 't :> TemplatedControl>(value: double) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<double>(TemplatedControl.FontSizeProperty, value, ValueNone)

        static member fontStyle<'t when 't :> TemplatedControl>(value: FontStyle) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<FontStyle>(TemplatedControl.FontStyleProperty, value, ValueNone)

        static member fontWeight<'t when 't :> TemplatedControl>(value: FontWeight) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<FontWeight>(TemplatedControl.FontWeightProperty, value, ValueNone)

        static member foreground<'t when 't :> TemplatedControl>(value: IBrush) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<IBrush>(TemplatedControl.ForegroundProperty, value, ValueNone)

        static member foreground<'t when 't :> TemplatedControl>(color: string) : Attr<'t> =
            Color.Parse(color) |> ImmutableSolidColorBrush |> TemplatedControl.foreground

        static member padding<'t when 't :> TemplatedControl>(value: Thickness) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<Thickness>(TemplatedControl.PaddingProperty, value, ValueNone)

        static member padding<'t when 't :> TemplatedControl>(value: float) : Attr<'t> =
            Thickness(value) |> TemplatedControl.padding

        static member padding<'t when 't :> TemplatedControl>(horizontal: float, vertical: float) : Attr<'t> =
            Thickness(horizontal, vertical) |> TemplatedControl.padding

        static member padding<'t when 't :> TemplatedControl>(left: float, top: float, right: float, bottom: float) : Attr<'t> =
            Thickness(left, top, right, bottom) |> TemplatedControl.padding

        static member cornerRadius<'t when 't :> TemplatedControl>(value: CornerRadius) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<CornerRadius>(TemplatedControl.CornerRadiusProperty, value, ValueNone)

        static member cornerRadius<'t when 't :> TemplatedControl>(value: float) : Attr<'t> =
            CornerRadius(value) |> TemplatedControl.cornerRadius

        static member cornerRadius<'t when 't :> TemplatedControl>(horizontal: float, vertical: float) : Attr<'t> =
            CornerRadius(horizontal, vertical) |> TemplatedControl.cornerRadius

        static member cornerRadius<'t when 't :> TemplatedControl>(left: float, top: float, right: float, bottom: float) : Attr<'t> =
            CornerRadius(left, top, right, bottom) |> TemplatedControl.cornerRadius

        static member template<'t when 't :> TemplatedControl>(value: IControlTemplate) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<IControlTemplate>(TemplatedControl.TemplateProperty, value, ValueNone)

        static member isTemplateFocusTarget<'t when 't :> TemplatedControl>(value: bool) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(TemplatedControl.IsTemplateFocusTargetProperty, value, ValueNone)