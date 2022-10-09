namespace Avalonia.FuncUI.VirtualDom

open System
open System.Threading

open Avalonia.FuncUI.Types

module internal rec Delta =

    type AttrDelta =
        | Property of PropertyDelta
        | Content of ContentDelta
        | Subscription of SubscriptionDelta
        | Inline of InlineDelta

        static member From (attr: IAttr) : AttrDelta =
            match attr with
            | Property' property -> Property (PropertyDelta.From property)
            | Content' content -> Content (ContentDelta.From content)
            | Subscription' subscription -> Subscription (SubscriptionDelta.From subscription)
            | Inlines' inlineElement -> Inline (InlineDelta.From inlineElement)
            | _ -> raise (Exception "unknown IAttr type. (not a Property, Content ore Subscription attribute)")


    [<CustomEquality; NoComparison>]
    type PropertyDelta =
        { Accessor: Accessor
          Value: obj option
          DefaultValueFactory: (unit -> obj) voption }

        static member From (property: Property) : PropertyDelta =
            { Accessor = property.Accessor
              Value = Some property.Value
              DefaultValueFactory = property.DefaultValueFactory }

        override this.Equals (other: obj) : bool =
            match other with
            | :? PropertyDelta as other ->
                this.Accessor = other.Accessor &&
                this.Value = other.Value
            | _ -> false

        override this.GetHashCode () =
            (this.Accessor, this.Value).GetHashCode()


    [<CustomEquality; NoComparison>]
    type SubscriptionDelta =
        { Name: string
          Subscribe: Avalonia.Controls.IControl * Delegate -> CancellationTokenSource
          Func: Delegate option }

        override this.Equals (other: obj) : bool =
            match other with
            | :? Subscription as other ->
                this.Name = other.Name
            | _ -> false

        override this.GetHashCode () =
            this.Name.GetHashCode()

        static member From (subscription: Subscription) : SubscriptionDelta =
            { Name = subscription.Name;
              Subscribe = subscription.Subscribe;
              Func = Some subscription.Func }

        member this.UniqueName = this.Name

    type ContentDelta =
        { Accessor: Accessor
          Content: ViewContentDelta }

        static member From (content: Content) : ContentDelta =
            { Accessor = content.Accessor;
              Content = ViewContentDelta.From content.Content }
            
    type InlineDelta =
        { HostAccessor: Avalonia.AvaloniaProperty
          Inlines: (Avalonia.Controls.Documents.Inline * AttrDelta list) list }
        
        static member From (inlineElement: Inlines) : InlineDelta =
            { HostAccessor = inlineElement.HostAccessor
              Inlines =
                  inlineElement.Inlines
                 |> List.map (fun inl ->
                     let deltas =
                         inl.Attrs
                         |> List.map AttrDelta.From
                     
                     (inl.InlineElement, deltas)) }

    type ViewContentDelta =
        | Single of ViewDelta option
        | Multiple of ViewDelta list

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

    [<CustomEquality; NoComparison>]
    type ViewDelta =
        { ViewType: Type
          Attrs: AttrDelta list
          ConstructorArgs: obj array
          KeyDidChange: bool
          Outlet: (Avalonia.Controls.IControl -> unit) voption }

        static member From (view: IView, ?keyDidChange: bool) : ViewDelta =
            { ViewType = view.ViewType
              Attrs = view.Attrs |> List.map AttrDelta.From
              ConstructorArgs = view.ConstructorArgs
              KeyDidChange = defaultArg keyDidChange false
              Outlet = view.Outlet}
            
        override this.Equals(other) =
            match other with
            | :? ViewDelta as other ->
                this.ViewType = other.ViewType &&
                this.Attrs = other.Attrs &&
                this.ConstructorArgs = other.ConstructorArgs &&
                this.KeyDidChange = other.KeyDidChange &&
                (ValueOption.isSome this.Outlet = ValueOption.isSome other.Outlet)
            | _ -> false
