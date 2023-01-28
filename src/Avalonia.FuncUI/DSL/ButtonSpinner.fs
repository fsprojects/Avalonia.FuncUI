namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module ButtonSpinner =
    open Avalonia.Controls
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder

    let create (attrs: Attr<ButtonSpinner> list): IView<ButtonSpinner> =
        ViewBuilder.Create<ButtonSpinner>(attrs)

    type ButtonSpinner with

        /// <summary>
        /// Sets a value indicating whether the <see cref="ButtonSpinner"/> should allow to spin.
        /// </summary>
        static member allowSpin<'t when 't :> ButtonSpinner>(value: bool) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(ButtonSpinner.AllowSpinProperty, value, ValueNone)

        /// <summary>
        /// Sets a value indicating whether the spin buttons should be shown.
        /// </summary>
        static member showButtonSpinner<'t when 't :> ButtonSpinner>(value: bool) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(ButtonSpinner.ShowButtonSpinnerProperty, value, ValueNone)

        /// <summary>
        /// Sets current location of the <see cref="ButtonSpinner"/>.
        /// </summary>
        static member buttonSpinnerLocation<'t when 't :> ButtonSpinner>(value: Location) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<Location>(ButtonSpinner.ButtonSpinnerLocationProperty, value, ValueNone)