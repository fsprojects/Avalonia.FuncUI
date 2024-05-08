namespace Avalonia.FuncUI.DSL

open System
open Avalonia.Controls.Presenters
open Avalonia.Media
open Avalonia.Media.Immutable

[<AutoOpen>]
module TextPresenter =
    open Avalonia.FuncUI.Builder
    open Avalonia.FuncUI.Types

    let create (attrs: IAttr<TextPresenter> list): IView<TextPresenter> =
        ViewBuilder.Create<TextPresenter>(attrs)

    type TextPresenter with
        static member onCaretBoundsChanged<'t when 't :> TextPresenter>(func: 't -> unit, ?subPatchOptions) : IAttr<'t> =
            let name = nameof Unchecked.defaultof<'t>.CaretBoundsChanged
            let factory: SubscriptionFactory<'t> =
                fun (control, func, token) ->
                    let control = control :?> 't
                    let handler = EventHandler(fun s e -> func(s :?> 't))
                    let event = control.CaretBoundsChanged

                    event.AddHandler(handler)
                    token.Register(fun _ -> event.RemoveHandler(handler)) |> ignore

            AttrBuilder<'t>.CreateSubscription<'t>(name, factory, func, ?subPatchOptions = subPatchOptions)

        static member background<'t when 't :> TextPresenter>(value: IBrush) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<IBrush>(TextPresenter.BackgroundProperty, value, ValueNone)

        static member background<'t when 't :> TextPresenter>(color: string) : IAttr<'t> =
            color |> Color.Parse |> ImmutableSolidColorBrush |> TextPresenter.background

        static member background<'t when 't :> TextPresenter>(color: Color) : IAttr<'t> =
            color |> ImmutableSolidColorBrush |> TextPresenter.background

        static member preeditText<'t when 't :> TextPresenter>(value: string) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<string>(TextPresenter.PreeditTextProperty, value, ValueNone)

        static member fontFamily<'t when 't :> TextPresenter>(value: FontFamily) : IAttr<'t> =
            let name = nameof Unchecked.defaultof<'t>.FontFamily
            let getter: 't -> FontFamily = (fun control -> control.FontFamily)
            let setter: 't * FontFamily -> unit = (fun (control, value) -> control.FontFamily <- value)

            AttrBuilder<'t>.CreateProperty<FontFamily>(name, value, ValueSome getter, ValueSome setter, ValueNone)

        static member fontSize<'t when 't :> TextPresenter>(value: double) : IAttr<'t> =
            let name = nameof Unchecked.defaultof<'t>.FontSize
            let getter: 't -> double = (fun control -> control.FontSize)
            let setter: 't * double -> unit = (fun (control, value) -> control.FontSize <- value)

            AttrBuilder<'t>.CreateProperty<double>(name, value, ValueSome getter, ValueSome setter, ValueNone)

        static member fontStyle<'t when 't :> TextPresenter>(value: FontStyle) : IAttr<'t> =
            let name = nameof Unchecked.defaultof<'t>.FontStyle
            let getter: 't -> FontStyle = (fun control -> control.FontStyle)
            let setter: 't * FontStyle -> unit = (fun (control, value) -> control.FontStyle <- value)

            AttrBuilder<'t>.CreateProperty<FontStyle>(name, value, ValueSome getter, ValueSome setter, ValueNone)

        static member fontWeight<'t when 't :> TextPresenter>(value: FontWeight) : IAttr<'t> =
            let name = nameof Unchecked.defaultof<'t>.FontWeight
            let getter: 't -> FontWeight = (fun control -> control.FontWeight)
            let setter: 't * FontWeight -> unit = (fun (control, value) -> control.FontWeight <- value)

            AttrBuilder<'t>.CreateProperty<FontWeight>(name, value, ValueSome getter, ValueSome setter, ValueNone)

        static member fontStretch<'t when 't :> TextPresenter>(value: FontStretch) : IAttr<'t> =
            let name = nameof Unchecked.defaultof<'t>.FontStretch
            let getter: 't -> FontStretch = (fun control -> control.FontStretch)
            let setter: 't * FontStretch -> unit = (fun (control, value) -> control.FontStretch <- value)

            AttrBuilder<'t>.CreateProperty<FontStretch>(name, value, ValueSome getter, ValueSome setter, ValueNone)

        static member foreground<'t when 't :> TextPresenter>(value: IBrush) : IAttr<'t> =
            let name = nameof Unchecked.defaultof<'t>.Foreground
            let getter: 't -> IBrush = (fun control -> control.Foreground)
            let setter: 't * IBrush -> unit = (fun (control, value) -> control.Foreground <- value)

            AttrBuilder<'t>.CreateProperty<IBrush>(name, value, ValueSome getter, ValueSome setter, ValueNone)

        static member foreground<'t when 't :> TextPresenter>(color: string) : IAttr<'t> =
            color |> Color.Parse |> ImmutableSolidColorBrush |> TextPresenter.foreground

        static member foreground<'t when 't :> TextPresenter>(color: Color) : IAttr<'t> =
            color |> ImmutableSolidColorBrush |> TextPresenter.foreground

        static member textWrapping<'t when 't :> TextPresenter>(value: TextWrapping) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<TextWrapping>(TextPresenter.TextWrappingProperty, value, ValueNone)

        static member lineHeight<'t when 't :> TextPresenter>(value: double) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<double>(TextPresenter.LineHeightProperty, value, ValueNone)

        static member letterSpacing<'t when 't :> TextPresenter>(value: double) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<double>(TextPresenter.LetterSpacingProperty, value, ValueNone)

        static member textAlignment<'t when 't :> TextPresenter>(value: TextAlignment) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<TextAlignment>(TextPresenter.TextAlignmentProperty, value, ValueNone)

        static member caretIndex<'t when 't :> TextPresenter>(value: int) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<int>(TextPresenter.CaretIndexProperty, value, ValueNone)

        static member passwordChar<'t when 't :> TextPresenter>(value: char) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<char>(TextPresenter.PasswordCharProperty, value, ValueNone)

        static member revealPassword<'t when 't :> TextPresenter>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(TextPresenter.RevealPasswordProperty, value, ValueNone)

        static member selectionBrush<'t when 't :> TextPresenter>(value: IBrush) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<IBrush>(TextPresenter.SelectionBrushProperty, value, ValueNone)

        static member selectionBrush<'t when 't :> TextPresenter>(color: string) : IAttr<'t> =
            color |> Color.Parse |> ImmutableSolidColorBrush |> TextPresenter.selectionBrush

        static member selectionBrush<'t when 't :> TextPresenter>(color: Color) : IAttr<'t> =
            color |> ImmutableSolidColorBrush |> TextPresenter.selectionBrush

        static member selectionForegroundBrush<'t when 't :> TextPresenter>(value: IBrush) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<IBrush>(TextPresenter.SelectionForegroundBrushProperty, value, ValueNone)

        static member selectionForegroundBrush<'t when 't :> TextPresenter>(color: string) : IAttr<'t> =
            color |> Color.Parse |> ImmutableSolidColorBrush |> TextPresenter.selectionForegroundBrush

        static member selectionForegroundBrush<'t when 't :> TextPresenter>(color: Color) : IAttr<'t> =
            color |> ImmutableSolidColorBrush |> TextPresenter.selectionForegroundBrush

        static member caretBrush<'t when 't :> TextPresenter>(value: IBrush) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<IBrush>(TextPresenter.CaretBrushProperty, value, ValueNone)

        static member caretBrush<'t when 't :> TextPresenter>(color: string) : IAttr<'t> =
            color |> Color.Parse |> ImmutableSolidColorBrush |> TextPresenter.caretBrush

        static member caretBrush<'t when 't :> TextPresenter>(color: Color) : IAttr<'t> =
            color |> ImmutableSolidColorBrush |> TextPresenter.caretBrush

        static member selectionStart<'t when 't :> TextPresenter>(value: int) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<int>(TextPresenter.SelectionStartProperty, value, ValueNone)

        static member selectionEnd<'t when 't :> TextPresenter>(value: int) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<int>(TextPresenter.SelectionEndProperty, value, ValueNone)
