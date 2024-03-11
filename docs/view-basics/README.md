# View Basics

FuncUI comes with a DSL to describe views and their attributes.&#x20;

```fsharp
DockPanel.create [
    DockPanel.children [
        Button.create [
            Button.dock Dock.Bottom
            Button.onClick (fun _ -> state.Current - 1 |> state.Set)
            Button.content "-"
            Button.horizontalAlignment HorizontalAlignment.Stretch
        ]
        Button.create [
            Button.dock Dock.Bottom
            Button.onClick (fun _ -> state.Current + 1 |> state.Set)
            Button.content "+"
            Button.horizontalAlignment HorizontalAlignment.Stretch
        ]
        TextBlock.create [
            TextBlock.dock Dock.Top
            TextBlock.fontSize 48.0
            TextBlock.verticalAlignment VerticalAlignment.Center
            TextBlock.horizontalAlignment HorizontalAlignment.Center
            TextBlock.text (string state.Current)
        ]
    ]
]
```
