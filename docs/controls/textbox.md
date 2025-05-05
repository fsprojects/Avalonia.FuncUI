# TextBox

> _Note_: You can check the Avalonia docs for the [TextBox](https://docs.avaloniaui.net/docs/controls/textbox) and [TextBox API](http://reference.avaloniaui.net/api/Avalonia.Controls/TextBox/) if you need more information.
>
> For Avalonia.FuncUI's DSL properties you can check [TextBox.fs](https://github.com/fsprojects/Avalonia.FuncUI/blob/master/src/Avalonia.FuncUI/DSL/TextBox.fs)

The textbox is a control that allows a user to input strings.

### Usage

#### Basic usage

```fsharp
TextBox.create [
    TextBox.text <text-for-box>
    TextBox.onTextChanged (fun newString ->
        // Do something with the changed string
    )
]
```

**Example**

Often you simply save the string in the state and use it for other things from there:

```fsharp
type State = {
    myString: string
}

type Msg =
    | ChangeMyString of string

let update msg state =
    match msg with
    | ChangeMyString newString ->
        { state with myString = newString }

let view state dispatch =
    TextBox.create [
        TextBox.text state.myString
        TextBox.onTextChanged (ChangeMyString >> dispatch)
    ]
```
