namespace Avalonia.FuncUI.DSL
open Avalonia.Media.Immutable

[<AutoOpen>]
module TextBlock =
    open Avalonia
    open Avalonia.Controls
    open Avalonia.Controls.Documents
    open Avalonia.Media
    open Avalonia.FuncUI.Builder
    open Avalonia.FuncUI.Types

    let create (attrs: Attr<TextBlock> list): IView<TextBlock> =
        ViewBuilder.Create<TextBlock>(attrs)

    type TextBlock with

        static member text<'t when 't :> TextBlock>(value: string) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<string>(TextBlock.TextProperty, value, ValueNone)

        static member background<'t when 't :> TextBlock>(value: IBrush) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<IBrush>(TextBlock.BackgroundProperty, value, ValueNone)

        static member background<'t when 't :> TextBlock>(color: string) : Attr<'t> =
            color |> Color.Parse |> ImmutableSolidColorBrush |> TextBlock.background

        static member fontFamily<'t when 't :> TextBlock>(value: FontFamily) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<FontFamily>(TextBlock.FontFamilyProperty, value, ValueNone)

        static member fontSize<'t when 't :> TextBlock>(value: double) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<double>(TextBlock.FontSizeProperty, value, ValueNone)

        static member fontStyle<'t when 't :> TextBlock>(value: FontStyle) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<FontStyle>(TextBlock.FontStyleProperty, value, ValueNone)

        static member fontWeight<'t when 't :> TextBlock>(value: FontWeight) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<FontWeight>(TextBlock.FontWeightProperty, value, ValueNone)

        static member foreground<'t when 't :> TextBlock>(value: IBrush) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<IBrush>(TextBlock.ForegroundProperty, value, ValueNone)

        static member foreground<'t when 't :> TextBlock>(color: string) : Attr<'t> =
            color |> Color.Parse |> ImmutableSolidColorBrush |> TextBlock.foreground

        static member inlines<'t when 't :> TextBlock>(value: InlineCollection) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<InlineCollection>(TextBlock.InlinesProperty, value, ValueNone)

        static member inlines<'t when 't :> TextBlock>(values: IView list (* TODO: Change to IView<Inline> *)) : Attr<'t> =
            let getter : ('t -> obj) = (fun control -> control.Inlines :> obj)
            AttrBuilder<'t>.CreateContentMultiple("Inlines", ValueSome getter, ValueNone, values)

        static member lineHeight<'t when 't :> TextBlock>(value: float) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<float>(TextBlock.LineHeightProperty, value, ValueNone)

        static member maxLines<'t when 't :> TextBlock>(value: int) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<int>(TextBlock.MaxLinesProperty, value, ValueNone)

        static member padding<'t when 't :> TextBlock>(value: Thickness) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<Thickness>(TextBlock.PaddingProperty, value, ValueNone)

        static member padding<'t when 't :> TextBlock>(horizontal: float, vertical: float) : Attr<'t> =
            (horizontal, vertical) |> Thickness |> TextBlock.padding

        static member padding<'t when 't :> TextBlock>(left: float, top: float, right: float, bottom: float) : Attr<'t> =
            (left, top, right, bottom) |> Thickness |> TextBlock.padding

        static member textAlignment<'t when 't :> TextBlock>(alignment: TextAlignment) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<TextAlignment>(TextBlock.TextAlignmentProperty, alignment, ValueNone)

        static member textDecorations<'t when 't :> TextBlock>(value: TextDecorationCollection) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<TextDecorationCollection>(TextBlock.TextDecorationsProperty, value, ValueNone)

        static member textTrimming<'t when 't :> TextBlock>(value: TextTrimming) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<TextTrimming>(TextBlock.TextTrimmingProperty, value, ValueNone)

        static member textWrapping<'t when 't :> TextBlock>(value: TextWrapping) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<TextWrapping>(TextBlock.TextWrappingProperty, value, ValueNone)