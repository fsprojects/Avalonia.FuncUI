namespace Avalonia.FuncUI.DSL



[<AutoOpen>]
module Calendar =
    open System
    open Avalonia.Media
    open Avalonia.Media.Immutable
    open Avalonia.Controls
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
    
    let create (attrs: IAttr<Calendar> list): IView<Calendar> =
        ViewBuilder.Create<Calendar>(attrs)
     
    type Calendar with
  
        static member firstDayOfWeek<'t when 't :> Calendar>(value: DayOfWeek) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<DayOfWeek>(Calendar.FirstDayOfWeekProperty, value, ValueNone)
        
        static member isTodayHighlighted<'t when 't :> Calendar>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(Calendar.IsTodayHighlightedProperty , value, ValueNone)

        static member headerBackground<'t when 't :> Calendar>(value: IBrush) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<IBrush>(Calendar.HeaderBackgroundProperty, value, ValueNone)
        
        static member headerBackground<'t when 't :> Calendar>(color: string) : IAttr<'t> =
            color |> Color.Parse |> ImmutableSolidColorBrush |> Calendar.headerBackground

        static member displayMode<'t when 't :> Calendar>(value: CalendarMode) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<CalendarMode>(Calendar.DisplayModeProperty, value, ValueNone)

        static member onDisplayModeChanged<'t when 't :> Calendar>(func: CalendarMode -> unit, ?subPatchOptions) : IAttr<'t> =
            AttrBuilder<'t>.CreateSubscription<CalendarMode>(Calendar.DisplayModeProperty, func, ?subPatchOptions = subPatchOptions)

        static member selectionMode<'t when 't :> Calendar>(value: CalendarSelectionMode) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<CalendarSelectionMode>(Calendar.SelectionModeProperty, value, ValueNone)

        static member onSelectionModeChanged<'t when 't :> Calendar>(func: CalendarSelectionMode  -> unit, ?subPatchOptions) : IAttr<'t> =
            AttrBuilder<'t>.CreateSubscription<CalendarSelectionMode >(Calendar.SelectionModeProperty, func, ?subPatchOptions = subPatchOptions)

        static member selectedDate<'t when 't :> Calendar>(value: DateTime Nullable) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<DateTime Nullable>(Calendar.SelectedDateProperty, value, ValueNone)

        static member selectedDate<'t when 't :> Calendar>(value: DateTime) : IAttr<'t> =
            value |> Nullable |> Calendar.selectedDate

        static member selectedDate<'t when 't :> Calendar>(value: DateTime option) : IAttr<'t> =
            value |> Option.toNullable |> Calendar.selectedDate

        static member onSelectedDateChanged<'t when 't :> Calendar>(func: Nullable<DateTime> -> unit, ?subPatchOptions) : IAttr<'t> =
            AttrBuilder<'t>.CreateSubscription<DateTime Nullable>(Calendar.SelectedDateProperty, func, ?subPatchOptions = subPatchOptions)

        static member displayDate<'t when 't :> Calendar>(value: DateTime) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<DateTime>(Calendar.DisplayDateProperty, value, ValueNone)

        static member displayDateStart<'t when 't :> Calendar>(value: DateTime Nullable) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<DateTime Nullable>(Calendar.DisplayDateStartProperty, value, ValueNone)

        static member displayDateStart<'t when 't :> Calendar>(value: DateTime) : IAttr<'t> =
            value |> Nullable |> Calendar.displayDateStart

        static member displayDateStart<'t when 't :> Calendar>(value: DateTime option) : IAttr<'t> =
            value |> Option.toNullable |> Calendar.displayDateStart

        static member displayDateEnd<'t when 't :> Calendar>(value: DateTime Nullable) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<DateTime Nullable>(Calendar.DisplayDateEndProperty, value, ValueNone)

        static member displayDateEnd<'t when 't :> Calendar>(value: DateTime) : IAttr<'t> =
            value |> Nullable |> Calendar.displayDateEnd

        static member displayDateEnd<'t when 't :> Calendar>(value: DateTime option) : IAttr<'t> =
            value |> Option.toNullable |> Calendar.displayDateEnd

