namespace Avalonia.FuncUI.Core

open Avalonia.FuncUI.Core
open Avalonia.Controls
open System

type Attr<'t> =
    {
        Name : string
        Value : obj
    }

type Attr =
    {
        Name : string
        Value : obj
    }

type ViewElement =
    {
        ViewType: Type
        Attrs: Attr list
    }  

module ViewElement =
    let create<'t>(attrs: Attr<'t> list) =
        let mappedAttrs =
            attrs
            |> List.map (fun attr ->
                {
                    Name = attr.Name
                    Value = attr.Value
                }
            )

        { ViewType = typeof<'t>; Attrs = mappedAttrs; }

module Attr =
    open Avalonia.Media
    open Avalonia.Interactivity
    
    let inline background<'T when 'T : (member set_Background : IBrush -> unit)>(brush: IBrush) : Attr<'T> =
        { Name = "Background"; Value = brush }

    let inline text<'T when 'T : (member set_Text : string -> unit)>(text: string) : Attr<'T> =
        { Name = "Text"; Value = text }

    let inline orientation<'T when 'T : (member set_Orientation : Orientation -> unit)>(orientation: Orientation) : Attr<'T> =
        { Name = "Orientation"; Value = orientation }

    let inline children<'T when 'T : (member get_Children : unit -> Controls)>(children: ViewElement list) : Attr<'T> =
        { Name = "Children"; Value = children }

    let inline content<'T when 'T : (member set_Content : IControl -> unit)>(content: ViewElement) : Attr<'T> =
        { Name = "Content"; Value = content }
    
    let inline click<'T when 'T : (member add_Click : EventHandler<RoutedEventArgs> -> unit)>(click: RoutedEventArgs -> unit) : Attr<'T> =
        { Name = "Click"; Value = click }

module View =

    let button (attrs: Attr<Button> list): ViewElement =
        ViewElement.create<Button>(attrs)

    let repeatbutton (attrs: Attr<RepeatButton> list): ViewElement =
        ViewElement.create<RepeatButton>(attrs)

    let textblock (attrs: Attr<TextBlock> list): ViewElement =
        ViewElement.create<TextBlock>(attrs)

    let stackpanel (attrs: Attr<StackPanel> list): ViewElement =
        ViewElement.create<StackPanel>(attrs)

    let dockpanel (attrs: Attr<DockPanel> list): ViewElement =
        ViewElement.create<DockPanel>(attrs)