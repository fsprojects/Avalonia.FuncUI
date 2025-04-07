# StackPanel

> _Note_: You can check the Avalonia docs for the [StackPanel](https://docs.avaloniaui.net/docs/controls/stackpanel) and [StackPanel API](http://reference.avaloniaui.net/api/Avalonia.Controls/StackPanel/) if you need more information.
>
> For Avalonia.FuncUI's DSL properties you can check [StackPanel.fs](https://github.com/fsprojects/Avalonia.FuncUI/blob/master/src/Avalonia.FuncUI/DSL/Panels/StackPanel.fs)

The StackPanel is a layout construct that stacks its children in horizontal or vertical direction.

### Usage

#### Basic Usage

```fsharp
StackPanel.create [
    StackPanel.orientation Orientation.Horizontal // Orientation can be Horizontal or Vertical
    StackPanel.children [
        // This can be a list of different controls, which are stacked inside of the StackPanel
    ]
]
```

**Example**

```fsharp
StackPanel.create [
    StackPanel.orientation Orientation.Vertical
    StackPanel.children [
        Button.create [
            Button.content "Import"
            Button.padding (40., 14.)
        ]
        Button.create [
            Button.content "Analyse"
            Button.padding (40., 14.)
        ]
        Button.create [
            Button.content "Publish"
            Button.padding (40., 14.)
        ]
    ]
]
```

#### Spacing

```fsharp
StackPanel.create [
    StackPanel.orientation Orientation.Horizontal
    StackPanel.spacing 10. // Adds space between stacked items
    StackPanel.children [
        // List of stacked controls
    ]
]
```

**Example**

```fsharp
StackPanel.create [
    StackPanel.orientation Orientation.Vertical
    StackPanel.spacing 10.
    StackPanel.children [
        Button.create [
            Button.content "Import"
            Button.padding (40., 14.)
        ]
        Button.create [
            Button.content "Analyse"
            Button.padding (40., 14.)
        ]
        Button.create [
            Button.content "Publish"
            Button.padding (40., 14.)
        ]
    ]
]
```
