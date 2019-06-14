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

        static member inline borderThickness<'T when 'T : (member set_BorderThickness : Avalonia.Thickness -> unit)>(value: Avalonia.Thickness) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "BorderThickness"; Value = value }

        static member inline borderBrush<'T when 'T : (member set_BorderBrush : Avalonia.Media.IBrush -> unit)>(brush: Avalonia.Media.IBrush) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "BorderBrush"; Value = brush }

        static member inline viewportSize<'T when 'T : (member set_ViewportSize : double -> unit)>(size: double) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "ViewportSize"; Value = size }

        static member inline viewport<'T when 'T : (member set_Viewport : Avalonia.Size -> unit)>(size: Avalonia.Size) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "Viewport"; Value = size }

        static member inline useFloatingWatermark<'T when 'T : (member set_UseFloatingWatermark : bool -> unit)>(value: bool) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "UseFloatingWatermark"; Value = value }

        static member inline topmost<'T when 'T : (member set_Topmost : bool -> unit)>(value: bool) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "Topmost"; Value = value }

        static member inline textWrapping<'T when 'T : (member set_TextWrapping : Avalonia.Media.TextWrapping -> unit)>(value: Avalonia.Media.TextWrapping) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "TextWrapping"; Value = value }

        static member inline textAlignment<'T when 'T : (member set_TextAlignment : Avalonia.Media.TextAlignment -> unit)>(value: Avalonia.Media.TextAlignment) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "TextAlignment"; Value = value }

        static member inline tabStripPlacement<'T when 'T : (member set_TabStripPlacement : Dock -> unit)>(value: Dock) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "TabStripPlacement"; Value = value }

        static member inline showButtonSpinner<'T when 'T : (member set_ShowButtonSpinner : bool -> unit)>(value: bool) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "ShowButtonSpinner"; Value = value }

        static member inline selectionStart<'T when 'T : (member set_SelectionStart : int -> unit)>(value: int) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "SelectionStart"; Value = value }

        static member inline selectionMode<'T when 'T : (member set_SelectionMode : SelectionMode -> unit)>(mode: SelectionMode) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "SelectionMode"; Value = mode }

        static member inline selectionEnd<'T when 'T : (member set_SelectionEnd : int -> unit)>(value: int) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "SelectionEnd"; Value = value }

        static member inline selectedIndex<'T when 'T : (member set_SelectedIndex : int -> unit)>(value: int) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "SelectedIndex"; Value = value }

        static member inline selectedDate<'T when 'T : (member set_SelectedDate : Nullable<DateTime> -> unit)>(value: Nullable<DateTime>) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "SelectedDate"; Value = value }

        static member inline selectedDate<'T when 'T : (member set_SelectedDate : Nullable<DateTime> -> unit)>(value: DateTime option) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "SelectedDate"; Value = Option.toNullable value }

        static member inline points<'T when 'T : (member set_Points : System.Collections.Generic.IList<Avalonia.Point> -> unit)>(value: System.Collections.Generic.IList<Avalonia.Point>) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "Points"; Value = value }

        static member inline passwordChar<'T when 'T : (member set_PasswordChar : char -> unit)>(value: char) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "PasswordChar"; Value = value }

        static member inline pageTransition<'T when 'T : (member set_PageTransition : Avalonia.Animation.IPageTransition -> unit)>(value: Avalonia.Animation.IPageTransition) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "PageTransition"; Value = value }

        static member inline offset<'T when 'T : (member set_Offset : Avalonia.Vector -> unit)>(value: Avalonia.Vector) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "Offset"; Value = value }

        static member inline memberSelector<'T when 'T : (member set_MemberSelector : Avalonia.Controls.Templates.IMemberSelector -> unit)>(value: Avalonia.Controls.Templates.IMemberSelector) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "MemberSelector"; Value = value }





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
