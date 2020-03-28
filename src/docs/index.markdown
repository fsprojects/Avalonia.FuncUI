
<p align="center"><img src="https://raw.githubusercontent.com/AvaloniaCommunity/Avalonia.FuncUI/master/github/img/logo/FuncUI.png" width="224px" alt="Avalonia FuncUI"></p>
<h1 align="center">Avalonia FuncUI</h1>
<p align="center">Develop cross-platform MVU GUI Applications using F# and Avalonia!</p>
<p align="center">
<a href="https://voyonic-labs.visualstudio.com/Avalonia.FuncUI/_apis/build/status/AvaloniaCommunity.Avalonia.FuncUI?branchName=master"><img src="https://voyonic-labs.visualstudio.com/Avalonia.FuncUI/_apis/build/status/AvaloniaCommunity.Avalonia.FuncUI?branchName=master"></a>
<img src="https://img.shields.io/github/languages/top/AvaloniaCommunity/Avalonia.FuncUI" alt="GitHub top language">
<img alt="GitHub repo size" src="https://img.shields.io/github/repo-size/AvaloniaCommunity/Avalonia.FuncUI">
<img src="https://img.shields.io/github/license/AvaloniaCommunity/Avalonia.FuncUI">
<a href="https://gitter.im/Avalonia-FuncUI/community?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge"><img src="https://badges.gitter.im/Avalonia-FuncUI/community.svg"></a>
</p>
<br>

![](https://raw.githubusercontent.com/AvaloniaCommunity/Avalonia.FuncUI/master/github/img/hero.png)
*(Application was created using Avalonia.FuncUI!)*

## About
This library allows you to write cross-platform GUI Applications entirely in F# - No XAML, but a declarative Elm-like DSL. MVU (Model-View-Update) architecture support is built in, and bindings to use it with Elmish are also ready to use.

## Getting started
Check out the [Wiki](https://github.com/AvaloniaCommunity/Avalonia.FuncUI/wiki) and [Examples](https://github.com/JaggerJo/Avalonia.FuncUI/tree/master/src/Examples).

### 🧱 [Templates](https://github.com/AvaloniaCommunity/Avalonia.FuncUI.ProjectTemplates)

## Contributing
Please contribute to this library through issue reports, pull requests, code reviews, documentation, and discussion. 

## Example
Below is the code of a simple counter app (using the Avalonia.FuncUI.Elmish package).

```fsharp
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
