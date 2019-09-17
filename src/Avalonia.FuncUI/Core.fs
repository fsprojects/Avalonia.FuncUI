namespace Avalonia.FuncUI

open System

module Core =
   
    module rec Domain =
        type Accessor =
            | Instance of string
            | Avalonia of Avalonia.AvaloniaProperty
            | Event of Avalonia.Interactivity.RoutedEvent
                
        type PropertyType = Direct | Attached
        
        type Property =
            {
                accessor: Accessor
                value: obj
            }
            
        type Subscription =
            {
                accessor: Accessor
                handler: Delegate option
                funcType: Type
            }
            
        type Content =
            {
                accessor: Accessor
                content: ViewContent
            }
            
        type ViewContent =
            | Single of IView option
            | Multiple of IView list
                               
        type IAttr =
            abstract member UniqueName : string
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
                        | Avalonia avalonia -> avalonia.Name
                        | Instance name -> name
                    | Content content ->
                        match content.accessor with
                        | Avalonia avalonia -> avalonia.Name
                        | Instance name -> name
                    | Subscription content ->
                        match content.accessor with
                        | Avalonia avalonia -> String.Concat(avalonia.Name, ".Subscription")
                        | Instance name -> name
                
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
            
            module Property =
                /// create a direct property attr
                let createDirect (accessor: Accessor, value: #obj) =
                    {
                        Property.accessor = accessor;
                        Property.value = value
                    }
                    
                /// create an attached property attr
                let createAttached (accessor: Accessor, value: #obj) =
                    {
                        Property.accessor = accessor;
                        Property.value = value
                    }
                    
            module Subscription =
                /// create a subscription attr
                let create (accessor: Accessor, func: 'arg -> unit) =
                    {
                        Subscription.accessor = accessor;
                        Subscription.handler = Some (Action<_>(func) :> Delegate)
                        Subscription.funcType = func.GetType()
                    }
                    
            module Content =
                /// create single content attr
                let createSingle (accessor: Accessor, content: IView option) =
                    {
                        Content.accessor = accessor;
                        Content.content = ViewContent.Single content;
                    }
                    
                /// create single content attr
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
                let create<'viewType>(attrs: #IAttr<'viewType> list) : IView<'viewType> =
                    {
                        View.viewType = typeof<'viewType>
                        View.attrs = attrs
                    }
                    :> IView<'viewType>
                    
                /// create a new loosely typed View
                let create'<'viewType>(attrs: #IAttr<'viewType> list) : IView =
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