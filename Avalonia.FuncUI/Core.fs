namespace rec Avalonia.FuncUI.Core

open Avalonia.Controls
open System
open Avalonia
open Avalonia.Interactivity

type PropertyAttr =
    {
        Property : AvaloniaProperty
        Value : obj
    }

type EventAttr =
    {
        Event : RoutedEvent
        Value : RoutedEventArgs
    }

type Attr =
    | Property of PropertyAttr
    | Event of EventAttr

    member this.Id =
        match this with
        | Property property ->
            "Property." + property.Property.Name
        | Event event ->
            "Event." + event.Event.Name

type AttrInfo =
    {
        ViewType : Type
        Attr : Attr
    }

    static member Create(viewType: Type, attr : Attr) =
        { ViewType = viewType; Attr = attr }


type ViewElement =
    {
        ViewType: Type
        Attrs: AttrInfo list
    }  

    static member Create(viewType: Type, attrs: AttrInfo list) =
        { ViewType = viewType; Attrs = attrs; }
