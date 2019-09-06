namespace Avalonia.FuncUI.VirtualDom

open System
open Avalonia.FuncUI.Core
open Avalonia.FuncUI.Core.Domain

module internal rec Delta =
    
    type AttrDelta =
        | Property of PropertyDelta
        | Content of ContentDelta
        with
            static member From (attr: IAttr) : AttrDelta =
                match attr with
                | Property' property -> Property (PropertyDelta.From property)
                | Content' content -> Content (ContentDelta.From content)
           
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
                
    type SubscriptionDelta =
        {
            accessor: Accessor
            handler: Delegate option
            funcType: Type
        }
        with
            static member From (subscription: Subscription) : SubscriptionDelta =
                {
                    accessor = subscription.accessor;
                    handler = subscription.handler;
                    funcType = subscription.funcType;
                }
    
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