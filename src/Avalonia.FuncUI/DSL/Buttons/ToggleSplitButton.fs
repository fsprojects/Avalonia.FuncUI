namespace Avalonia.FuncUI.DSL

open Avalonia.Controls
open Avalonia.Interactivity

[<AutoOpen>]
module ToggleSplitButton =
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder

    let create (attrs: IAttr<ToggleSplitButton> list): IView<ToggleSplitButton> =
        ViewBuilder.Create<ToggleSplitButton>(attrs)

    type ToggleSplitButton with

        /// <summary>
        /// Raised when the <see cref="IsChecked"/> property value changes.
        /// </summary>
        static member onIsCheckedChanged<'t when 't :> ToggleSplitButton>(func: RoutedEventArgs -> unit, ?subPatchOptions) =
            AttrBuilder<'t>.CreateSubscription<RoutedEventArgs>(ToggleSplitButton.IsCheckedChangedEvent, func, ?subPatchOptions = subPatchOptions)

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="ToggleSplitButton"/> is checked.
        /// </summary>
        static member isChecked<'t when 't :> ToggleSplitButton>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(ToggleSplitButton.IsCheckedProperty, value, ValueNone)
