[![Stand With Ukraine](https://raw.githubusercontent.com/vshymanskyy/StandWithUkraine/main/banner2-direct.svg)](https://vshymanskyy.github.io/StandWithUkraine)

<h1 align="center"><img src="github/img/logo/FuncUI_Boxed.png" width="32px" alt="Avalonia FuncUI"> 
Avalonia FuncUI
</h1>

<p align="center">Develop cross-platform GUI Applications using <a href="https://fsharp.org">F#<a> and <a href="https://github.com/AvaloniaUI/Avalonia">AvaloniaUI</a>!</p>
<p align="center">
<img src="https://img.shields.io/github/languages/top/fsprojects/Avalonia.FuncUI" alt="GitHub top language">
<img alt="GitHub repo size" src="https://img.shields.io/github/repo-size/fsprojects/Avalonia.FuncUI">
<img src="https://img.shields.io/github/license/fsprojects/Avalonia.FuncUI">
<img alt="Nuget (with prereleases)" src="https://img.shields.io/nuget/vpre/Avalonia.FuncUI?color=green&label=package%20Avalonia.FuncUI">
</p><br>

![](github/img/hero.png)
*(Application was created using Avalonia.FuncUI!)*

## About
FuncUI is a thin layer built on top of [AvaloniaUI](https://github.com/AvaloniaUI/Avalonia). It contains abstractions 
for writing UI applications in different programming styles. It also contains a component & state management system. 

FuncUI mostly consists of:
- Virtual DOM
- View DSL
- Component model

## Resources

- 💨 [Getting started](https://funcui.avaloniaui.net)

- 📚 [Documentation](https://funcui.avaloniaui.net/)

- 📓 [Examples](https://github.com/fsprojects/Avalonia.FuncUI/tree/master/src/Examples)

## Contributing
Please contribute to this library through issue reports, pull requests, code reviews, documentation, and discussion. 

## Examples
### Example using components
A simple counter made with the component library:

``` f#
type Components =
    static member Counter () =
        Component (fun ctx ->
            let state = ctx.useState 0
    
            DockPanel.create [
                DockPanel.children [
                    Button.create [
                        Button.onClick (fun _ -> state.Set(state.Current - 1))
                        Button.content "click to decrement"
                    ]
                    Button.create [
                        Button.onClick (fun _ -> state.Set(state.Current + 1))
                        Button.content "click to increment"
                    ]
                    TextBlock.create [
                        TextBlock.dock Dock.Top
                        TextBlock.text (string state.Current)
                    ]
                ]
            ]
        )
```

Find more examples using components in the [Components Examples folder](https://github.com/fsprojects/Avalonia.FuncUI/tree/master/src/Examples/Component%20Examples).

### Example using Elmish
The same counter as above but using the `Avalonia.FuncUI.Elmish` package:

```f#
module Counter =

    type CounterState = {
        count : int
    }

    let init = {
        count = 0
    }

    type Msg =
        | Increment
        | Decrement

    let update (msg: Msg) (state: CounterState) : CounterState =
        match msg with
        | Increment -> { state with count =  state.count + 1 }
        | Decrement -> { state with count =  state.count - 1 }
    
    let view (state: CounterState) (dispatch): IView =
        DockPanel.create [
            DockPanel.children [
                Button.create [
                    Button.onClick (fun _ -> dispatch Increment)
                    Button.content "click to increment"
                ]
                Button.create [
                    Button.onClick (fun _ -> dispatch Decrement)
                    Button.content "click to decrement" 
                ]
                TextBlock.create [
                    TextBlock.dock Dock.Top
                    TextBlock.text (sprintf "the count is %i" state.count)
                ]
            ]
        ]    
```

Find more examples using Elmish in the [Elmish Examples folder](https://github.com/fsprojects/Avalonia.FuncUI/tree/master/src/Examples/Elmish%20Examples)

# Maintainer(s)

The current co-maintainers of Avalonia.FuncUI are

* @JordanMarr
* @sleepyfran
* @JaggerJo (project originator)

The default maintainer account for projects under "fsprojects" is @fsprojectsgit - F# Community Project Incubation Space (repo management)

# Commercial Support & Development Services

Please reach out to mail@jaggerjo.com if you need a commercial support contract or are interested in contract work.


