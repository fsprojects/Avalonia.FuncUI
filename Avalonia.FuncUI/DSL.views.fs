namespace Avalonia.FuncUI

open Avalonia.Controls
open System
open Types

[<AutoOpen>]
module DSL_Views =

    type Views with

        static member inline background<'T when 'T : (member set_Background : Avalonia.Media.IBrush -> unit)>(brush: Avalonia.Media.IBrush) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "Background"; Value = brush }

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
    
        static member inline click<'T when 'T : (member add_Click : EventHandler<Avalonia.Interactivity.RoutedEventArgs> -> unit)>(click: obj -> Avalonia.Interactivity.RoutedEventArgs -> unit) : TypedAttr<'T> =
                TypedAttr<_>.Event { Name = "Click"; Value = new EventHandler<Avalonia.Interactivity.RoutedEventArgs>(click)}


        static member button (attrs: TypedAttr<Button> list): View =
            Views.create<Button>(attrs)

        static member repeatbutton (attrs: TypedAttr<RepeatButton> list): View =
            Views.create<RepeatButton>(attrs)

        static member textblock (attrs: TypedAttr<TextBlock> list): View =
            Views.create<TextBlock>(attrs)

        static member stackpanel (attrs: TypedAttr<StackPanel> list): View =
            Views.create<StackPanel>(attrs)

        static member dockpanel (attrs: TypedAttr<DockPanel> list): View =
            Views.create<DockPanel>(attrs)
