namespace rec Avalonia.FuncUI

open Avalonia
open Avalonia.Controls
open System
open System.Threading
open Avalonia.Interactivity

module Types =

    [<CustomEquality; NoComparison>]
    type PropertyAccessor =
        { Name: string
          Getter: (IAvaloniaObject -> obj) voption
          Setter: (IAvaloniaObject * obj -> unit) voption }

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
          Subscribe: IControl  * Delegate -> CancellationTokenSource
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

    /// <summary>
    /// Defines an inline element with the accessor to the property inside the
    /// control that contains the element itself and the properties of the inline
    /// element that we are modifying.
    /// </summary>
    type Inlines =
        { /// Reference to the accessor that allows us to modify the host's property that contains the inlines.
          HostAccessor: AvaloniaProperty
          /// Reference to all the inlines inside of a host with their attributes.
          Inlines: IInline list }
    
    type IAttr =
        abstract member UniqueName : string
        abstract member Property : Property option
        abstract member Content : Content option
        abstract member Subscription : Subscription option
        abstract member Inlines : Inlines option

    type IAttr<'viewType> =
        inherit IAttr

    type Attr<'viewType> =
        | Property of Property
        | Content of Content
        | Subscription of Subscription
        | Inline of Inlines

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
                
                | Inline inlines -> inlines.HostAccessor.Name

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
                
            member this.Inlines =
                match this with
                | Inline value -> Some value
                | _ -> None

    type IView =
        abstract member ViewType: Type with get
        abstract member ViewKey: string voption
        abstract member Attrs: IAttr list with get
        abstract member ConstructorArgs: obj array with get
        abstract member Outlet: (IAvaloniaObject -> unit) voption with get

    type IView<'viewType> =
        inherit IView
        abstract member Attrs: IAttr<'viewType> list with get

    type IInline =
        abstract member InlineElement: Avalonia.Controls.Documents.Inline with get
        abstract member Attrs: IAttr list with get
        
    type IInline<'inlineType> =
        abstract member Attrs: IAttr<'inlineType> list with get
    
    type View<'viewType> =
        { ViewType: Type
          ViewKey: string voption
          Attrs: IAttr<'viewType> list
          ConstructorArgs: obj array
          Outlet: (IAvaloniaObject-> unit) voption }

        interface IView with
            member this.ViewType =  this.ViewType
            member this.ViewKey = this.ViewKey
            member this.Attrs =
                this.Attrs |> List.map (fun attr -> attr :> IAttr)

            member this.ConstructorArgs = this.ConstructorArgs
            member this.Outlet = this.Outlet

        interface IView<'viewType> with
            member this.Attrs = this.Attrs

    type Inline<'inlineType> =
        { InlineElement: Avalonia.Controls.Documents.Inline
          Attrs: IAttr<'inlineType> list }
        
        interface IInline with
            member this.InlineElement = this.InlineElement
            member this.Attrs =
                this.Attrs |> List.map (fun attr -> attr :> IAttr)
                
        interface IInline<'inlineType> with
            member this.Attrs = this.Attrs

    // TODO: maybe move active patterns to Virtual DON Misc

    let internal (|Property'|_|) (attr: IAttr)  =
        attr.Property

    let internal (|Content'|_|) (attr: IAttr)  =
        attr.Content

    let internal (|Subscription'|_|) (attr: IAttr)  =
        attr.Subscription
        
    let internal (|Inlines'|_|) (attr: IAttr) =
        attr.Inlines
    