namespace Avalonia.FuncUI

open Avalonia
open Avalonia.Controls
open System
open System.Threading
open System.Diagnostics.CodeAnalysis

module rec Types =

    [<CustomEquality; NoComparison; Struct>]
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

    [<Struct; CustomEquality; NoComparison>]
    type InitFunction =
        { Function: obj -> unit }

        override this.Equals (obj: obj) =
            Object.ReferenceEquals (this, obj)

        override this.GetHashCode () =
            (this.Function :> obj).GetHashCode()

    type IAttr =
        abstract member UniqueName : string

        abstract member Property : Property voption
        abstract member Content : Content voption
        abstract member Subscription : Subscription voption
        abstract member InitFunction: InitFunction voption

    type IAttr<'viewType> =
        inherit IAttr

    type Attr<'viewType> =
        | Property of Property
        | Content of Content
        | Subscription of Subscription
        | InitFunction of InitFunction

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

                | Subscription subscription ->
                    subscription.Name

                | InitFunction _ ->
                    "initFunction"

            member this.Property =
                match this with
                | Property value -> ValueSome value
                | _ -> ValueNone

            member this.Content =
                match this with
                | Content value -> ValueSome value
                | _ -> ValueNone

            member this.Subscription =
                match this with
                | Subscription value -> ValueSome value
                | _ -> ValueNone

            member this.InitFunction =
                match this with
                | InitFunction value -> ValueSome value
                | _ -> ValueNone

    type IView =
        abstract member ViewType: Type with get
        abstract member ViewKey: string voption
        abstract member Attrs: IAttr list with get
        abstract member ConstructorArgs: obj array with get
        abstract member Outlet: (AvaloniaObject -> unit) voption with get

    type IView<[<DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)>]'viewType> =
        inherit IView
        abstract member Attrs: IAttr<'viewType> list with get

    type View<[<DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)>]'viewType> =
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

    [<return: Struct>]
    let internal (|Property'|_|) (attr: IAttr) : Property voption =
        attr.Property

    [<return: Struct>]
    let internal (|Content'|_|) (attr: IAttr) : Content voption =
        attr.Content

    [<return: Struct>]
    let internal (|Subscription'|_|) (attr: IAttr)  : Subscription voption =
        attr.Subscription

    [<return: Struct>]
    let internal (|InitFunction|_|) (attr: IAttr) : InitFunction voption =
        attr.InitFunction