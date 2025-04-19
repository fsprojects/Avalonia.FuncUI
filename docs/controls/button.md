# Button

> _Note_: You can check the Avalonia docs for the [Button](https://docs.avaloniaui.net/docs/controls/button) and [Button API](http://reference.avaloniaui.net/api/Avalonia.Controls/Button/) if you need more information.
>
> For Avalonia.FuncUI's DSL properties you can check [Button.fs](https://github.com/fsprojects/Avalonia.FuncUI/blob/master/src/Avalonia.FuncUI/DSL/Buttons/Button.fs)

Buttons are basic controls for any application you may build, buttons are often used to trigger an action.

### Usage

> You can check the general usage of Avalonia.FuncUI's views and attributes in the following link Views and Attributes

**Create a Button**

```fsharp
Button.create []
```

**Register Click**

```fsharp
Button.create [
  Button.onClick(fun _ -> dispatch MyMsg)
]
```

**Set Click Mode**

```fsharp
Button.create [
  Button.clickMode ClickMode.Press
]
// or
Button.create [
  Button.clickMode ClickMode.Release
]
```

> for more information check the [Click Mode](http://reference.avaloniaui.net/api/Avalonia.Controls/ClickMode/) docs

**Set Content**

```fsharp
Button.create [ Button.content "My Button" ]
```

Buttons can have arbitrary content, for example it can be a string as the example above. It also can be another entire control like a StackPanel

```fsharp
let playIcon =
  Canvas.create [ ... ]
let textbox =
  TextBox.create [ ... ]

let iconAndTextBlock =
  StackPanel.create [
    StackPanel.orientation Horizontal
    StackPanel.spacing 8.0
    StackPanel.children [ playIcon; textbox ]
  ]
Button.create [
  Button.content iconAndTextBlock
]
```
