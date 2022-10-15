namespace Avalonia.FuncUI.DSL

open System

type [<Struct>] SubPatchOptions =
    /// Always updates the subscription. This should be used if you can't explicitly express your outer dependencies.
    | Always
    /// Never updates the subscription. This should be used most of the time. Use this if you don't depend on outer dependencies.
    | Never
    /// Update if 't changed. This is useful if your using some state ('t) and need to update the subscription if that state changed.
    | OnChangeOf of obj

    member internal this.ToScope () : obj =
        match this with
        | Always -> Guid.NewGuid() :> _
        | Never -> null
        | OnChangeOf t -> t


namespace Avalonia.FuncUI.Builder

open System
open System.Threading

open Avalonia
open Avalonia.Interactivity
open Avalonia.Controls
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Types
open Avalonia.FuncUI.Library

module private Helpers =
    let wrappedGetter<'view, 'value>(func: 'view -> 'value) : IAvaloniaObject -> obj =
        let wrapper (control: IAvaloniaObject) : obj =
            let view = control :> obj :?> 'view
            let value = func view
            value :> obj
        wrapper

    let wrappedSetter<'view, 'value>(func: 'view * 'value -> unit) : IAvaloniaObject * obj -> unit =
        let wrapper (control: IAvaloniaObject, value: obj) : unit =
            let view = control :> obj :?> 'view
            let value = value :?> 'value
            func(view, value)
        wrapper

type Comparer = obj * obj -> bool

