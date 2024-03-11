# Attributes

### 🔧 Properties

For each .NET Property defined on an Avalonia Control there is a corresponding Attribute. Most of them are Property Attributes, but not all of them.

```fsharp
Button.create [
    Button.margin 5.0
    Button.content "button text"
]
...
```

### ⚡ Events

Events are just like other attributes. You can easily recognize them by their prefix. Events are named like this

> **{ControlName}**.on\*\*{EventName}\*\*

```fsharp
Button.onClick (fun args -> // do something )
TextBox.onKeyDown (fun args -> // do something )
TextBox.onKeyUp (fun args -> // do something )
ListBox.onSelectionChanged (fun args -> // do something )
...
```

When subscribing to an event you can also provide `SubPatchOptions` to configure when the subscription will be updated. If your handler function captures state you will run into issues if your handler captures state you'd expect to change. \
\
In the example below the handler function captures `current` . When the button is clicked we update the state with current + 1 but the value of `current` in our handler function **never changes**.&#x20;

<figure><img src="../.gitbook/assets/Screenshot 2023-11-10 at 09.34.54.png" alt=""><figcaption></figcaption></figure>

Capturing state that changes over time should be avoided is most cases. You can provide FuncUI with a way of knowing when to update your handler function. \
\
Update handler function on render if current value changed:

```fsharp
Button.onClick (
    func = (fun _ -> state.Set (current + 1)),
    subPatchOptions = SubPatchOptions.OnChangeOf current
)
```

Update handler function on each render pass:

```fsharp
Button.onClick (
    func = (fun _ -> state.Set (current + 1)),
    subPatchOptions = SubPatchOptions.Always
)
```

By default the handler function is only updated if the underlaying delegate type changes.

### 🧲 Attached Properties

Attached Attributes are used like Events and normal Properties.

> **{ControlName}**.**{name}**

```fsharp
StackPanel.dock Dock.Top
StackPanel.row 1
StackPanel.column 1
...
```

> ⚠ Currently not all attached properties are supported / declared. This is currently in process, feel free to create an issue if something is missing

### 📦 Content Properties

Content Properties are attributes containing either a single View or a list of Views. They are often named `content`, `children`, `viewItems`, … you get it.

Here are some examples.

```fsharp
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
