namespace Avalonia.FuncUI.DSL

open System
open Avalonia.Controls

[<AutoOpen>]
module DatePickerPresenter =
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder

    let create (attrs: IAttr<DatePickerPresenter> list): IView<DatePickerPresenter> =
        ViewBuilder.Create<DatePickerPresenter>(attrs)

    type DatePickerPresenter with
        /// Gets or sets the current Date for the picker
        static member date<'t when 't :> DatePickerPresenter>(value: DateTimeOffset) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<DateTimeOffset>(DatePickerPresenter.DateProperty, value, ValueNone)

        /// Gets or sets the DayFormat
        static member dayFormat<'t when 't :> DatePickerPresenter>(value: string) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<string>(DatePickerPresenter.DayFormatProperty, value, ValueNone)

        /// Get or sets whether the Day selector is visible
        static member dayVisible<'t when 't :> DatePickerPresenter>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(DatePickerPresenter.DayVisibleProperty, value, ValueNone)

        /// Gets or sets the maximum pickable year
        static member maxYear<'t when 't :> DatePickerPresenter>(value: DateTimeOffset) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<DateTimeOffset>(DatePickerPresenter.MaxYearProperty, value, ValueNone)

        /// Gets or sets the minimum pickable year
        static member minYear<'t when 't :> DatePickerPresenter>(value: DateTimeOffset) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<DateTimeOffset>(DatePickerPresenter.MinYearProperty, value, ValueNone)

        /// Gets or sets the month format
        static member monthFormat<'t when 't :> DatePickerPresenter>(value: string) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<string>(DatePickerPresenter.MonthFormatProperty, value, ValueNone)

        /// Gets or sets whether the month selector is visible
        static member monthVisible<'t when 't :> DatePickerPresenter>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(DatePickerPresenter.MonthVisibleProperty, value, ValueNone)

        /// Gets or sets the year format
        static member yearFormat<'t when 't :> DatePickerPresenter>(value: string) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<string>(DatePickerPresenter.YearFormatProperty, value, ValueNone)

        /// Gets or sets whether the year selector is visible
        static member yearVisible<'t when 't :> DatePickerPresenter>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(DatePickerPresenter.YearVisibleProperty, value, ValueNone)
