# TimePicker

> _Note_: You can check the Avalonia docs for the [TimePicker](https://docs.avaloniaui.net/docs/controls/timepicker) and [TimePicker API](http://reference.avaloniaui.net/api/Avalonia.Controls/TimePicker/) if you need more information.
>
> For Avalonia.FuncUI's DSL properties you can check [TimePicker.fs](https://github.com/fsprojects/Avalonia.FuncUI/blob/master/src/Avalonia.FuncUI/DSL/TimePicker.fs)

The TimePicker control allows the user to pick a time value.

### Usage

**Create a TimePicker**

```fsharp
TimePicker.create [
    TimePicker.header "Arrival Time"
]
```

**Change in 15 Minute Increments**

```fsharp
TimePicker.create [
    TimePicker.header "Arrival Time"
    TimePicker.minuteIncrement 15
]
```

**Choose a 12 or 24 Hour Clock**

For more information about the attribute values here you can check out the documentation for [TimeSpan](https://docs.microsoft.com/en-us/dotnet/api/system.timespan) and the [ClockIdentifier](https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.controls.timepicker.clockidentifier?view=winrt-19041#Windows\_UI\_Xaml\_Controls\_TimePicker\_ClockIdentifier).

```fsharp
TimePicker.create [
    TimePicker.header "12 Hour Clock"
    TimePicker.selectedTime (TimeSpan(14, 30, 0))
    TimePicker.clockIdentifier "12HourClock"
]

TimePicker.create [
    TimePicker.header "24 Hour Clock"
    TimePicker.selectedTime (TimeSpan(14, 30, 0))
    TimePicker.clockIdentifier "24HourClock"
]
```
