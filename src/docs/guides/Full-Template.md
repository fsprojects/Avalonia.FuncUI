---
layout: guide
title: Full Template
author: Avalonia Community
list-order: 2
guide-category: beginner
---
[Counter.fs]: guides/Basic-Template.html#counterfs
[Basic Template]: guides/Basic-Template.html
[Program.fs]: guides/Basic-Template.html#programfs
[Quickstart Template]: guides/Quickstart-Template.html

> **Note**: It's recommended that you take a look at the [Basic Template] document before reading this document.

The Full Template contains 6 files
- **{ProjectName}**.fsproj
- About.fs
- Counter.fs
- Shell.fs
- Program.fs
- Styles.xaml

You can check [Counter.fs] and [Program.fs] for more details.

## About.fs
The "About" file is a pretty simple one, it contains links to useful resources, most of that functionality is explained in the Basic Template's Counter section. There's a piece though that can be useful if you intend to show links inside your application
```fsharp
let update (msg: Msg) (state: State) =
    match msg with
    | OpenUrl link -> 
        let url = 
            match link with 
            | FuncUIGitter -> "https://gitter.im/Avalonia-FuncUI"
                
        if RuntimeInformation.IsOSPlatform(OSPlatform.Windows) then
            let start = sprintf "/c start %s" url
            Process.Start(ProcessStartInfo("cmd", start)) |> ignore
        else if RuntimeInformation.IsOSPlatform(OSPlatform.Linux) then
            Process.Start("xdg-open", url) |> ignore
        else if RuntimeInformation.IsOSPlatform(OSPlatform.OSX) then
            Process.Start("open", url) |> ignore
        state, Cmd.none
```
Since Avalonia (therefore Avalonia.FuncUI) runs on top of .net core you can use .netstandard API's
here we use the following namespaces to determine on which platform we're running and what kind of way to open a link that works for our OS.
``` fsharp
open System.Diagnostics
open System.Runtime.InteropServices
```

## Shell.fs
The `Shell.fs` file is a little more complex than the Counter or the Program you found in the Basic Template. Here we show you how you can use the `TabControl` to create an application that can show multiple views on the same module, people coming from a web background might think it looks like a `Single Page Application` and they would be right. All of the application is inside a single MainWindow class but, how does this impact the way the application is structured?

There are some ways to do it one of them is to use `ViewBuilder.Create<MODULE_NAME.Host>([])` which we will talk about in the [Quickstart Template]
and the other is the approach used inside `Shell.fs`. The main points to talk about are
```fsharp
type State =
    { aboutState: About.State; counterState: Counter.State;}
type Msg =
    | AboutMsg of About.Msg
    | CounterMsg of Counter.Msg

let init =
    let aboutState, aboutCmd = About.init
    let counterState = Counter.init
    { aboutState = aboutState; counterState = counterState },
    Cmd.batch [ aboutCmd ]

let update (msg: Msg) (state: State): State * Cmd<_> =
    match msg with
    | AboutMsg bpmsg ->
        let aboutState, cmd =
            About.update bpmsg state.aboutState
        { state with aboutState = aboutState },
        /// map the message to the kind of message 
        /// your child control needs to handle
        Cmd.map AboutMsg cmd
    /// ... omitted code

let view (state: State) (dispatch) =
   /// ... more omitted code
   TabItem.content (Counter.view state.counterState (CounterMsg >> dispatch))
   TabItem.content (About.view state.aboutState (AboutMsg >> dispatch)) 
   /// ... mode omitted code
```

The main take away from this is that most Elmish modules are structured in the same way and contain the same public functions, therefore you can use Elmish modules inside other modules themselves (`Shell.fs` is a Elmish module that includes other two Elmish modules).

The state contains the children's state definitions 
- `aboutState: About.State;`
- `counterState: Counter.State;`


The Msg also contains the children's Msg types
- `AboutMsg of About.Msg`
- `CounterMsg of Counter.Msg`


The init function also has some changes; There are modules that require some sort of initialization in those cases you often use [Commands](https://elmish.github.io/elmish/#Commands) or [Subscriptions](https://elmish.github.io/elmish/#Subscriptions) and it's very likely that the init function of that module returns the state with a command, the module may not return a command at all just the initial state (model)
```fsharp
let init =
    let aboutState, aboutCmd = About.init
    let counterState = Counter.init
    { aboutState = aboutState; counterState = counterState },
    /// If your children controls don't emit any commands
    /// in the init function, you can just return Cmd.none
    /// otherwise, you can use a batch operation on all of them
    /// you can add more init commands as you need
    Cmd.batch [ aboutCmd ]
```
then we only need to assign the states where they belong and if we have a command from a child module, you can batch the commands (in case you have more than one) as shown in the sample code

In the update function we also make a similar change
```fsharp
let aboutState, cmd =
    About.update bpmsg state.aboutState
{ state with aboutState = aboutState },
/// map the message to the kind of message 
/// your child control needs to handle
Cmd.map AboutMsg cmd
```
Here we're calling the child's module update function with the child's command, we simply take the returned state and command and apply them to our current state, and map the message to the kind of message it is `Cmd.map AboutMsg cmd` since the child module might be chaining messages (for example calling a `Save` and then returning the saved state and an `AfterSave` message

For the view function we use some composition (>>) to indicate what is the correct kind of dispatch that the child needs
```fsharp
let view (state: State) (dispatch) =
   /// ... more omitted code
   TabItem.content (Counter.view state.counterState (CounterMsg >> dispatch))
   TabItem.content (About.view state.aboutState (AboutMsg >> dispatch)) 
   /// ... mode omitted code
```
Here we use the view function from the Counter and the About modules.

Finally the `MainWindow`. You may ask yourself 
> Why would I need to have the window defined in the same module as my view?

The reason is that you are able to show an Elmish module inside its own window (yes you can have multiple windows) this only shows you that you don't necessarily need to define the MainWindow inside the `Program.fs` file. There's also another significant change

```fsharp
Elmish.Program.mkProgram (fun () -> init) update view
```
instead using `mkSimple` we now use `mkProgram` which takes a function that returns a `State, Cmd`
to begin the program.

## Program.fs
The only difference this time is to include a custom styles file
```fsharp
this.Styles.Load "avares://PROJECT_NAME/Styles.xaml"
```

## Styles
The Full Template includes a sample of custom styles, it is a simple Xaml file
```xaml
<Styles
    xmlns="https://github.com/avaloniaui"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">
    <Style Selector="Button /template/ ContentPresenter">
        <Setter Property="CornerRadius" Value="5" />
    </Style>
<!-- ... more code -->
</Styles>
```
You can read more about styles in the [Avalonia Style's Docs](https://avaloniaui.net/docs/styles/styles)
you are able to include other resources from other libraries if they expose them. One of our samples does this, you can check it [here](https://github.com/AvaloniaCommunity/Avalonia.FuncUI/blob/master/src/Examples/Examples.MusicPlayer/Program.fs#L12)


