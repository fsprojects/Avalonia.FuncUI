namespace Avalonia.FuncUI.VirtualDom

open System
open System.Runtime.CompilerServices
open System.Threading

open Avalonia.Controls
open Avalonia.FuncUI.Types

module internal rec Delta =

    [<Struct; IsReadOnly>]
    type AttrDelta =
        | Property of property: PropertyDelta
        | Content of content: ContentDelta
        | Subscription of subscription: SubscriptionDelta

        static member From (attr: IAttr) : AttrDelta =
            match attr with
            | Property' property -> Property (PropertyDelta.From property)
            | Content' content -> Content (ContentDelta.From content)
            | Subscription' subscription -> Subscription (SubscriptionDelta.From subscription)
            | _ -> raise (Exception "unknown IAttr type. (not a Property, Content ore Subscription attribute)")


    [<Struct; IsReadOnly; CustomEquality; NoComparison>]
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


    [<Struct; IsReadOnly; CustomEquality; NoComparison>]
    type SubscriptionDelta =
        { Name: string
          Subscribe: IControl * Delegate -> CancellationTokenSource
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

    [<Struct; IsReadOnly>]
    type ContentDelta =
        { Accessor: Accessor
          Content: ViewContentDelta }

        static member From (content: Content) : ContentDelta =
            { Accessor = content.Accessor;
              Content = ViewContentDelta.From content.Content }

    [<Struct; IsReadOnly>]
    type ViewContentDelta =
        | Single of single: ViewDelta voption
        | Multiple of multiple: ViewDelta list

        static member From (viewContent: ViewContent) : ViewContentDelta =
            match viewContent with
            | ViewContent.Single single ->
                match single with
                | ValueSome view ->
                    (ViewDelta.From view)
                    |> ValueSome
                    |> ViewContentDelta.Single
                | ValueNone ->
                    ValueNone
                    |> ViewContentDelta.Single

            | ViewContent.Multiple multiple ->
                multiple
                |> List.map (fun view -> ViewDelta.From view)
                |> ViewContentDelta.Multiple

    [<Struct; IsReadOnly; CustomEquality; NoComparison>]
    type ViewDelta =
        { ViewType: Type
          Attrs: AttrDelta list
          ConstructorArgs: obj array
          KeyDidChange: bool
          Outlet: (Avalonia.IAvaloniaObject -> unit) voption }

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

        override this.GetHashCode() =
            struct(this.ViewType, this.Attrs, this.ConstructorArgs, this.KeyDidChange).GetHashCode()
