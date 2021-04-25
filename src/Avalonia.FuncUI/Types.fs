namespace rec Avalonia.FuncUI

open Avalonia.Controls
open System
open System.Reactive.Linq
open System.Threading

module Types =
    
    [<CustomEquality; NoComparison>]
    type PropertyAccessor =
        { Name: string
          Getter: (IControl -> obj) voption
          Setter: (IControl * obj -> unit) voption }
        
        override this.Equals (other: obj) : bool =
            match other with
            | :? PropertyAccessor as other -> this.Name = other.Name
            | _ -> false
            
        override this.GetHashCode () =
            this.Name.GetHashCode()

    type Accessor =
        | InstanceProperty of PropertyAccessor
        | AvaloniaProperty of Avalonia.AvaloniaProperty
        
    let accessorName = function
        | InstanceProperty p -> p.Name
        | AvaloniaProperty p -> p.Name
            
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
          Subscribe: IControl * Delegate -> CancellationTokenSource
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
            
    type PropertyController =
        { SetControlledValue: obj -> unit
          Cancellation: CancellationTokenSource }
    
    [<CustomEquality; NoComparison>]
    type ControlledProperty =
        { Property: Property
          MakeController: IControl -> (obj -> unit) -> PropertyController
          Func: (obj -> unit)
          FuncType: Type
          Scope: obj }
        
        override this.Equals (other: obj) : bool =
            match other with
            | :? ControlledProperty as other ->
                this.Property = other.Property &&
                this.FuncType = other.FuncType &&
                this.Scope = other.Scope
            | _ -> false
        
        override this.GetHashCode () =
            (this.Property, this.FuncType, this.Scope).GetHashCode()
                           
    type IAttr =
        abstract member UniqueName : string
        abstract member Property : Property option
        abstract member Content : Content option
        abstract member Subscription : Subscription option
        abstract member ControlledProperty : ControlledProperty option
        
    type IAttr<'viewType> =
        inherit IAttr        
                 
    type Attr<'viewType> =
        | Property of Property
        | Content of Content
        | Subscription of Subscription
        | ControlledProperty of ControlledProperty
        
        interface IAttr<'viewType>
        
        interface IAttr with
            member this.UniqueName =
                match this with
                | Property property -> property.Accessor |> accessorName
                | Content content -> content.Accessor |> accessorName
                | Subscription subscription -> subscription.Name
                | ControlledProperty cp -> cp.Property.Accessor |> accessorName
            
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
                
            member this.ControlledProperty =
                match this with
                | ControlledProperty value -> Some value
                | _ -> None

    type IView =
        abstract member ViewType: Type with get
        abstract member Attrs: IAttr list with get
        
    type IView<'viewType> =
        inherit IView
        abstract member Attrs: IAttr<'viewType> list with get
        
    type View<'viewType> =
        { ViewType: Type
          Attrs: IAttr<'viewType> list }
        
        interface IView with
            member this.ViewType =  this.ViewType
            member this.Attrs =
                this.Attrs |> List.map (fun attr -> attr :> IAttr)
            
        interface IView<'viewType> with
            member this.Attrs = this.Attrs
