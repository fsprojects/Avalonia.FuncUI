namespace rec Avalonia.FuncUI

open Avalonia.Controls
open System
open System.Reactive.Linq
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
            defaultValueFactory: (unit -> obj) voption
            comparer: (obj * obj -> bool) voption
        }
        with
            override this.Equals (other: obj) : bool =
                match other with
                | :? Property as other ->
                    match this.accessor.Equals other.accessor with
                    | true ->
                        match this.comparer with
                        | ValueSome comparer -> comparer(this.value, other.value)
                        | ValueNone -> this.value = other.value
                        
                    | false ->
                        false
                    
                | _ ->
                    false
                
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
            funcType: Type
            scope: obj
        }
        with
            override this.Equals (other: obj) : bool =
                match other with
                | :? Subscription as other ->
                    this.name = other.name &&
                    this.funcType = other.funcType &&
                    this.scope = other.scope
                | _ -> false
                
            override this.GetHashCode () =
                (this.name, this.funcType, this.scope).GetHashCode()
                               
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
                    | Accessor.AvaloniaProperty p -> p.Name
                    | Accessor.InstanceProperty p -> p.name
                | Content content ->
                    match content.accessor with
                    | Accessor.AvaloniaProperty p -> p.Name
                    | Accessor.InstanceProperty p -> p.name
                | Subscription subscription -> subscription.name
            
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

    
    // TODO: maybe move active patterns to Virtual DON Misc

    let internal (|Property'|_|) (attr: IAttr)  =
        attr.Property
        
    let internal (|Content'|_|) (attr: IAttr)  =
        attr.Content
        
    let internal (|Subscription'|_|) (attr: IAttr)  =
        attr.Subscription