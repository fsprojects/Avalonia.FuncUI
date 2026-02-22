# RepeatButton

> _Note_: You can check the Avalonia docs for the [RepeatButton](https://docs.avaloniaui.net/docs/controls/repeatbutton) and [RepeatButton API](http://reference.avaloniaui.net/api/Avalonia.Controls/RepeatButton/) if you need more information.
>
> For Avalonia.FuncUI's DSL properties you can check [RepeatButton.fs](https://github.com/fsprojects/Avalonia.FuncUI/blob/master/src/Avalonia.FuncUI/DSL/RepeatButton.fs)

A `RepeatButton` is a subclasses of \[Button] so they share all the same attributes as described on that documentation page. The biggest difference is that when a `RepeatButton` is held down, the button submits multiple `onClick` events.

**Creating a RepeatButton**

The `RepeatButton.delay` sets the amount of time in milliseconds before the extra `onClick` events start triggering. The `RepeatButton.interval` sets the amount of time in milliseconds between successive `onClick` events

```fsharp
RepeatButton.create [
    RepeatButton.delay 100 // ms
    RepeatButton.interval 250 // ms
    RepeatButton.onClick (fun _ -> dispatch RepeatButtonCicked)
]
```
