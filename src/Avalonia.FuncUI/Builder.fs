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
open System.Reactive.Linq
open System.Threading

open Avalonia
open Avalonia.Interactivity
open Avalonia.Controls
open Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Types
open Avalonia.FuncUI.Library

type Comparer = obj * obj -> bool

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
    
    let controlAvaloniaProperty<'arg> (p: AvaloniaProperty<'arg>) (c: IControl) (func: obj -> unit) =
        let mutable isControlling = false       
        let uncontrolledChanges =
            c.GetPropertyChangedObservable(p).Where(fun _ -> isControlling = false)
        let onUncontrolledChange (e: AvaloniaPropertyChangedEventArgs) =
            // Reset value
            isControlling <- true
            c.SetValue(p, e.OldValue) |> ignore
            isControlling <- false
            // Submit value to callback
            e.NewValue |> func
        let setValue (v: obj) =
            isControlling <- true
            let value = v :?> 'arg
            c.SetValue(p, value) |> ignore
            isControlling <- false
        let cts = new CancellationTokenSource()
        uncontrolledChanges.Subscribe(onUncontrolledChange, cts.Token)
        { SetControlledValue = setValue; Cancellation = cts }
        
    let toScope (spo: SubPatchOptions option) =
        let v = spo |> Option.defaultValue SubPatchOptions.Never
        v.ToScope()
        
    let toProperty (accessor, value: obj, comparer: Comparer voption, defaultValueFactory) =
        { Accessor = accessor
          Value = value
          Comparer = comparer
          DefaultValueFactory = defaultValueFactory }
        
    let toSubscription (name, subscribeFunc, func, subPatchOptions) =
        { Name = name
          Subscribe = subscribeFunc
          Func = Action<_>(func)
          FuncType = func.GetType()
          Scope = subPatchOptions |> toScope }
        
    let toControlledProperty (makeController, property, func, subPatchOptions) =
        { MakeController = makeController
          Property = property
          Func = func
          FuncType = func.GetType()
          Scope = subPatchOptions |> toScope }
        
    let toInstanceAccessor (name, getter, setter) =
        Accessor.InstanceProperty {
            Name = name
            Getter = getter |> ValueOption.map wrappedGetter
            Setter = setter |> ValueOption.map wrappedSetter
        }    
    
    let toPropertyAttr p = p |> Attr<'view>.Property :> IAttr<'view>
    let toSubAttr s = s |> Attr<'view>.Subscription :> IAttr<'view>
    let toControlledPropertyAttr cp = cp |> Attr<'view>.ControlledProperty :> IAttr<'view>                
        
