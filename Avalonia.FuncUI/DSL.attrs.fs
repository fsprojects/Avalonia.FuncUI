namespace Avalonia.FuncUI

open Avalonia.Controls
open System
open Types

[<AutoOpen>]
module DSL_Attrs =

    type Attrs with

        static member inline orientation<'T when 'T : (member set_Orientation : Orientation -> unit)>(orientation: Orientation) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "Orientation"; Value = orientation }

        static member inline text<'T when 'T : (member set_Text : string -> unit)>(text: string) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "Text"; Value = text }

        static member inline background<'T when 'T : (member set_Background : Avalonia.Media.IBrush -> unit)>(brush: Avalonia.Media.IBrush) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "Background"; Value = brush }

        static member inline watermark<'T when 'T : (member set_Watermark : string -> unit)>(text: string) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "Watermark"; Value = text }

        static member inline stretch<'T when 'T : (member set_Stretch : Avalonia.Media.Stretch -> unit)>(stretch: Avalonia.Media.Stretch) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "Stretch"; Value = stretch }

        static member inline isSelected<'T when 'T : (member set_IsSelected : bool -> unit)>(isSelected: bool) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "IsSelected"; Value = isSelected }

        static member inline virtualizationMode<'T when 'T : (member set_VirtualizationMode : ItemVirtualizationMode -> unit)>(virtualizationMode: ItemVirtualizationMode) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "VirtualizationMode"; Value = virtualizationMode }

        static member inline verticalContentAlignment<'T when 'T : (member set_VerticalContentAlignment : bool -> unit)>(alignment: Avalonia.Layout.VerticalAlignment) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "VerticalContentAlignment"; Value = alignment }

        static member inline value<'T when 'T : (member set_Value : double -> unit)>(value: double) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "Value"; Value = value }

        static member inline selectedItem<'T when 'T : (member set_SelectedItem : obj -> unit)>(item: obj) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "SelectedItem"; Value = item }

        static member inline padding<'T when 'T : (member set_Padding : Avalonia.Thickness -> unit)>(padding: Avalonia.Thickness) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "Padding"; Value = padding }

        static member inline minimum<'T when 'T : (member set_Minimum : double -> unit)>(minimum: double) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "Minimum"; Value = minimum }
       
        static member inline maximum<'T when 'T : (member set_Maximum : double -> unit)>(maximum: double) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "Maximum"; Value = maximum }

        static member inline items<'T when 'T : (member set_Items : System.Collections.IEnumerable -> unit)>(value: System.Collections.IEnumerable) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "Items"; Value = value }

        static member inline itemTemplate<'T when 'T : (member set_ItemTemplate : Avalonia.Controls.Templates.IDataTemplate -> unit)>(template: Avalonia.Controls.Templates.IDataTemplate) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "ItemTemplate"; Value = template }

        static member inline isDropDownOpen<'T when 'T : (member set_IsDropDownOpen : bool -> unit)>(value: bool) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "IsDropDownOpen"; Value = value }

        static member inline horizontalContentAlignment<'T when 'T : (member set_HorizontalContentAlignment : bool -> unit)>(alignment: Avalonia.Layout.HorizontalAlignment) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "HorizontalContentAlignment"; Value = alignment }

        static member inline headerPresenter<'T when 'T : (member set_HeaderPresenter : Avalonia.Controls.Presenters.IContentPresenter -> unit)>(presenter: Avalonia.Controls.Presenters.IContentPresenter) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "HeaderPresenter"; Value = presenter }

        static member inline header<'T when 'T : (member set_Header : IControl -> unit)>(content: View) : TypedAttr<'T> =
            TypedAttr<_>.Content {
                Name = "Header"
                Content = (ViewContent.Single (Some content))
            }

        static member inline header<'T when 'T : (member set_Header : string -> unit)>(text: string) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "Header"; Value = text }

        static member inline contentTemplate<'T when 'T : (member set_ContentTemplate : Avalonia.Controls.Templates.IDataTemplate -> unit)>(template: Avalonia.Controls.Templates.IDataTemplate) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "ContentTemplate"; Value = template }




        static member inline foreground<'T when 'T : (member set_Foreground : Avalonia.Media.IBrush -> unit)>(brush: Avalonia.Media.IBrush) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "Foreground"; Value = brush }

        static member inline fontsize<'T when 'T : (member set_FontSize : double -> unit)>(size: double) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "FontSize"; Value = size }

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
