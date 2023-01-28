namespace Avalonia.FuncUI.DSL

open Avalonia

[<AutoOpen>]
module Button =
    open System.Windows.Input
    open Avalonia.Controls
    open Avalonia.Interactivity
    open Avalonia.Input
    open Avalonia.FuncUI.Builder
    open Avalonia.FuncUI.Types

    let create (attrs: Attr<Button> list): View<Button> =
        ViewBuilder.Create<Button>(attrs)

    type Button with

        static member clickMode<'t when 't :> Button>(value: ClickMode) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<ClickMode>(Button.ClickModeProperty, value, ValueNone)

        static member command<'t when 't :> Button>(value: ICommand) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<ICommand>(Button.CommandProperty, value, ValueNone)

        static member hotKey<'t when 't :> Button>(value: KeyGesture) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<KeyGesture>(Button.HotKeyProperty, value, ValueNone)

        static member commandParameter<'t when 't :> Button>(value: obj) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<obj>(Button.CommandParameterProperty, value, ValueNone)

        static member isDefault<'t when 't :> Button>(value: bool) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(Button.IsDefaultProperty, value, ValueNone)

        static member isPressed<'t when 't :> Button>(value: bool) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(Button.IsPressedProperty, value, ValueNone)

        static member onIsPressedChanged<'t when 't :> Button>(func: bool -> unit, ?subPatchOptions) =
            AttrBuilder<'t>.CreateSubscription<bool>(Button.IsPressedProperty, func, ?subPatchOptions = subPatchOptions)

        static member onClick<'t when 't :> Button>(func: RoutedEventArgs -> unit, ?subPatchOptions) =
            AttrBuilder<'t>.CreateSubscription<RoutedEventArgs>(Button.ClickEvent, func, ?subPatchOptions = subPatchOptions)

        static member flyout<'t when 't :> Button>(value: IView option) : Attr<'t> =
            AttrBuilder<'t>.CreateContentSingle(Button.FlyoutProperty, value)

        static member flyout<'t when 't :> Button>(value: IView) : Attr<'t> =
            value
            |> Some
            |> Button.flyout
