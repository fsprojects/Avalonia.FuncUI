<img src="github/img/icon.png" width="100"/>

# Avalonia.FuncUI

[![Build Status](https://voyonic-labs.visualstudio.com/Avalonia.FuncUI/_apis/build/status/JaggerJo.Avalonia.FuncUI?branchName=master)](https://voyonic-labs.visualstudio.com/Avalonia.FuncUI/_build/latest?definitionId=10&branchName=master)

Develop cross-plattform MVU GUI Applications using F# and Avalonia!

## About
This library allows you to write cross-plattform GUI Applications entirely in F# - No XAML, but a declarative elm like DSL. MVU (Model-View-Update) architecture support is built in, and bindings to use it with Elmish are also ready to use.

## Current State
Should be usable, but expect bugs and some breaking changes. Basically wet paint.

## Contributing
Please contribute to this library through issue reports, pull requests, code reviews and discussion.

## Getting started
I'm currently working on making this availiable on nuget.
Project templates will follow soon.

For now check out the [Examples](https://github.com/JaggerJo/Avalonia.FuncUI/tree/master/src/Examples).

## Example
Below is the code of a simple counter app (using the Avalonia.FuncUI.Elmish package).

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
    
    let view (state: CounterState) (dispatch): View =
        Views.dockPanel [
            Attrs.children [
                Views.button [
                    Attrs.click (fun sender args -> dispatch Increment)
                    Attrs.content "click to increment"
                ]
                Views.button [
                    Attrs.click (fun sender args -> dispatch Decrement)
                    Attrs.content "click to decrement" 
                ]
                Views.textBlock [
                    Attrs.dockPanel_dock Dock.Top
                    Attrs.text (sprintf "the count is %i" state.count)
                ]
            ]
        ]    
```
