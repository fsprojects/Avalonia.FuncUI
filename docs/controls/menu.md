# Menu

> _Note_: You can check the Avalonia docs for the [Menu API](http://reference.avaloniaui.net/api/Avalonia.Controls/Menu/) and [Menu](http://docs.avaloniaui.net/docs/controls/menu) if you need more information.
>
> For Avalonia.FuncUI's DSL properties you can check [Menu.fs](https://github.com/fsprojects/Avalonia.FuncUI/blob/master/src/Avalonia.FuncUI/DSL/Menu.fs)

The menu control allows you to add a list of buttons in a horizontal manner which supports sub-items, it's usually put at the top of the application inside a DockPanel, but it can be placed anywhere in the application.

### Usage

**Top-Level Menu Items**

To create top-level navigation menus you just need to provide a list of `MenuItem` controls and use the `.viewItems` property on the [Menu](http://docs.avaloniaui.net/docs/controls/menu) control

```fsharp
let menuItems = [
    MenuItem.Create [
        MenuItem.header "File"
    ]
    MenuItem.Create [
        MenuItem.header "Edit"
    ]
]

Menu.create [
  Menu.viewItems menuItems
]
```

**Set Sub-Menus**

Each MenuItem can contain MenuItems themselves if you need a sub-menu you just need to provide the appropriate children

```fsharp
let fileItems = [
  MenuItem.Create [
    MenuItem.header "Open File"
  ]
  MenuItem.Create [
    MenuItem.header "Open Folder"
  ]
]

let menuItems = [
  MenuItem.Create [
   MenuItem.header "Files"
   MenuItem.viewItems fleItems
  ]
  MenuItem.Create [
    MenuItem.header "Preferences"
  ]
]

Menu.create [
  Menu.viewItems menuItems
]
```

**Set Icons**

To add Icons to the menu item you just need to provide an [Image](http://avaloniaui.net/docs/controls/image), you can check this [sample](https://github.com/fsprojects/Avalonia.FuncUI/blob/master/src/Examples/Elmish%20Examples/Examples.Elmish.MusicPlayer/Shell.fs#L160) which uses an extension method defined in [this file](https://github.com/fsprojects/Avalonia.FuncUI/blob/master/src/Examples/Elmish%20Examples/Examples.Elmish.MusicPlayer/Extensions.fs#L22)

```fsharp
let icon = (* obtain an Image instance *)
let menuItems = [
  MenuItem.Create [
    MenuItem.header "Files"
    MenuItem.icon icon
  ]
  MenuItem.Create [
    MenuItem.header "Preferences"
  ]
]

Menu.create [
  Menu.viewItems menuItems
]
```

**Dispatch Actions From Menu Items**

```fsharp
let menuItems = [
  MenuItem.Create [
    MenuItem.header "About"
    MenuItem.onClick(fun _ -> dispatch GoToAbout)
  ]
]

Menu.create [
  Menu.viewItems menuItems
]
```
