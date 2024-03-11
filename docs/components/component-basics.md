# Component basics

FuncUI offers a Component inspired by ReactJS. Components allow you to organize views into reusable pieces.&#x20;

There are two ways of creating a component.&#x20;

## Component (fun ctx -> ...)

Creating a component using the regular constructor of the `Component` class. `Component` inherits from `Border` and therefor can be used like any other Avalonia control.

```fsharp
// create a Component that can be directly used in a Avalonia app
let component: Component = Component (fun ctx -> 
    TextBlock.create [
        TextBlock.text "Hello World!"
    ]
)

// use component as main view of an app
type MainWindow() as this =
    inherit HostWindow()
    do
        this.Content <- component

// embedd component in avalonia app
let control: ContentControl = ..
control.Content <- component
// Creating a component using the View DSL.
// The resulting IView can be used inside other components and with the View DSL.

```

## Component.create ("key", fun ctx -> ...)

Declaratlvely describes a component. Can be embedded in other views as this returns an `IView`

```fsharp
let greetingView (): IView = 
    Component.create ("greetingView", fun ctx -> 
        TextBlock.create [
            TextBlock.text "Hello World!"
        ]
    )

let view (): IView = 
    Component.create ("mainView", fun ctx -> 
        DockPanel.create [
            DockPanel.children [
                // use other component
                greetingView ()
            ]
        ]
    )
```
