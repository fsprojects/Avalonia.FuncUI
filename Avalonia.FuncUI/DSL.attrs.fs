namespace Avalonia.FuncUI

open Avalonia.Controls
open System
open Types

[<AutoOpen>]
module DSL_Attrs =

    type Attrs with

        static member inline background<'T when 'T : (member set_Background : Avalonia.Media.IBrush -> unit)>(brush: Avalonia.Media.IBrush) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "Background"; Value = brush }

        static member inline foreground<'T when 'T : (member set_Foreground : Avalonia.Media.IBrush -> unit)>(brush: Avalonia.Media.IBrush) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "Foreground"; Value = brush }

        static member inline text<'T when 'T : (member set_Text : string -> unit)>(text: string) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "Text"; Value = text }

        static member inline orientation<'T when 'T : (member set_Orientation : Orientation -> unit)>(orientation: Orientation) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "Orientation"; Value = orientation }

        static member inline children<'T when 'T : (member get_Children : unit -> Controls)>(children: View list) : TypedAttr<'T> =
            TypedAttr<_>.Content {
                Name = "Children"
                Content = (ViewContent.Multiple children)
            }

        static member inline content<'T when 'T : (member set_Content : IControl -> unit)>(content: View) : TypedAttr<'T> =
            TypedAttr<_>.Content {
                Name = "Content"
                Content = (ViewContent.Single (Some content))
            }

        static member inline items<'a, 'T when 'T : (member set_Items : unit -> System.Collections.Generic.IEnumerable<'a>)>(items: 'a list) : TypedAttr<'T> =
            TypedAttr<_>.Property {
                Name = "Items"
                Value = items
            }
    
        static member inline click<'T when 'T : (member add_Click : EventHandler<Avalonia.Interactivity.RoutedEventArgs> -> unit)>(click: obj -> Avalonia.Interactivity.RoutedEventArgs -> unit) : TypedAttr<'T> =
                TypedAttr<_>.Event { Name = "Click"; Value = new EventHandler<Avalonia.Interactivity.RoutedEventArgs>(click)}
