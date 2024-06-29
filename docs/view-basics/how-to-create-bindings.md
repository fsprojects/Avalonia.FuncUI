# How to create bindings

Creating bindings for Avalonia controls is fairly easy. You just need to know a little bit about the source of the control and its public properties/events.

You can create bindings for any public styled/direct properties for example the `IsPressed` [Property of Button.cs](https://github.com/AvaloniaUI/Avalonia/blob/master/src/Avalonia.Controls/Button.cs#L78)

```csharp
public static readonly StyledProperty<bool> IsPressedProperty = 
    AvaloniaProperty.Register<Button, bool>(nameof(IsPressed));
```

In the case of an event, the event should be a `RoutedEvent` for example the [ClickEvent of Button.cs](https://github.com/AvaloniaUI/Avalonia/blob/master/src/Avalonia.Controls/Button.cs#L75)

```csharp
/// <summary>
/// Defines the <see cref="Click"/> event.
/// </summary>
public static readonly RoutedEvent<RoutedEventArgs> ClickEvent =
    RoutedEvent.Register<Button, RoutedEventArgs>(nameof(Click), RoutingStrategies.Bubble);
```

That's all we need to know before we start creating a binding. To create bindings like Avalonia.FuncUI ones, we need to define a module with the name of the control and add two things, a `create` function and _augment_ the existing control with some static methods.

Let's check [Button.fs](https://github.com/AvaloniaCommunity/Avalonia.FuncUI/blob/master/src/Avalonia.FuncUI.DSL/Button.fs) in Avalonia.FuncUI.source code

```fsharp
[<AutoOpen>]
module Button =
    (* omitting the other existing open statements for clarity *)
    open Avalonia.Controls
    open Avalonia.FuncUI.Builder
    open Avalonia.FuncUI.Types
    (* omitting the other existing open statements for clarity *)

    let create(attrs: IAttr<Button> list): IView<Button> =
        ViewBuilder.Create<Button>(attrs)

    type Button with 
        (* omitting the other existing bindings for clarity *)

        static member isPressed<'t when 't :> Button>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(Button.IsPressedProperty, value, ValueNone)

        static member onClick<'t when 't :> Button>(func: RoutedEventArgs -> unit, ?subPatchOptions) =
            AttrBuilder<'t>.CreateSubscription<RoutedEventArgs>(Button.ClickEvent, func, ?subPatchOptions = subPatchOptions)

        (* omitting the other existing bindings for clarity *)
```

Please note that in the case of events, there is an optional value `subPatchOptions` that is provided for performance reasons

```fsharp
type [<Struct>] SubPatchOptions =
    /// Always updates the subscription. This should be used if you can't explicitly express your outer dependencies.
    | Always
    /// Never updates the subscription. This should be used most of the time. Use this if you don't depend on outer dependencies.
    | Never
    /// Update if 't changed. This is useful if you're using some state ('t) and need to update the subscription if that state changed.
    | OnChangeOf of obj
```

this property will indicate to Avalonia.FuncUI when to update a subscription.

You can also provide overloaded methods to improve the API surface of a control for example in [Textblock.fs](https://github.com/AvaloniaCommunity/Avalonia.FuncUI/blob/master/src/Avalonia.FuncUI.DSL/TextBlock.fs) we provide two `background` functions, one takes an IBrush and the other one takes a string.

```fsharp

[<AutoOpen>]
module TextBlock =
    open Avalonia
    open Avalonia.Controls
    open Avalonia.Media
    open Avalonia.Media.Immutable
    open Avalonia.FuncUI.Builder
    open Avalonia.FuncUI.Types

    let create (attrs: IAttr<TextBlock> list): IView<TextBlock> =
        ViewBuilder.Create<TextBlock>(attrs)

    type TextBlock with
        (* omitting other bindings for clarity *)

        static member background<'t when 't :> TextBlock>(value: IBrush) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<IBrush>(TextBlock.BackgroundProperty, value, ValueNone)

        static member background<'t when 't :> TextBlock>(color: string) : IAttr<'t> =
            color |> Color.Parse |> ImmutableSolidColorBrush |> TextBlock.background

        (* omitting other bindings for clarity *)
```

If you wanted you could also add a `Color` overload to ease developer's time if it were the case

```fsharp
static member background<'t when 't :> TextBlock>(color: Color) : IAttr<'t> =
    color |> ImmutableSolidColorBrush |> TextBlock.background
```
