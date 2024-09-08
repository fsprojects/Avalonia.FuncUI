# Common Questions

## How do I obtain the reference of a Control?

There are 2 recommended ways of obtaining the reference of an underlying control. \\

### 1. Execute code on control creation

Use the Control.init attribute to run code on control creation.

```fsharp
ListBox.create [
    ListBox.init (fun listBox ->
        listBox.Items <- [ 1 .. 3 ]
    )
]
```

### 2. Obtain a view reference via an outlet

Calls the `listBox.Set` function when the control is created.

```fsharp
Component(fun ctx ->
    let listBox = ctx.useState<ListBox>(null)
    
    ctx.useEffect (
        handler = (fun _ ->
            listBox.Current.Items <- [ 1 .. 3 ]    
        ),
        triggers = [ EffectTrigger.AfterInit ]
    )
    
    View.createWithOutlet listBox.Set ListBox.create [ ]
)
```

## How do I obtain the reference of a Component?

The Component reference can be accessed via `ctx.control`.

```fsharp
Component(fun ctx ->
    ctx.useEffect (
        handler = (fun _ ->
            ctx.control.Tag <- 0
        ),
        triggers = [ EffectTrigger.AfterInit ]
    )
    Button.create []
)
```

## How to set attributes on Component level?

```fsharp
Component(fun ctx ->
    ctx.attrs [
        Component.background "transparent"
        Component.borderThickness 1
    ]
    Button.create []
)
```

## How do I restrict what a user can input in a TextBox / AutoCompleteBox / InputElement ?

This is possible by intercepting the [TextInputEvent](https://reference.avaloniaui.net/api/Avalonia.Input/InputElement/FEA4DB21) and modifying its event args. It's important to attach the handler to the tunnelled event. More details about event routing can be found [here](https://docs.avaloniaui.net/docs/input/routed-events#routing-strategies).\
\
In the example below whatever a user types in a TextBox will end up as uppercase text.

```fsharp
TextBox.create [
    TextBox.init (fun control ->
        control.AddHandler(
            TextBox.TextInputEvent,
            (fun sender args ->
                args.Text <- args.Text.ToUpper()
            ),
            RoutingStrategies.Tunnel
        )
    )
]
```

Here another example that prevents users from entering anything but numbers.

```fsharp
TextBox.create [
    TextBox.init (fun control ->
        control.AddHandler(
            TextBox.TextInputEvent,
            (fun sender args -> args.Text <- String.filter Char.IsNumber args.Text),
            RoutingStrategies.Tunnel
        )
    )
] 
```

## How to render a Control to an Image?

```fsharp
let renderToFile (target : Control, path : string) = 
    let pixelSize = PixelSize(int target.Bounds.Width, int target.Bounds.Height) 
    let size = Size(target.Bounds.Width, target.Bounds.Height) 
    use bitmap = new RenderTargetBitmap(pixelSize, new Vector(96.0, 96.0)) 
    target.Measure(size) 
    target.Arrange(Rect(size)) 
    bitmap.Render(target) 
    bitmap.Save(path) 
```

## Managing Focus

<figure><img src=".gitbook/assets/CleanShot 2024-09-08 at 15.00.12.gif" alt="" width="345"><figcaption></figcaption></figure>

```fsharp
Component.create ("view", fun ctx ->
    let textBoxA = ctx.useState<TextBox>(null, renderOnChange = false)
    let textBoxB = ctx.useState<TextBox>(null, renderOnChange = false)
    let textBoxAFocus = ctx.useState(false, renderOnChange = true)
    let textBoxBFocus = ctx.useState(false, renderOnChange = true)
    StackPanel.create [
        StackPanel.margin 10
        StackPanel.spacing 10
        StackPanel.children [
            TextBox.create [
                TextBox.init textBoxA.Set
                TextBox.onGotFocus (fun _ -> textBoxAFocus.Set true)
                TextBox.onLostFocus (fun _ -> textBoxAFocus.Set false)
            ]
            TextBox.create [
                TextBox.init textBoxB.Set
                TextBox.onGotFocus (fun _ -> textBoxAFocus.Set true)
                TextBox.onLostFocus (fun _ -> textBoxAFocus.Set false)
            ]
            StackPanel.create [
                StackPanel.orientation Orientation.Horizontal
                StackPanel.margin 10
                StackPanel.spacing 10
                StackPanel.children [
                    Button.create [
                        Button.content "Focus A"
                        Button.background (if textBoxAFocus.Current then Brushes.Green else Brushes.Red)
                        Button.onClick (fun _ ->
                            let _ = textBoxA.Current.Focus()
                            ()
                        )
                    ]
                    Button.create [
                        Button.content "Focus B"
                        Button.background (if textBoxBFocus.Current then Brushes.Green else Brushes.Red)
                        Button.onClick (fun _ ->
                            let _ = textBoxB.Current.Focus()
                            ()
                        )
                    ]
                ]
            ]
        ]
    ]
)
```
