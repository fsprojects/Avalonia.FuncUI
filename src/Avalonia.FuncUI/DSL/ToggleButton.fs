namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module ToggleButton =
    open System
    open Avalonia.Controls.Primitives
    open Avalonia.Interactivity
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder

    let create (attrs: Attr<ToggleButton> list): IView<ToggleButton> =
        ViewBuilder.Create<ToggleButton>(attrs)

    type ToggleButton with
        static member isThreeState<'t when 't :> ToggleButton>(value: bool) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(ToggleButton.IsThreeStateProperty, value, ValueNone)

        static member isChecked<'t when 't :> ToggleButton>(value: Nullable<bool>) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<Nullable<bool>>(ToggleButton.IsCheckedProperty, value, ValueNone)

        static member isChecked<'t when 't :> ToggleButton>(value: bool) : Attr<'t> =
            value |> Nullable |> ToggleButton.isChecked

        static member isChecked<'t when 't :> ToggleButton>(value: bool option) : Attr<'t> =
            value |> Option.toNullable |> ToggleButton.isChecked

        [<Obsolete "use 'onChecked' or 'onUnchecked' instead">]
        static member onIsCheckedChanged<'t when 't :> ToggleButton>(func: Nullable<bool> -> unit, ?subPatchOptions) : Attr<'t> =
            AttrBuilder<'t>.CreateSubscription<Nullable<bool>>(ToggleButton.IsCheckedProperty, func, ?subPatchOptions = subPatchOptions)

        static member onChecked<'t when 't :> ToggleButton>(func: RoutedEventArgs -> unit, ?subPatchOptions) : Attr<'t> =
            AttrBuilder<'t>.CreateSubscription<RoutedEventArgs>(ToggleButton.CheckedEvent, func, ?subPatchOptions = subPatchOptions)

        static member onUnchecked<'t when 't :> ToggleButton>(func: RoutedEventArgs -> unit, ?subPatchOptions) : Attr<'t> =
            AttrBuilder<'t>.CreateSubscription<RoutedEventArgs>(ToggleButton.UncheckedEvent, func, ?subPatchOptions = subPatchOptions)

        static member onIndeterminate<'t when 't :> ToggleButton>(func : RoutedEventArgs -> unit, ?subPatchOptions) : Attr<'t> =
            AttrBuilder<'t>.CreateSubscription<RoutedEventArgs>(ToggleButton.IndeterminateEvent, func, ?subPatchOptions = subPatchOptions)