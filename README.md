[![Stand With Ukraine](https://raw.githubusercontent.com/vshymanskyy/StandWithUkraine/main/banner2-direct.svg)](https://vshymanskyy.github.io/StandWithUkraine)


<p align="center"><img src="github/img/logo/FuncUI.png" width="400px" alt="Avalonia FuncUI"></p>
<h1 align="center">Avalonia FuncUI</h1>
<p align="center">Develop cross-platform MVU GUI Applications using F# and Avalonia!</p>
<p align="center">
<a href="https://voyonic-labs.visualstudio.com/Avalonia.FuncUI/_apis/build/status/AvaloniaCommunity.Avalonia.FuncUI?branchName=master"><img src="https://voyonic-labs.visualstudio.com/Avalonia.FuncUI/_apis/build/status/AvaloniaCommunity.Avalonia.FuncUI?branchName=master"></a>
<img src="https://img.shields.io/github/languages/top/JaggerJo/Avalonia.FuncUI" alt="GitHub top language">
<img alt="GitHub repo size" src="https://img.shields.io/github/repo-size/JaggerJo/Avalonia.FuncUI">
<img src="https://img.shields.io/github/license/JaggerJo/Avalonia.FuncUI">
<a href="https://gitter.im/Avalonia-FuncUI/community?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge"><img src="https://badges.gitter.im/Avalonia-FuncUI/community.svg"></a>
</p><br>

![](github/img/hero.png)
*(Application was created using Avalonia.FuncUI!)*

## About
This library allows you to write cross-platform GUI Applications entirely in F# - No XAML, but either using React/Sutil inspired components or a declarative Elm-like DSL with MVU (Model-View-Update) architecture support and Elmish bindings built-in.

## Getting started

- 🧱[Templates](./src/Avalonia.FuncUI.Templates)

- 📓[Examples](https://github.com/fsprojects/Avalonia.FuncUI/tree/master/src/Examples)

- 📚[Documentation](https://avaloniacommunity.github.io/Avalonia.FuncUI.Docs/)

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
                        Button.onClick (fun _ -> state.Current - 1 |> state.Set)
                        Button.content "click to decrement"
                    ]
                    Button.create [
                        Button.onClick (fun _ -> state.Current + 1 |> state.Set)
                        Button.content "click to increment"
                    ]
                    ]
                    TextBlock.create [
                        TextBlock.dock Dock.Top
                        TextBlock.text (string state.Current)
                    ]
                ]
            ]
        )
```

This and more examples using the component library in the [Components Examples folder](https://github.com/fsprojects/Avalonia.FuncUI/tree/master/src/Examples/Component%20Examples).

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

This and more examples using Elmish in the [Elmish Examples folder](https://github.com/fsprojects/Avalonia.FuncUI/tree/master/src/Examples/Elmish%20Examples)

# Maintainer(s)

The current co-maintainers of Avalonia.FuncUI are

* @JordanMarr
* @sleepyfran

with advice from the project originator @JaggerJo.

The default maintainer account for projects under "fsprojects" is @fsprojectsgit - F# Community Project Incubation Space (repo management)
