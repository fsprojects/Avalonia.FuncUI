namespace Avalonia.FuncUI

open Avalonia.Controls
open Types
open System
open System.Threading

[<AutoOpen>]
module DSL_Attrs =

    type Attrs with

        static member inline orientation<'T when 'T : (member set_Orientation : Orientation -> unit)>(orientation: Orientation) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "Orientation"; Value = orientation }

        static member inline text<'T when 'T : (member set_Text : string -> unit)>(text: string) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "Text"; Value = text }

        static member inline background<'T when 'T : (member set_Background : Avalonia.Media.IBrush -> unit)>(brush: Avalonia.Media.IBrush) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "Background"; Value = brush }

        static member inline background<'T when 'T : (member set_Background : Avalonia.Media.IBrush -> unit)>(brush: string) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "Background"; Value = Avalonia.Media.SolidColorBrush.Parse(brush).ToImmutable() }

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

        static member inline padding<'T when 'T : (member set_Padding : Avalonia.Thickness -> unit)>(value: double) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "Padding"; Value = Avalonia.Thickness(value) }

        static member inline padding<'T when 'T : (member set_Padding : Avalonia.Thickness -> unit)>(horizontal: double, vertical: double) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "Padding"; Value = Avalonia.Thickness(horizontal, vertical) }

        static member inline padding<'T when 'T : (member set_Padding : Avalonia.Thickness -> unit)>(left: double, top: double, right: double, bottom: double) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "Padding"; Value = Avalonia.Thickness(left, top, right, bottom) }

        static member inline minimum<'T when 'T : (member set_Minimum : double -> unit)>(minimum: double) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "Minimum"; Value = minimum }
       
        static member inline maximum<'T when 'T : (member set_Maximum : double -> unit)>(maximum: double) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "Maximum"; Value = maximum }

        static member inline items<'T when 'T : (member set_Items : System.Collections.IEnumerable -> unit)>(value: System.Collections.IEnumerable) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "Items"; Value = value }

        static member inline items<'T when 'T : (member get_Items : unit -> System.Collections.IEnumerable)>(children: View list) : TypedAttr<'T> =
            TypedAttr<_>.Content {
                Name = "Items"
                Content = (ViewContent.Multiple children)
            }

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

        static member inline maxDropDownHeight<'T when 'T : (member set_MaxDropDownHeight : double -> unit)>(size: double) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "MaxDropDownHeight"; Value = size }

        static member inline itemsPanel<'T when 'T : (member set_ItemsPanel : ITemplate<IPanel> -> unit)>(size: ITemplate<IPanel>) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "ItemsPanel"; Value = size }

        static member inline isVirtualized<'T when 'T : (member set_IsVirtualized : bool -> unit)>(value: bool) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "IsVirtualized"; Value = value }

        static member inline isTodayHighlighted<'T when 'T : (member set_IsTodayHighlighted : bool -> unit)>(value: bool) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "IsTodayHighlighted"; Value = value }

        static member inline isReadOnly<'T when 'T : (member set_IsReadOnly : bool -> unit)>(value: bool) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "IsReadOnly"; Value = value }

        static member inline isOpen<'T when 'T : (member set_IsOpen : bool -> unit)>(value: bool) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "IsOpen"; Value = value }

        static member inline isExpaneded<'T when 'T : (member set_IsExpaneded : bool -> unit)>(value: bool) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "IsExpaneded"; Value = value }

        static member inline hotKey<'T when 'T : (member set_HotKey : Avalonia.Input.KeyGesture -> unit)>(value: Avalonia.Input.KeyGesture) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "HotKey"; Value = value }

        static member inline headerBackground<'T when 'T : (member set_HeaderBackground : Avalonia.Media.IBrush -> unit)>(brush: Avalonia.Media.IBrush) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "HeaderBackground"; Value = brush }

        static member inline foreground<'T when 'T : (member set_Foreground : Avalonia.Media.IBrush -> unit)>(brush: Avalonia.Media.IBrush) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "Foreground"; Value = brush }

        static member inline foreground<'T when 'T : (member set_Foreground : Avalonia.Media.IBrush -> unit)>(brush: string) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "Foreground"; Value = Avalonia.Media.SolidColorBrush.Parse(brush).ToImmutable() }

        static member inline fontWeight<'T when 'T : (member set_FontWeight : Avalonia.Media.FontWeight -> unit)>(weight: Avalonia.Media.FontWeight) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "FontWeight"; Value = weight }

        static member inline fontStyle<'T when 'T : (member set_FontStyle : Avalonia.Media.FontStyle -> unit)>(style: Avalonia.Media.FontStyle) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "FontStyle"; Value = style }

        static member inline fontSize<'T when 'T : (member set_FontSize : double -> unit)>(size: double) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "FontSize"; Value = size }

        static member inline fontFamily<'T when 'T : (member set_FontFamily : Avalonia.Media.FontFamily -> unit)>(value: Avalonia.Media.FontFamily) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "FontFamily"; Value = value }

        static member inline firstDayOfWeek<'T when 'T : (member set_FirstDayOfWeek : DayOfWeek -> unit)>(value: DayOfWeek) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "FirstDayOfWeek"; Value = value }

        static member inline extent<'T when 'T : (member set_Extent : Avalonia.Size -> unit)>(value: Avalonia.Size) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "Extent"; Value = value }

        static member inline displayDateStart<'T when 'T : (member set_DisplayDateStart : Nullable<DateTime> -> unit)>(value: Nullable<DateTime>) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "DisplayDateStart"; Value = value }

        static member inline displayDateStart<'T when 'T : (member set_DisplayDateStart : Nullable<DateTime> -> unit)>(value: DateTime option) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "DisplayDateStart"; Value = Option.toNullable value }

        static member inline displayDateEnd<'T when 'T : (member set_DisplayDateEnd : Nullable<DateTime> -> unit)>(value: Nullable<DateTime>) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "DisplayDateEnd"; Value = value }

        static member inline displayDateEnd<'T when 'T : (member set_DisplayDateEnd : Nullable<DateTime> -> unit)>(value: DateTime option) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "DisplayDateEnd"; Value = Option.toNullable value }

        static member inline displayDate<'T when 'T : (member set_DisplayDate : DateTime -> unit)>(value: DateTime) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "DisplayDate"; Value = value }

        // TODO: make more fsharp friendly
        static member inline cornerRadius<'T when 'T : (member set_CornerRadius : Avalonia.CornerRadius -> unit)>(value: Avalonia.CornerRadius) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "CornerRadius"; Value = value }

        static member inline cornerRadius<'T when 'T : (member set_CornerRadius : Avalonia.CornerRadius -> unit)>(value: double) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "CornerRadius"; Value = Avalonia.CornerRadius(value) }

        static member inline cornerRadius<'T when 'T : (member set_CornerRadius : Avalonia.CornerRadius -> unit)>(horizontal: double, vertical: double) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "CornerRadius"; Value = Avalonia.CornerRadius(horizontal, vertical) }

        static member inline cornerRadius<'T when 'T : (member set_CornerRadius : Avalonia.CornerRadius -> unit)>(left: double, top: double, right: double, bottom: double) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "CornerRadius"; Value = Avalonia.CornerRadius(left, top, right, bottom) }

        static member inline content<'T when 'T : (member set_Content : obj -> unit)>(content: obj) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "Content"; Value = content }

        static member inline content<'T when 'T : (member set_Content : string -> unit)>(content: string) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "Content"; Value = content }

        static member inline content<'T when 'T : (member set_Content : IControl -> unit)>(content: IControl) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "Content"; Value = content }

        static member inline content<'T when 'T : (member set_Content : IControl -> unit)>(content: View) : TypedAttr<'T> =
            TypedAttr<_>.Content {
                Name = "Content"
                Content = (ViewContent.Single (Some content))
            }

        static member inline commandParameter<'T when 'T : (member set_CommandParameter : obj -> unit)>(value: obj) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "CommandParameter"; Value = value }

        static member inline command<'T when 'T : (member set_Command : System.Windows.Input.ICommand -> unit)>(value: System.Windows.Input.ICommand) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "Command"; Value = value }

        static member inline child<'T when 'T : (member set_Child : IControl -> unit)>(child: View) : TypedAttr<'T> =
            TypedAttr<_>.Content {
                Name = "Child"
                Content = (ViewContent.Single (Some child))
            }

        static member inline child<'T when 'T : (member set_Child : IControl -> unit)>(child: IControl) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "Child"; Value = child }

        static member inline caretIndex<'T when 'T : (member set_CaretIndex : int -> unit)>(value: int) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "CaretIndex"; Value = value }

        static member inline buttonSpinnerLocation<'T when 'T : (member set_ButtonSpinnerLocation : Location -> unit)>(value: Location) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "ButtonSpinnerLocation"; Value = value }

        static member inline blackoutDates<'T when 'T : (member set_BlackoutDates : Avalonia.Controls.Primitives.CalendarBlackoutDatesCollection -> unit)>(value: Avalonia.Controls.Primitives.CalendarBlackoutDatesCollection) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "BlackoutDates"; Value = value }

        static member inline autoScrollToSelectedItem<'T when 'T : (member set_AutoScrollToSelectedItem : bool -> unit)>(value: bool) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "AutoScrollToSelectedItem"; Value = value }

        static member inline allowSpin<'T when 'T : (member set_AllowSpin : bool -> unit)>(value: bool) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "AllowSpin"; Value = value }

        static member inline zIndex<'T when 'T : (member set_ZIndex : int -> unit)>(value: int) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "ZIndex"; Value = value }

        static member inline width<'T when 'T : (member set_Width : double -> unit)>(value: double) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "Width"; Value = value }

        static member inline height<'T when 'T : (member set_Height : double -> unit)>(value: double) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "Height"; Value = value }

        static member inline visibility<'T when 'T : (member set_Visibility : Avalonia.Controls.Primitives.ScrollBarVisibility -> unit)>(value: Avalonia.Controls.Primitives.ScrollBarVisibility) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "Visibility"; Value = value }

        static member inline verticalScrollBarVisibility<'T when 'T : (member set_VerticalScrollBarVisibility : Avalonia.Controls.Primitives.ScrollBarVisibility -> unit)>(value: Avalonia.Controls.Primitives.ScrollBarVisibility) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "VerticalScrollBarVisibility"; Value = value }

        static member inline verticalOffset<'T when 'T : (member set_VerticalOffset : double -> unit)>(value: double) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "VerticalOffset"; Value = value }

        static member inline verticalAlignment<'T when 'T : (member set_VerticalAlignment : Avalonia.Layout.VerticalAlignment -> unit)>(value: Avalonia.Layout.VerticalAlignment) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "VerticalAlignment"; Value = value }

        static member inline valueMemberSelector<'T when 'T : (member set_ValueMemberSelector : Avalonia.Controls.Templates.IMemberSelector -> unit)>(value: Avalonia.Controls.Templates.IMemberSelector) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "ValueMemberSelector"; Value = value }

        static member inline valueMemberBinding<'T when 'T : (member set_ValueMemberBinding : Avalonia.Data.IBinding -> unit)>(value: Avalonia.Data.IBinding) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "ValueMemberBinding"; Value = value }

        static member inline valueBinding<'T when 'T : (member set_ValueBinding : Avalonia.Data.IBinding -> unit)>(value: Avalonia.Data.IBinding) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "ValueBinding"; Value = value }

        static member inline value<'T, 'value when 'T : (member set_Value : 'value -> unit)>(value: 'value) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "Value"; Value = value }

        static member inline validSpinDirection<'T when 'T : (member set_ValidSpinDirection : ValidSpinDirections -> unit)>(value: ValidSpinDirections) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "ValidSpinDirection"; Value = value }

        static member inline useLayoutRounding<'T when 'T : (member set_UseLayoutRounding : bool -> unit)>(value: bool) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "UseLayoutRounding"; Value = value }

        static member inline transitions<'T when 'T : (member set_Transitions : Avalonia.Animation.Transitions -> unit)>(value: Avalonia.Animation.Transitions) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "Transitions"; Value = value }

        static member inline tickFrequency<'T when 'T : (member set_TickFrequency : double -> unit)>(value: double) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "TickFrequency"; Value = value }

        static member inline thumb<'T when 'T : (member set_Thumb : Avalonia.Controls.Primitives.Thumb -> unit)>(value: Avalonia.Controls.Primitives.Thumb) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "Thumb"; Value = value }

        static member inline textFilter<'T when 'T : (member set_TextFilter : AutoCompleteFilterPredicate<string> -> unit)>(value: AutoCompleteFilterPredicate<string>) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "TextFilter"; Value = value }

        static member inline templatedParent<'T when 'T : (member set_TemplatedParent : Avalonia.Styling.ITemplatedControl -> unit)>(value: Avalonia.Styling.ITemplatedControl) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "TemplatedParent"; Value = value }

        static member inline template<'T when 'T : (member set_Template : Avalonia.Controls.Templates.IControlTemplate -> unit)>(value: Avalonia.Controls.Templates.IControlTemplate) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "Template"; Value = value }

        static member inline tag<'T when 'T : (member set_Tag : obj -> unit)>(value: obj) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "Tag"; Value = value }

        static member inline styles<'T when 'T : (member set_Styles : Avalonia.Styling.Styles -> unit)>(value: Avalonia.Styling.Styles) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "Styles"; Value = value }

        static member inline strokeThickness<'T when 'T : (member set_StrokeThickness : double -> unit)>(value: double) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "StrokeThickness"; Value = value }

        static member inline strokeStartLineCap<'T when 'T : (member set_StrokeStartLineCap : Avalonia.Media.PenLineCap -> unit)>(value: Avalonia.Media.PenLineCap) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "StrokeStartLineCap"; Value = value }

        static member inline strokeJoin<'T when 'T : (member set_StrokeJoin : Avalonia.Media.PenLineJoin -> unit)>(value: Avalonia.Media.PenLineJoin) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "StrokeJoin"; Value = value }

        static member inline strokeEndLineCap<'T when 'T : (member set_StrokeEndLineCap : Avalonia.Media.PenLineCap -> unit)>(value: Avalonia.Media.PenLineCap) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "StrokeEndLineCap"; Value = value }

        static member inline strokeDashOffset<'T when 'T : (member set_StrokeDashOffset : double -> unit)>(value: double) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "StrokeDashOffset"; Value = value }

        static member inline strokeDashCap<'T when 'T : (member set_StrokeDashCap : Avalonia.Media.PenLineCap -> unit)>(value: Avalonia.Media.PenLineCap) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "StrokeDashCap"; Value = value }

        static member inline strokeDashArray<'T when 'T : (member set_StrokeDashArray : Avalonia.Collections.AvaloniaList<double> -> unit)>(value: Avalonia.Collections.AvaloniaList<double>) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "StrokeDashArray"; Value = value }

        static member inline stroke<'T when 'T : (member set_Stroke : Avalonia.Media.IBrush -> unit)>(brush: Avalonia.Media.IBrush) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "Stroke"; Value = brush }

        static member inline staysOpen<'T when 'T : (member set_StaysOpen : bool -> unit)>(value: bool) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "StaysOpen"; Value = value }

        static member inline startPoint<'T when 'T : (member set_StartPoint : Avalonia.Point -> unit)>(value: Avalonia.Point) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "StartPoint"; Value = value }

        static member inline endPoint<'T when 'T : (member set_EndPoint : Avalonia.Point -> unit)>(value: Avalonia.Point) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "EndPoint"; Value = value }

        static member inline spacing<'T when 'T : (member set_Spacing : double -> unit)>(value: double) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "Spacing"; Value = value }

        static member inline source<'T when 'T : (member set_Source : Avalonia.Media.Imaging.IBitmap -> unit)>(value: Avalonia.Media.Imaging.IBitmap) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "Source"; Value = value }

        static member inline smallChange<'T when 'T : (member set_SmallChange : double -> unit)>(value: double) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "SmallChange"; Value = value }

        static member inline showAccessKey<'T when 'T : (member set_ShowAccessKey : bool -> unit)>(value: bool) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "ShowAccessKey"; Value = value }

        static member inline selectionMode<'T when 'T : (member set_SelectionMode : CalendarSelectionMode -> unit)>(value: CalendarSelectionMode) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "SelectionMode"; Value = value }

        static member inline selectedDates<'T when 'T : (member set_SelectedDates : Avalonia.Controls.Primitives.SelectedDatesCollection -> unit)>(value: Avalonia.Controls.Primitives.SelectedDatesCollection) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "SelectedDates"; Value = value }

        static member inline selectedDateFormat<'T when 'T : (member set_SelectedDateFormat : DatePickerFormat -> unit)>(value: DatePickerFormat) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "SelectedDateFormat"; Value = value }

        static member inline selectedContentTemplate<'T when 'T : (member set_SelectedContentTemplate : Avalonia.Controls.Templates.IDataTemplate -> unit)>(value: Avalonia.Controls.Templates.IDataTemplate) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "SelectedContentTemplate"; Value = value }

        static member inline selectedContent<'T when 'T : (member set_SelectedContent : obj -> unit)>(value: obj) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "SelectedContent"; Value = value }

        static member inline searchText<'T when 'T : (member set_SearchText : string -> unit)>(value: string) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "SearchText"; Value = value }

        static member inline scroll<'T when 'T : (member set_Scroll : Avalonia.Controls.Primitives.IScrollable -> unit)>(value: Avalonia.Controls.Primitives.IScrollable) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "Scroll"; Value = value }

        static member inline rows<'T when 'T : (member set_Rows : int -> unit)>(value: int) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "Rows"; Value = value }

        static member inline rowDefinitions<'T when 'T : (member set_RowDefinitions : RowDefinitions -> unit)>(value: RowDefinitions) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "RowDefinitions"; Value = value }

        static member inline resources<'T when 'T : (member set_Resources : IResourceDictionary -> unit)>(value: IResourceDictionary) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "Resources"; Value = value }

        static member inline renderTransformOrigin<'T when 'T : (member set_RenderTransformOrigin : Avalonia.RelativePoint -> unit)>(value: Avalonia.RelativePoint) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "RenderTransformOrigin"; Value = value }

        static member inline renderTransform<'T when 'T : (member set_RenderTransform : Avalonia.Media.Transform -> unit)>(value: Avalonia.Media.Transform) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "RenderTransform"; Value = value }

        static member inline presenter<'T when 'T : (member set_Presenter : Avalonia.Controls.Presenters.IItemsPresenter -> unit)>(value: Avalonia.Controls.Presenters.IItemsPresenter) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "Presenter"; Value = value }

        static member inline placementTarget<'T when 'T : (member set_PlacementTarget : Control -> unit)>(value: Control) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "PlacementTarget"; Value = value }

        static member inline placementMode<'T when 'T : (member set_PlacementMode : PlacementMode -> unit)>(value: PlacementMode) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "PlacementMode"; Value = value }

        static member inline parsingNumberStyle<'T when 'T : (member set_ParsingNumberStyle : System.Globalization.NumberStyles -> unit)>(value: System.Globalization.NumberStyles) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "ParsingNumberStyle"; Value = value }

        static member inline owner<'T when 'T : (member set_Owner : Control -> unit)>(value: Control) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "Owner"; Value = value }

        static member inline opacityMask<'T when 'T : (member set_OpacityMask : Avalonia.Media.IBrush -> unit)>(brush: Avalonia.Media.IBrush) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "OpacityMask"; Value = brush }

        static member inline opacity<'T when 'T : (member set_Opacity : double -> unit)>(value: double) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "Opacity"; Value = value }

        static member inline obeyScreenEdges<'T when 'T : (member set_ObeyScreenEdges : bool -> unit)>(value: bool) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "ObeyScreenEdges"; Value = value }
      
        static member inline newLine<'T when 'T : (member set_NewLine : string -> unit)>(value: string) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "NewLine"; Value = value }

        static member inline name<'T when 'T : (member set_Name : string -> unit)>(value: string) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "Name"; Value = value }

        static member inline mode<'T when 'T : (member set_Mode : Avalonia.Controls.Remote.RemoteWidget.SizingMode -> unit)>(value: Avalonia.Controls.Remote.RemoteWidget.SizingMode) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "Mode"; Value = value }

        static member inline minimumPrefixLength<'T when 'T : (member set_MinimumPrefixLength : int -> unit)>(value: int) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "MinimumPrefixLength"; Value = value }

        static member inline minimumPopulateDelay<'T when 'T : (member set_MinimumPopulateDelay : TimeSpan -> unit)>(value: TimeSpan) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "MinimumPopulateDelay"; Value = value }

        static member inline minWidth<'T when 'T : (member set_MinWidth : double -> unit)>(value: double) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "MinWidth"; Value = value }

        static member inline minHeight<'T when 'T : (member set_MinHeight : double -> unit)>(value: double) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "MinHeight"; Value = value }

        static member inline maxWidth<'T when 'T : (member set_MaxWidth : double -> unit)>(value: double) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "MaxWidth"; Value = value }

        static member inline maxHeight<'T when 'T : (member set_MaxHeight : double -> unit)>(value: double) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "MaxHeight"; Value = value }

        static member inline margin<'T when 'T : (member set_Margin : Avalonia.Thickness -> unit)>(value: Avalonia.Thickness) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "Margin"; Value = value }

        static member inline margin<'T when 'T : (member set_Margin : Avalonia.Thickness -> unit)>(value: double) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "Margin"; Value = Avalonia.Thickness(value) }

        static member inline margin<'T when 'T : (member set_Margin : Avalonia.Thickness -> unit)>(horizontal: double, vertical: double) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "Margin"; Value = Avalonia.Thickness(horizontal, vertical) }

        static member inline margin<'T when 'T : (member set_Margin : Avalonia.Thickness -> unit)>(left: double, top: double, right: double, bottom: double) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "Margin"; Value = Avalonia.Thickness(left, top, right, bottom) }

        static member inline level<'T when 'T : (member set_Level : int -> unit)>(value: int) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "Level"; Value = value }

        static member inline layoutTransform<'T when 'T : (member set_LayoutTransform : Avalonia.Media.Transform -> unit)>(value: Avalonia.Media.Transform) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "LayoutTransform"; Value = value }

        static member inline lastChildFill<'T when 'T : (member set_LastChildFill : bool -> unit)>(value: bool) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "LastChildFill"; Value = value }

        static member inline largeChange<'T when 'T : (member set_LargeChange : double -> unit)>(value: double) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "LargeChange"; Value = value }

        static member inline itemFilter<'T when 'T : (member set_ItemFilter : AutoCompleteFilterPredicate<obj> -> unit)>(value: AutoCompleteFilterPredicate<obj>) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "ItemFilter"; Value = value }

        static member inline isVisible<'T when 'T : (member set_IsVisible : bool -> unit)>(value: bool) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "IsVisible"; Value = value }

        static member inline isThreeState<'T when 'T : (member set_IsThreeState : bool -> unit)>(value: bool) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "IsThreeState"; Value = value }

        static member inline isTextCompletionEnabled<'T when 'T : (member set_IsTextCompletionEnabled : bool -> unit)>(value: bool) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "IsTextCompletionEnabled"; Value = value }

        static member inline isSubMenuOpen<'T when 'T : (member set_IsSubMenuOpen : bool -> unit)>(value: bool) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "IsSubMenuOpen"; Value = value }

        static member inline isSnapToTickEnabled<'T when 'T : (member set_IsSnapToTickEnabled : bool -> unit)>(value: bool) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "IsSnapToTickEnabled"; Value = value }

        static member inline isIntermediate<'T when 'T : (member set_IsIntermediate : bool -> unit)>(value: bool) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "IsIntermediate"; Value = value }

        static member inline isHitTestVisible<'T when 'T : (member set_IsHitTestVisible : bool -> unit)>(value: bool) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "IsHitTestVisible"; Value = value }

        static member inline isEnabled<'T when 'T : (member set_IsEnabled : bool -> unit)>(value: bool) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "IsEnabled"; Value = value }

        static member inline isDefault<'T when 'T : (member set_IsDefault : bool -> unit)>(value: bool) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "IsDefault"; Value = value }

        //static member inline isChecked<'T when 'T : (member set_IsChecked : Nullable<bool> -> unit)>(value: Nullable<bool>) : TypedAttr<'T> =
        //    TypedAttr<_>.Property { Name = "IsChecked"; Value = value }

        //static member inline isChecked<'T when 'T : (member set_IsChecked : Nullable<bool> -> unit)>(value: bool option) : TypedAttr<'T> =
        //    TypedAttr<_>.Property { Name = "IsChecked"; Value = Option.toNullable value }

        static member inline isChecked<'T when 'T : (member set_IsChecked : Nullable<bool> -> unit)>(value: bool) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "IsChecked"; Value = Nullable(value) }

        static member inline interval<'T when 'T : (member set_Interval : int -> unit)>(value: int) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "Interval"; Value = value }

        static member inline increment<'T when 'T : (member set_Increment : double -> unit)>(value: double) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "Increment"; Value = value }

        // TODO: make less generic -> View, IControl, ... ?
        static member inline icon<'T when 'T : (member set_Icon : obj -> unit)>(value: obj) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "Icon"; Value = value }

        static member inline horizontalScrollBarVisibility<'T when 'T : (member set_HorizontalScrollBarVisibility : Avalonia.Controls.Primitives.ScrollBarVisibility -> unit)>(value: Avalonia.Controls.Primitives.ScrollBarVisibility) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "HorizontalScrollBarVisibility"; Value = value }

        static member inline horizontalOffset<'T when 'T : (member set_HorizontalOffset : double -> unit)>(value: double) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "HorizontalOffset"; Value = value }

        static member inline horizontalAlignment<'T when 'T : (member set_HorizontalAlignment : Avalonia.Layout.HorizontalAlignment -> unit)>(value: Avalonia.Layout.HorizontalAlignment) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "HorizontalAlignment"; Value = value }

        static member inline headerTemplate<'T when 'T : (member set_HeaderTemplate : Avalonia.Controls.Templates.IDataTemplate -> unit)>(value: Avalonia.Controls.Templates.IDataTemplate) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "HeaderTemplate"; Value = value }

        static member inline groupName<'T when 'T : (member set_GroupName : string -> unit)>(value: string) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "GroupName"; Value = value }

        static member inline formatString<'T when 'T : (member set_FormatString : string -> unit)>(value: string) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "FormatString"; Value = value }

        static member inline focusable<'T when 'T : (member set_Focusable : bool -> unit)>(value: bool) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "Focusable"; Value = value }

        static member inline focusAdorner<'T when 'T : (member set_FocusAdorner : ITemplate<IControl> -> unit)>(value: ITemplate<IControl>) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "FocusAdorner"; Value = value }

        static member inline firstColumn<'T when 'T : (member set_FirstColumn : int -> unit)>(value: int) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "FirstColumn"; Value = value }

        static member inline filterMode<'T when 'T : (member set_FilterMode : AutoCompleteFilterMode -> unit)>(value: AutoCompleteFilterMode) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "FilterMode"; Value = value }

        static member inline fill<'T when 'T : (member set_Fill : Avalonia.Media.IBrush -> unit)>(brush: Avalonia.Media.IBrush) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "Fill"; Value = brush }

        static member inline expandDirection<'T when 'T : (member set_ExpandDirection : ExpandDirection -> unit)>(value: ExpandDirection) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "ExpandDirection"; Value = value }

        static member inline errorTemplate<'T when 'T : (member set_ErrorTemplate : Avalonia.Controls.Templates.IDataTemplate -> unit)>(value: Avalonia.Controls.Templates.IDataTemplate ) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "ErrorTemplate"; Value = value }

        static member inline drawing<'T when 'T : (member set_Drawing : Avalonia.Media.Drawing -> unit)>(value: Avalonia.Media.Drawing) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "Drawing"; Value = value }

        static member inline displayMode<'T when 'T : (member set_DisplayMode : CalendarMode -> unit)>(value: CalendarMode) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "DisplayMode"; Value = value }

        static member inline dependencyResolver<'T when 'T : (member set_DependencyResolver : Avalonia.IAvaloniaDependencyResolver -> unit)>(value: Avalonia.IAvaloniaDependencyResolver) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "DependencyResolver"; Value = value }

        static member inline delay<'T when 'T : (member set_Delay : int -> unit)>(value: int) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "Delay"; Value = value }

        static member inline dayTitleTemplate<'T when 'T : (member set_DayTitleTemplate : ITemplate<IControl> -> unit)>(value: ITemplate<IControl>) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "DayTitleTemplate"; Value = value }

        static member inline dataContext<'T when 'T : (member set_DataContext : obj -> unit)>(value: obj) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "DataContext"; Value = value }

        static member inline data<'T when 'T : (member set_Data : Avalonia.Media.Geometry -> unit)>(value: Avalonia.Media.Geometry) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "Data"; Value = value }

        static member inline customDateFormatString<'T when 'T : (member set_CustomDateFormatString : string -> unit)>(value: string) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "CustomDateFormatString"; Value = value }

        static member inline cursor<'T when 'T : (member set_Cursor : Avalonia.Input.Cursor -> unit)>(value: Avalonia.Input.Cursor) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "Cursor"; Value = value }

        static member inline cultureInfo<'T when 'T : (member set_CultureInfo : System.Globalization.CultureInfo -> unit)>(value: System.Globalization.CultureInfo) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "CultureInfo"; Value = value }

        static member inline contextMenu<'T when 'T : (member set_ContextMenu : ContextMenu -> unit)>(value: ContextMenu) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "ContextMenu"; Value = value }

        static member inline contentTransition<'T when 'T : (member set_ContentTransition : Avalonia.Animation.IPageTransition -> unit)>(value: Avalonia.Animation.IPageTransition) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "ContentTransition"; Value = value }

        static member inline columns<'T when 'T : (member set_Columns : int -> unit)>(value: int) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "Columns"; Value = value }

        static member inline columnDefinitions<'T when 'T : (member set_ColumnDefinitions : ColumnDefinitions -> unit)>(value: ColumnDefinitions) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "ColumnDefinitions"; Value = value }

        static member inline clock<'T when 'T : (member set_Clock : Avalonia.Animation.IClock -> unit)>(value: Avalonia.Animation.IClock) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "Clock"; Value = value }

        static member inline clipValueToMinMax<'T when 'T : (member set_ClipValueToMinMax : bool -> unit)>(value: bool) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "ClipValueToMinMax"; Value = value }

        static member inline clipToBounds<'T when 'T : (member set_ClipToBounds : bool -> unit)>(value: bool) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "ClipToBounds"; Value = value }

        static member inline clip<'T when 'T : (member set_Clip : Avalonia.Media.Geometry -> unit)>(value: Avalonia.Media.Geometry) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "Clip"; Value = value }

        static member inline clientSize<'T when 'T : (member set_ClientSize : Avalonia.Size -> unit)>(value: Avalonia.Size) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "ClientSize"; Value = value }

        static member inline clickMode<'T when 'T : (member set_ClickMode : ClickMode -> unit)>(value: ClickMode) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "ClickMode"; Value = value }

        static member inline classes<'T when 'T : (member set_Classes : Classes -> unit)>(value: Classes) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "Classes"; Value = value }

        static member inline children<'T when 'T : (member get_Children : unit -> Controls)>(children: View list) : TypedAttr<'T> =
            TypedAttr<_>.Content {
                Name = "Children"
                Content = (ViewContent.Multiple children)
            }

        static member inline canVerticallyScroll<'T when 'T : (member set_CanVerticallyScroll : bool -> unit)>(value: bool) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "CanVerticallyScroll"; Value = value }

        static member inline canHorizontallyScroll<'T when 'T : (member set_CanHorizontallyScroll : bool -> unit)>(value: bool) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "CanHorizontallyScroll"; Value = value }

        static member inline bounds<'T when 'T : (member set_Bounds : Avalonia.Rect -> unit)>(value: Avalonia.Rect) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "Bounds"; Value = value }

        static member inline asyncPopulator<'T when 'T : (member set_AsyncPopulator : Func<string, CancellationToken, System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<obj>>> -> unit)>(value: Func<string, CancellationToken, System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<obj>>>) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "AsyncPopulator"; Value = value }
            
        static member inline accessKey<'T when 'T : (member set_AccessKey : char -> unit)>(value: char) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "AccessKey"; Value = value }

        static member inline acceptsTab<'T when 'T : (member set_AcceptsTab : bool -> unit)>(value: bool) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "AcceptsTab"; Value = value }

        static member inline acceptsReturn<'T when 'T : (member set_AcceptsReturn : bool -> unit)>(value: bool) : TypedAttr<'T> =
            TypedAttr<_>.Property { Name = "AcceptsReturn"; Value = value }

        static member inline click<'T when 'T : (member add_Click : EventHandler<Avalonia.Interactivity.RoutedEventArgs> -> unit)>(click: obj -> Avalonia.Interactivity.RoutedEventArgs -> unit) : TypedAttr<'T> =
            TypedAttr<_>.Event { Name = "Click"; Value = new EventHandler<Avalonia.Interactivity.RoutedEventArgs>(click)}

        static member inline dockPanel_dock<'T when 'T :> Control>(value: Dock) : TypedAttr<'T> =
            let handler (view: obj, value: obj option) =
                match value with
                    | Some some -> DockPanel.SetDock(view :?> Control, some :?> Dock)
                    | None -> DockPanel.SetDock(view :?> Control, Dock.Left)

            TypedAttr<_>.AttachedProperty {
                Name = "AccessKey";
                Value = value;
                Handler = handler;
            }
