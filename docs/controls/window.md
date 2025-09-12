# Window

> _Note_: You can check the Avalonia docs for the [Window](https://docs.avaloniaui.net/docs/controls/window) and [Window API](http://reference.avaloniaui.net/api/Avalonia.Controls/Window/) if you need more information.
>
> For Avalonia.FuncUI's DSL properties you can check [Window.fs](https://github.com/AvaloniaCommunity/Avalonia.FuncUI/blob/master/src/Avalonia.FuncUI.DSL/Window.fs).

Window corresponds to your application's host window, and allows you to set top-level properties like window title and key bindings.

### Usage

**Create a Window**

```fsharp
Window.create []
```

**Set Window Title**

```fsharp
Window.title "My Application"
```

**Set Window Icon**

```fsharp
let icon =
    Path.Combine("Assets", "Icons", "icon.ico")
        |> WindowIcon

Window.icon icon
```

**Size Window to Content**

```fsharp
Window.sizeToContent SizeToContent.WidthAndHeight
```

**Switch to Full-Screen Mode**

```fsharp
Window.windowState (
    if state.FullScreen then WindowState.FullScreen
    else WindowState.Normal)
```

**Create Key Bindings**

```fsharp
Window.keyBindings [
    KeyBinding.create [
        KeyBinding.key Key.F11
        KeyBinding.execute (fun _ ->
            FullScreen true
                |> dispatch)
    ]
    KeyBinding.create [
        KeyBinding.key Key.Escape
        KeyBinding.execute (fun _ ->
            FullScreen false
                |> dispatch)
    ]
]
```
