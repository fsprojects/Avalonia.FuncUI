# Calendar

> _Note_: You can check the Avalonia docs for the [Calendar](https://docs.avaloniaui.net/docs/controls/calendar) and [Calendar API](http://reference.avaloniaui.net/api/Avalonia.Controls/Calendar/) if you need more information.
>
> For Avalonia.FuncUI's DSL properties you can check [Calendar.fs](https://github.com/AvaloniaCommunity/Avalonia.FuncUI/blob/master/src/Avalonia.FuncUI.DSL/Calendar/Calendar.fs)

The Calendar control is a standard Calendar control for users to select date(s) or date ranges.

### Usage

**Set a Date**

If no calendar date is set, it will default to today's date.

```fsharp
Calendar.create [
  Calendar.selectedDate DateTime.Today
]
```

**Select Multiple Dates**

For more information about selection modes you can see the [SelectionModes API](http://reference.avaloniaui.net/api/Avalonia.Controls/SelectionMode/) but more verbosely you can look at the [ListBox Selection Modes](https://docs.avaloniaui.net/docs/controls/listbox#selectionmode) for a better description of what they do.

```fsharp
Calendar.create [
  Calendar.selectionMode SelectionMode.Multiple
]
```

**Select Calendar Month First**

You can change the [CalendarMode](http://reference.avaloniaui.net/api/Avalonia.Controls/CalendarMode/) so that you can select the year first with `CalendarMode.Decade`, the month first with `CalendarMode.Year`, or have the standard (default) format with `CalendarMode.Month`.jok

```fsharp
Calendar.create [
  Calendar.displayMode CalendarMode.Decade
]
```

**Display Only the Upcoming Week**

```fsharp
Calendar.create [
  Calendar.displayDateStart DateTime.Today
  Calendar.displayDateEnd (DateTime.Today + TimeSpan(7, 0, 0, 0, 0)) // TimeSpan constructor for 7 days
]
```
