# CheckBox

> _Note_: You can check the Avalonia docs for the [CheckBox](https://docs.avaloniaui.net/docs/controls/checkbox) and [CheckBox API](http://reference.avaloniaui.net/api/Avalonia.Controls/CheckBox/) if you need more information.
>
> For Avalonia.FuncUI's DSL properties you can check [CheckBox.fs](https://github.com/fsprojects/Avalonia.FuncUI/blob/master/src/Avalonia.FuncUI/DSL/Buttons/CheckBox.fs)

The checkbox is a control that allows a user to represent boolean values or the absense of a value

### Usage

**Set Label**

```fsharp
CheckBox.create [
  CheckBox.content "I Accept the terms and conditions."
]
```

**Set Is Checked**

```fsharp
CheckBox.create [
  // can be either true or false
  CheckBox.isChecked state.booleanValue
]
```

**Set Indeterminate**

```fsharp
CheckBox.create [
  // can be either true or false
  CheckBox.isThreeState state.indeterminate
  // this value is required to be either a nullable boolean
  // or a boolean option
  CheckBox.isChecked None
]
```

> To be able to set the indeterminate state, the `isThreeState` value must be true and the `isChecked` value must be None or Nullable boolean set to null

**Set Dynamic State Checkbox**

You can mix and match the three states of a checkbox. In this example if the count value is greater than 0 the box will be checked, if the value is 0 then it will be indeterminate, lastly if the value is less than 0 it will be unchecked

```fsharp
let isChecked =
  if state.count = 0 then
    None
  else if state.count > 0 then
    Some true
  else
    Some false

CheckBox.create [
  CheckBox.content "Dynamic CheckBox"
  // this is not required
  CheckBox.isEnabled false
  CheckBox.isThreeState (state.count = 0)
  CheckBox.isChecked isChecked
]
```
