# Slider

> _Note_: You can check the Avalonia docs for the [Slider](https://docs.avaloniaui.net/docs/controls/slider) and [Slider API](http://reference.avaloniaui.net/api/Avalonia.Controls/Slider/) if you need more information.
>
> For Avalonia.FuncUI's DSL properties you can check [Slider.fs](../../src/Avalonia.FuncUI.DSL/Slider.fs)

The Slider control is a control that lets the user select from a range of values by moving a Thumb control along a track.

### Usage

**Percentage Slider**

```fsharp
Slider.create [
    Slider.minimum 0.
    Slider.maximum 0.
    Slider.value state.Percentage
    Slider.onValueChanged (fun newPercentage -> ChangePercentage newPercentage |> dispatch)

]
```
