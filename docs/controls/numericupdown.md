# NumericUpDown

> _Note_: You can check the Avalonia docs for the [NumericUpDown](https://docs.avaloniaui.net/docs/controls/numericupdown) and [NumericUpDown API](http://reference.avaloniaui.net/api/Avalonia.Controls/NumericUpDown/) if you need more information.
>
> For Avalonia.FuncUI's DSL properties you can check [NumericUpDown.fs](https://github.com/fsprojects/Avalonia.FuncUI/blob/master/src/Avalonia.FuncUI/DSL/NumericUpDown.fs)

The NumericUpDown is an editable numeric input field. The control has a up and down button spinner attached, used to increment and decrement the value in the input field. The value can also be incremented or decremented using the arrow keys or the mouse wheel when the control is selected.

### Usage

**Input with local currency**

Adding the `NumericUpDown.minimum` or `NumericUpDown.maximum` attributes will limit the input for both button increment/decrement changes and text input changes.

For more information about rendering the controls value you can check out the documentation for [Numeric String Formats](https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-numeric-format-strings).

```fsharp
NumericUpDown.create [
    NumericUpDown.minimum 0.
    NumericUpDown.maximum 10.
    NumericUpDown.formatString "C2"
    NumericUpDown.increment 0.25
]
```

**Simple numeric input**

If you want to disable the increment/decrement buttons you can add `NumericUpDown.allowSpin false`. If you then also want to remove visibility of the buttons you can add `NumericUpDown.showButtonSpinner false`.

```fsharp
NumericUpDown.create [
    NumericUpDown.minimum 0.
    NumericUpDown.minimum 100.
    NumericUpDown.showButtonSpinner false
    NumericUpDown.allowSpin false
]
```

**State controlled input**

```fsharp
NumericUpDown.create [
    NumericUpDown.minimum 0.
    NumericUpDown.formatString "C2"
    NumericUpDown.value state.price
    NumericUpDown.onValueChanged (fun newPrice -> ChangePrice price |> dispatch )
]
```
