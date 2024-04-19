namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module TextBox =
    open Avalonia.Controls
    open Avalonia.Layout
    open Avalonia.Media.Immutable
    open Avalonia.Media
    open Avalonia.FuncUI.Builder
    open Avalonia.FuncUI.Types
    open Avalonia.Interactivity

    let create (attrs: IAttr<TextBox> list): IView<TextBox> =
        ViewBuilder.Create<TextBox>(attrs)

    type TextBox with

        static member acceptsReturn<'t when 't :> TextBox>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(TextBox.AcceptsReturnProperty, value, ValueNone)

        static member acceptsTab<'t when 't :> TextBox>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(TextBox.AcceptsTabProperty, value, ValueNone)

        static member caretIndex<'t when 't :> TextBox>(value: int) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<int>(TextBox.CaretIndexProperty, value, ValueNone)

        static member isReadOnly<'t when 't :> TextBox>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(TextBox.IsReadOnlyProperty, value, ValueNone)

        static member horizontalContentAlignment<'t when 't :> TextBox>(value: HorizontalAlignment) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<HorizontalAlignment>(TextBox.HorizontalContentAlignmentProperty, value, ValueNone)

        static member passwordChar<'t when 't :> TextBox>(value: char) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<char>(TextBox.PasswordCharProperty, value, ValueNone)

        static member selectionBrush<'t when 't :> TextBox>(value: IBrush) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<IBrush>(TextBox.SelectionBrushProperty, value, ValueNone)

        static member selectionBrush<'t when 't :> TextBox>(color: string) : IAttr<'t> =
            color |> Color.Parse |> ImmutableSolidColorBrush |> TextBox.selectionBrush

        static member selectionBrush<'t when 't :> TextBox>(color: Color) : IAttr<'t> =
            color |> ImmutableSolidColorBrush |> TextBox.selectionBrush

        static member selectionForegroundBrush<'t when 't :> TextBox>(value: IBrush) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<IBrush>(TextBox.SelectionForegroundBrushProperty, value, ValueNone)

        static member selectionForegroundBrush<'t when 't :> TextBox>(color: string) : IAttr<'t> =
            color |> Color.Parse |> ImmutableSolidColorBrush |> TextBox.selectionForegroundBrush

        static member selectionForegroundBrush<'t when 't :> TextBox>(color: Color) : IAttr<'t> =
            color |> ImmutableSolidColorBrush |> TextBox.selectionForegroundBrush

        static member caretBrush<'t when 't :> TextBox>(value: IBrush) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<IBrush>(TextBox.CaretBrushProperty, value, ValueNone)

        static member caretBrush<'t when 't :> TextBox>(color: string) : IAttr<'t> =
            color |> Color.Parse |> ImmutableSolidColorBrush |> TextBox.selectionBrush

        static member caretBrush<'t when 't :> TextBox>(color: Color) : IAttr<'t> =
            color |> ImmutableSolidColorBrush |> TextBox.selectionBrush

        static member selectionStart<'t when 't :> TextBox>(value: int) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<int>(TextBox.SelectionStartProperty, value, ValueNone)

        static member selectionEnd<'t when 't :> TextBox>(value: int) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<int>(TextBox.SelectionEndProperty, value, ValueNone)

        static member maxLength<'t when 't :> TextBox>(value: int) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<int>(TextBox.MaxLengthProperty, value, ValueNone)

        static member maxLines<'t when 't :> TextBox>(value: int) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<int>(TextBox.MaxLinesProperty, value, ValueNone)

        static member letterSpacing<'t when 't :> TextBox>(value: float) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<float>(TextBox.LetterSpacingProperty, value, ValueNone)

        static member lineHeight<'t when 't :> TextBox>(value: float) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<float>(TextBox.LineHeightProperty, value, ValueNone)
        static member text<'t when 't :> TextBox>(value: string) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<string>(TextBox.TextProperty, value, ValueNone)

        static member selectedText<'t when 't :> TextBox>(value: string) : IAttr<'t> =
            let name = nameof Unchecked.defaultof<'t>.SelectedText
            let getter: 't -> string = fun control -> control.SelectedText
            let setter: 't * string -> unit = fun (control, value) -> control.SelectedText <- value

            AttrBuilder<'t>.CreateProperty<string>(name, value, ValueSome getter, ValueSome setter, ValueNone, (fun () -> ""))
        
        static member innerLeftContent<'t when 't :> TextBox>(text: string) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<string>(TextBox.InnerLeftContentProperty, text, ValueNone)

        static member innerLeftContent<'t when 't :> TextBox>(value: obj) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<obj>(TextBox.InnerLeftContentProperty, value, ValueNone)

        static member innerLeftContent<'t when 't :> TextBox>(view: IView option) : IAttr<'t> =
            AttrBuilder<'t>.CreateContentSingle(TextBox.InnerLeftContentProperty, view)

        static member innerLeftContent<'t when 't :> TextBox>(view: IView) : IAttr<'t> =
            Some view |> TextBox.innerLeftContent

        static member innerRightContent<'t when 't :> TextBox>(text: string) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<string>(TextBox.InnerRightContentProperty, text, ValueNone)

        static member innerRightContent<'t when 't :> TextBox>(value: obj) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<obj>(TextBox.InnerRightContentProperty, value, ValueNone)

        static member innerRightContent<'t when 't :> TextBox>(view: IView option) : IAttr<'t> =
            AttrBuilder<'t>.CreateContentSingle(TextBox.InnerRightContentProperty, view)

        static member innerRightContent<'t when 't :> TextBox>(view: IView) : IAttr<'t> =
            Some view |> TextBox.innerRightContent

        static member revealPassword<'t when 't :> TextBox>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(TextBox.RevealPasswordProperty, value, ValueNone)

        static member isUndoEnabled<'t when 't :> TextBox>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(TextBox.IsUndoEnabledProperty, value, ValueNone)

        static member undoLimit<'t when 't :> TextBox>(value: int) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<int>(TextBox.UndoLimitProperty, value, ValueNone)
        
        static member onCopyingToClipboard<'t when 't :> TextBox>(func: RoutedEventArgs -> unit, ?subPatchOptions) =
            AttrBuilder<'t>.CreateSubscription<RoutedEventArgs>(TextBox.CopyingToClipboardEvent, func, ?subPatchOptions = subPatchOptions)

        static member onCuttingToClipboard<'t when 't :> TextBox>(func: RoutedEventArgs -> unit, ?subPatchOptions) =
            AttrBuilder<'t>.CreateSubscription<RoutedEventArgs>(TextBox.CuttingToClipboardEvent, func, ?subPatchOptions = subPatchOptions)

        static member onPastingFromClipboard<'t when 't :> TextBox>(func: RoutedEventArgs -> unit, ?subPatchOptions) =
            AttrBuilder<'t>.CreateSubscription<RoutedEventArgs>(TextBox.PastingFromClipboardEvent, func, ?subPatchOptions = subPatchOptions)

        static member onTextChanged<'t when 't :> TextBox>(func: string -> unit, ?subPatchOptions) =
            AttrBuilder<'t>.CreateSubscription<string>(TextBox.TextProperty, func, ?subPatchOptions = subPatchOptions)

        static member onTextChanged<'t when 't :> TextBox>(func: TextChangedEventArgs -> unit, ?subPatchOptions) =
            AttrBuilder<'t>.CreateSubscription<TextChangedEventArgs>(TextBox.TextChangedEvent, func, ?subPatchOptions = subPatchOptions)

        static member onTextChanging<'t when 't :> TextBox>(func: TextChangingEventArgs -> unit, ?subPatchOptions) =
            AttrBuilder<'t>.CreateSubscription<TextChangingEventArgs>(TextBox.TextChangingEvent, func, ?subPatchOptions = subPatchOptions)

        static member textAlignment<'t when 't :> TextBox>(alignment: TextAlignment) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<TextAlignment>(TextBox.TextAlignmentProperty, alignment, ValueNone)

        static member textWrapping<'t when 't :> TextBox>(value: TextWrapping) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<TextWrapping>(TextBox.TextWrappingProperty, value, ValueNone)

        static member useFloatingWatermark<'t when 't :> TextBox>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(TextBox.UseFloatingWatermarkProperty, value, ValueNone)

        static member newLine<'t when 't :> TextBox>(value: string) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<string>(TextBox.NewLineProperty, value, ValueNone)

        static member verticalContentAlignment<'t when 't :> TextBox>(value: VerticalAlignment) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<VerticalAlignment>(TextBox.VerticalContentAlignmentProperty, value, ValueNone)

        static member watermark<'t when 't :> TextBox>(value: string) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<string>(TextBox.WatermarkProperty, value, ValueNone)
