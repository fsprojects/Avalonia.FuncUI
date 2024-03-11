# DatePicker

> _Note_: You can check the Avalonia docs for the [DatePicker](https://docs.avaloniaui.net/docs/controls/datepicker) and [DatePicker API](http://reference.avaloniaui.net/api/Avalonia.Controls/DatePicker/) if you need more information.
>
> For Avalonia.FuncUI's DSL properties you can check [DatePicker.fs](https://github.com/AvaloniaCommunity/Avalonia.FuncUI/blob/master/src/Avalonia.FuncUI.DSL/DatePicker.fs)

The DatePicker control is a single date picker that displays a calendar, it is also possible to enter a date via the TextBox the control has

### Usage

**Set Label**

```fsharp
DatePicker.create [
  DatePicker.header "Title"
]
```

**Set Date**

```fsharp
DatePicker.create [
  DatePicker.selectedDate DateTime.Today
]
```

**Set DateFormat**

```fsharp
DatePicker.create [
  DatePicker.yearFormat "yyyy"
  DatePicker.monthFormat "MMMM"
  DatePicker.dayFormat "dd"
]
```

> You can check [Custom date and time format strings](https://docs.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings/) Microsoft docs for more information about the format strings.

**Limit Year Range**

> You can check [DateTimeOffset](https://docs.microsoft.com/en-us/dotnet/api/system.datetimeoffset?view=net-6.0) Microsoft docs for more information about setting time offsets.

```fsharp
DatePicker.create [
  DatePicker.maxYear (DateTimeOffset(DateTime.Now))
]
```

**Show Only Month and Year**

You can control the visibility of the day, month, and year with similarly named functions.

```fsharp
DatePicker.create [
  DatePicker.dayVisible false
]
```

**Register Selected Date**

```fsharp
DatePicker.create [
  DatePicker.onSelectedDateChanged (fun dateOffset -> OnChangeDateOffset dateOffset |> dispatch)
]
```
