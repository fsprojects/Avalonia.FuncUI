namespace Avalonia.FuncUI.DSL

open System
open Avalonia.Controls

[<AutoOpen>]
module TimePickerPresenter =
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder

    let create (attrs: IAttr<TimePickerPresenter> list): IView<TimePickerPresenter> =
        ViewBuilder.Create<TimePickerPresenter>(attrs)

    type TimePickerPresenter with
        /// Gets or sets the minute increment in the selector
        static member minuteIncrement<'t when 't :> TimePickerPresenter>(value: int) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<int>(TimePickerPresenter.MinuteIncrementProperty, value, ValueNone)

        /// Gets or sets the second increment in the selector
        static member secondIncrement<'t when 't :> TimePickerPresenter>(value: int) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<int>(TimePickerPresenter.SecondIncrementProperty, value, ValueNone)

        /// Gets or sets the current clock identifier, either 12HourClock or 24HourClock
        static member clockIdentifier<'t when 't :> TimePickerPresenter>(value: string) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<string>(TimePickerPresenter.ClockIdentifierProperty, value, ValueNone)

        /// Gets or sets the current time
        static member time<'t when 't :> TimePickerPresenter>(value: TimeSpan) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<TimeSpan>(TimePickerPresenter.TimeProperty, value, ValueNone)

        /// Gets or sets a value indicating whether seconds are displayed in the picker or not
        static member useSeconds<'t when 't :> TimePickerPresenter>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(TimePickerPresenter.UseSecondsProperty, value, ValueNone)