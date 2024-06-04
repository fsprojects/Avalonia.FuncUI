namespace Avalonia.FuncUI.DSL

open Avalonia.Controls
open Avalonia.Interactivity
open Avalonia.Media
open Avalonia.Media.Immutable

[<AutoOpen>]
module SelectableTextBlock =  
    open Avalonia.FuncUI.Builder
    open Avalonia.FuncUI.Types

    let create (attrs: IAttr<SelectableTextBlock> list): IView<SelectableTextBlock> =
        ViewBuilder.Create<SelectableTextBlock>(attrs)

    type SelectableTextBlock with
        static member onCopyingToClipboard<'t when 't :> SelectableTextBlock>(func: RoutedEventArgs -> unit, ?subPatchOptions) : IAttr<'t> =
            AttrBuilder<'t>.CreateSubscription(SelectableTextBlock.CopyingToClipboardEvent, func, ?subPatchOptions = subPatchOptions)

        static member onSelectedTextChanged<'t when 't :> SelectableTextBlock>(func: string -> unit, ?subPatchOptions) : IAttr<'t> =
            AttrBuilder<'t>.CreateSubscription(SelectableTextBlock.SelectedTextProperty, func, ?subPatchOptions = subPatchOptions)

        static member onCanCopyChanged<'t when 't :> SelectableTextBlock>(func: bool -> unit, ?subPatchOptions) : IAttr<'t> =
            AttrBuilder<'t>.CreateSubscription(SelectableTextBlock.CanCopyProperty, func, ?subPatchOptions = subPatchOptions)

        static member selectionBrush<'t when 't :> SelectableTextBlock>(value: IBrush) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<IBrush>(SelectableTextBlock.SelectionBrushProperty, value, ValueNone)

        static member selectionBrush<'t when 't :> SelectableTextBlock>(color: string) : IAttr<'t> =
            Color.Parse(color) |> ImmutableSolidColorBrush |> SelectableTextBlock.selectionBrush

        static member selectionBrush<'t when 't :> SelectableTextBlock>(color: Color) : IAttr<'t> =
            ImmutableSolidColorBrush(color) |> SelectableTextBlock.selectionBrush

        static member selectionStart<'t when 't :> SelectableTextBlock>(value: int) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<int>(SelectableTextBlock.SelectionStartProperty, value, ValueNone)

        static member selectionEnd<'t when 't :> SelectableTextBlock>(value: int) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<int>(SelectableTextBlock.SelectionEndProperty, value, ValueNone)
