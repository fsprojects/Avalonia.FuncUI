namespace Avalonia.FuncUI.Builder

open System
open System.Threading

open Avalonia
open Avalonia.Interactivity
open Avalonia.Controls
open Avalonia.FuncUI.Types
open Avalonia.FuncUI.Library

module private Helpers =
    let wrappedGetter<'view, 'value>(func: 'view -> 'value) : IControl -> obj =
        let wrapper (control: IControl) : obj =
            let view = control :> obj :?> 'view
            let value = func view
            value :> obj
        wrapper
        
    let wrappedSetter<'view, 'value>(func: 'view * 'value -> unit) : IControl * obj -> unit =
        let wrapper (control: IControl, value: obj) : unit =
            let view = control :> obj :?> 'view
            let value = value :?> 'value
            func(view, value)
        wrapper
        
type Comparer = obj * obj -> bool
        
[<AbstractClass; Sealed>]
type AttrBuilder<'view>() =
    
    static member private CreateProperty(accessor: Accessor, value: obj, comparer: Comparer voption) : IAttr<'view> =
        let attr = Attr<'view>.Property {
            accessor = accessor
            value = value
            comparer = comparer
        }
        attr :> IAttr<'view>
        
    static member private CreateContent(accessor: Accessor, content: ViewContent) : IAttr<'view> =
        let attr = Attr<'view>.Content {
            accessor = accessor
            content = content
        }
        attr :> IAttr<'view>
        
    /// <summary>
    /// Create a Property Attribute for an Avalonia Property
    /// </summary>
    static member CreateProperty<'value>(property: AvaloniaProperty, value: 'value, comparer: Comparer voption) : IAttr<'view> =
        AttrBuilder<'view>.CreateProperty(Accessor.AvaloniaProperty property, value :> obj, comparer)
        
    /// <summary>
    /// Create a Property Attribute for an instance (non Avalonia) Property
    /// </summary>
    static member CreateProperty<'value>(name: string, value: 'value, getter: ('view -> 'value) voption, setter: ('view * 'value -> unit) voption, comparer: Comparer voption): IAttr<'view> =
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
        AttrBuilder.CreateProperty(accessor, value, comparer)
    
    /// <summary>
    /// Create a Single Content Attribute for an Avalonia Property
    /// </summary>
    static member CreateContentSingle(property: AvaloniaProperty, singleContent: IView option) : IAttr<'view> =
        AttrBuilder<'view>.CreateContent(Accessor.AvaloniaProperty property, ViewContent.Single singleContent)
        
    /// <summary>
    /// Create a Single Content Attribute for an instance (non Avalonia) Property
    /// </summary>
    static member CreateContentSingle(name: string, getter: ('view -> obj) voption, setter: ('view * obj -> unit) voption, singleContent: IView option) : IAttr<'view> =
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
        AttrBuilder<'view>.CreateContent(accessor, ViewContent.Single singleContent)
        
    /// <summary>
    /// Create a Multiple Content Attribute for an Avalonia Property
    /// </summary>
    static member CreateContentMultiple(property: AvaloniaProperty, multipleContent: IView list) : IAttr<'view> =
        AttrBuilder<'view>.CreateContent(Accessor.AvaloniaProperty property, ViewContent.Multiple multipleContent)
    
    /// <summary>
    /// Create a Multiple Content Attribute for an instance (non Avalonia) Property
    /// </summary>
    static member CreateContentMultiple(name: string, getter: ('view -> obj) voption, setter: ('view * obj -> unit) voption, multipleContent: IView list) : IAttr<'view> =
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
        AttrBuilder<'view>.CreateContent(accessor, ViewContent.Multiple multipleContent)
        
    /// <summary>
    /// Create a Property Subscription Attribute for an Avalonia Property
    /// </summary>
    static member CreateSubscription<'arg>(property: AvaloniaProperty<'arg>, func: 'arg -> unit) : IAttr<'view> =
        // subscribe to avalonia property
        // TODO: extract to helpers module
        let subscribeFunc (control: IControl, handler: 'h) =
            let cts = new CancellationTokenSource()
            control
                .GetObservable(property)
                .Subscribe(func, cts.Token)
            cts
                    
        let attr = Attr<'view>.Subscription {
            name = property.Name + ".PropertySub"
            subscribe = subscribeFunc
            funcType = func.GetType()
            funcCapturesState = FunctionAnalysis.capturesState (func :> obj)
            func = Action<_>(func)
        }
        attr :> IAttr<'view>
        
    /// <summary>
    /// Create a Routed Event Subscription Attribute for a Routed Event
    /// </summary>
    static member CreateSubscription<'arg when 'arg :> RoutedEventArgs>(property: RoutedEvent<'arg>, func: 'arg -> unit) : IAttr<'view> =
        // subscribe to avalonia property
        // TODO: extract to helpers module
        let subscribeFunc (control: IControl, handler: 'h) =
            let cts = new CancellationTokenSource()
            control
                .GetObservable(property)
                .Subscribe(func, cts.Token)
            cts
            
        let attr = Attr<'view>.Subscription {
            Subscription.name = property.Name + ".RoutedEventSub"
            Subscription.subscribe = subscribeFunc
            Subscription.funcType = func.GetType()
            Subscription.funcCapturesState = FunctionAnalysis.capturesState func
            Subscription.func = Action<_>(func)
        }
        attr :> IAttr<'view>
        
    /// <summary>
    /// Create a Event Subscription Attribute for a .Net Event
    /// </summary>
    static member CreateSubscription<'arg>(name: string, factory: IControl * ('arg -> unit) * CancellationToken -> unit, func: 'arg -> unit) =
        // TODO: extract to helpers module
        // subscribe to event
        let subscribeFunc (control: IControl, handler: 'h) =
            let cts = new CancellationTokenSource()
            factory(control, func, cts.Token)
            cts
        
        {
            Subscription.name = name + ".EventSub"
            Subscription.subscribe = subscribeFunc
            Subscription.funcType = func.GetType()
            Subscription.funcCapturesState = FunctionAnalysis.capturesState func
            Subscription.func = Action<_>(func)
        }

[<AbstractClass; Sealed>] 
type ViewBuilder() =
    
    static member Create<'view>(attrs: IAttr<'view> list) : IView<'view> =
        {
            View.viewType = typeof<'view>
            View.attrs = attrs
        }
        :> IView<'view>