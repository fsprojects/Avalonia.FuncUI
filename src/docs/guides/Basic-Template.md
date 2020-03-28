---
layout: guide
title: Basic Template
author: Avalonia Community
list-order: 1
guide-category: beginner
---
[Views and Attributes]: guides/Views-and-Attributes.html
[Full Template]: guides/Full-Template.html#styles


The basic template contains three files

- **{ProjectName}**.fsproj
- Counter.fs
- Program.fs

this is the simplest way to get started with `Avalonia.FuncUI` it's a straight "counter" sample 

## Program.fs
Inside `Program.fs` two main classes allow you to start your Avalonia application

The first one you'll see it's 
```fsharp
type MainWindow() as this =
    inherit HostWindow()
    do
        base.Title <- "Basic"
        base.Width <- 400.0
        base.Height <- 400.0
        
        //this.VisualRoot.VisualRoot.Renderer.DrawFps <- true
        //this.VisualRoot.VisualRoot.Renderer.DrawDirtyRects <- true


        Elmish.Program.mkSimple (fun () -> Counter.init) Counter.update Counter.view
        |> Program.withHost this
        |> Program.run
```
this is (as the name says) the main window of the basic template, it inherits from [HostWindow](https://github.com/AvaloniaCommunity/Avalonia.FuncUI/blob/master/src/Avalonia.FuncUI/Components/Hosts.fs#L11)
which is a special class that adds some of the functionality needed to allow normal [Avalonia Windows](https://github.com/AvaloniaUI/Avalonia/blob/master/src/Avalonia.Controls/Window.cs#L51) to work fine with the Elmish based approach.

Anything you would do with a window in Avalonia, you may do so with any class that inherits HostWindow, in this case, the most important part is the last three lines of code
```fsharp
Elmish.Program.mkSimple (fun () -> Counter.init) Counter.update Counter.view
|> Program.withHost this
|> Program.run
```

Here we're starting the Elmish program with our `Counter` module defined inside `Counter.fs`, then we set up the host `Program.withHost` and lastly run the program.

the second class is
```fsharp
type App() =
    inherit Application()

    override this.Initialize() =
        this.Styles.Load "avares://Avalonia.Themes.Default/DefaultTheme.xaml"
        this.Styles.Load "avares://Avalonia.Themes.Default/Accents/BaseDark.xaml"

    override this.OnFrameworkInitializationCompleted() =
        match this.ApplicationLifetime with
        | :? IClassicDesktopStyleApplicationLifetime as desktopLifetime ->
            desktopLifetime.MainWindow <- MainWindow()
        | _ -> ()
```
This class is the equivalent of `App.xaml` and `App.xaml.cs` in Avalonia here we override the the `Initialize()` method to load the default styles that come from Avalonia, try switching `BaseDark.xaml` to `BaseLight.xaml` to use the light theme on your application, here you may also load your custom styles. For more information on that please check the [Full Template].

the `OnFrameworkInitializationCompleted()` method is overridden to set our MainWindow class the application's `MainWindow`


## Counter.fs
The counter is a plain Elmish module with the Avalonia.FuncUI DSL in action
```fsharp
namespace Basic

module Counter =
    open Avalonia.Controls
    open Avalonia.FuncUI.DSL
    open Avalonia.Layout

    type State = { count : int }
    type Msg = Increment | Decrement | Reset

    let init = { count = 0 }
    let update (msg: Msg) (state: State) : State =
        match msg with
        | Increment -> { state with count = state.count + 1 }
        | Decrement -> { state with count = state.count - 1 }
        | Reset -> init
    
    let view (state: State) (dispatch) =
        DockPanel.create [
            DockPanel.children [
                Button.create [
                    Button.dock Dock.Bottom
                    Button.onClick (fun _ -> dispatch Reset)
                    Button.content "reset"
                ]                
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
You can refer the patern represented here as MVU (Model, View, Update) and can learn more about it [here](https://elmish.github.io/elmish/) in the meantime, we will give a brief explanation here

### State
```fsharp
type State = { count : int }
```
Also known as `Model` in the `MVU` acronym. The `State` type represents the shape of your data for this module.

In this case, we defined the shape of our data as `{ counter: int }` which means our model has a counter that contains an integer value. The init function gives us the initial value which we used inside `Program.fs`
```fsharp
Elmish.Program.mkSimple (fun _ -> Counter.init) Counter.update Counter.view
```
in this case, the counter starts at 0 `{ count = 0 }`

### Msg
The `Msg` type allows us to identify which "events" is our application responding to
```fsharp
type Msg = Increment | Decrement | Reset
```
in this case, we have three possible events
- Increment
- Decrement
- Reset

these Msg types should be descriptive on what they are reacting/doing in your module

### Update
In F# the data is often immutable this means every time you need to do changes you have to return updated copies of your data. The update function is the central place where you make changes to your state (model) depending on the message that was dispatched
```fsharp
let update (msg: Msg) (state: State) : State =
    match msg with
    | Increment -> { state with count = state.count + 1 }
    | Decrement -> { state with count = state.count - 1 }
    | Reset -> init
```
In this case, the messages are simple as well as the state, so the changes are pretty self-descriptive 
`Increment` updates the count plus 1 integer, `Decrement` substracts 1 integer, `Reset` takes our initial model back to 0

### View
This will be a brief explanation of the parts that glue everything together, to have other examples and a more concise explanation for the View Controls and their attributes, you can check [Views and Attributes]

```fsharp
let view (state: State) (dispatch: Msg -> unit) =
    DockPanel.create [
        DockPanel.children [
            Button.create [
                Button.onClick (fun _ -> dispatch Reset)
            ]                
            Button.create [
                Button.onClick (fun _ -> dispatch Decrement)
            ]
            Button.create [
                Button.onClick (fun _ -> dispatch Increment)
            ]
            TextBlock.create [
                TextBlock.text (string state.count)
            ]
        ]
    ]       
```
The view has two parameters: `state` which is the current state (model) of your module as well as a `dispatch` function, this dispatch function takes a `Msg` (It can be any of our Msg types: `Increment`, `Decrement`, `Reset`) and doesn't have a special return type (unit) `Elmish` and `Avalonia.FuncUI` glue the dispatch function.
to use the information that is stored in the state, we simply use `state.{property}` in this case, we have a text block (label) that renders out the current count on the screen.