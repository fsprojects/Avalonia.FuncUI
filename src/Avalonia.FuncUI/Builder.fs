namespace Avalonia.FuncUI.DSL

open System

type [<Struct>] SubPatchOptions =
    /// Always updates the subscription. This should be used if you can't explicitly express your outer dependencies.
    | Always
    /// Never updates the subscription. This should be used most of the time. Use this if you don't depend on outer dependencies.
    | Never
    /// Update if 't changed. This is useful if your using some state ('t) and need to update the subscription if that state changed.
    | OnChangeOf of obj
    
     with
         member internal this.ToScope () : obj =
                match this with
                | Always -> Guid.NewGuid() :> obj
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
type AttrBuilder =
        
    static member private CreateContent(accessor: Accessor, content: ViewContent) : IAttr<'view> =
        let attr = Attr<'view>.Content {
            accessor = accessor
            content = content
        }
        attr :> IAttr<'view>
        
    static member private CreateProperty(accessor: Accessor, value: obj, comparer, defaultValueFactory) : IAttr<'view> =
        let attr = Attr<'view>.Property {
            accessor = accessor
            value = value
            comparer = comparer
            defaultValueFactory = defaultValueFactory
        }
        attr :> IAttr<'view>
        
    /// Create a Property Attribute for an Avalonia Property
    static member CreateProperty<'view, 'value>(property: AvaloniaProperty, value: 'value, comparer) : IAttr<'view> =
        AttrBuilder.CreateProperty(Accessor.AvaloniaProperty property, value :> obj, comparer, ValueNone)

    /// Create a Property Attribute for an Avalonia Property
    static member CreateProperty<'view, 'value>(property: AvaloniaProperty, value: 'value, comparer, defaultValueFactory: (unit -> 'value)) : IAttr<'view> =
        let objFactory = (fun () -> defaultValueFactory() :> obj) |> ValueSome
        AttrBuilder.CreateProperty(Accessor.AvaloniaProperty property, value :> obj, comparer, objFactory)
        
    /// Create a Property Attribute for an instance (non Avalonia) Property
    static member private CreateInstanceProperty<'view, 'value>(name: string, value: 'value, getter: ('view -> 'value) voption, setter: ('view * 'value -> unit) voption, comparer: Comparer voption, defaultValueFactory: (unit -> 'value) voption): IAttr<'view> =
        let accessor = Accessor.InstanceProperty {
            name = name
            getter =
                match getter with
                | ValueSome getter -> Helpers.wrappedGetter<'view, 'value>(getter) |> ValueSome
                | ValueNone -> ValueNone
            setter =
                match setter with
                | ValueSome setter -> Helpers.wrappedSetter<'view, 'value>(setter) |> ValueSome
                | ValueNone -> ValueNone
        }
        
        let defValueFactory = defaultValueFactory |> ValueOption.map (fun f -> fun () -> f() :> obj)
        
        AttrBuilder.CreateProperty(accessor, value, comparer, defValueFactory)

    /// Create a Property Attribute for an instance (non Avalonia) Property
    static member CreateProperty<'view, 'value>(name, value, getter, setter, comparer, defaultValueFactory): IAttr<'view> =
        AttrBuilder.CreateInstanceProperty<'view, 'value>(name, value, getter, setter, comparer, defaultValueFactory |> ValueSome)
        
    /// <summary>
    /// Create a Property Attribute for an instance (non Avalonia) Property
    /// </summary>
    static member CreateProperty<'view, 'value>(name, value, getter, setter, comparer): IAttr<'view> =
        AttrBuilder.CreateInstanceProperty<'view, 'value>(name, value, getter, setter, comparer, ValueNone)
    
    /// <summary>
    /// Create a Single Content Attribute for an Avalonia Property
    /// </summary>
    static member CreateContentSingle<'view>(property: AvaloniaProperty, singleContent: IView option) : IAttr<'view> =
        AttrBuilder.CreateContent(Accessor.AvaloniaProperty property, ViewContent.Single singleContent)
        
    /// <summary>
    /// Create a Single Content Attribute for an instance (non Avalonia) Property
    /// </summary>
    static member CreateContentSingle<'view>(name: string, getter, setter, singleContent: IView option) : IAttr<'view> =
        let accessor = Accessor.InstanceProperty {
            name = name
            getter =
                match getter with
                | ValueSome getter -> Helpers.wrappedGetter<'view, obj>(getter) |> ValueSome
                | ValueNone -> ValueNone
            setter =
                match setter with
                | ValueSome setter -> Helpers.wrappedSetter<'view, obj>(setter) |> ValueSome
                | ValueNone -> ValueNone
        }
        AttrBuilder.CreateContent(accessor, ViewContent.Single singleContent)
        
    /// <summary>
    /// Create a Multiple Content Attribute for an Avalonia Property
    /// </summary>
    static member CreateContentMultiple<'view>(property: AvaloniaProperty, multipleContent: IView list) : IAttr<'view> =
        AttrBuilder.CreateContent(Accessor.AvaloniaProperty property, ViewContent.Multiple multipleContent)
    
    /// <summary>
    /// Create a Multiple Content Attribute for an instance (non Avalonia) Property
    /// </summary>
    static member CreateContentMultiple<'view>(name: string, getter, setter, multipleContent: IView list) : IAttr<'view> =
        let accessor = Accessor.InstanceProperty {
            name = name
            getter =
                match getter with
                | ValueSome getter -> Helpers.wrappedGetter<'view, obj>(getter) |> ValueSome
                | ValueNone -> ValueNone
            setter =
                match setter with
                | ValueSome setter -> Helpers.wrappedSetter<'view, obj>(setter) |> ValueSome
                | ValueNone -> ValueNone
        }
        AttrBuilder.CreateContent(accessor, ViewContent.Multiple multipleContent)
        
    /// <summary>
    /// Create a Property Subscription Attribute for an Avalonia Property
    /// </summary>
    static member CreateSubscription<'view, 'arg>(property: AvaloniaProperty<'arg>, func: 'arg -> unit, ?subPatchOptions: SubPatchOptions) : IAttr<'view> =
        // subscribe to avalonia property
        // TODO: extract to helpers module
        let subscribeFunc (control: IAvaloniaObject, _handler: 'h) =
            let cts = new CancellationTokenSource()
            control
                .GetObservable(property)
                .Subscribe(func, cts.Token)
            cts
                    
        let attr = Attr<'view>.Subscription {
            name = property.Name + ".PropertySub"
            subscribe = subscribeFunc
            func = Action<_>(func)
            funcType = func.GetType()
            scope = (Option.defaultValue SubPatchOptions.Never subPatchOptions).ToScope()
        }
        attr :> IAttr<'view>
        
     /// <summary>
    /// Create a Routed Event Subscription Attribute for a Routed Event
    /// </summary>
    static member CreateSubscription<'view, 'arg when 'arg :> RoutedEventArgs and 'view :> IInteractive>(routedEvent: RoutedEvent<'arg>, func: 'arg -> unit, ?subPatchOptions: SubPatchOptions) : IAttr<'view> =
        // subscribe to avalonia property
        // TODO: extract to helpers module
        let subscribeFunc (control: IAvaloniaObject, _handler: 'h) =
            let cts = new CancellationTokenSource()
            (control :?> IInteractive)
                .GetObservable(routedEvent)
                .Subscribe(func, cts.Token)
            cts
            
        let attr = Attr<'view>.Subscription {
            name = routedEvent.Name + ".RoutedEventSub"
            subscribe = subscribeFunc
            func = Action<_>(func)
            funcType = func.GetType()
            scope = (Option.defaultValue SubPatchOptions.Never subPatchOptions).ToScope()
        }
        attr :> IAttr<'view>
        
    /// <summary>
    /// Create a Event Subscription Attribute for a .Net Event
    /// </summary>
    static member CreateSubscription<'view, 'arg>(name: string, factory: IAvaloniaObject * ('arg -> unit) * CancellationToken -> unit, func: 'arg -> unit, ?subPatchOptions: SubPatchOptions) =
        // TODO: extract to helpers module
        // subscribe to event
        let subscribeFunc (control: IAvaloniaObject, _handler: 'h) =
            let cts = new CancellationTokenSource()
            factory(control, func, cts.Token)
            cts
        
        let attr = Attr<'view>.Subscription {
            name = name + ".EventSub"
            subscribe = subscribeFunc
            func = Action<_>(func)
            funcType = func.GetType()
            scope = (Option.defaultValue SubPatchOptions.Never subPatchOptions).ToScope()
        }
        attr :> IAttr<'view>

[<AbstractClass; Sealed>] 
type ViewBuilder() =
    
    static member Create<'view>(attrs: IAttr<'view> list) : IView<'view> =
        {
            View.viewType = typeof<'view>
            View.attrs = attrs
        }
        :> IView<'view>