---
layout: guide
title: Views And Attributes
author: Avalonia Community
list-order: 4
guide-category: beginner
---
F# is a great language to built DSLs (Domain Specific Languages), this library contains a [elm like](https://package.elm-lang.org/packages/elm/html/latest/) DSL used to describe Views.

```fsharp
let view (state: CounterState) (dispatch): View =
    DockPanel.create [
        DockPanel.children [
            Button.create [
                Button.dock Dock.Bottom
                Button.onClick (fun _ -> dispatch Decrement)
                Button.content "-"
            ]
            Button.create [
                Button.dock Dock.Bottom
                Button.onClick (fun _ -> dispatch Increment)
                Button.content "+"
            ]
            TextBlock.create [
                TextBlock.dock Dock.Top
                TextBlock.fontSize 48.0
                TextBlock.verticalAlignment VerticalAlignment.Center
                TextBlock.horizontalAlignment HorizontalAlignment.Center
                TextBlock.text (string state.count)
            ]
        ]
    ]   
```

<img width="200" src="https://raw.githubusercontent.com/AvaloniaCommunity/Avalonia.FuncUI/master/github/img/counter_screenshot.png"/>

# 🔰 Basics

Support for all Avalonia Controls is built-in. You can use them like this:
> **{ControlName}**.create [ **{ControlName}**.**{attributeName}** ]

```fsharp
Button.create [ Button.content ""; Button.onClick(fun args -> printf "%A" args) ]
TextBox.create [ Textbox.watermark "I'm a placeholder"; Textbox.text state.textboxValue ]
Border.Create [ attributes ]
TabControl.Create [ attributes ]
...
```

Attributes are type-safe, so you don't have to remember what Attributes a Button has. The compiler will complain if you try to use an unsupported attribute 😉. (This applies to all kinds of attributes, including events, attached properties, ...)

```fsharp
Button.create [
    Button.margin 5.0
    Button.content "button text"
    Button.children … // ⚠ compiler error - not supported on button
]
...
```

Also if you, for example, try to set the margin there are several overloads available to simplify what you are trying to do.

```fsharp
Button.margin 5.0
TextBox.margin (5.0, 5.0)
TextBlock.margin (horizontal = 5.0, vertical = 5.0)
TabControl.margin (5.0, 5.0, 5.0, 5.0)
ListBox.margin (left = 5.0, top = 5.0, right = 5.0, bottom = 5.0)
```
Pretty neat, Hah. Those overloads exist for a lot of attributes (you can also add them yourself), my favorite on is:
```
Button.background "green" // or "#00ff00"
```
## 🔧 Properties
For each .NET Property defined on an Avalonia Control there is a corresponding Attribute. Most of them are Property Attributes, but not all of them.

```fsharp
Button.create [
    Button.margin 5.0
    Button.content "button text"
]
...
```

## ⚡ Events
Events are just like other attributes. You can easily recognize them by their prefix. Events are named like this
> **{ControlName}**.on**{EventName}**
```fsharp
TextBox.onClick (fun sender args -> // do something )
TextBox.onKeyDown (fun sender args -> // do something )
TextBox.onKeyUp (fun sender args -> // do something )
TextBox.onSelectionChanged (fun sender args -> // do something )
...
```
## 🧲 Attached Properties
Attached Attributes are used like Events and normal Properties.
> **{ControlName}**.**{name}**
```fsharp
StackPanel.dock Dock.Top
StackPanel.row 1
StackPanel.colum 1
...
```
> ⚠ Currently not all attached properties are supported / declared. This is currently in process, feel free to create an issue if something is missing

## 📦 Content Properties
Content Properties are attributes containing either a single View or a list of Views. They are often named `content`, `children`, `viewItems`, … you get it.

Here are some examples.
``` fsharp
// single view content
Button.create [
    // takes 'View' 
    Button.content (
        TextBlock.create [
            TextBlock.text "some text"
        ]
    )
]

// content view list
StackPanel.create [
    // takes 'View list' 
    StackPanel.children [
        TextBox.create [
            TextBox.text "one"
        ]
        TextBox.create [
            TextBox.text "two"
        ]
        ...
    ]
]   

```

