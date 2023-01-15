namespace rec Avalonia.FuncUI

open Avalonia
open Avalonia.Controls
open System
open System.Threading

module Types =

    [<CustomEquality; NoComparison>]
    type PropertyAccessor =
        { Name: string
          Getter: (AvaloniaObject -> obj) voption
          Setter: (AvaloniaObject * obj -> unit) voption }

        override this.Equals (other: obj) : bool =
            match other with
            | :? PropertyAccessor as other -> this.Name = other.Name
            | _ -> false

        override this.GetHashCode () =
            this.Name.GetHashCode()

    type Accessor =
        | InstanceProperty of PropertyAccessor
        | AvaloniaProperty of Avalonia.AvaloniaProperty

    [<CustomEquality; NoComparison>]
    type Property =
        { Accessor: Accessor
          Value: obj
          DefaultValueFactory: (unit -> obj) voption
          Comparer: (obj * obj -> bool) voption }

        override this.Equals (other: obj) : bool =
            match other with
            | :? Property as other ->
                match this.Accessor.Equals other.Accessor with
                | true ->
                    match this.Comparer with
                    | ValueSome comparer -> comparer(this.Value, other.Value)
                    | ValueNone -> this.Value = other.Value

                | false ->
                    false

            | _ ->
                false

        override this.GetHashCode () =
            (this.Accessor, this.Value).GetHashCode()

    type Content =
        { Accessor: Accessor
          Content: ViewContent }

    type ViewContent =
        | Single of IView option
        | Multiple of IView list

    [<CustomEquality; NoComparison>]
    type Subscription =
        { Name: string
          Subscribe: Control  * Delegate -> CancellationTokenSource
          Func: Delegate
          FuncType: Type
          Scope: obj }

        override this.Equals (other: obj) : bool =
            match other with
            | :? Subscription as other ->
                this.Name = other.Name &&
                this.FuncType = other.FuncType &&
                this.Scope = other.Scope
            | _ -> false

        override this.GetHashCode () =
            (this.Name, this.FuncType, this.Scope).GetHashCode()
    
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
                    match property.Accessor with
                    | Accessor.AvaloniaProperty p -> p.Name
                    | Accessor.InstanceProperty p -> p.Name

                | Content content ->
                    match content.Accessor with
                    | Accessor.AvaloniaProperty p -> p.Name
                    | Accessor.InstanceProperty p -> p.Name

                | Subscription subscription -> subscription.Name

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
        abstract member ViewKey: string voption
        abstract member Attrs: IAttr list with get
        abstract member ConstructorArgs: obj array with get
        abstract member Outlet: (AvaloniaObject -> unit) voption with get

    type IView<'viewType> =
        inherit IView
        abstract member Attrs: IAttr<'viewType> list with get

    type View<'viewType> =
        { ViewType: Type
          ViewKey: string voption
          Attrs: IAttr<'viewType> list
          ConstructorArgs: obj array
          Outlet: (AvaloniaObject-> unit) voption }

        interface IView with
            member this.ViewType =  this.ViewType
            member this.ViewKey = this.ViewKey
            member this.Attrs =
                this.Attrs |> List.map (fun attr -> attr :> IAttr)

            member this.ConstructorArgs = this.ConstructorArgs
            member this.Outlet = this.Outlet

        interface IView<'viewType> with
            member this.Attrs = this.Attrs

    // TODO: maybe move active patterns to Virtual DON Misc

    let internal (|Property'|_|) (attr: IAttr)  =
        attr.Property

    let internal (|Content'|_|) (attr: IAttr)  =
        attr.Content

    let internal (|Subscription'|_|) (attr: IAttr)  =
        attr.Subscription
    