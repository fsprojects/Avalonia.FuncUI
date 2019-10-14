namespace rec Avalonia.FuncUI

open System
open Types
open Avalonia.FuncUI.Lib

type TypedAttr<'t> =
    | Property of PropertyAttr
    | AttachedProperty of AttachedPropertyAttr
    | Event of EventAttr
    | Content of ContentAttr
    | Lifecycle of LifecycleAttr

[<AbstractClass; Sealed>]
type Views () =

    static let cache = CuncurrentDict<int, View>()

    static member val CacheMaxLength : int = 1000 with get, set

    (* create view - intended for internal use *)
    static member create<'t>(attrs: TypedAttr<'t> list) : View =
        let mappedAttrs =
            attrs |> List.map (fun attr ->
                match attr with
                | TypedAttr.Property property -> Attr.Property property
                | TypedAttr.AttachedProperty property -> Attr.AttachedProperty property
                | TypedAttr.Event event -> Attr.Event event
                | TypedAttr.Content content -> Attr.Content content
            )

        { ViewType = typeof<'t>; Attrs = mappedAttrs; }

    (* lazy views with caching *)
    static member viewLazy (state: 'state) (args: 'args) (func: 'state -> 'args -> View) : View =

        let key = Tuple(state, func.GetType()).GetHashCode()

        if (cache.Count >= Views.CacheMaxLength) then
            cache.Clear()

        let hasValue, value = cache.TryGetValue key

        match hasValue with
        | true -> value
        | false ->
            cache.AddOrUpdate(
                key,
                (fun _ -> func state args),
                (fun _ _ -> func state args)
            )


[<AbstractClass; Sealed>]
type Attrs private () =
    do ()

    (* Lifecycle on create *)
//   static member viewOnCreate<'T>(func: obj -> unit) : TypedAttr<'T> =
//        TypedAttr<_>.Lifecycle {
//            Lifecylce = Lifecycle.OnCreate
//            Func = func
//        }
    
    (* Lifecycle on update *)
//    static member viewOnUpdate<'T>(func: obj -> unit) : TypedAttr<'T> =
//        TypedAttr<_>.Lifecycle {
//            Lifecylce = Lifecycle.OnUpdate
//            Func = func
//        }