namespace Avalonia.FuncUI.VirtualDom

open System
open System.Threading

open Avalonia.FuncUI.Types

module internal rec Delta =
    
    type AttrDelta =
        | Property of PropertyDelta
        | Content of ContentDelta
        | Subscription of SubscriptionDelta
        | Effect of EffectDelta
        with
            static member From (attr: IAttr) : AttrDelta =
                match attr with
                | Property' property -> Property (PropertyDelta.From property)
                | Content' content -> Content (ContentDelta.From content)
                | Subscription' subscription -> Subscription (SubscriptionDelta.From subscription)
                | Effect' effect -> Effect (EffectDelta.From effect)
                | _ -> raise (Exception "unknown IAttr type. (not a Property, Content ore Subscription attribute)")
           
    [<CustomEquality; NoComparison>]
    type PropertyDelta =
        {
            accessor: Accessor
            value: obj option
            defaultValueFactory: (unit -> obj) voption
        }
        with
            static member From (property: Property) : PropertyDelta =
                {
                    accessor = property.accessor
                    value = Some property.value
                    defaultValueFactory = property.defaultValueFactory
                }
            override this.Equals (other: obj) : bool =
                match other with
                | :? PropertyDelta as other -> 
                    this.accessor = other.accessor &&
                    this.value = other.value
                | _ -> false
                    
            override this.GetHashCode () =
                (this.accessor, this.value).GetHashCode()
            
    [<CustomEquality; NoComparison>]
    type SubscriptionDelta =
        {
            name: string
            subscribe:  Avalonia.Controls.IControl * Delegate -> CancellationTokenSource
            func: Delegate option
        }
        with
            override this.Equals (other: obj) : bool =
                match other with
                | :? Subscription as other -> 
                    this.name = other.name
                | _ -> false
                    
            override this.GetHashCode () =
                (this.name).GetHashCode()
                
            static member From (subscription: Subscription) : SubscriptionDelta =
                {
                    name = subscription.name;
                    subscribe = subscription.subscribe;
                    func = Some subscription.func
                }
            member this.UniqueName = this.name
            
    [<CustomEquality; NoComparison>]
    type EffectDelta =
        {
            name: string
            func: obj -> unit
        }
        with
            override this.Equals (other: obj) : bool =
                Object.ReferenceEquals(this, other)
                    
            override this.GetHashCode () =
                this.GetHashCode()
                
            static member From (effect: Effect) : EffectDelta =
                {
                    name = effect.name
                    func = effect.func
                }
            member this.UniqueName = this.name
    
    type  ContentDelta =
        {
            accessor: Accessor
            content: ViewContentDelta
        }
        with
            static member From (content: Content) : ContentDelta =
                {
                    accessor = content.accessor;
                    content = ViewContentDelta.From content.content
                }
        
    type ViewContentDelta =
        | Single of ViewDelta option
        | Multiple of ViewDelta list
        with
            static member From (viewContent: ViewContent) : ViewContentDelta =
                match viewContent with
                | ViewContent.Single single ->
                    match single with
                    | Some view ->
                        (ViewDelta.From view)
                        |> Some
                        |> ViewContentDelta.Single
                    | None ->
                        None
                        |> ViewContentDelta.Single    
                | ViewContent.Multiple multiple ->
                    multiple
                    |> List.map (fun view -> ViewDelta.From view)
                    |> ViewContentDelta.Multiple

    type ViewDelta =
        {
            viewType: Type
            attrs: AttrDelta list
        }
        with
            static member From (view: IView) : ViewDelta =
                {
                    viewType = view.ViewType
                    attrs = view.Attrs |> List.map AttrDelta.From
                }