[<AbstractClass; Sealed>]
type AttrBuilder<'view>() =

    static member private CreateContent(accessor: Accessor, content: ViewContent) : IAttr<'view> =
        let attr = Attr<'view>.Content {
            Accessor = accessor
            Content = content
        }
        attr :> IAttr<'view>

    static member private CreateProperty(accessor: Accessor, value: obj, comparer, defaultValueFactory) : IAttr<'view> =
        let attr = Attr<'view>.Property {
            Accessor = accessor
            Value = value
            Comparer = comparer
            DefaultValueFactory = defaultValueFactory
        }
        attr :> IAttr<'view>

    /// Create a Property Attribute for an Avalonia Property
    static member CreateProperty<'value>(property: AvaloniaProperty<'value>, value: 'value, comparer) : IAttr<'view> =
        AttrBuilder<'view>.CreateProperty(Accessor.AvaloniaProperty property, value :> obj, comparer, ValueNone)

    /// Create a Property Attribute for an Avalonia Property
    static member CreateProperty<'value>(property: AvaloniaProperty<'value>, value: 'value, comparer, defaultValueFactory: (unit -> 'value)) : IAttr<'view> =
        let objFactory = (fun () -> defaultValueFactory() :> obj) |> ValueSome
        AttrBuilder<'view>.CreateProperty(Accessor.AvaloniaProperty property, value :> obj, comparer, objFactory)

    // Create a Property Attribute for an Avalonia Property of ITemplate<_>
    static member CreateProperty<'value>(property: AvaloniaProperty<ITemplate<_>>, value: 'value, comparer) : IAttr<'view> =
        AttrBuilder<'view>.CreateProperty(Accessor.AvaloniaProperty property, value :> obj, comparer, ValueNone)

    // Create a string-type Property Attribute for an Avalonia Property of type obj
    static member CreateProperty<'value>(property: AvaloniaProperty<obj>, value: string, comparer) : IAttr<'view> =
        AttrBuilder<'view>.CreateProperty(Accessor.AvaloniaProperty property, value :> obj, comparer, ValueNone)

    /// Create a Property Attribute for an instance (non Avalonia) Property
    static member private CreateInstanceProperty<'value>(name: string, value: 'value, getter: ('view -> 'value) voption, setter: ('view * 'value -> unit) voption, comparer: Comparer voption, defaultValueFactory: (unit -> 'value) voption): IAttr<'view> =
        let accessor = Accessor.InstanceProperty {
            Name = name
            Getter =
                match getter with
                | ValueSome getter -> Helpers.wrappedGetter<'view, 'value>(getter) |> ValueSome
                | ValueNone -> ValueNone
            Setter =
                match setter with
                | ValueSome setter -> Helpers.wrappedSetter<'view, 'value>(setter) |> ValueSome
                | ValueNone -> ValueNone
        }

        let defValueFactory = defaultValueFactory |> ValueOption.map (fun f -> fun () -> f() :> obj)

        AttrBuilder.CreateProperty(accessor, value, comparer, defValueFactory)

    /// Create a Property Attribute for an instance (non Avalonia) Property
    static member CreateProperty<'value>(name, value, getter, setter, comparer, defaultValueFactory): IAttr<'view> =
        AttrBuilder<'view>.CreateInstanceProperty<'value>(name, value, getter, setter, comparer, defaultValueFactory |> ValueSome)

    /// <summary>
    /// Create a Property Attribute for an instance (non Avalonia) Property
    /// </summary>
    static member CreateProperty<'value>(name, value, getter, setter, comparer): IAttr<'view> =
        AttrBuilder<'view>.CreateInstanceProperty<'value>(name, value, getter, setter, comparer, ValueNone)

    /// <summary>
    /// Create a Single Content Attribute for an Avalonia Property
    /// </summary>
    static member CreateContentSingle(property: AvaloniaProperty, singleContent: IView option) : IAttr<'view> =
        AttrBuilder<'view>.CreateContent(Accessor.AvaloniaProperty property, ViewContent.Single singleContent)

    /// <summary>
    /// Create a Single Content Attribute for an instance (non Avalonia) Property
    /// </summary>
    static member CreateContentSingle(name: string, getter, setter, singleContent: IView option) : IAttr<'view> =
        let accessor = Accessor.InstanceProperty {
            Name = name
            Getter =
                match getter with
                | ValueSome getter -> Helpers.wrappedGetter<'view, obj>(getter) |> ValueSome
                | ValueNone -> ValueNone
            Setter =
                match setter with
                | ValueSome setter -> Helpers.wrappedSetter<'view, obj>(setter) |> ValueSome
                | ValueNone -> ValueNone
        }
        AttrBuilder<'view>.CreateContent(accessor, ViewContent.Single singleContent)

    /// <summary>
    /// Create a Multiple Content Attribute for an Avalonia Property
    /// </summary>
    static member CreateContentMultiple(property: AvaloniaProperty, multipleContent: IView list) : IAttr<'view> =
        AttrBuilder<'view>.CreateContent(Accessor.AvaloniaProperty property, ViewContent.Multiple multipleContent)

    /// <summary>
    /// Create a Multiple Content Attribute for an instance (non Avalonia) Property
    /// </summary>
    static member CreateContentMultiple(name: string, getter, setter, multipleContent: IView list) : IAttr<'view> =
        let accessor = Accessor.InstanceProperty {
            Name = name
            Getter =
                match getter with
                | ValueSome getter -> Helpers.wrappedGetter<'view, obj>(getter) |> ValueSome
                | ValueNone -> ValueNone
            Setter =
                match setter with
                | ValueSome setter -> Helpers.wrappedSetter<'view, obj>(setter) |> ValueSome
                | ValueNone -> ValueNone
        }
        AttrBuilder<'view>.CreateContent(accessor, ViewContent.Multiple multipleContent)

    /// <summary>
    /// Create a Property Subscription Attribute for an Avalonia Direct Property
    /// </summary>
    static member CreateSubscription<'arg, 'owner when 'owner :> AvaloniaObject>(property: DirectProperty<'owner, 'arg>, func: 'arg -> unit, ?subPatchOptions: SubPatchOptions) : IAttr<'view> =
        // subscribe to avalonia property
        // TODO: extract to helpers module
        let subscribeFunc (control: IAvaloniaObject, _handler: 'h) =
            let cts = new CancellationTokenSource()
            control
                .GetObservable(property)
                .Subscribe(func, cts.Token)
            cts

        let attr = Attr<'view>.Subscription {
            Name = property.Name + ".PropertySub"
            Subscribe = subscribeFunc
            Func = Action<_>(func)
            FuncType = func.GetType()
            Scope = (Option.defaultValue SubPatchOptions.Never subPatchOptions).ToScope()
        }
        attr :> IAttr<'view>

    /// <summary>
    /// Create a Property Subscription Attribute for an Avalonia Property
    /// </summary>
    static member CreateSubscription<'arg>(property: AvaloniaProperty<'arg>, func: 'arg -> unit, ?subPatchOptions: SubPatchOptions) : IAttr<'view> =
        // subscribe to avalonia property
        // TODO: extract to helpers module
        let subscribeFunc (control: IAvaloniaObject, _handler: 'h) =
            let cts = new CancellationTokenSource()
            control
                .GetObservable(property)
                .Subscribe(func, cts.Token)
            cts

        let attr = Attr<'view>.Subscription {
            Name = property.Name + ".PropertySub"
            Subscribe = subscribeFunc
            Func = Action<_>(func)
            FuncType = func.GetType()
            Scope = (Option.defaultValue SubPatchOptions.Never subPatchOptions).ToScope()
        }
        attr :> IAttr<'view>

     /// <summary>
    /// Create a Routed Event Subscription Attribute for a Routed Event
    /// </summary>
    static member CreateSubscription<'arg when 'arg :> RoutedEventArgs>(routedEvent: RoutedEvent<'arg>, func: 'arg -> unit, ?subPatchOptions: SubPatchOptions) : IAttr<'view> =
        // subscribe to avalonia property
        // TODO: extract to helpers module
        let subscribeFunc (control: IControl, _handler: 'h) =
            let cts = new CancellationTokenSource()
            control
                .GetObservable(routedEvent)
                .Subscribe(func, cts.Token)
            cts

        let attr = Attr<'view>.Subscription {
            Name = routedEvent.Name + ".RoutedEventSub"
            Subscribe = subscribeFunc
            Func = Action<_>(func)
            FuncType = func.GetType()
            Scope = (Option.defaultValue SubPatchOptions.Never subPatchOptions).ToScope()
        }
        attr :> IAttr<'view>

    /// <summary>
    /// Create a Event Subscription Attribute for a .Net Event
    /// </summary>
    static member CreateSubscription<'arg>(name: string, factory: IAvaloniaObject * ('arg -> unit) * CancellationToken -> unit, func: 'arg -> unit, ?subPatchOptions: SubPatchOptions) =
        // TODO: extract to helpers module
        // subscribe to event
        let subscribeFunc (control: IAvaloniaObject, _handler: 'h) =
            let cts = new CancellationTokenSource()
            factory(control, func, cts.Token)
            cts

        let attr = Attr<'view>.Subscription {
            Name = name + ".EventSub"
            Subscribe = subscribeFunc
            Func = Action<_>(func)
            FuncType = func.GetType()
            Scope = (Option.defaultValue SubPatchOptions.Never subPatchOptions).ToScope()
        }

        attr :> IAttr<'view>
        
[<AbstractClass; Sealed>]
type ViewBuilder() =

    static member Create<'view>(attrs: IAttr<'view> list) : IView<'view> =
        { View.ViewType = typeof<'view>
          View.ViewKey = ValueNone
          View.Attrs = attrs
          View.ConstructorArgs = null
          View.Outlet = ValueNone }
