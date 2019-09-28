namespace Avalonia.FuncUI.VirtualDom

open System
open System.Threading

open Avalonia.FuncUI.Core
open Avalonia.FuncUI.Core.Domain

module internal rec Delta =
    
    type AttrDelta =
        | Property of PropertyDelta
        | Content of ContentDelta
        | Subscription of SubscriptionDelta   
        with
            static member From (attr: IAttr) : AttrDelta =
                match attr with
                | Property' property -> Property (PropertyDelta.From property)
                | Content' content -> Content (ContentDelta.From content)
                | Subscription' subscription -> Subscription (SubscriptionDelta.From subscription)
                | _ -> raise (Exception "unknown IAttr type. (not a Property, Content ore Subscription attribute)")
           
    type PropertyDelta =
        {
            accessor: Accessor
            value: obj option
        }
        with
            static member From (property: Property) : PropertyDelta =
                {
                    accessor = property.accessor;
                    value = Some property.value
                }
                
    [<CustomEquality; NoComparison>]
    type SubscriptionDelta =
        {
            name: string
            subscribe:  Avalonia.Controls.IControl * Delegate -> CancellationTokenSource
            funcType: Type
            func: Delegate option
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
                
            static member From (subscription: Subscription) : SubscriptionDelta =
                {
                    name = subscription.name;
                    subscribe = subscription.subscribe;
                    funcType = subscription.funcType;
                    func = Some subscription.func
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