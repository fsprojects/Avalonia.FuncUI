---
layout: control
name: Calendar
group: controls
---
[Calendar]: https://avaloniaui.net/docs/controls/calendar
[Calendar API]: https://avaloniaui.net/api/Avalonia.Controls/Calendar/
[Calendar.fs]: https://github.com/AvaloniaCommunity/Avalonia.FuncUI/blob/master/src/Avalonia.FuncUI.DSL/Calendar/Calendar.fs
[CalendarMode]: https://avaloniaui.net/api/Avalonia.Controls/CalendarMode/


> For more information about this control you can visit [Calendar] and [Calendar API] as well as [Calendar.fs]

Displays a Calendar control that allows you to select dates, this control allows you to display a start and end date

## Usage

**Set Selected Date**
```fsharp
Calendar.create [
    Calendar.selectedDate state.date
]
```

**Set Start Date**

Sets the first date that is available in the calendar, any date before is not shown
```fsharp
Calendar.create [
    Calendar.displayDateStart DateTime.Today
]
```

**Set End Date**

Sets the last date that is available in the calendar, any date after is not shown
```fsharp
let tenDaysFromNow = DateTime.Today.Add(TimeSpan.FromDays(10.0))
Calendar.create [
    Calendar.displayDateEnd tenDaysFromNow
]
```

**Set Calendar Mode**

Sets the way the selection for the date is presented to the user using [CalendarMode]

*Decade*, *Year*, *Month*
> ![Decade](https://i.imgur.com/jgYIFb6.png) ![Year](https://imgur.com/trgx9WW.png) ![Month](https://imgur.com/J0MUdPU.png)

```fsharp
Calendar.create [
    Calendar.displayMode CalendarMode.Decade
]
Calendar.create [
    Calendar.displayMode CalendarMode.Year
]
Calendar.create [
    Calendar.displayMode CalendarMode.Month
]
```

**Set First Day of the Week**

Sets the way a user can select dates in the calendar
```fsharp
Calendar.create [
    Calendar.firstDayOfWeek System.DayOfWeek.Thursday
]
```

**Set Calendar Selection Mode**
```fsharp
Calendar.create [
    Calendar.selectionMode CalendarSelectionMode.SingleDate
]
```
> ***Note***: While the Avalonia Calendar supports ranges and multiple ranges Avalonia.FuncUI does not have a way to present the internal collection of dates in a functional way therefore selecting ranges or multiple ranges is not currently supported. We will try to address this situation for multiple controls in the future.
