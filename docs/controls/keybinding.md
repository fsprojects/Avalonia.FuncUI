# KeyBinding

> _Note_: You can check the Avalonia docs for [KeyBinding](https://docs.avaloniaui.net/docs/concepts/input/binding-key-and-mouse) if you need more information.
>
> For Avalonia.FuncUI's DSL properties you can check [KeyBinding.fs](https://github.com/AvaloniaCommunity/Avalonia.FuncUI/blob/master/src/Avalonia.FuncUI.DSL/KeyBinding.fs).

A key binding allows you to dispatch a message (or trigger some other action) when the user presses a key on the keyboard. For example:

```fsharp
KeyBinding.create [
    KeyBinding.key Key.F11
    KeyBinding.execute (fun _ ->
        FullScreen true
            |> dispatch)
]
```

### Usage

**Create a Key Binding**

```fsharp
KeyBinding.create []
```

**Bind to a Specific Key**

```fsharp
KeyBinding.key Key.Escape
```

**Trigger a Message**

```fsharp
KeyBinding.execute (fun _ ->
    dispatch MyMessage)
```
