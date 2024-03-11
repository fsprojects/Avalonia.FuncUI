# TextBlock

> _Note_: You can check the Avalonia docs for the [TextBlock](https://docs.avaloniaui.net/docs/controls/textblock) and [TextBlock API](http://reference.avaloniaui.net/api/Avalonia.Controls/TextBlock/) if you need more information.
>
> For Avalonia.FuncUI's DSL properties you can check [TextBlock.fs](https://github.com/AvaloniaCommunity/Avalonia.FuncUI/blob/master/src/Avalonia.FuncUI.DSL/TextBlock.fs)

The textblock control allows you to present text within the application.

### Usage

#### Basic usage

```fsharp
TextBlock.create [
    TextBlock.text <text-for-box>
]
```

#### Properties

You will often want to specify how you want the text to look and TextBlock implements a number of properties to that end

**Background** You can pass either a basic string or an IBrush instance for more control

```fsharp
TextBlock.create [
    TextBlock.background "red"
    TextBlock.text "Critical malfunction!"
]
```

**Foreground** To set the text color, you can again pass either a basic string or an IBrush instance

```fsharp
TextBlock.create [
    TextBlock.foreground "green"
    TextBlock.text "All systems operational."
]
```

**Font** The look of the font is specified by way of the fontFamily, fontSize, fontWeight and fontStyle properties

```fsharp
TextBlock.create [
    TextBlock.fontFamily font   // where font is an Avalonia.Media.FontFamily instance
    TextBlock.fontSize 24.0
    TextBlock.fontWeight Avalonia.Media.FontWeight.Bold
    TextBlock.fontStyle Avalonia.Media.FontStyle.Italic
    TextBlock.text "Entrance restricted."
]
```

**Padding** TextBlock allows you to set the padding in several ways

```fsharp
TextBlock.create [
    // using horizontal, vertical values
    TextBlock.padding (20.0, 10.0)
    // using left, top, right, bottom values 
    TextBlock.padding (5.0, 10.0, 15.0, 20.0)
    // using an Avalonia.Thickness struct
    TextBlock.padding thickness
    TextBlock.text "It's nice with some space."
]
```

**Text formatting** Several properties are available to adjust how the content of the TextBlock is formatted

```fsharp
TextBlock.create [
    TextBlock.lineHeight 16.0
    TextBlock.maxLines 4
    TextBlock.textWrapping Avalonia.Media.TextWrapping.Wrap
    TextBlock.textAlignment Avalonia.Media.TextAlignment.Center
    TextBlock.textTrimming Avalonia.Media.TextTrimming.WordEllipsis
    TextBlock.text "A longer paragraph could at times use some more formatting."
]
```
