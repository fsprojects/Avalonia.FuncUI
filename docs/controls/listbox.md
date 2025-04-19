# ListBox

> _Note_: You can check the Avalonia docs for the [ListBox](https://docs.avaloniaui.net/docs/controls/listbox) and [ListBox API](http://reference.avaloniaui.net/api/Avalonia.Controls/ListBox/) if you need more information.
>
> For Avalonia.FuncUI's DSL properties you can check [ListBox.fs](https://github.com/fsprojects/Avalonia.FuncUI/blob/master/src/Avalonia.FuncUI/DSL/ListBox.fs)

The list box is a multi-line control box for allowing a user to choose value.

### Usage

**Create a list box**

```fsharp
ListBox.create [
    ListBox.dataItems [ "Linux"; "Mac"; "Windows" ]
]
```

**Multiple Item Selection Mode**

You can choose different [ListBox Selection Modes](https://docs.avaloniaui.net/docs/controls/listbox#selectionmode). The default is to only select a single element.

```fsharp
ListBox.create [
    ListBox.dataItems [ "Linux"; "Mac"; "Windows" ]
    ListBox.selectionMode Selection.Multiple
]
```

**Using Discriminated Unions**

```fsharp
type OperatingSystem =
    | Linux
    | Mac
    | Windows

ListBox.create [
    ListBox.dataItems [ Linux; Mac; Windows ]
]
```

**Controlling Selected Item**

To override the controls default behavior you need to add both `selectedItem` and `onSelectedItemChanged`

```fsharp
ListBox.create [
    ListBox.dataItems [ "Linux"; "Mac"; "Windows" ]
    ListBox.selectedItem state.os
    ListBox.onSelectedItemChanged (fun os -> dispatch ChangeOs)
]
```

**Controlling Selected Item by Index**

To override the controls default behavior you need to add both `selectedItem` and `onSelectedItemChanged`

```fsharp
ListBox.create [
    ListBox.dataItems [ "Linux"; "Mac"; "Windows" ]
    ListBox.selectedIndex state.osIndex
    ListBox.onSelectedIndexChanged (fun os -> dispatch ChangeOsIndex)
]
```
