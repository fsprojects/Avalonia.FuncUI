namespace Avalonia.FuncUI

open Avalonia.Controls
open System
open Types

module Views =

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


    let button (attrs: TypedAttr<Button> list): View =
        View.create<Button>(attrs)

    let repeatbutton (attrs: TypedAttr<RepeatButton> list): View =
        View.create<RepeatButton>(attrs)

    let textblock (attrs: TypedAttr<TextBlock> list): View =
        View.create<TextBlock>(attrs)

    let stackpanel (attrs: TypedAttr<StackPanel> list): View =
        View.create<StackPanel>(attrs)

    let dockpanel (attrs: TypedAttr<DockPanel> list): View =
        View.create<DockPanel>(attrs)
