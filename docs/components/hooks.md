# Hooks

### useState

This is the most basic hook which allows you to create an instance of a value which can be both read and updated in your component's code, the state is kept between renders and any updates to it will cause it to re-render by default

```fsharp
Component(fun ctx ->
    let state = ctx.useState 0
    
    // state var that does not rerender on change
    let state = ctx.useState (0, renderOnChange = false)
    
    // Component's UI
)
```

### useEffect

Sometimes you need to subscribe to an observable, to an `IWritable<'T>`, create a disposable value, or make some logging. For these and other scenarios you can use the `useEffect` hook.

This hook requires a `handler` which is a function that can return `unit` or `IDisposable` depending on your needs. For cases where you need to handle subscriptions the second will be better.

This hook also asks for a `triggers` list, these help Avalonia.FuncUI to decide when to run the handler function. The possible trigger values are defined as follows:

```fsharp
[<RequireQualifiedAccess>]
type EffectTrigger =
    /// triggers the effect to run every time after the passed dependency has changed.
    | AfterChange of state: IAnyReadable
    /// triggers the effect to run once after the component initially rendered.
    | AfterInit
    /// triggers the effect to run every time after the component is rendered.
    | AfterRender
```

For example if we had to fetch some resources from a server after the component is initialized we would do something like the following:

```fsharp
Component("use-effect-component", fun ctx ->
    let names = ctx.useState []
    let count = ctx.useState 0
    ctx.useEffect (
        handler = (fun _ ->
            // Common use cases here are HTTP Requests, Event and Observable Subscriptions
            // or any other code that could be considered a side effect
            async {
                let! nameList = httpLib.get "https://my-resources.com"
                // update the names value
                names.Set nameList
            }
            |> Async.Start
        ),
        triggers = [ EffectTrigger.AfterInit ]
    )
    // DSL code
)
```

We can also re-execute these handlers if we make them dependant of any readable values, for example let us try to compute the sum of the ages of a user list whenever the user list changes.

```fsharp
Component("use-effect-component", fun ctx ->
    let users = ctx.useState []
    let ageSum = ctx.useState 0
    ctx.useEffect (
        handler = (fun _ ->
            // Common use cases here are HTTP Requests, Event and Observable Subscriptions
            // or any other code that could be considered a side effect
            async {
                let! nameList = httpLib.get "https://my-resources.com"
                // update the names value
                names.Set nameList
            }
            |> Async.Start
        ),
        triggers = [ EffectTrigger.AfterInit ]
    )
    ctx.useEffect(
        handler = (fun _ ->
            // get sum of the ages of the users list
            users
            |> List.sumBy (fun user -> user.age)
            |> ageSum.Set
        ),
        // whenever the users list changes, trigger this effect
        triggers = [ EffectTrigger.AfterChange users ]
    )
)
```

### IReadable<'T>

```fsharp
type IReadable<'value> =
    inherit IAnyReadable
    abstract member Current: 'value with get
    abstract member Subscribe : ('value -> unit) -> IDisposable
```

As is in the source comments:

> Readable state value that can be subscribed to.

Readables are values which you can subscribe to get updates, this are common values used

An example would be the following

```fsharp
Component(fun ctx ->
    let state = ctx.useState 0

    Button.create [
        Button.content $"Count: {state.Current}"
    ]
)
```

At this point this component will be pretty much static, it won't ever be re-rendered because there are no changes to it's state.

### IWritable<'T>

```fsharp
type IWritable<'value> =
    inherit IReadable<'value>
    abstract member Set : 'value -> unit
```

As is in the source comments:

> Readable and writable state value that can be subscribed to.

If we take the previous example and add mutations it would look like the following:

```fsharp
Component(fun ctx ->
    let state = ctx.useState 0

    Button.create [
        Button.content $"Count: {state.Current}"
        Button.onClick (fun _ -> state.Current + 1 |> state.Set)
    ]
)
```

#### Passed Values

Sometimes when you already have an `IWritable<'T>` value, you would like to use it on sibling components so they can show the same data in different formats, or just to communicate changes between different component trees without having to drill the values into the component's tree.

For that we can use Passed Values.

```fsharp

let ComponentA id (value: IWritable<string>) =
    Component(id, fun ctx ->
        // Right here we can use ctx.usePassed to ensure we can both read/update a value
        let value = ctx.usePassed value
        StackPanel.create [
            StackPanel.children [
                TextBlock.create [
                    TextBlock.text $"This component can read and update this value: \"{value.Current}\""
                ]
                Button.create [
                    Button.content "Add 3"
                    Button.onClick (fun _ -> $"{value.Current}3" |> value.Set )
                ]
            ]
        ]
    )

let ComponentB id (value: IReadable<string>) =
    Component(id, fun ctx ->
        // Right here we can use ctx.usePassedRead to ensure we can only read a value
        let value = ctx.usePassedRead value
        StackPanel.create [
            StackPanel.children [
                TextBlock.create [
                    TextBlock.text $"This component can only read this value: \"{value.Current}\""
                ]
            ]
        ]
    )

let View =
    Component(fun ctx ->
        let value = ctx.useState "This is my value"
        StackPanel.create [
            StackPanel.spacing 12.
            StackPanel.children [
                // here we can use our components with the existing value
                // in other implementations these could also be called
                // "Stores"
                ComponentA "component-a" value
                ComponentB "component-b" value
                Button.create [
                    Button.content "I can add 4 from outside"
                    // since state is both readable and writable we can also
                    // modify the values from outside the child components
                    // as usual
                    Button.onClick (fun _ -> $"{value.Current}4" |> value.Set )
                ]
            ]
        ]
    )
```
