namespace rec Avalonia.FuncUI

open Avalonia
open Avalonia.Controls
open Avalonia.Interactivity
open Avalonia.FuncUI.Library
open System
open System.Threading

module Types =
    
    [<CustomEquality; NoComparison>]
    type PropertyAccessor =
        {
            name: string
            getter: (IControl -> obj) voption
            setter: (IControl * obj -> unit) voption
        }
        with
            override this.Equals (other: obj) : bool =
                match other with
                | :? PropertyAccessor as other ->
                    // getter and setter does not matter
                    this.name = other.name
                | _ -> false
                
            override this.GetHashCode () =
                this.name.GetHashCode()

    type Accessor =
        | InstanceProperty of PropertyAccessor
        | AvaloniaProperty of Avalonia.AvaloniaProperty        
            
    [<CustomEquality; NoComparison>]
    type Property =
        {
            accessor: Accessor
            value: obj
            comparer: (obj * obj -> bool) voption
        }
        with
            override this.Equals (other: obj) : bool =
                match other with
                | :? Property as other ->
                    let valueIsEqual =
                       match this.comparer with
                       | ValueSome comparer -> comparer(this.value, other.value)
                       | ValueNone -> this.value = other.value
                    
                    //this.accessor = other.accessor &&
                    valueIsEqual
                | _ -> false
                
            override this.GetHashCode () =
                (this.accessor, this.value).GetHashCode()
        
    type Content =
        {
            accessor: Accessor
            content: ViewContent
        }
         
    type ViewContent =
        | Single of IView option
        | Multiple of IView list
         
    [<CustomEquality; NoComparison>]
    type Subscription =
        {
            name: string
            subscribe:  IControl * Delegate -> CancellationTokenSource
            func: Delegate
            funcCapturesState: bool
            funcType: Type
        }
        with
            override this.Equals (other: obj) : bool =
                match other with
                | :? Subscription as other -> 
                    this.name = other.name &&
                    this.funcType = other.funcType
                | _ -> false
                
            override this.GetHashCode () =
                (this.name, this.funcType).GetHashCode()
                               
    type IAttr =
        abstract member UniqueName : string
        abstract member ForcePatch : bool
        abstract member Property : Property option
        abstract member Content : Content option
        abstract member Subscription : Subscription option
        
    type IAttr<'viewType> =
        inherit IAttr
                 
    type Attr<'viewType> =
        | Property of Property
        | Content of Content
        | Subscription of Subscription
        
        interface IAttr<'viewType> 
        interface IAttr with
            member this.UniqueName =
                match this with
                | Property property ->
                    match property.accessor with
                    | Accessor.AvaloniaProperty p -> p.Name
                    | Accessor.InstanceProperty p -> p.name
                | Content content ->
                    match content.accessor with
                    | Accessor.AvaloniaProperty p -> p.Name
                    | Accessor.InstanceProperty p -> p.name
                | Subscription subscription -> subscription.name
                
            member this.ForcePatch =
                match this with
                | Subscription subscription -> subscription.funcCapturesState
                | _ -> false
            
            member this.Property =
                match this with
                | Property value -> Some value
                | _ -> None
                
            member this.Content =
                match this with
                | Content value -> Some value
                | _ -> None
                
            member this.Subscription =
                match this with
                | Subscription value -> Some value
                | _ -> None

    type IView =
        abstract member ViewType: Type with get
        abstract member Attrs: IAttr list with get
        
    type IView<'viewType> =
        inherit IView
        abstract member Attrs: IAttr<'viewType> list with get
        
    type View<'viewType> =
        {
            viewType: Type
            attrs: IAttr<'viewType> list
        }
        
        interface IView with
            member this.ViewType =  this.viewType
            member this.Attrs =
                this.attrs |> List.map (fun attr -> attr :> IAttr)
            
        interface IView<'viewType> with
            member this.Attrs = this.attrs
                        
    [<AutoOpen>]
    module DomainFunctions =
        
        module Accessor =
            let create(name: string, getter, setter) =
                {
                    name = name
                    getter = getter
                    setter = setter
                }
        
        module Property =
            // TODO: maybe use extension methods instead ?
            // overloading and optional arguments could be really
            // helpful in the future. Or maybe just make a easier
            // to use "all in one" version of this for the DSL to use
            
            /// create a direct property attr
            let createDirect (accessor: Accessor, value: #obj) =
                {
                    Property.accessor = accessor;
                    Property.value = value
                    comparer = ValueNone
                }
                
            let createDirect' (accessor: Accessor, value: #obj, comparer) =
                {
                    Property.accessor = accessor;
                    Property.value = value
                    comparer = ValueSome comparer
                }
                
            /// create an attached property attr
            let createAttached (accessor: Accessor, value: #obj) =
                {
                    Property.accessor = accessor;
                    Property.value = value
                    comparer = ValueNone
                }
                
        module Subscription =

            // create a subscription attr
            let createFromProperty (property: AvaloniaProperty<'arg>, func: 'arg -> unit) =
                
                // subscribe to avalonia property
                let subscribeFunc (control: IControl, handler: 'h) =
                    let cts = new CancellationTokenSource()
                    control
                        .GetObservable(property)
                        .Subscribe(func, cts.Token)
                    cts
                    
                {
                    Subscription.name = property.Name + ".PropertySub"
                    Subscription.subscribe = subscribeFunc
                    Subscription.funcType = func.GetType()
                    Subscription.funcCapturesState = FunctionAnalysis.capturesState func
                    Subscription.func = Action<_>(func)
                }
                
            // create a subscription attr
            let createFromRoutedEvent (property: RoutedEvent<'arg>, func: 'arg -> unit) =
                
                // subscribe to avalonia property
                let subscribeFunc (control: IControl, handler: 'h) =
                    let cts = new CancellationTokenSource()
                    control
                        .GetObservable(property)
                        .Subscribe(func, cts.Token)
                    cts
                    
                {
                    Subscription.name = property.Name + ".RoutedEventSub"
                    Subscription.subscribe = subscribeFunc
                    Subscription.funcType = func.GetType()
                    Subscription.funcCapturesState = FunctionAnalysis.capturesState func
                    Subscription.func = Action<_>(func)
                }
                
            // create a subscription attr
            let createFromEvent (name: string, factory: IControl * ('arg -> unit) * CancellationToken -> unit, func: 'arg -> unit) =
                
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
                
        module Content =
            /// create single content attr
            let createSingle (accessor: Accessor, content: IView option) =
                {
                    Content.accessor = accessor;
                    Content.content = ViewContent.Single content;
                }
                
            /// create multiple content attr
            let createMultiple (accessor: Accessor, content: IView list) =
                {
                    Content.accessor = accessor;
                    Content.content = ViewContent.Multiple content;
                }
        
        module Attr =         
            /// create property attr
            let createProperty<'viewType>(property: Property) =
                Attr<'viewType>.Property property
                
            // create content attr
            let createContent<'viewType>(content: Content) =
                Attr<'viewType>.Content content
                
            // create subscription attr
            let createSubscription<'viewType>(subscription: Subscription) =
                Attr<'viewType>.Subscription subscription
                
        module View =
            /// create a new strongly typed View
            let create<'viewType>(attrs: IAttr<'viewType> list) : IView<'viewType> =
                {
                    View.viewType = typeof<'viewType>
                    View.attrs = attrs
                }
                :> IView<'viewType>
                
            /// create a new loosely typed View
            let create'<'viewType>(attrs: IAttr<'viewType> list) : IView =
                {
                    View.viewType = typeof<'viewType>
                    View.attrs = attrs
                }
                :> IView            
            
        let (|Property'|_|) (attr: IAttr)  =
            attr.Property
            
        let (|Content'|_|) (attr: IAttr)  =
            attr.Content
            
        let (|Subscription'|_|) (attr: IAttr)  =
            attr.Subscription