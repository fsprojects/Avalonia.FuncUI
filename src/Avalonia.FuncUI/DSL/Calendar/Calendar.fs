namespace Avalonia.FuncUI.DSL



[<AutoOpen>]
module Calendar =
    open System
    open Avalonia.Controls.Primitives
    open Avalonia.Media
    open Avalonia.Media.Immutable
    open Avalonia.Controls
    open Avalonia.FuncUI
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder

    let create (attrs: IAttr<Calendar> list): IView<Calendar> =
        ViewBuilder.Create<Calendar>(attrs)

    type Calendar with

        static member onSelectedDatesChanged<'t when 't :> Calendar>(func: SelectionChangedEventArgs -> unit, ?subPatchOptions) : IAttr<'t> =
            let name = nameof Unchecked.defaultof<'t>.SelectedDatesChanged
            let factory: SubscriptionFactory<SelectionChangedEventArgs> =
                fun (control, func, token) ->
                    let control = control :?> 't
                    let handler = EventHandler<SelectionChangedEventArgs>(fun s e -> func e)
                    let event = control.SelectedDatesChanged

                    event.AddHandler(handler)
                    token.Register(fun () -> event.RemoveHandler(handler)) |> ignore

            AttrBuilder<'t>.CreateSubscription(name, factory, func, ?subPatchOptions = subPatchOptions)

        static member onDisplayDateChanged<'t when 't :> Calendar>(func: CalendarDateChangedEventArgs -> unit, ?subPatchOptions) : IAttr<'t> =
            let name = nameof Unchecked.defaultof<'t>.DisplayDateChanged
            let factory: SubscriptionFactory<CalendarDateChangedEventArgs> =
                fun (control, func, token) ->
                    let control = control :?> 't
                    let handler = EventHandler<CalendarDateChangedEventArgs>(fun s e -> func e)
                    let event = control.DisplayDateChanged

                    event.AddHandler(handler)
                    token.Register(fun () -> event.RemoveHandler(handler)) |> ignore

            AttrBuilder<'t>.CreateSubscription(name, factory, func, ?subPatchOptions = subPatchOptions)

        static member firstDayOfWeek<'t when 't :> Calendar>(value: DayOfWeek) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<DayOfWeek>(Calendar.FirstDayOfWeekProperty, value, ValueNone)

        static member isTodayHighlighted<'t when 't :> Calendar>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(Calendar.IsTodayHighlightedProperty , value, ValueNone)

        static member headerBackground<'t when 't :> Calendar>(value: IBrush) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<IBrush>(Calendar.HeaderBackgroundProperty, value, ValueNone)

        static member headerBackground<'t when 't :> Calendar>(color: string) : IAttr<'t> =
            color |> Color.Parse |> ImmutableSolidColorBrush |> Calendar.headerBackground

        static member headerBackground<'t when 't :> Calendar>(color: Color) : IAttr<'t> =
            color |> ImmutableSolidColorBrush |> Calendar.headerBackground

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

        static member selectedDates<'t when 't :> Calendar>(value: SelectedDatesCollection) : IAttr<'t> =
            let name = nameof Unchecked.defaultof<'t>.SelectedDates
            let getter: 't -> SelectedDatesCollection = fun control -> control.SelectedDates
            let setter: 't * SelectedDatesCollection -> unit = fun (control, value) -> Setters.iList control.SelectedDates value
            let compare: Comparer = EqualityComparers.compareSeq<SelectedDatesCollection,_>

            AttrBuilder<'t>.CreateProperty(name, value, ValueSome getter, ValueSome setter, ValueSome compare)

        static member selectedDates<'t when 't :> Calendar>(value: DateTime list) : IAttr<'t> =
            let name = nameof Unchecked.defaultof<'t>.SelectedDates
            let getter: 't -> DateTime list = fun control -> control.SelectedDates |> Seq.toList
            let setter: 't * DateTime list -> unit = fun (control, value) -> Setters.iList control.SelectedDates value

            AttrBuilder<'t>.CreateProperty(name, value, ValueSome getter, ValueSome setter, ValueNone)

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
