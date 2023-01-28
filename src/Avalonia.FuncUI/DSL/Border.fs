namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module Border =
    open Avalonia
    open Avalonia.Controls
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
    open Avalonia.Media
    open Avalonia.Media.Immutable

    let create (attrs: Attr<Border> list): View<Border> =
        ViewBuilder.Create<Border>(attrs)

    type Border with
        static member background<'t when 't :> Border>(brush: IBrush) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<IBrush>(Border.BackgroundProperty, brush, ValueNone)

        static member background<'t when 't :> Border>(color: string) : Attr<'t> =
            color |> Color.Parse |> ImmutableSolidColorBrush |> Border.background

        static member background<'t when 't :> Border>(color: Color) : Attr<'t> =
            color |> ImmutableSolidColorBrush |> Border.background

        static member borderBrush<'t when 't :> Border>(brush: IBrush) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<IBrush>(Border.BorderBrushProperty, brush, ValueNone)

        static member borderBrush<'t when 't :> Border>(color: string) : Attr<'t> =
            color |> Color.Parse |> ImmutableSolidColorBrush |> Border.borderBrush

        static member borderThickness<'t when 't :> Border>(value: Thickness) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<Thickness>(Border.BorderThicknessProperty, value, ValueNone)

        static member borderThickness<'t when 't :> Border>(value: float) : Attr<'t> =
            value |> Thickness |> Border.borderThickness

        static member borderThickness<'t when 't :> Border>(horizontal: float, vertical: float) : Attr<'t> =
            (horizontal, vertical) |> Thickness |> Border.borderThickness

        static member borderThickness<'t when 't :> Border>(left: float, top: float, right: float, bottom: float) : Attr<'t> =
            (left, top, right, bottom) |> Thickness |> Border.borderThickness

        static member cornerRadius<'t when 't :> Border>(value: CornerRadius) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<CornerRadius>(Border.CornerRadiusProperty, value, ValueNone)

        static member cornerRadius<'t when 't :> Border>(value: float) : Attr<'t> =
            value |> CornerRadius |> Border.cornerRadius

        static member cornerRadius<'t when 't :> Border>(horizontal: float, vertical: float) : Attr<'t> =
            (horizontal, vertical) |> CornerRadius |> Border.cornerRadius

        static member cornerRadius<'t when 't :> Border>(left: float, top: float, right: float, bottom: float) : Attr<'t> =
            (left, top, right, bottom) |> CornerRadius |> Border.cornerRadius

        static member boxShadows<'t when 't :> Border>(value: BoxShadows) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty(Border.BoxShadowProperty, value, ValueNone)

        static member boxShadow<'t when 't :> Border>(value: BoxShadow) : Attr<'t> =
            value |> BoxShadows |> Border.boxShadows

        static member child<'t when 't :> Border>(value: IView option) : Attr<'t> =
            AttrBuilder<'t>.CreateContentSingle(Border.ChildProperty, value)

        static member child<'t when 't :> Border>(value: IView) : Attr<'t> =
            value |> Some |> Border.child