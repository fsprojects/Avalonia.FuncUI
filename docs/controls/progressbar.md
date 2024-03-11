# ProgressBar

> _Note_: You can check the Avalonia docs for the [ProgressBar](https://docs.avaloniaui.net/docs/controls/progressbar) and [ProgressBar API](http://reference.avaloniaui.net/api/Avalonia.Controls/ProgressBar/) if you need more information.
>
> For Avalonia.FuncUI's DSL properties you can check [ProgressBar.fs](../../src/Avalonia.FuncUI.DSL/ProgressBar.fs)

The ProgressBar control allow for showing dynamic progress status.

### Usage

**Basic Progress Bar**

```fsharp
ProgressBar.create [
    ProgressBar.value 50.
    ProgressBar.maximum 100.
    // Minimum default value is set to 0
]
```

**Indeterminate Animated Progress Bar**

```fsharp
ProgressBar.create [
    ProgressBar.isIndeterminate true
]
```
