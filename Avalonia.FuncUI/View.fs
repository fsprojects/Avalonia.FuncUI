namespace rec Avalonia.FuncUI

open Avalonia.Controls
open System
open Types
open Avalonia.FuncUI.Lib

type TypedAttr<'t> =
    | Property of PropertyAttr
    | Event of EventAttr
    | Content of ContentAttr
    | Lifecycle of LifecylceAttr

module Attr =
    open Types
    open Avalonia.Media
    open Avalonia.Interactivity



module View =
    open Types

    [<AutoOpen>]
    module Attr =
    
        module Lifecycle =
    
            (* Lifecycle on create *)
            let viewOnCreate<'T>(func: obj -> unit) : TypedAttr<'T> =
                TypedAttr<_>.Lifecycle {
                    Lifecylce = Lifecycle.OnCreate
                    Func = func
                }
    
            (* Lifecycle on update *)
            let viewOnUpdate<'T>(func: obj -> unit) : TypedAttr<'T> =
                TypedAttr<_>.Lifecycle {
                    Lifecylce = Lifecycle.OnUpdate
                    Func = func
                }

        let inline background<'T when 'T : (member set_Background : Avalonia.Media.IBrush -> unit)>(brush: Avalonia.Media.IBrush) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "Background"; Value = brush }

        let inline text<'T when 'T : (member set_Text : string -> unit)>(text: string) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "Text"; Value = text }

        let inline orientation<'T when 'T : (member set_Orientation : Orientation -> unit)>(orientation: Orientation) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "Orientation"; Value = orientation }

        let inline children<'T when 'T : (member get_Children : unit -> Controls)>(children: View list) : TypedAttr<'T> =
            TypedAttr<_>.Content {
                Name = "Children"
                Content = (ViewContent.Multiple children)
            }

        let inline content<'T when 'T : (member set_Content : IControl -> unit)>(content: View) : TypedAttr<'T> =
            TypedAttr<_>.Content {
                Name = "Content"
                Content = (ViewContent.Single (Some content))
            }
    
        let inline click<'T when 'T : (member add_Click : EventHandler<Avalonia.Interactivity.RoutedEventArgs> -> unit)>(click: obj -> Avalonia.Interactivity.RoutedEventArgs -> unit) : TypedAttr<'T> =
            TypedAttr<_>.Event { Name = "Click"; Value = new EventHandler<Avalonia.Interactivity.RoutedEventArgs>(click)}
             
    module Lazy =
        // TODO: Check if using a mutable hash map makes a bug difference
        let private cache = ref Map.empty

        let viewLazy (state: 'state) (dispatch: 'dispatch) (func: 'state -> 'dispatch -> View) : View =
            let hash (state: 'state, func: 'state -> 'dispatch -> View) : int =
                Tuple(state, Func.hashMethodBody func).GetHashCode()

            let key = hash(state, func)

            match (!cache).TryFind key with
            | Some cached -> cached
            | None ->
                let computedValue = func state dispatch
                cache := (!cache).Add (key, computedValue)
                computedValue
                
    let private create<'t>(attrs: TypedAttr<'t> list) =
        let mappedAttrs =
            attrs |> List.map (fun attr ->
                match attr with
                | TypedAttr.Property property -> Attr.Property property
                | TypedAttr.Event event -> Attr.Event event
                | TypedAttr.Content content -> Attr.Content content
            )

        { ViewType = typeof<'t>; Attrs = mappedAttrs; }

    let button (attrs: TypedAttr<Button> list): View =
        create<Button>(attrs)

    let repeatbutton (attrs: TypedAttr<RepeatButton> list): View =
        create<RepeatButton>(attrs)

    let textblock (attrs: TypedAttr<TextBlock> list): View =
        create<TextBlock>(attrs)

    let stackpanel (attrs: TypedAttr<StackPanel> list): View =
        create<StackPanel>(attrs)

    let dockpanel (attrs: TypedAttr<DockPanel> list): View =
        create<DockPanel>(attrs)
