namespace Avalonia.FuncUI.Builders

open Avalonia.Controls
open Avalonia.FuncUI.Core.Model

[<AutoOpen>]
module Builders =
    open Avalonia.FuncUI.Core
    open Avalonia

    type BaseBuilder<'view when 'view :> IControl>() =
        member __.Yield (item: 'a) =
            ViewElement.create (typeof<'view>, [])

        [<CustomOperation("pass")>]
        member __.Pass (view: ViewElement) : ViewElement =
            view

    type AnimatableBuilder<'view when 'view :> IControl>() =
        inherit BaseBuilder<'view>()

        [<CustomOperation("clock")>]
        member __.Clock (view: ViewElement, value: Avalonia.Animation.IClock) : ViewElement =
            let attr = Attr.createProperty (Avalonia.Animation.Animatable.ClockProperty, value)
            
            { view with Attrs = attr :: view.Attrs }

        [<CustomOperation("transitions")>]
        member __.Transitions (view: ViewElement, value: Avalonia.Animation.Transitions) : ViewElement=
            let attr = Attr.createProperty (Avalonia.Animation.Animatable.TransitionsProperty, value)
            
            { view with Attrs = attr :: view.Attrs }

    type StyledElementBuilder<'view when 'view :> IControl>() =
        inherit AnimatableBuilder<'view>()

        [<CustomOperation("dataContext")>]
        member __.DataContext (view: ViewElement, value: obj) : ViewElement =
            let attr = Attr.createProperty (StyledElement.DataContextProperty, value)
            
            { view with Attrs = attr :: view.Attrs }

        [<CustomOperation("name")>]
        member __.Name (view: ViewElement, value: string) : ViewElement =
            let attr = Attr.createProperty (StyledElement.NameProperty, value)
            
            { view with Attrs = attr :: view.Attrs }

        [<CustomOperation("parent")>]
        member __.Parent (view: ViewElement, value: IStyledElement) : ViewElement =
            let attr = Attr.createProperty (StyledElement.ParentProperty, value)
            
            { view with Attrs = attr :: view.Attrs }

        [<CustomOperation("templatedParent")>]
        member __.TemplatedParent (view: ViewElement, value: Avalonia.Styling.ITemplatedControl) : ViewElement =
            let attr = Attr.createProperty (StyledElement.TemplatedParentProperty, value)
            
            { view with Attrs = attr :: view.Attrs }

    type VisualBuilder<'view when 'view :> IControl>() =
        inherit StyledElementBuilder<'view>()

        [<CustomOperation("bounds")>]
        member __.Bounds (view: ViewElement, value: Rect) : ViewElement =
            let attr = Attr.createProperty (Visual.BoundsProperty, value)
            
            { view with Attrs = attr :: view.Attrs }

        [<CustomOperation("transformedBounds")>]
        member __.TransformedBounds (view: ViewElement, value: Avalonia.VisualTree.TransformedBounds option) : ViewElement =
            let attr = Attr.createProperty (Visual.TransformedBoundsProperty, value)
            
            { view with Attrs = attr :: view.Attrs }

        [<CustomOperation("clipToBounds")>]
        member __.ClipToBounds (view: ViewElement, value: bool) : ViewElement =
            let attr = Attr.createProperty (Visual.ClipToBoundsProperty, value)
            
            { view with Attrs = attr :: view.Attrs }

        [<CustomOperation("clip")>]
        member __.Clip (view: ViewElement, value: Avalonia.Media.Geometry) : ViewElement =
            let attr = Attr.createProperty (Visual.ClipToBoundsProperty, value)
            
            { view with Attrs = attr :: view.Attrs }

        [<CustomOperation("isVisible")>]
        member __.IsVisible (view: ViewElement, value: bool) : ViewElement =
            let attr = Attr.createProperty (Visual.IsVisibleProperty, value)
            
            { view with Attrs = attr :: view.Attrs }

        [<CustomOperation("opacity")>]
        member __.Opacity (view: ViewElement, value: double) : ViewElement =
            let attr = Attr.createProperty (Visual.OpacityProperty, value)
            
            { view with Attrs = attr :: view.Attrs }

        [<CustomOperation("opacityMask")>]
        member __.OpacityMast (view: ViewElement, value: Avalonia.Media.IBrush) : ViewElement =
            let attr = Attr.createProperty (Visual.OpacityMaskProperty, value)
            
            { view with Attrs = attr :: view.Attrs }

        [<CustomOperation("renderTransform")>]
        member __.RenderTransform (view: ViewElement, value: Avalonia.Media.Transform) : ViewElement =
            let attr = Attr.createProperty (Visual.RenderTransformProperty, value)
            
            { view with Attrs = attr :: view.Attrs }

        [<CustomOperation("renderTransformOrigin")>]
        member __.RenderTransformOrigin (view: ViewElement, value: RelativePoint) : ViewElement =
            let attr = Attr.createProperty (Visual.RenderTransformOriginProperty, value)
            
            { view with Attrs = attr :: view.Attrs }

        [<CustomOperation("visualParent")>]
        member __.VisualParent (view: ViewElement, value: Avalonia.VisualTree.IVisual) : ViewElement =
            let attr = Attr.createProperty (Visual.VisualParentProperty, value)
            
            { view with Attrs = attr :: view.Attrs }

        [<CustomOperation("zIndex")>]
        member __.ZIndex (view: ViewElement, value: int) : ViewElement =
            let attr = Attr.createProperty (Visual.ZIndexProperty, value)
            
            { view with Attrs = attr :: view.Attrs }

    type LayoutableBuilder<'view when 'view :> IControl>() =
        inherit VisualBuilder<'view>()

        [<CustomOperation("desiredSize")>]
        member __.DesiredSize (view: ViewElement, value: Size) : ViewElement =
            let attr = Attr.createProperty (Avalonia.Layout.Layoutable.DesiredSizeProperty, value)
            
            { view with Attrs = attr :: view.Attrs }

        [<CustomOperation("width")>]
        member __.Width (view: ViewElement, value: double) : ViewElement =
            let attr = Attr.createProperty (Avalonia.Layout.Layoutable.WidthProperty, value)
            
            { view with Attrs = attr :: view.Attrs }

        [<CustomOperation("height")>]
        member __.Height (view: ViewElement, value: double) : ViewElement =
            let attr = Attr.createProperty (Avalonia.Layout.Layoutable.HeightProperty, value)
            
            { view with Attrs = attr :: view.Attrs }

        [<CustomOperation("minWidth")>]
        member __.MinWidth (view: ViewElement, value: double) : ViewElement =
            let attr = Attr.createProperty (Avalonia.Layout.Layoutable.MinWidthProperty, value)
            
            { view with Attrs = attr :: view.Attrs }

        [<CustomOperation("minHeight")>]
        member __.MinHeight (view: ViewElement, value: double) : ViewElement =
            let attr = Attr.createProperty (Avalonia.Layout.Layoutable.MinHeightProperty, value)
            
            { view with Attrs = attr :: view.Attrs }

        [<CustomOperation("maxWidth")>]
        member __.MaxWidth (view: ViewElement, value: double) : ViewElement =
            let attr = Attr.createProperty (Avalonia.Layout.Layoutable.MaxWidthProperty, value)
            
            { view with Attrs = attr :: view.Attrs }

        [<CustomOperation("maxHeight")>]
        member __.MaxHeight (view: ViewElement, value: double) : ViewElement =
            let attr = Attr.createProperty (Avalonia.Layout.Layoutable.MaxHeightProperty, value)
            
            { view with Attrs = attr :: view.Attrs }

        [<CustomOperation("margin")>]
        member __.Margin (view: ViewElement, value: Thickness) : ViewElement =
            let attr = Attr.createProperty (Avalonia.Layout.Layoutable.MarginProperty, value)
            
            { view with Attrs = attr :: view.Attrs }

        [<CustomOperation("horizontalAlignment")>]
        member __.HorizontalAlignment (view: ViewElement, value: Avalonia.Layout.HorizontalAlignment) : ViewElement =
            let attr = Attr.createProperty (Avalonia.Layout.Layoutable.HorizontalAlignmentProperty, value)
            
            { view with Attrs = attr :: view.Attrs }

        [<CustomOperation("verticalAlignment")>]
        member __.VerticalAlignment (view: ViewElement, value: Avalonia.Layout.VerticalAlignment) : ViewElement =
            let attr = Attr.createProperty (Avalonia.Layout.Layoutable.VerticalAlignmentProperty, value)
            
            { view with Attrs = attr :: view.Attrs }

        [<CustomOperation("useLayoutRounding")>]
        member __.UseLayoutRounding (view: ViewElement, value: bool) : ViewElement =
            let attr = Attr.createProperty (Avalonia.Layout.Layoutable.UseLayoutRoundingProperty, value)
            
            { view with Attrs = attr :: view.Attrs }

    type InputElementBuilder<'view when 'view :> IControl>() =
        inherit LayoutableBuilder<'view>()

        [<CustomOperation("focusable")>]
        member __.Focusable (view: ViewElement, value: bool) : ViewElement =
            let attr = Attr.createProperty (Avalonia.Input.InputElement.FocusableProperty, value)
            
            { view with Attrs = attr :: view.Attrs }

        [<CustomOperation("isEnabled")>]
        member __.IsEnabled (view: ViewElement, value: bool) : ViewElement =
            let attr = Attr.createProperty (Avalonia.Input.InputElement.IsEnabledProperty, value)
            
            { view with Attrs = attr :: view.Attrs }

        [<CustomOperation("isEnabledCore")>]
        member __.IsEnabledCore (view: ViewElement, value: bool) : ViewElement =
            let attr = Attr.createProperty (Avalonia.Input.InputElement.IsEnabledCoreProperty, value)
            
            { view with Attrs = attr :: view.Attrs }

        [<CustomOperation("cursor")>]
        member __.Cursor (view: ViewElement, value: Avalonia.Input.Cursor) : ViewElement =
            let attr = Attr.createProperty (Avalonia.Input.InputElement.CursorProperty, value)
            
            { view with Attrs = attr :: view.Attrs }

        [<CustomOperation("isFocused")>]
        member __.IsFocused (view: ViewElement, value: bool) : ViewElement =
            let attr = Attr.createProperty (Avalonia.Input.InputElement.IsFocusedProperty, value)
            
            { view with Attrs = attr :: view.Attrs }

        [<CustomOperation("isHitTestVisible")>]
        member __.IsHitTestVisible (view: ViewElement, value: bool) : ViewElement =
            let attr = Attr.createProperty (Avalonia.Input.InputElement.IsHitTestVisibleProperty, value)
            
            { view with Attrs = attr :: view.Attrs }

        [<CustomOperation("isPointerOver")>]
        member __.IsPointerOverProperty (view: ViewElement, value: bool) : ViewElement =
            let attr = Attr.createProperty (Avalonia.Input.InputElement.IsPointerOverProperty, value)
            
            { view with Attrs = attr :: view.Attrs }

    type ControlBuilder<'view when 'view :> IControl>() =
        inherit InputElementBuilder<'view>()
    
        [<CustomOperation("focusAdorner")>]
        member __.FocusAdorner (view: ViewElement, value: ITemplate<IControl>) : ViewElement =
            let attr = Attr.createProperty (Control.FocusAdornerProperty, value)
            
            { view with Attrs = attr :: view.Attrs }

        [<CustomOperation("tag")>]
        member __.Tag (view: ViewElement, value: obj) : ViewElement =
            let attr = Attr.createProperty (Control.TagProperty, value)
            
            { view with Attrs = attr :: view.Attrs }

        [<CustomOperation("contextMenu")>]
        member __.ContextMenu (view: ViewElement, value: ContextMenu) : ViewElement =
            let attr = Attr.createProperty (Control.ContextMenuProperty, value)
            
            { view with Attrs = attr :: view.Attrs }

    type TemplatedControlBuilder<'view when 'view :> IControl>() =
        inherit ControlBuilder<'view>()
    
        [<CustomOperation("background")>]
        member __.Background (view: ViewElement, value: Avalonia.Media.IBrush) : ViewElement =
            let attr = Attr.createProperty (Avalonia.Controls.Primitives.TemplatedControl.BackgroundProperty, value)
            
            { view with Attrs = attr :: view.Attrs }

        [<CustomOperation("borderBrush")>]
        member __.BorderBrush (view: ViewElement, value: Avalonia.Media.IBrush) : ViewElement =
            let attr = Attr.createProperty (Avalonia.Controls.Primitives.TemplatedControl.BorderBrushProperty, value)
            
            { view with Attrs = attr :: view.Attrs }

        [<CustomOperation("borderThickness")>]
        member __.BorderThickness (view: ViewElement, value: Thickness) : ViewElement =
            let attr = Attr.createProperty (Avalonia.Controls.Primitives.TemplatedControl.BorderThicknessProperty, value)
            
            { view with Attrs = attr :: view.Attrs }

        [<CustomOperation("fontFamily")>]
        member __.FontFamily (view: ViewElement, value: Avalonia.Media.FontFamily) : ViewElement =
            let attr = Attr.createProperty (Avalonia.Controls.Primitives.TemplatedControl.FontFamilyProperty, value)
            { view with Attrs = attr :: view.Attrs }

        [<CustomOperation("fontSize")>]
        member __.FontSize (view: ViewElement, value: double) : ViewElement =
            let attr = Attr.createProperty (Avalonia.Controls.Primitives.TemplatedControl.FontSizeProperty, value)
            { view with Attrs = attr :: view.Attrs }

        [<CustomOperation("fontStyle")>]
        member __.FontStyle (view: ViewElement, value: Avalonia.Media.FontStyle) : ViewElement =
            let attr = Attr.createProperty (Avalonia.Controls.Primitives.TemplatedControl.FontStyleProperty, value)
            { view with Attrs = attr :: view.Attrs }

        [<CustomOperation("fontWeight")>]
        member __.FontWeight (view: ViewElement, value: Avalonia.Media.FontWeight) : ViewElement =
            let attr = Attr.createProperty (Avalonia.Controls.Primitives.TemplatedControl.FontWeightProperty, value)
            { view with Attrs = attr :: view.Attrs }

        [<CustomOperation("foreground")>]
        member __.Foreground (view: ViewElement, value: Avalonia.Media.IBrush) : ViewElement =
            let attr = Attr.createProperty (Avalonia.Controls.Primitives.TemplatedControl.ForegroundProperty, value)
            { view with Attrs = attr :: view.Attrs }

        [<CustomOperation("padding")>]
        member __.Padding (view: ViewElement, value: Thickness) : ViewElement =
            let attr = Attr.createProperty (Avalonia.Controls.Primitives.TemplatedControl.PaddingProperty, value)
            { view with Attrs = attr :: view.Attrs }

        [<CustomOperation("template")>]
        member __.Template (view: ViewElement, value: Avalonia.Controls.Templates.IControlTemplate) : ViewElement =
            let attr = Attr.createProperty (Avalonia.Controls.Primitives.TemplatedControl.TemplateProperty, value)
            { view with Attrs = attr :: view.Attrs }

    type ContentControlBuilder<'view when 'view :> IControl>() =
        inherit TemplatedControlBuilder<'view>()
    
        [<CustomOperation("content")>]
        member __.Content (view: ViewElement, value: IControl) : ViewElement =
            let attr = Attr.createProperty (ContentControl.ContentProperty, value)
            { view with Attrs = attr :: view.Attrs }

        [<CustomOperation("contentView")>]
        member __.ContentView (view: ViewElement, value: ViewElement) : ViewElement =
            let attr = Attr.createContent ("Content", value |> Some |> ViewContent.Single)
            { view with Attrs = attr :: view.Attrs }

        [<CustomOperation("contentTemplate")>]
        member __.ContentTemplate (view: ViewElement, value: Avalonia.Controls.Templates.IDataTemplate) : ViewElement =
            let attr = Attr.createProperty (ContentControl.ContentTemplateProperty, value)
            { view with Attrs = attr :: view.Attrs }

        [<CustomOperation("horizontalContentAlignment")>]
        member __.HorizontalContentAlignment (view: ViewElement, value: Avalonia.Layout.HorizontalAlignment) : ViewElement =
            let attr = Attr.createProperty (ContentControl.HorizontalContentAlignmentProperty, value)
            { view with Attrs = attr :: view.Attrs }

        [<CustomOperation("verticalContentAlignment")>]
        member __.VerticalContentAlignment (view: ViewElement, value: Avalonia.Layout.VerticalAlignment) : ViewElement =
            let attr = Attr.createProperty (ContentControl.VerticalContentAlignmentProperty, value)
            { view with Attrs = attr :: view.Attrs }
 
    type PanelBuilder<'view when 'view :> IControl>() =
        inherit ControlBuilder<'view>()
    
         [<CustomOperation("background")>]
         member __.Background (view: ViewElement, value: Avalonia.Media.IBrush) : ViewElement =
             let attr = Attr.createProperty (Panel.BackgroundProperty, value)
             { view with Attrs = attr :: view.Attrs }

         [<CustomOperation("children")>]
         member __.Children (view: ViewElement, value: ViewElement list) : ViewElement =
             let attr = Attr.createContent ("Children", ViewContent.Multiple value)
             { view with Attrs = attr :: view.Attrs }
    
    type StackPanelBuilder<'view when 'view :> IControl>() =
        inherit PanelBuilder<'view>()

         [<CustomOperation("spacing")>]
         member __.Spacing (view: ViewElement, value: double) : ViewElement =
             let attr = Attr.createProperty (StackPanel.SpacingProperty, value)
             { view with Attrs = attr :: view.Attrs }

        [<CustomOperation("orientation")>]
        member __.Orientation (view: ViewElement, value: Orientation) : ViewElement =
            let attr = Attr.createProperty (StackPanel.OrientationProperty, value)
            { view with Attrs = attr :: view.Attrs }

    type DockPanelBuilder<'view when 'view :> IControl>() =
        inherit PanelBuilder<'view>()

         [<CustomOperation("lastChildFill")>]
         member __.Spacing (view: ViewElement, value: bool) : ViewElement =
             let attr = Attr.createProperty (DockPanel.LastChildFillProperty, value)
             { view with Attrs = attr :: view.Attrs }

    type ControlBuilder<'view when 'view :> IControl> with

        [<CustomOperation("dockpanel_dock")>]
        member __.DockPanel_Dock (view: ViewElement, value: Dock) : ViewElement =
            let attr = Attr.createProperty (DockPanel.DockProperty, value)
            { view with Attrs = attr :: view.Attrs }

    type ButtonBuilder<'view when 'view :> Button>() =
        inherit ContentControlBuilder<Button>()

        [<CustomOperation("command")>]
        member __.Command (view: ViewElement, value: System.Windows.Input.ICommand) : ViewElement=
            let attr = Attr.createProperty (Button.CommandProperty, value)
            { view with Attrs = attr :: view.Attrs }

        [<CustomOperation("commandParameter")>]
        member __.Click (view: ViewElement, value: obj) : ViewElement=
            let attr = Attr.createProperty (Button.CommandParameterProperty, value)
            { view with Attrs = attr :: view.Attrs }

    type TextBlockBuilder<'view when 'view :> TextBlock>() =
        inherit ControlBuilder<TextBlock>()

        [<CustomOperation("background")>]
        member __.Background (view: ViewElement, value: Avalonia.Media.IBrush) : ViewElement =
            let attr = Attr.createProperty (TextBlock.BackgroundProperty, value)
            { view with Attrs = attr :: view.Attrs }

        [<CustomOperation("fontFamily")>]
        member __.FontFamily (view: ViewElement, value: Avalonia.Media.FontFamily) : ViewElement =
            let attr = Attr.createProperty (TextBlock.FontFamilyProperty, value)
            { view with Attrs = attr :: view.Attrs }

        [<CustomOperation("fontSize")>]
        member __.FontSize (view: ViewElement, value: double) : ViewElement =
            let attr = Attr.createProperty (TextBlock.FontSizeProperty, value)
            { view with Attrs = attr :: view.Attrs }

        [<CustomOperation("fontStyle")>]
        member __.FontStyle (view: ViewElement, value: Avalonia.Media.FontStyle) : ViewElement =
            let attr = Attr.createProperty (TextBlock.FontStyleProperty, value)
            { view with Attrs = attr :: view.Attrs }

        [<CustomOperation("fontWeight")>]
        member __.FontWeight (view: ViewElement, value: Avalonia.Media.FontWeight) : ViewElement =
            let attr = Attr.createProperty (TextBlock.FontWeightProperty, value)
            { view with Attrs = attr :: view.Attrs }

        [<CustomOperation("foreground")>]
        member __.Foreground (view: ViewElement, value: Avalonia.Media.IBrush) : ViewElement =
            let attr = Attr.createProperty (TextBlock.ForegroundProperty, value)
            { view with Attrs = attr :: view.Attrs }

        [<CustomOperation("text")>]
        member __.Text (view: ViewElement, value: string) : ViewElement=
            let attr = Attr.createProperty (TextBlock.TextProperty, value)         
            { view with Attrs = attr :: view.Attrs }

        [<CustomOperation("textAlignment")>]
        member __.TextAlignment (view: ViewElement, value: Avalonia.Media.TextAlignment) : ViewElement =
            let attr = Attr.createProperty (TextBlock.TextAlignmentProperty, value)
            { view with Attrs = attr :: view.Attrs }

        [<CustomOperation("textWrapping")>]
        member __.TextWrapping (view: ViewElement, value: Avalonia.Media.TextWrapping) : ViewElement =
            let attr = Attr.createProperty (TextBlock.TextWrappingProperty, value)
            { view with Attrs = attr :: view.Attrs }
    
    type TextBoxBuilder<'view when 'view :> TextBox>() =
        inherit ControlBuilder<TextBox>()

        [<CustomOperation("acceptsReturn")>]
        member __.AcceptsReturn (view: ViewElement, value: bool) : ViewElement =
            let attr = Attr.createProperty (TextBox.AcceptsReturnProperty, value)
            { view with Attrs = attr :: view.Attrs }

        [<CustomOperation("acceptsTab")>]
        member __.AcceptsTab (view: ViewElement, value: bool) : ViewElement =
            let attr = Attr.createProperty (TextBox.AcceptsTabProperty, value)
            { view with Attrs = attr :: view.Attrs }

        [<CustomOperation("caretIndex")>]
        member __.CaretIndex (view: ViewElement, value: int) : ViewElement =
            let attr = Attr.createProperty (TextBox.CaretIndexProperty, value)
            { view with Attrs = attr :: view.Attrs }

        [<CustomOperation("isReadOnly")>]
        member __.IsReadOnly (view: ViewElement, value: bool) : ViewElement =
            let attr = Attr.createProperty (TextBox.IsReadOnlyProperty, value)
            { view with Attrs = attr :: view.Attrs }

        [<CustomOperation("passwordChar")>]
        member __.PasswordChar (view: ViewElement, value: char) : ViewElement =
            let attr = Attr.createProperty (TextBox.PasswordCharProperty, value)
            { view with Attrs = attr :: view.Attrs }

        [<CustomOperation("selectionStart")>]
        member __.SelectionStart (view: ViewElement, value: int) : ViewElement =
            let attr = Attr.createProperty (TextBox.SelectionStartProperty, value)
            { view with Attrs = attr :: view.Attrs }

        [<CustomOperation("selectionEnd")>]
        member __.SelectionEnd (view: ViewElement, value: int) : ViewElement =
            let attr = Attr.createProperty (TextBox.SelectionEndProperty, value)
            { view with Attrs = attr :: view.Attrs }

        [<CustomOperation("text")>]
        member __.Text (view: ViewElement, value: string) : ViewElement=
            let attr = Attr.createProperty (TextBox.TextProperty, value)         
            { view with Attrs = attr :: view.Attrs }

        [<CustomOperation("textAlignment")>]
        member __.TextAlignment (view: ViewElement, value: Avalonia.Media.TextAlignment) : ViewElement =
            let attr = Attr.createProperty (TextBox.TextAlignmentProperty, value)
            { view with Attrs = attr :: view.Attrs }

        [<CustomOperation("textWrapping")>]
        member __.TextWrapping (view: ViewElement, value: Avalonia.Media.TextWrapping) : ViewElement =
            let attr = Attr.createProperty (TextBox.TextWrappingProperty, value)
            { view with Attrs = attr :: view.Attrs }

        [<CustomOperation("watermark")>]
        member __.Watermark (view: ViewElement, value: string) : ViewElement =
            let attr = Attr.createProperty (TextBox.WatermarkProperty, value)
            { view with Attrs = attr :: view.Attrs }

        [<CustomOperation("useFloatingWatermark")>]
        member __.UseFloatingWatermark (view: ViewElement, value: bool) : ViewElement =
            let attr = Attr.createProperty (TextBox.UseFloatingWatermarkProperty, value)
            { view with Attrs = attr :: view.Attrs }

        [<CustomOperation("newLine")>]
        member __.NewLine (view: ViewElement, value: string) : ViewElement =
            let attr = Attr.createProperty (TextBox.NewLineProperty, value)
            { view with Attrs = attr :: view.Attrs }

    let stackpanel = StackPanelBuilder<StackPanel>()
    let dockpanel = DockPanelBuilder<DockPanel>()

    let button = ButtonBuilder<Button>()
    let textblock = TextBlockBuilder<TextBlock>()
    let textbox = TextBoxBuilder<TextBox>()


