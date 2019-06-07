namespace rec Avalonia.FuncUI

open Avalonia.Controls
open System
open Types
open Avalonia.FuncUI.Lib
open Types

type TypedAttr<'t> =
    | Property of PropertyAttr
    | Event of EventAttr
    | Content of ContentAttr
    | Lifecycle of LifecylceAttr

[<AbstractClass; Sealed>]
type Views private () =

    // TODO: Check if using a mutable hash map makes a bug difference
    static let cache = ref Map.empty

    (* create view - intended for internal use *)
    static member create<'t>(attrs: TypedAttr<'t> list) : View =
        let mappedAttrs =
            attrs |> List.map (fun attr ->
                match attr with
                | TypedAttr.Property property -> Attr.Property property
                | TypedAttr.Event event -> Attr.Event event
                | TypedAttr.Content content -> Attr.Content content
            )

        { ViewType = typeof<'t>; Attrs = mappedAttrs; }

    (* lazy views with caching *)
    static member viewLazy (state: 'state) (dispatch: 'dispatch) (func: 'state -> 'dispatch -> View) : View =
        let hash (state: 'state, func: 'state -> 'dispatch -> View) : int =
            Tuple(state, Func.hashMethodBody func).GetHashCode()

        let key = hash(state, func)

        match (!cache).TryFind key with
        | Some cached -> cached
        | None ->
            let computedValue = func state dispatch
            cache := (!cache).Add (key, computedValue)
            computedValue

[<AbstractClass; Sealed>]
type Attrs private () =
    
    (* Lifecycle on create *)
    static member viewOnCreate<'T>(func: obj -> unit) : TypedAttr<'T> =
        TypedAttr<_>.Lifecycle {
            Lifecylce = Lifecycle.OnCreate
            Func = func
        }
    
    (* Lifecycle on update *)
    static member viewOnUpdate<'T>(func: obj -> unit) : TypedAttr<'T> =
        TypedAttr<_>.Lifecycle {
            Lifecylce = Lifecycle.OnUpdate
            Func = func
        }