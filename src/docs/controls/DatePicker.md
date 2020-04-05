---
layout: control
name: DatePicker
group: controls
---
[DatePicker API]: https://avaloniaui.net/api/Avalonia.Controls/DatePicker/
[DatePicker.fs]: https://github.com/AvaloniaCommunity/Avalonia.FuncUI/blob/master/src/Avalonia.FuncUI.DSL/Calendar/DatePicker.fs
[Custom date and time format strings]: https://docs.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings/
[DatePickerFormat]: http://avaloniaui.net/api/Avalonia.Controls/DatePickerFormat/

> *Note*: You can check the Avalonia docs for the [DatePicker API] if you need more information.
> 
> For Avalonia.FuncUI's DSL properties you can check [DatePicker.fs]

The DatePicker control is a single date picker that displays a calendar, it is also posible to enter a date via the TextBox the control has

## Usage

**Set Date**
```fsharp
DatePicker.create [
    DatePicker.selectedDate DateTime.Today
]
```

**Set DateFormat**
```fsharp
DatePicker.create [
    DatePicker.selectedDateFormat DatePickerFormat.Long
]

DatePicker.create [
    DatePicker.selectedDateFormat DatePickerFormat.Short
]

DatePicker.create [
    DatePicker.selectedDateFormat DatePickerFormat.Custom
    // It can be any valid DateFormat string
    DatePicker.customDateFormatString "MMMM dd, yyyy"
]
```
> For more information about the DatePickerFormat check [DatePickerFormat]

>You can check [Custom date and time format strings] Microsoft docs for more information about the format string

**Set Start Display Date**

Sets the first date available to display
```fsharp
let startFromYesterday =
    DateTime.Today.Subtract(TimeSpan.FromDays(1.0))
DatePicker.create [
    DatePicker.displayDateStart startFromYesterday
]
```

**Set End Display Date**

Sets the last date available to display
```fsharp
let showUpToTomorrow =
    DateTime.Today.Add(TimeSpan.FromDays(1.0))
DatePicker.create [
    DatePicker.displayDateStart showUpToTomorrow
]
```

**Set Watermark**

Sets the watermark (placeholder) for the TextBox that is included in this control
```fsharp
DatePicker.create [
    DatePicker.watermark "Select a date"
]
```