[<AbstractClass; Sealed>]
type AttrBuilder<'view>() =
        
    static member private CreateContent(accessor: Accessor, content: ViewContent) : IAttr<'view> =
        let attr = Attr<'view>.Content {
            Accessor = accessor
            Content = content
        }
        attr :> IAttr<'view>
        
    static member private CreateProperty(accessor: Accessor, value: obj, comparer, defaultValueFactory) : IAttr<'view> =
        (accessor, value, comparer, defaultValueFactory) |> Helpers.toProperty |> Helpers.toPropertyAttr   
        
    // Create a Property Attribute for an Avalonia Property
    static member CreateProperty<'value>(property: AvaloniaProperty, value: 'value, comparer) : IAttr<'view> =
        let accessor = property |> Accessor.AvaloniaProperty
        AttrBuilder.CreateProperty(accessor, value, comparer, ValueNone)

    // Create a Property Attribute for an Avalonia Property
    static member CreateProperty<'value>(property: AvaloniaProperty, value: 'value, comparer, defaultValueFactory: (unit -> 'value)) : IAttr<'view> =
        let accessor = property |> Accessor.AvaloniaProperty
        let objFactory = (fun () -> defaultValueFactory() :> obj) |> ValueSome
        AttrBuilder.CreateProperty(accessor, value, comparer, objFactory)
        
    // Create a Property Attribute for an instance (non Avalonia) Property
    static member private CreateInstanceProperty<'value>(name: string, value: 'value, getter: ('view -> 'value) voption, setter: ('view * 'value -> unit) voption, comparer: Comparer voption, defaultValueFactory: (unit -> 'value) voption): IAttr<'view> =
        let accessor = (name, getter, setter) |> Helpers.toInstanceAccessor        
        let defValueFactory = defaultValueFactory |> ValueOption.map (fun f -> fun () -> f() :> obj)        
        AttrBuilder.CreateProperty(accessor, value, comparer, defValueFactory)

    // Create a Property Attribute for an instance (non Avalonia) Property
    static member CreateProperty<'value>(name, value, getter, setter, comparer, defaultValueFactory): IAttr<'view> =
        AttrBuilder<'view>.CreateInstanceProperty<'value>(name, value, getter, setter, comparer, defaultValueFactory |> ValueSome)
        
    // Create a Property Attribute for an instance (non Avalonia) Property
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
        let accessor = (name, getter, setter) |> Helpers.toInstanceAccessor
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
        let accessor = (name, getter, setter) |> Helpers.toInstanceAccessor
        AttrBuilder<'view>.CreateContent(accessor, ViewContent.Multiple multipleContent)
        
    /// <summary>
    /// Create a Property Subscription Attribute for an Avalonia Property
    /// </summary>
    static member CreateSubscription<'arg>(property: AvaloniaProperty<'arg>, func: 'arg -> unit, ?subPatchOptions: SubPatchOptions) : IAttr<'view> =
        let name = property.Name + ".PropertySub"
        let subscribeFunc (control: IControl, _handler: 'h) =
            let cts = new CancellationTokenSource()
            control
                .GetObservable(property)
                .Subscribe(func, cts.Token)
            cts
            
        (name, subscribeFunc, func, subPatchOptions)
        |> Helpers.toSubscription
        |> Helpers.toSubAttr                    
        
     /// <summary>
    /// Create a Routed Event Subscription Attribute for a Routed Event
    /// </summary>
    static member CreateSubscription<'arg when 'arg :> RoutedEventArgs>(routedEvent: RoutedEvent<'arg>, func: 'arg -> unit, ?subPatchOptions: SubPatchOptions) : IAttr<'view> =
        let name = routedEvent.Name + ".RoutedEventSub"
        let subscribeFunc (control: IControl, _handler: 'h) =
            let cts = new CancellationTokenSource()
            control
                .GetObservable(routedEvent)
                .Subscribe(func, cts.Token)
            cts
            
        (name, subscribeFunc, func, subPatchOptions)
        |> Helpers.toSubscription
        |> Helpers.toSubAttr
        
    /// <summary>
    /// Create a Event Subscription Attribute for a .Net Event
    /// </summary>
    static member CreateSubscription<'arg>(name: string, factory: IControl * ('arg -> unit) * CancellationToken -> unit, func: 'arg -> unit, ?subPatchOptions: SubPatchOptions) =
        let name = name + ".EventSub"
        let subscribeFunc (control: IControl, _handler: 'h) =
            let cts = new CancellationTokenSource()
            factory(control, func, cts.Token)
            cts
        
        (name, subscribeFunc, func, subPatchOptions)
        |> Helpers.toSubscription
        |> Helpers.toSubAttr
    
    // Create a ControlledProperty Attribute for an Avalonia Property        
    static member CreateControlledProperty<'value>(avaloniaProperty: AvaloniaProperty<'value>, value: 'value, func: 'value -> unit, comparer, ?subPatchOptions: SubPatchOptions) : IAttr<'view> =
        let makeController = Helpers.controlAvaloniaProperty avaloniaProperty
        let accessor = avaloniaProperty :> AvaloniaProperty |> Accessor.AvaloniaProperty
        let property = (accessor, value, comparer, ValueNone) |> Helpers.toProperty
        let castFn (arg: obj) = arg :?> 'value |> func
        (makeController, property, castFn, subPatchOptions)
        |> Helpers.toControlledProperty
        |> Helpers.toControlledPropertyAttr
        
    // Create a ControlledProperty Attribute for an Avalonia Property (with a default value factory)
    static member CreateControlledProperty<'value>(avaloniaProperty: AvaloniaProperty<'value>, value: 'value, func: 'value -> unit, comparer, defaultValueFactory: (unit -> 'value), ?subPatchOptions: SubPatchOptions) : IAttr<'view> =
        let objFactory = (fun () -> defaultValueFactory() :> obj) |> ValueSome
        let makeController = Helpers.controlAvaloniaProperty avaloniaProperty
        let accessor = avaloniaProperty :> AvaloniaProperty |> Accessor.AvaloniaProperty
        let property = (accessor, value, comparer, objFactory) |> Helpers.toProperty
        let castFn (arg: obj) = arg :?> 'value |> func
        (makeController, property, castFn, subPatchOptions)
        |> Helpers.toControlledProperty
        |> Helpers.toControlledPropertyAttr
        
    // Create a ControlledProperty Attribute for an instance (non Avalonia) Property
    static member private CreateControlledInstanceProperty<'value>(name: string, value: 'value, func: 'value -> unit, getter: ('view -> 'value) voption, setter: ('view * 'value -> unit) voption, comparer: Comparer voption, defaultValueFactory: (unit -> 'value) voption, ?subPatchOptions: SubPatchOptions) : IAttr<'view> =
        failwith "todo"
        
    // Create a ControlledProperty Attribute for an instance (non Avalonia) Property    
    static member CreateControlledInstanceProperty<'value>(name, value, func, getter, setter, comparer, defaultValueFactory): IAttr<'view> =
        AttrBuilder<'view>.CreateControlledInstanceProperty(name, value, func, getter, setter, comparer, defaultValueFactory |> ValueSome)
        
    // Create a ControlledProperty Attribute for an instance (non Avalonia) Property (with sub patch options)
    static member CreateControlledInstanceProperty<'value>(name, value, func, getter, setter, comparer, defaultValueFactory, subPatchOptions: SubPatchOptions): IAttr<'view> =
        AttrBuilder<'view>.CreateControlledInstanceProperty(name, value, func, getter, setter, comparer, defaultValueFactory |> ValueSome, subPatchOptions)
    
[<AbstractClass; Sealed>] 
type ViewBuilder() =
    
    static member Create<'view>(attrs: IAttr<'view> list) : IView<'view> =
        { View.ViewType = typeof<'view>
          View.Attrs = attrs }
        :> IView<'view>
