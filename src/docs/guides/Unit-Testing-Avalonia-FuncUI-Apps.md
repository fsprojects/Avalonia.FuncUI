---
layout: guide
title: Unit Testing Avalonia.FuncUI
author: Avalonia Community
list-order: 5
guide-category: beginner
---

[Full Template]: guides/Full-Template.html
[Expecto]: https://github.com/haf/expecto

Testing `Avalonia.FuncUI apps` is pretty simple if you have previous experience using Elmish it should be not much different
for the moment let's dive into it.

> **Note**: For this document, we'll use [Expecto] unit test library.


I'm using Powershell Core but feel free to follow on the terminal you like most (if it's bash like remember to change `;` for `&`)
```
PS ~/github> mkdir TestingExample; cd TestingExample

PS ~/github/TestingExample> dotnet new sln
The template "Solution File" was created successfully.

PS ~/github/TestingExample> dotnet new funcui.full -o TestingExample ; dotnet new expecto -o TestingExample.Tests
The template "Avalonia FuncUI App (with extras)" was created successfully.
The template "Expecto .net core Template" was created successfully.

PS ~/github/TestingExample> dotnet sln add TestingExample ; dotnet sln add TestingExample.Tests
Project `TestingExample\TestingExample.fsproj` added to the solution.
Project `TestingExample.Tests\TestingExample.Tests.fsproj` added to the solution.

PS ~/github/TestingExample> dotnet add TestingExample.Tests reference TestingExample
Reference `..\TestingExample\TestingExample.fsproj` added to the project.

```
This gives us a [Full Template] and a Expecto project to start our testing.

First, let's replace the default content of the tests inside `TestingExample.Tests.Sample.fs` with the following

```fsharp
module Tests

open Expecto

[<Tests>]
let tests =
  testList "Counter Tests" []
```

Our **Full Template** has a fully working counter ready to run.

```fsharp
namespace TestingExample

module Counter =
    open Avalonia.Controls
    open Avalonia.FuncUI.DSL
    open Avalonia.Layout
    
    type State = { count : int }
    let init = { count = 0 }

    type Msg = Increment | Decrement | Reset

    let update (msg: Msg) (state: State) : State =
        match msg with
        | Increment -> { state with count = state.count + 1 }
        | Decrement -> { state with count = state.count - 1 }
        | Reset -> init
    
    let view (state: State) (dispatch) =
        DockPanel.create [
            DockPanel.children [
                StackPanel.create [
                    StackPanel.dock Dock.Bottom
                    StackPanel.margin 5.0
                    StackPanel.spacing 5.0
                    StackPanel.children [
                        Button.create [
                            Button.onClick (fun _ -> dispatch Increment)
                            Button.content "+"
                            Button.classes [ "plus" ]
                        ]
                        Button.create [
                            Button.onClick (fun _ -> dispatch Decrement)
                            Button.content "-"
                            Button.classes [ "minus" ]
                        ]
                        Button.create [
                            Button.onClick (fun _ -> dispatch Reset)
                            Button.content "reset"
                        ]                         
                    ]
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

let's start adding test cases, `Increment` first:
```fsharp
module Tests

open Expecto
open TestingExample

[<Tests>]
let tests =
    testList "Counter Tests"
        [ testCase "Increment should increment counter by 1" <| fun _ ->
            let initialState: Counter.State = { count = 1 }
            let updateMessages = [ Counter.Msg.Increment; Counter.Msg.Increment ]

            let actual =
                updateMessages |> List.fold (fun state message -> Counter.update message state) initialState
            Expect.equal actual.count 2 "Expected count to be 2" ]
```
If you run that code with `dotnet test` right now you will get a failure
```
Microsoft (R) Test Execution Command Line Tool Version 16.3.0
Copyright (c) Microsoft Corporation.  All rights reserved.

Starting test execution, please wait...

A total of 1 test files matched the specified pattern.

  X Counter Tests/Increment should increment counter by 1 [65ms]
  Error Message:
   
Expected count to be 2.
expected: 2
  actual: 3
  // ... stack trace ...

Test Run Failed.
Total tests: 1
     Passed: 0
     Failed: 1
```
the simple fix is to change this line
```fsharp
let initialState: Counter.State = { count = 1 }
```

to

```fsharp
let initialState: Counter.State = { count = 0 }
```

If you run `dotnet test` again
```
Microsoft (R) Test Execution Command Line Tool Version 16.3.0
Copyright (c) Microsoft Corporation.  All rights reserved.

Starting test execution, please wait...

A total of 1 test files matched the specified pattern.


Test Run Successful.
Total tests: 1
     Passed: 1
```
let's add the `Decrement` test

```fsharp
module Tests

open Expecto
open TestingExample

[<Tests>]
let tests =
    testList "Counter Tests"
        [ // ... testCase "Increment should increment counter by 1" ...
          testCase "Decrement should decrement counter by 1" <| fun _ ->
              let initialState: Counter.State = { count = 0 }
              let updateMessages = [ Counter.Msg.Decrement; Counter.Msg.Decrement ]

              let actual =
                  updateMessages |> List.fold (fun state message -> Counter.update message state) initialState
              Expect.equal actual.count -2 "Expected count to be -2" ]
```
Pretty simple huh?

Let's add the `Reset` test case
```fsharp
module Tests

open Expecto
open TestingExample

[<Tests>]
let tests =
    testList "Counter Tests"
        [ // ... testCase "Increment should increment counter by 1" ...
          // ..testCase "Decrement should decrement counter by 1" ...
          testCase "Reset should get counter back to 0" <| fun _ ->
              let initialState: Counter.State = { count = 5 }

              let actual = Counter.update Counter.Reset initialState
              Expect.equal actual.count 0 "Expected count to be 0" ]
```
the advantage of having a central place to do updates and that we always return a `State` is that we can provide the exact state we want and have a predictable test ouput.

You don't need to provide a list of `Msg`'s but here we put a simple example on how can you provide a set of "steps" to archieve a specific state, this could be useful to you if you need to test a specific workflow or something similar.

Lastly, Here's the full suite of tests.
```fsharp
module Tests

open Expecto
open TestingExample

[<Tests>]
let tests =
    testList "Counter Tests"
        [ testCase "Increment should increment counter by 1" <| fun _ ->
            let initialState: Counter.State = { count = 0 }
            // "fire" two increments
            let updateMessages = [ Counter.Msg.Increment; Counter.Msg.Increment ]

            let actual =
                updateMessages |> List.fold (fun state message -> Counter.update message state) initialState
            Expect.equal actual.count 2 "Expected count to be 2"
          testCase "Decrement should decrement counter by 1" <| fun _ ->
              let initialState: Counter.State = { count = 0 }
              // "fire" two decrements
              let updateMessages = [ Counter.Msg.Decrement; Counter.Msg.Decrement ]

              let actual =
                  updateMessages |> List.fold (fun state message -> Counter.update message state) initialState
              Expect.equal actual.count -2 "Expected count to be -2"
          testCase "Reset should get counter back to 0" <| fun _ ->
              // set a initial state different from 0
              let initialState: Counter.State = { count = 5 }

              let actual = Counter.update Counter.Reset initialState
              Expect.equal actual.count 0 "Expected count to be 0" ]

```
