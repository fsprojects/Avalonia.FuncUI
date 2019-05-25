namespace Avalonia.FuncUI.Builders

open Avalonia.Controls

[<AutoOpen>]
module Builders =
    open Avalonia.FuncUI.Core
    open Avalonia

    type BaseBuilder<'view when 'view :> IControl>() =
        member __.Yield (item: 'a) =
            ViewElement.create (typeof<'view>, [])

    type AnimatableBuilder<'view when 'view :> IControl>() =
        inherit BaseBuilder<'view>()

        [<CustomOperation("clock")>]
        member __.Clock (view: ViewElement, value: Avalonia.Animation.IClock) : ViewElement =
            let attr = Attr.createProperty (Avalonia.Animation.Animatable.ClockProperty, value)
            let attrInfo = AttrInfo.create(typeof<'view>, attr)
            { view with Attrs = attrInfo :: view.Attrs }

        [<CustomOperation("transitions")>]
        member __.Transitions (view: ViewElement, value: Avalonia.Animation.Transitions) : ViewElement=
            let attr = Attr.createProperty (Avalonia.Animation.Animatable.TransitionsProperty, value)
            let attrInfo = AttrInfo.create(typeof<'view>, attr)
            { view with Attrs = attrInfo :: view.Attrs }

    type StyledElementBuilder<'view when 'view :> IControl>() =
        inherit AnimatableBuilder<'view>()

        [<CustomOperation("dataContext")>]
        member __.DataContext (view: ViewElement, value: obj) : ViewElement =
            let attr = Attr.createProperty (StyledElement.DataContextProperty, value)
            let attrInfo = AttrInfo.create(typeof<'view>, attr)
            { view with Attrs = attrInfo :: view.Attrs }

        [<CustomOperation("name")>]
        member __.Name (view: ViewElement, value: string) : ViewElement =
            let attr = Attr.createProperty (StyledElement.NameProperty, value)
            let attrInfo = AttrInfo.create(typeof<'view>, attr)
            { view with Attrs = attrInfo :: view.Attrs }

        [<CustomOperation("parent")>]
        member __.Parent (view: ViewElement, value: IStyledElement) : ViewElement =
            let attr = Attr.createProperty (StyledElement.ParentProperty, value)
            let attrInfo = AttrInfo.create(typeof<'view>, attr)
            { view with Attrs = attrInfo :: view.Attrs }

        [<CustomOperation("templatedParent")>]
        member __.TemplatedParent (view: ViewElement, value: Avalonia.Styling.ITemplatedControl) : ViewElement =
            let attr = Attr.createProperty (StyledElement.TemplatedParentProperty, value)
            let attrInfo = AttrInfo.create(typeof<'view>, attr)
            { view with Attrs = attrInfo :: view.Attrs }

    type VisualBuilder<'view when 'view :> IControl>() =
        inherit StyledElementBuilder<'view>()

        [<CustomOperation("bounds")>]
        member __.Bounds (view: ViewElement, value: Rect) : ViewElement =
            let attr = Attr.createProperty (Visual.BoundsProperty, value)
            let attrInfo = AttrInfo.create(typeof<'view>, attr)
            { view with Attrs = attrInfo :: view.Attrs }

        [<CustomOperation("transformedBounds")>]
        member __.TransformedBounds (view: ViewElement, value: Avalonia.VisualTree.TransformedBounds option) : ViewElement =
            let attr = Attr.createProperty (Visual.TransformedBoundsProperty, value)
            let attrInfo = AttrInfo.create(typeof<'view>, attr)
            { view with Attrs = attrInfo :: view.Attrs }

        [<CustomOperation("clipToBounds")>]
        member __.ClipToBounds (view: ViewElement, value: bool) : ViewElement =
            let attr = Attr.createProperty (Visual.ClipToBoundsProperty, value)
            let attrInfo = AttrInfo.create(typeof<'view>, attr)
            { view with Attrs = attrInfo :: view.Attrs }

        [<CustomOperation("clip")>]
        member __.Clip (view: ViewElement, value: Avalonia.Media.Geometry) : ViewElement =
            let attr = Attr.createProperty (Visual.ClipToBoundsProperty, value)
            let attrInfo = AttrInfo.create(typeof<'view>, attr)
            { view with Attrs = attrInfo :: view.Attrs }

        [<CustomOperation("isVisible")>]
        member __.IsVisible (view: ViewElement, value: bool) : ViewElement =
            let attr = Attr.createProperty (Visual.IsVisibleProperty, value)
            let attrInfo = AttrInfo.create(typeof<'view>, attr)
            { view with Attrs = attrInfo :: view.Attrs }

        [<CustomOperation("opacity")>]
        member __.Opacity (view: ViewElement, value: double) : ViewElement =
            let attr = Attr.createProperty (Visual.OpacityProperty, value)
            let attrInfo = AttrInfo.create(typeof<'view>, attr)
            { view with Attrs = attrInfo :: view.Attrs }

        [<CustomOperation("opacityMask")>]
        member __.OpacityMast (view: ViewElement, value: Avalonia.Media.IBrush) : ViewElement =
            let attr = Attr.createProperty (Visual.OpacityMaskProperty, value)
            let attrInfo = AttrInfo.create(typeof<'view>, attr)
            { view with Attrs = attrInfo :: view.Attrs }

        [<CustomOperation("renderTransform")>]
        member __.RenderTransform (view: ViewElement, value: Avalonia.Media.Transform) : ViewElement =
            let attr = Attr.createProperty (Visual.RenderTransformProperty, value)
            let attrInfo = AttrInfo.create(typeof<'view>, attr)
            { view with Attrs = attrInfo :: view.Attrs }

        [<CustomOperation("renderTransformOrigin")>]
        member __.RenderTransformOrigin (view: ViewElement, value: RelativePoint) : ViewElement =
            let attr = Attr.createProperty (Visual.RenderTransformOriginProperty, value)
            let attrInfo = AttrInfo.create(typeof<'view>, attr)
            { view with Attrs = attrInfo :: view.Attrs }

        [<CustomOperation("visualParent")>]
        member __.VisualParent (view: ViewElement, value: Avalonia.VisualTree.IVisual) : ViewElement =
            let attr = Attr.createProperty (Visual.VisualParentProperty, value)
            let attrInfo = AttrInfo.create(typeof<'view>, attr)
            { view with Attrs = attrInfo :: view.Attrs }

        [<CustomOperation("zIndex")>]
        member __.ZIndex (view: ViewElement, value: int) : ViewElement =
            let attr = Attr.createProperty (Visual.ZIndexProperty, value)
            let attrInfo = AttrInfo.create(typeof<'view>, attr)
            { view with Attrs = attrInfo :: view.Attrs }

    type LayoutableBuilder<'view when 'view :> IControl>() =
        inherit VisualBuilder<'view>()

        [<CustomOperation("desiredSize")>]
        member __.DesiredSize (view: ViewElement, value: Size) : ViewElement =
            let attr = Attr.createProperty (Avalonia.Layout.Layoutable.DesiredSizeProperty, value)
            let attrInfo = AttrInfo.create(typeof<'view>, attr)
            { view with Attrs = attrInfo :: view.Attrs }

        [<CustomOperation("width")>]
        member __.Width (view: ViewElement, value: double) : ViewElement =
            let attr = Attr.createProperty (Avalonia.Layout.Layoutable.WidthProperty, value)
            let attrInfo = AttrInfo.create(typeof<'view>, attr)
            { view with Attrs = attrInfo :: view.Attrs }

        [<CustomOperation("height")>]
        member __.Height (view: ViewElement, value: double) : ViewElement =
            let attr = Attr.createProperty (Avalonia.Layout.Layoutable.HeightProperty, value)
            let attrInfo = AttrInfo.create(typeof<'view>, attr)
            { view with Attrs = attrInfo :: view.Attrs }

        [<CustomOperation("minWidth")>]
        member __.MinWidth (view: ViewElement, value: double) : ViewElement =
            let attr = Attr.createProperty (Avalonia.Layout.Layoutable.MinWidthProperty, value)
            let attrInfo = AttrInfo.create(typeof<'view>, attr)
            { view with Attrs = attrInfo :: view.Attrs }

        [<CustomOperation("minHeight")>]
        member __.MinHeight (view: ViewElement, value: double) : ViewElement =
            let attr = Attr.createProperty (Avalonia.Layout.Layoutable.MinHeightProperty, value)
            let attrInfo = AttrInfo.create(typeof<'view>, attr)
            { view with Attrs = attrInfo :: view.Attrs }

        [<CustomOperation("maxWidth")>]
        member __.MaxWidth (view: ViewElement, value: double) : ViewElement =
            let attr = Attr.createProperty (Avalonia.Layout.Layoutable.MaxWidthProperty, value)
            let attrInfo = AttrInfo.create(typeof<'view>, attr)
            { view with Attrs = attrInfo :: view.Attrs }

        [<CustomOperation("maxHeight")>]
        member __.MaxHeight (view: ViewElement, value: double) : ViewElement =
            let attr = Attr.createProperty (Avalonia.Layout.Layoutable.MaxHeightProperty, value)
            let attrInfo = AttrInfo.create(typeof<'view>, attr)
            { view with Attrs = attrInfo :: view.Attrs }

        [<CustomOperation("margin")>]
        member __.Margin (view: ViewElement, value: Thickness) : ViewElement =
            let attr = Attr.createProperty (Avalonia.Layout.Layoutable.MarginProperty, value)
            let attrInfo = AttrInfo.create(typeof<'view>, attr)
            { view with Attrs = attrInfo :: view.Attrs }

        [<CustomOperation("horizontalAlignment")>]
        member __.HorizontalAlignment (view: ViewElement, value: Avalonia.Layout.HorizontalAlignment) : ViewElement =
            let attr = Attr.createProperty (Avalonia.Layout.Layoutable.HorizontalAlignmentProperty, value)
            let attrInfo = AttrInfo.create(typeof<'view>, attr)
            { view with Attrs = attrInfo :: view.Attrs }

        [<CustomOperation("verticalAlignment")>]
        member __.VerticalAlignment (view: ViewElement, value: Avalonia.Layout.VerticalAlignment) : ViewElement =
            let attr = Attr.createProperty (Avalonia.Layout.Layoutable.VerticalAlignmentProperty, value)
            let attrInfo = AttrInfo.create(typeof<'view>, attr)
            { view with Attrs = attrInfo :: view.Attrs }

        [<CustomOperation("useLayoutRounding")>]
        member __.UseLayoutRounding (view: ViewElement, value: bool) : ViewElement =
            let attr = Attr.createProperty (Avalonia.Layout.Layoutable.UseLayoutRoundingProperty, value)
            let attrInfo = AttrInfo.create(typeof<'view>, attr)
            { view with Attrs = attrInfo :: view.Attrs }

    type InputElementBuilder<'view when 'view :> IControl>() =
        inherit LayoutableBuilder<'view>()

        [<CustomOperation("focusable")>]
        member __.Focusable (view: ViewElement, value: bool) : ViewElement =
            let attr = Attr.createProperty (Avalonia.Input.InputElement.FocusableProperty, value)
            let attrInfo = AttrInfo.create(typeof<'view>, attr)
            { view with Attrs = attrInfo :: view.Attrs }

        [<CustomOperation("isEnabled")>]
        member __.IsEnabled (view: ViewElement, value: bool) : ViewElement =
            let attr = Attr.createProperty (Avalonia.Input.InputElement.IsEnabledProperty, value)
            let attrInfo = AttrInfo.create(typeof<'view>, attr)
            { view with Attrs = attrInfo :: view.Attrs }

        [<CustomOperation("isEnabledCore")>]
        member __.IsEnabledCore (view: ViewElement, value: bool) : ViewElement =
            let attr = Attr.createProperty (Avalonia.Input.InputElement.IsEnabledCoreProperty, value)
            let attrInfo = AttrInfo.create(typeof<'view>, attr)
            { view with Attrs = attrInfo :: view.Attrs }

        [<CustomOperation("cursor")>]
        member __.Cursor (view: ViewElement, value: Avalonia.Input.Cursor) : ViewElement =
            let attr = Attr.createProperty (Avalonia.Input.InputElement.CursorProperty, value)
            let attrInfo = AttrInfo.create(typeof<'view>, attr)
            { view with Attrs = attrInfo :: view.Attrs }

        [<CustomOperation("isFocused")>]
        member __.IsFocused (view: ViewElement, value: bool) : ViewElement =
            let attr = Attr.createProperty (Avalonia.Input.InputElement.IsFocusedProperty, value)
            let attrInfo = AttrInfo.create(typeof<'view>, attr)
            { view with Attrs = attrInfo :: view.Attrs }

        [<CustomOperation("isHitTestVisible")>]
        member __.IsHitTestVisible (view: ViewElement, value: bool) : ViewElement =
            let attr = Attr.createProperty (Avalonia.Input.InputElement.IsHitTestVisibleProperty, value)
            let attrInfo = AttrInfo.create(typeof<'view>, attr)
            { view with Attrs = attrInfo :: view.Attrs }

        [<CustomOperation("isPointerOver")>]
        member __.IsPointerOverProperty (view: ViewElement, value: bool) : ViewElement =
            let attr = Attr.createProperty (Avalonia.Input.InputElement.IsPointerOverProperty, value)
            let attrInfo = AttrInfo.create(typeof<'view>, attr)
            { view with Attrs = attrInfo :: view.Attrs }

    type ControlBuilder<'view when 'view :> IControl>() =
        inherit InputElementBuilder<'view>()
    
        [<CustomOperation("focusAdorner")>]
        member __.FocusAdorner (view: ViewElement, value: ITemplate<IControl>) : ViewElement =
            let attr = Attr.createProperty (Control.FocusAdornerProperty, value)
            let attrInfo = AttrInfo.create(typeof<'view>, attr)
            { view with Attrs = attrInfo :: view.Attrs }

        [<CustomOperation("tag")>]
        member __.Tag (view: ViewElement, value: obj) : ViewElement =
            let attr = Attr.createProperty (Control.TagProperty, value)
            let attrInfo = AttrInfo.create(typeof<'view>, attr)
            { view with Attrs = attrInfo :: view.Attrs }

        [<CustomOperation("contextMenu")>]
        member __.ContextMenu (view: ViewElement, value: ContextMenu) : ViewElement =
            let attr = Attr.createProperty (Control.ContextMenuProperty, value)
            let attrInfo = AttrInfo.create(typeof<'view>, attr)
            { view with Attrs = attrInfo :: view.Attrs }

    type TemplatedControlBuilder<'view when 'view :> IControl>() =
        inherit ControlBuilder<'view>()
    
        [<CustomOperation("background")>]
        member __.Background (view: ViewElement, value: Avalonia.Media.IBrush) : ViewElement =
            let attr = Attr.createProperty (Avalonia.Controls.Primitives.TemplatedControl.BackgroundProperty, value)
            let attrInfo = AttrInfo.create(typeof<'view>, attr)
            { view with Attrs = attrInfo :: view.Attrs }

        [<CustomOperation("borderBrush")>]
        member __.BorderBrush (view: ViewElement, value: Avalonia.Media.IBrush) : ViewElement =
            let attr = Attr.createProperty (Avalonia.Controls.Primitives.TemplatedControl.BorderBrushProperty, value)
            let attrInfo = AttrInfo.create(typeof<'view>, attr)
            { view with Attrs = attrInfo :: view.Attrs }

        [<CustomOperation("borderThickness")>]
        member __.BorderThickness (view: ViewElement, value: Thickness) : ViewElement =
            let attr = Attr.createProperty (Avalonia.Controls.Primitives.TemplatedControl.BorderThicknessProperty, value)
            let attrInfo = AttrInfo.create(typeof<'view>, attr)
            { view with Attrs = attrInfo :: view.Attrs }

        [<CustomOperation("fontFamily")>]
        member __.FontFamily (view: ViewElement, value: Avalonia.Media.FontFamily) : ViewElement =
            let attr = Attr.createProperty (Avalonia.Controls.Primitives.TemplatedControl.FontFamilyProperty, value)
            let attrInfo = AttrInfo.create(typeof<'view>, attr)
            { view with Attrs = attrInfo :: view.Attrs }

        [<CustomOperation("fontSize")>]
        member __.FontSize (view: ViewElement, value: double) : ViewElement =
            let attr = Attr.createProperty (Avalonia.Controls.Primitives.TemplatedControl.FontSizeProperty, value)
            let attrInfo = AttrInfo.create(typeof<'view>, attr)
            { view with Attrs = attrInfo :: view.Attrs }

        [<CustomOperation("fontStyle")>]
        member __.FontStyle (view: ViewElement, value: Avalonia.Media.FontStyle) : ViewElement =
            let attr = Attr.createProperty (Avalonia.Controls.Primitives.TemplatedControl.FontStyleProperty, value)
            let attrInfo = AttrInfo.create(typeof<'view>, attr)
            { view with Attrs = attrInfo :: view.Attrs }

        [<CustomOperation("fontWeight")>]
        member __.FontWeight (view: ViewElement, value: Avalonia.Media.FontWeight) : ViewElement =
            let attr = Attr.createProperty (Avalonia.Controls.Primitives.TemplatedControl.FontWeightProperty, value)
            let attrInfo = AttrInfo.create(typeof<'view>, attr)
            { view with Attrs = attrInfo :: view.Attrs }

        [<CustomOperation("foreground")>]
        member __.Foreground (view: ViewElement, value: Avalonia.Media.IBrush) : ViewElement =
            let attr = Attr.createProperty (Avalonia.Controls.Primitives.TemplatedControl.ForegroundProperty, value)
            let attrInfo = AttrInfo.create(typeof<'view>, attr)
            { view with Attrs = attrInfo :: view.Attrs }

        [<CustomOperation("padding")>]
        member __.Padding (view: ViewElement, value: Thickness) : ViewElement =
            let attr = Attr.createProperty (Avalonia.Controls.Primitives.TemplatedControl.PaddingProperty, value)
            let attrInfo = AttrInfo.create(typeof<'view>, attr)
            { view with Attrs = attrInfo :: view.Attrs }

        [<CustomOperation("template")>]
        member __.Template (view: ViewElement, value: Avalonia.Controls.Templates.IControlTemplate) : ViewElement =
            let attr = Attr.createProperty (Avalonia.Controls.Primitives.TemplatedControl.TemplateProperty, value)
            let attrInfo = AttrInfo.create(typeof<'view>, attr)
            { view with Attrs = attrInfo :: view.Attrs }

    type ContentControlBuilder<'view when 'view :> IControl>() =
        inherit TemplatedControlBuilder<'view>()
    
        [<CustomOperation("content")>]
        member __.Content (view: ViewElement, value: IControl) : ViewElement =
            let attr = Attr.createProperty (ContentControl.ContentProperty, value)
            let attrInfo = AttrInfo.create(typeof<'view>, attr)
            { view with Attrs = attrInfo :: view.Attrs }

        [<CustomOperation("contentView")>]
        member __.ContentView (view: ViewElement, value: ViewElement) : ViewElement =
            let attr = Attr.createProperty (ContentControl.ContentProperty, value)
            let attrInfo = AttrInfo.create(typeof<'view>, attr)
            { view with Attrs = attrInfo :: view.Attrs }

        [<CustomOperation("contentTemplate")>]
        member __.ContentTemplate (view: ViewElement, value: Avalonia.Controls.Templates.IDataTemplate) : ViewElement =
            let attr = Attr.createProperty (ContentControl.ContentTemplateProperty, value)
            let attrInfo = AttrInfo.create(typeof<'view>, attr)
            { view with Attrs = attrInfo :: view.Attrs }

        [<CustomOperation("horizontalContentAlignment")>]
        member __.HorizontalContentAlignment (view: ViewElement, value: Avalonia.Layout.HorizontalAlignment) : ViewElement =
            let attr = Attr.createProperty (ContentControl.HorizontalContentAlignmentProperty, value)
            let attrInfo = AttrInfo.create(typeof<'view>, attr)
            { view with Attrs = attrInfo :: view.Attrs }

        [<CustomOperation("verticalContentAlignment")>]
        member __.VerticalContentAlignment (view: ViewElement, value: Avalonia.Layout.VerticalAlignment) : ViewElement =
            let attr = Attr.createProperty (ContentControl.VerticalContentAlignmentProperty, value)
            let attrInfo = AttrInfo.create(typeof<'view>, attr)
            { view with Attrs = attrInfo :: view.Attrs }
 
    type PanelBuilder<'view when 'view :> IControl>() =
        inherit ControlBuilder<'view>()
    
         [<CustomOperation("background")>]
         member __.Background (view: ViewElement, value: Avalonia.Media.IBrush) : ViewElement =
             let attr = Attr.createProperty (Panel.BackgroundProperty, value)
             let attrInfo = AttrInfo.create(typeof<'view>, attr)
             { view with Attrs = attrInfo :: view.Attrs }
    
    type StackPanelBuilder<'view when 'view :> IControl>() =
        inherit PanelBuilder<'view>()

         [<CustomOperation("spacing")>]
         member __.Spacing (view: ViewElement, value: double) : ViewElement =
             let attr = Attr.createProperty (StackPanel.SpacingProperty, value)
             let attrInfo = AttrInfo.create(typeof<'view>, attr)
             { view with Attrs = attrInfo :: view.Attrs }

        [<CustomOperation("orientation")>]
        member __.Orientation (view: ViewElement, value: Orientation) : ViewElement =
            let attr = Attr.createProperty (StackPanel.OrientationProperty, value)
            let attrInfo = AttrInfo.create(typeof<'view>, attr)
            { view with Attrs = attrInfo :: view.Attrs }

    type DockPanelBuilder<'view when 'view :> IControl>() =
        inherit PanelBuilder<'view>()

         [<CustomOperation("lastChildFill")>]
         member __.Spacing (view: ViewElement, value: bool) : ViewElement =
             let attr = Attr.createProperty (StackPanel.SpacingProperty, value)
             let attrInfo = AttrInfo.create(typeof<'view>, attr)
             { view with Attrs = attrInfo :: view.Attrs }

    type ControlBuilder<'view when 'view :> IControl> with

        [<CustomOperation("dockpanel_dock")>]
        member __.DockPanel_Dock (view: ViewElement, value: Dock) : ViewElement =
            let attr = Attr.createProperty (DockPanel.DockProperty, value)
            let attrInfo = AttrInfo.create(typeof<'view>, attr)
            { view with Attrs = attrInfo :: view.Attrs }

    type ButtonBuilder<'view when 'view :> Button>() =
        inherit ContentControlBuilder<Button>()

        [<CustomOperation("command")>]
        member __.Command (view: ViewElement, value: System.Windows.Input.ICommand) : ViewElement=
            let attr = Attr.createProperty (Button.CommandProperty, value)
            let attrInfo = AttrInfo.create(typeof<'view>, attr)
            { view with Attrs = attrInfo :: view.Attrs }

        [<CustomOperation("commandParameter")>]
        member __.Click (view: ViewElement, value: obj) : ViewElement=
            let attr = Attr.createProperty (Button.CommandParameterProperty, value)
            let attrInfo = AttrInfo.create(typeof<'view>, attr)
            { view with Attrs = attrInfo :: view.Attrs }

    type TextBlockBuilder<'view when 'view :> TextBlock>() =
        inherit ControlBuilder<TextBlock>()
            
        [<CustomOperation("text")>]
        member __.Text (view: ViewElement, value: string) : ViewElement=
            let attr = Attr.createProperty (TextBlock.TextProperty, value)
            let attrInfo = AttrInfo.create(typeof<'view>, attr)
            { view with Attrs = attrInfo :: view.Attrs }


    let button = ButtonBuilder<Button>()
    let textblock = TextBlockBuilder<TextBlock>()


