namespace Avalonia.FuncUI.DSL

open System.Globalization

[<AutoOpen>]
module NumericUpDown =
    open Avalonia.Controls
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder

    let create (attrs: IAttr<NumericUpDown> list): IView<NumericUpDown> =
        ViewBuilder.Create<NumericUpDown>(attrs)

    type NumericUpDown with

        /// <summary>
        /// Sets a value indicating whether the <see cref="NumericUpDown"/> should allow to spin.
        /// </summary>
        static member allowSpin<'t when 't :> NumericUpDown>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(NumericUpDown.AllowSpinProperty, value, ValueNone)

        /// <summary>
        /// Sets a value indicating whether the up and down buttons should be shown.
        /// </summary>
        static member showButtonSpinner<'t when 't :> NumericUpDown>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(NumericUpDown.ShowButtonSpinnerProperty, value, ValueNone)

        /// <summary>
        /// Sets current location of the <see cref="NumericUPDown"/>.
        /// </summary>
        static member buttonSpinnerLocation<'t when 't :> NumericUpDown>(value: Location) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<Location>(NumericUpDown.ButtonSpinnerLocationProperty, value, ValueNone)

        /// <summary>
        /// Sets if the value should be clipped if the minimum/maximum is reached
        /// </summary>
        static member clipValueToMinMax<'t when 't :> NumericUpDown>(value: bool) : IAttr<'t> =
            let getter : ('t -> bool) = (fun control -> control.ClipValueToMinMax)
            let setter : ('t * bool -> unit) = (fun (control, value) -> control.ClipValueToMinMax <- value)

            AttrBuilder<'t>.CreateProperty<bool>("ClipValueToMinMax", value, ValueSome getter, ValueSome setter, ValueNone)

        /// <summary>
        /// Sets the culture info used for formatting
        /// </summary>
        static member cultureInfo<'t when 't :> NumericUpDown>(value: CultureInfo) : IAttr<'t> =
            let getter : ('t -> CultureInfo) = (fun control -> control.CultureInfo)
            let setter : ('t * CultureInfo -> unit) = (fun (control, value) -> control.CultureInfo <- value)

            AttrBuilder<'t>.CreateProperty<CultureInfo>("ClipValueToMinMax", value, ValueSome getter, ValueSome setter, ValueNone)

        static member formatString<'t when 't :> NumericUpDown>(value: string) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<string>(NumericUpDown.FormatStringProperty, value, ValueNone)

        static member increment<'t when 't :> NumericUpDown>(value: double) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<double>(NumericUpDown.IncrementProperty, value, ValueNone)

        static member isReadOnly<'t when 't :> NumericUpDown>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(NumericUpDown.IsReadOnlyProperty, value, ValueNone)

        static member minimum<'t when 't :> NumericUpDown>(value: double) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<double>(NumericUpDown.MinimumProperty, value, ValueNone)

        static member maximum<'t when 't :> NumericUpDown>(value: double) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<double>(NumericUpDown.MaximumProperty, value, ValueNone)

        static member parsingNumberStyle<'t when 't :> NumericUpDown>(value: NumberStyles) : IAttr<'t> =
            let getter : ('t -> NumberStyles) = (fun control -> control.ParsingNumberStyle)
            let setter : ('t * NumberStyles -> unit) = (fun (control, value) -> control.ParsingNumberStyle <- value)

            AttrBuilder<'t>.CreateProperty<NumberStyles>("ParsingNumberStyle", value, ValueSome getter, ValueSome setter, ValueNone)

        static member text<'t when 't :> NumericUpDown>(value: string) : IAttr<'t> =
            let getter : ('t -> string) = (fun control -> control.Text)
            let setter : ('t * string -> unit) = (fun (control, value) -> control.Text <- value)

            AttrBuilder<'t>.CreateProperty<string>("Text", value, ValueSome getter, ValueSome setter, ValueNone)

        static member onTextChanged<'t when 't :> NumericUpDown>(func: string -> unit) =
            AttrBuilder<'t>.CreateSubscription<string>(NumericUpDown.TextProperty, func)

        static member value<'t when 't :> NumericUpDown>(value: double) : IAttr<'t> =
            let getter : ('t -> double) = (fun control -> control.Value)
            let setter : ('t * double -> unit) = (fun (control, value) -> control.Value <- value)

            AttrBuilder<'t>.CreateProperty<double>("Value", value, ValueSome getter, ValueSome setter, ValueNone)

        static member onValueChanged<'t when 't :> NumericUpDown>(func: double -> unit) =
            AttrBuilder<'t>.CreateSubscription<double>(NumericUpDown.ValueProperty, func)

        static member watermark<'t when 't :> NumericUpDown>(value: string) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<string>(NumericUpDown.WatermarkProperty, value, ValueNone)


