namespace rec Avalonia.FuncUI

open Avalonia.Controls
open System
open Types
open Avalonia.FuncUI.Lib
open Types
open System.Runtime.CompilerServices

type TypedAttr<'t> =
    | Property of PropertyAttr
    | AttachedProperty of AttachedPropertyAttr
    | Event of EventAttr
    | Content of ContentAttr
    | Lifecycle of LifecylceAttr

[<AbstractClass; Sealed>]
type Views () =

    // TODO: Check if using a mutable hash map makes a big difference
    static let cache = ref Map.empty

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
    static member viewLazy
        (
            state: 'state,
            args: 'args,
            func: 'state -> 'args -> View,
            [<CallerFilePath>] ?callerSourceFile : string,
            [<CallerMemberName>] ?callerMemberName : string,
            [<CallerLineNumber>] ?callerLineNumber : int
        )
        : View =

        let key =
            Tuple(state, callerSourceFile.Value, callerMemberName.Value, callerLineNumber.Value).GetHashCode()

        match (!cache).TryFind key with
        | Some cached -> cached
        | None ->
            let computedValue = func state args
            cache := (!cache).Add (key, computedValue)
            computedValue


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