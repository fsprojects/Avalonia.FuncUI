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

module PropertyAttr =
    let create (property: AvaloniaProperty, value: obj) =
        { Property = property; Value = value }

type EventAttr =
    {
        Event : RoutedEvent
        Args : Type
        Value : EventHandler
    }

module EventAttr =
    let create (event: RoutedEvent<'t>, handler: EventHandler) =
        { Event = event; Args = typeof<'t>; Value = handler }

type Attr =
    | Property of PropertyAttr
    | Event of EventAttr

    member this.Id =
        match this with
        | Property property ->
            "Property." + property.Property.Name
        | Event event ->
            "Event." + event.Event.Name

module Attr =
    let createProperty (property: AvaloniaProperty, value: obj) =
        (property, value) |> PropertyAttr.create |> Property

type AttrInfo =
    {
        ViewType : Type
        Attr : Attr
    }

module AttrInfo =
    let create(viewType: Type, attr : Attr) =
        { ViewType = viewType; Attr = attr }


type ViewElement =
    {
        ViewType: Type
        Attrs: AttrInfo list
    }  

module ViewElement =
    let create(viewType: Type, attrs: AttrInfo list) =
        { ViewType = viewType; Attrs = attrs; }
