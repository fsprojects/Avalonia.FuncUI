# Border

> _Note_: You can check the Avalonia docs for the [Border](https://docs.avaloniaui.net/docs/controls/border) and [Border Api](http://reference.avaloniaui.net/api/Avalonia.Controls/Border/) if you need more information.
>
> For Avalonia.FuncUI's DSL properties you can check [Border.fs](https://github.com/AvaloniaCommunity/Avalonia.FuncUI/blob/master/src/Avalonia.FuncUI.DSL/Border.fs)

The Border control allows you to decorate child controls

### Usage

**Set Background** Avalonia.FuncUI has some overloads for you to take advantage of

```fsharp
Border.create [
  Border.background "black"
  Border.child (StackPanel.create [ /* ... definition ... */ ])
]
Border.create [
  Border.background "#000000"
  Border.child (StackPanel.create [ /* ... definition ... */ ])
]
```

> You can pass any [IBrush](http://reference.avaloniaui.net/api/Avalonia.Media/IBrush/) compatible instance to the background for more control

**Set Border Brush** Avalonia.FuncUI has some overloads for you to take advantage of

```fsharp
Border.create [
  Border.borderBrush "red"
  Border.child (StackPanel.create [ /* ... definition ... */ ])
]
```

```fsharp
Border.create [
  Border.borderBrush "#FF0000"
  Border.child (StackPanel.create [ /* ... definition ... */ ])
]
```

> You can pass any [IBrush](http://reference.avaloniaui.net/api/Avalonia.Media/IBrush/) compatible instance to the background for more control

**Thickness**

```fsharp
Border.create [
  Border.borderThickness 2.0
  Border.child (StackPanel.create [ /* ... definition ... */ ])
]
```

**Horizontal and Vertical Thickness**

```fsharp
Border.create [
  Border.borderThickness (2.0, 5.0)
  Border.child (StackPanel.create [ /* ... definition ... */ ])
]
```

**Left, Top, Right, Bottom Thickness**

```fsharp
Border.create [
  Border.borderThickness (1.0, 2.0, 3.0, 4.0)
  Border.child (StackPanel.create [ /* ... definition ... */ ])
]
```

> You can also pass a [Thickness](http://reference.avaloniaui.net/api/Avalonia/Thickness/) struct to the borderThickness property

**Corner Radius**

```fsharp
Border.create [
  Border.cornerRadius 3.0
  Border.child (StackPanel.create [ /* ... definition ... */ ])
]
```

**Horizontal and Vertical Corner Radius**

```fsharp
Border.create [
  Border.cornerRadius (2.0, 5.0)
  Border.child (StackPanel.create [ /* ... definition ... */ ])
]
```

**Left, Top, Right, Bottom Corner Radius**

```fsharp
Border.create [
  Border.cornerRadius (1.0, 2.0, 3.0, 4.0)
  Border.child (StackPanel.create [ /* ... definition ... */ ])
]
```

> You can also pass a [Corner Radius](http://reference.avaloniaui.net/api/Avalonia/CornerRadius/) struct to the cornerRadius property

```fsharp
Border.create [
  Border.cornerRadius (CornerRadius 3.0)
  Border.child (StackPanel.create [ /* ... definition ... */ ])
]
```
