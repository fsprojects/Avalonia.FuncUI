# CalendarDatePicker

> _Note_: You can check the Avalonia docs for the [CalendarDatePicker API](http://reference.avaloniaui.net/api/Avalonia.Controls/CalendarDatePicker/) if you need more information.
>
> For Avalonia.FuncUI's DSL properties you can check [CalendarDatePicker.fs](https://github.com/fsprojects/Avalonia.FuncUI/blob/master/src/Avalonia.FuncUI/DSL/Calendar/CalendarDatePicker.fs)

The CalendarDatePicker control is a single date picker that displays a calendar, it is also possible to enter a date via the TextBox the control has

### Usage

**Set Date**

```fsharp
CalendarDatePicker.create [
  CalendarDatePicker.selectedDate DateTime.Today
]
```

**Set DateFormat**

```fsharp
CalendarDatePicker.create [
  CalendarDatePicker.selectedDateFormat DatePickerFormat.Long
]

CalendarDatePicker.create [
  CalendarDatePicker.selectedDateFormat DatePickerFormat.Short
]

CalendarDatePicker.create [
  CalendarDatePicker.selectedDateFormat DatePickerFormat.Custom
  // It can be any valid DateFormat string
  CalendarDatePicker.customDateFormatString "MMMM dd, yyyy"
]
```

> For more information about the CalendarDatePickerFormat check [DatePickerFormat](http://reference.avaloniaui.net/api/Avalonia.Controls/DatePickerFormat/)

> You can check [Custom date and time format strings](https://docs.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings/) Microsoft docs for more information about the format string

**Set Start Display Date**

Sets the first date available to display

```fsharp
let startFromYesterday =
   DateTime.Today.Subtract(TimeSpan.FromDays(1.0))
CalendarDatePicker.create [
  CalendarDatePicker.displayDateStart startFromYesterday
]
```

**Set End Display Date**

Sets the last date available to display

```fsharp
let showUpToTomorrow =
  DateTime.Today.Add(TimeSpan.FromDays(1.0))
CalendarDatePicker.create [
  CalendarDatePicker.displayDateStart showUpToTomorrow
]
```

**Set Watermark**

Sets the watermark (placeholder) for the TextBox that is included in this control

```fsharp
CalendarDatePicker.create [
  CalendarDatePicker.watermark "Select a date"
]
```
