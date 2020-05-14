namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module DatePicker =
    open System
    open Avalonia.Controls
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
    
    let create (attrs: IAttr<DatePicker> list): IView<DatePicker> =
        ViewBuilder.Create<DatePicker>(attrs)
     
    type DatePicker with
  
        static member displayDate<'t when 't :> DatePicker>(value: DateTime) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, DateTime>(DatePicker.DisplayDateProperty, value, ValueNone)

        static member displayDateStart<'t when 't :> DatePicker>(value: DateTime Nullable) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, DateTime Nullable>(DatePicker.DisplayDateStartProperty, value, ValueNone)

        static member displayDateStart<'t when 't :> DatePicker>(value: DateTime) : IAttr<'t> =
            value |> Nullable |> DatePicker.displayDateStart

        static member displayDateStart<'t when 't :> DatePicker>(value: DateTime option) : IAttr<'t> =
            value |> Option.toNullable |> DatePicker.displayDateStart

        static member displayDateEnd<'t when 't :> DatePicker>(value: DateTime Nullable) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, DateTime Nullable>(DatePicker.DisplayDateEndProperty, value, ValueNone)

        static member displayDateEnd<'t when 't :> DatePicker>(value: DateTime) : IAttr<'t> =
            value |> Nullable |> DatePicker.displayDateEnd

        static member displayDateEnd<'t when 't :> DatePicker>(value: DateTime option) : IAttr<'t> =
            value |> Option.toNullable |> DatePicker.displayDateEnd

        static member firstDayOfWeek<'t when 't :> DatePicker>(value: DayOfWeek) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, DayOfWeek>(DatePicker.FirstDayOfWeekProperty, value, ValueNone)

        static member isDropDownOpen<'t when 't :> DatePicker>(value: bool) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, bool>(DatePicker.IsDropDownOpenProperty, value, ValueNone)

        static member onDropDownOpenChanged<'t when 't :> DatePicker>(func: bool -> unit, ?subPatchOptions) : IAttr<'t> =
            AttrBuilder.CreateSubscription(DatePicker.IsDropDownOpenProperty, func, ?subPatchOptions = subPatchOptions)
        
        static member isTodayHighlighted<'t when 't :> DatePicker>(value: bool) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, bool>(DatePicker.IsTodayHighlightedProperty , value, ValueNone)

        static member selectedDate<'t when 't :> DatePicker>(value: DateTime Nullable) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, DateTime Nullable>(DatePicker.SelectedDateProperty, value, ValueNone)

        static member selectedDate<'t when 't :> DatePicker>(value: DateTime) : IAttr<'t> =
            value |> Nullable |> DatePicker.selectedDate

        static member selectedDate<'t when 't :> DatePicker>(value: DateTime option) : IAttr<'t> =
            value |> Option.toNullable |> DatePicker.selectedDate

        static member onSelectedDateChanged<'t when 't :> DatePicker>(func: Nullable<DateTime> -> unit, ?subPatchOptions) : IAttr<'t> =
            AttrBuilder.CreateSubscription<'t, DateTime Nullable>(DatePicker.SelectedDateProperty, func, ?subPatchOptions = subPatchOptions)

        static member selectedDateFormat<'t when 't :> DatePicker>(value: DatePickerFormat) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, DatePickerFormat>(DatePicker.SelectedDateFormatProperty, value, ValueNone)

        static member customDateFormatString<'t when 't :> DatePicker>(value: string) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, string>(DatePicker.CustomDateFormatStringProperty, value, ValueNone)

        static member text<'t when 't :> DatePicker>(value: string) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, string>(DatePicker.TextProperty, value, ValueNone)

        static member watermark<'t when 't :> DatePicker>(value: string) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, string>(DatePicker.WatermarkProperty, value, ValueNone)

        static member useFloatingWatermark<'t when 't :> DatePicker>(value: bool) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, bool>(DatePicker.UseFloatingWatermarkProperty, value, ValueNone)

