namespace Avalonia.FuncUI.Builders

open Avalonia.Controls

[<AutoOpen>]
module Builders =
    open Avalonia.FuncUI.Core
    open Avalonia

    type AnimatableBuilder<'view when 'view :> IControl>() =

        [<CustomOperation("clock")>]
        member __.Clock (view: ViewElement<'view>, value: Avalonia.Animation.IClock) : ViewElement<'view> =
            let attr = Attr.create<'view, Avalonia.Animation.IClock>(value, fun (c, v) ->
                c.SetValue(Avalonia.Animation.Animatable.ClockProperty, v)
            )
            { view with attrs = attr :: view.attrs }

        [<CustomOperation("transitions")>]
        member __.Transitions (view: ViewElement<'view>, value: Avalonia.Animation.Transitions) : ViewElement<'view> =
            let attr = Attr.create<'view, Avalonia.Animation.Transitions>(value, fun (c, v) ->
                c.SetValue(Avalonia.Animation.Animatable.TransitionsProperty, v)
            )
            { view with attrs = attr :: view.attrs }

    type StyledElementBuilder<'view when 'view :> IControl>() =
        inherit AnimatableBuilder<'view>()

        [<CustomOperation("dataContext")>]
        member __.DataContext (view: ViewElement<'view>, value: obj) : ViewElement<'view> =
            let attr = Attr.create<'view, obj>(value, fun (c, v) ->
                c.SetValue(StyledElement.DataContextProperty, v)
            )
            { view with attrs = attr :: view.attrs }

        [<CustomOperation("name")>]
        member __.Name (view: ViewElement<'view>, value: string) : ViewElement<'view> =
            let attr = Attr.create<'view, string>(value, fun (c, v) ->
                c.SetValue(StyledElement.NameProperty, v)
            )
            { view with attrs = attr :: view.attrs }

        [<CustomOperation("parent")>]
        member __.Parent (view: ViewElement<'view>, value: IStyledElement) : ViewElement<'view> =
            let attr = Attr.create<'view, IStyledElement>(value, fun (c, v) ->
                c.SetValue(StyledElement.ParentProperty, v)
            )
            { view with attrs = attr :: view.attrs }

        [<CustomOperation("templatedParent")>]
        member __.TemplatedParent (view: ViewElement<'view>, value: Avalonia.Styling.ITemplatedControl) : ViewElement<'view> =
            let attr = Attr.create<'view, Avalonia.Styling.ITemplatedControl>(value, fun (c, v) ->
                c.SetValue(StyledElement.TemplatedParentProperty, v)
            )
            { view with attrs = attr :: view.attrs }   

    type VisualBuilder<'view when 'view :> IControl>() =
        inherit StyledElementBuilder<'view>()

        [<CustomOperation("bounds")>]
        member __.Bounds (view: ViewElement<'view>, value: Rect) : ViewElement<'view> =
            let attr = Attr.create<'view, Rect>(value, fun (c, v) ->
                c.SetValue(Visual.BoundsProperty, v)
            )
            { view with attrs = attr :: view.attrs }

        [<CustomOperation("transformedBounds")>]
        member __.TransformedBounds (view: ViewElement<'view>, value: Avalonia.VisualTree.TransformedBounds option) : ViewElement<'view> =
            let attr = Attr.create<'view, Avalonia.VisualTree.TransformedBounds option>(value, fun (c, v) ->
                match v with
                | Some v -> c.SetValue(Visual.TransformedBoundsProperty, System.Nullable(v))
                | None -> c.SetValue(Visual.TransformedBoundsProperty, System.Nullable())
            )
            { view with attrs = attr :: view.attrs }

        [<CustomOperation("clipToBounds")>]
        member __.ClipToBounds (view: ViewElement<'view>, value: bool) : ViewElement<'view> =
            let attr = Attr.create<'view, bool>(value, fun (c, v) ->
                c.SetValue(Visual.ClipToBoundsProperty, v)
            )
            { view with attrs = attr :: view.attrs }

        [<CustomOperation("clip")>]
        member __.Clip (view: ViewElement<'view>, value: Avalonia.Media.Geometry) : ViewElement<'view> =
            let attr = Attr.create<'view, Avalonia.Media.Geometry>(value, fun (c, v) ->
                c.SetValue(Visual.ClipProperty, v)
            )
            { view with attrs = attr :: view.attrs }

        [<CustomOperation("isVisible")>]
        member __.IsVisible (view: ViewElement<'view>, value: bool) : ViewElement<'view> =
            let attr = Attr.create<'view, bool>(value, fun (c, v) ->
                c.SetValue(Visual.IsVisibleProperty, v)
            )
            { view with attrs = attr :: view.attrs }

        [<CustomOperation("opacity")>]
        member __.Opacity (view: ViewElement<'view>, value: double) : ViewElement<'view> =
            let attr = Attr.create<'view, double>(value, fun (c, v) ->
                c.SetValue(Visual.OpacityProperty, v)
            )
            { view with attrs = attr :: view.attrs }

        [<CustomOperation("opacityMask")>]
        member __.OpacityMast (view: ViewElement<'view>, value: Avalonia.Media.IBrush) : ViewElement<'view> =
            let attr = Attr.create<'view, Avalonia.Media.IBrush>(value, fun (c, v) ->
                c.SetValue(Visual.OpacityMaskProperty, v)
            )
            { view with attrs = attr :: view.attrs }

        [<CustomOperation("renderTransform")>]
        member __.RenderTransform (view: ViewElement<'view>, value: Avalonia.Media.Transform) : ViewElement<'view> =
            let attr = Attr.create<'view, Avalonia.Media.Transform>(value, fun (c, v) ->
                c.SetValue(Visual.RenderTransformProperty, v)
            )
            { view with attrs = attr :: view.attrs }

        [<CustomOperation("renderTransformOrigin")>]
        member __.RenderTransformOrigin (view: ViewElement<'view>, value: RelativePoint) : ViewElement<'view> =
            let attr = Attr.create<'view, RelativePoint>(value, fun (c, v) ->
                c.SetValue(Visual.RenderTransformOriginProperty, v)
            )
            { view with attrs = attr :: view.attrs }

        [<CustomOperation("visualParent")>]
        member __.VisualParent (view: ViewElement<'view>, value: Avalonia.VisualTree.IVisual) : ViewElement<'view> =
            let attr = Attr.create<'view, Avalonia.VisualTree.IVisual>(value, fun (c, v) ->
                c.SetValue(Visual.VisualParentProperty, v)
            )
            { view with attrs = attr :: view.attrs }

        [<CustomOperation("zIndex")>]
        member __.ZIndex (view: ViewElement<'view>, value: int) : ViewElement<'view> =
            let attr = Attr.create<'view, int>(value, fun (c, v) ->
                c.SetValue(Visual.ZIndexProperty, v)
            )
            { view with attrs = attr :: view.attrs }

    type LayoutableBuilder<'view when 'view :> IControl>() =
        inherit VisualBuilder<'view>()

        [<CustomOperation("desiredSize")>]
        member __.DesiredSize (view: ViewElement<'view>, value: Size) : ViewElement<'view> =
            let attr = Attr.create<'view, Size>(value, fun (c, v) ->
                c.SetValue(Avalonia.Layout.Layoutable.DesiredSizeProperty, v)
            )
            { view with attrs = attr :: view.attrs }

        [<CustomOperation("width")>]
        member __.Width (view: ViewElement<'view>, value: double) : ViewElement<'view> =
            let attr = Attr.create<'view, double>(value, fun (c, v) ->
                c.SetValue(Avalonia.Layout.Layoutable.WidthProperty, v)
            )
            { view with attrs = attr :: view.attrs }

        [<CustomOperation("height")>]
        member __.Height (view: ViewElement<'view>, value: double) : ViewElement<'view> =
            let attr = Attr.create<'view, double>(value, fun (c, v) ->
                c.SetValue(Avalonia.Layout.Layoutable.HeightProperty, v)
            )
            { view with attrs = attr :: view.attrs }

        [<CustomOperation("minWidth")>]
        member __.MinWidth (view: ViewElement<'view>, value: double) : ViewElement<'view> =
            let attr = Attr.create<'view, double>(value, fun (c, v) ->
                c.SetValue(Avalonia.Layout.Layoutable.MinWidthProperty, v)
            )
            { view with attrs = attr :: view.attrs }

        [<CustomOperation("minHeight")>]
        member __.MinHeight (view: ViewElement<'view>, value: double) : ViewElement<'view> =
            let attr = Attr.create<'view, double>(value, fun (c, v) ->
                c.SetValue(Avalonia.Layout.Layoutable.MinHeightProperty, v)
            )
            { view with attrs = attr :: view.attrs }

        [<CustomOperation("maxWidth")>]
        member __.MaxWidth (view: ViewElement<'view>, value: double) : ViewElement<'view> =
            let attr = Attr.create<'view, double>(value, fun (c, v) ->
                c.SetValue(Avalonia.Layout.Layoutable.MaxWidthProperty, v)
            )
            { view with attrs = attr :: view.attrs }

        [<CustomOperation("maxHeight")>]
        member __.MaxHeight (view: ViewElement<'view>, value: double) : ViewElement<'view> =
            let attr = Attr.create<'view, double>(value, fun (c, v) ->
                c.SetValue(Avalonia.Layout.Layoutable.MaxHeightProperty, v)
            )
            { view with attrs = attr :: view.attrs }

        [<CustomOperation("margin")>]
        member __.Margin (view: ViewElement<'view>, value: Thickness) : ViewElement<'view> =
            let attr = Attr.create<'view, Thickness>(value, fun (c, v) ->
                c.SetValue(Avalonia.Layout.Layoutable.MarginProperty, v)
            )
            { view with attrs = attr :: view.attrs }

        [<CustomOperation("horizontalAlignment")>]
        member __.HorizontalAlignment (view: ViewElement<'view>, value: Avalonia.Layout.HorizontalAlignment) : ViewElement<'view> =
            let attr = Attr.create<'view, Avalonia.Layout.HorizontalAlignment>(value, fun (c, v) ->
                c.SetValue(Avalonia.Layout.Layoutable.HorizontalAlignmentProperty, v)
            )
            { view with attrs = attr :: view.attrs }

        [<CustomOperation("verticalAlignment")>]
        member __.VerticalAlignment (view: ViewElement<'view>, value: Avalonia.Layout.VerticalAlignment) : ViewElement<'view> =
            let attr = Attr.create<'view, Avalonia.Layout.VerticalAlignment>(value, fun (c, v) ->
                c.SetValue(Avalonia.Layout.Layoutable.VerticalAlignmentProperty, v)
            )
            { view with attrs = attr :: view.attrs }

        [<CustomOperation("useLayoutRounding")>]
        member __.UseLayoutRounding (view: ViewElement<'view>, value: bool) : ViewElement<'view> =
            let attr = Attr.create<'view, bool>(value, fun (c, v) ->
                c.SetValue(Avalonia.Layout.Layoutable.UseLayoutRoundingProperty, v)
            )
            { view with attrs = attr :: view.attrs }

    type InputElementBuilder<'view when 'view :> IControl>() =
        inherit LayoutableBuilder<'view>()

        [<CustomOperation("focusable")>]
        member __.Focusable (view: ViewElement<'view>, value: bool) : ViewElement<'view> =
            let attr = Attr.create<'view, bool>(value, fun (c, v) ->
                c.SetValue(Avalonia.Input.InputElement.FocusableProperty, v)
            )
            { view with attrs = attr :: view.attrs }

        [<CustomOperation("isEnabled")>]
        member __.IsEnabled (view: ViewElement<'view>, value: bool) : ViewElement<'view> =
            let attr = Attr.create<'view, bool>(value, fun (c, v) ->
                c.SetValue(Avalonia.Input.InputElement.IsEnabledProperty, v)
            )
            { view with attrs = attr :: view.attrs }

        [<CustomOperation("isEnabledCore")>]
        member __.IsEnabledCore (view: ViewElement<'view>, value: bool) : ViewElement<'view> =
            let attr = Attr.create<'view, bool>(value, fun (c, v) ->
                c.SetValue(Avalonia.Input.InputElement.IsEnabledCoreProperty, v)
            )
            { view with attrs = attr :: view.attrs }

        [<CustomOperation("cursor")>]
        member __.Cursor (view: ViewElement<'view>, value: Avalonia.Input.Cursor) : ViewElement<'view> =
            let attr = Attr.create<'view, Avalonia.Input.Cursor>(value, fun (c, v) ->
                c.SetValue(Avalonia.Input.InputElement.CursorProperty, v)
            )
            { view with attrs = attr :: view.attrs }

        [<CustomOperation("isFocused")>]
        member __.IsFocused (view: ViewElement<'view>, value: bool) : ViewElement<'view> =
            let attr = Attr.create<'view, bool>(value, fun (c, v) ->
                c.SetValue(Avalonia.Input.InputElement.IsFocusedProperty, v)
            )
            { view with attrs = attr :: view.attrs }

        [<CustomOperation("isHitTestVisible")>]
        member __.IsHitTestVisible (view: ViewElement<'view>, value: bool) : ViewElement<'view> =
            let attr = Attr.create<'view, bool>(value, fun (c, v) ->
                c.SetValue(Avalonia.Input.InputElement.IsHitTestVisibleProperty, v)
            )
            { view with attrs = attr :: view.attrs }

        [<CustomOperation("isPointerOver")>]
        member __.IsPointerOverProperty (view: ViewElement<'view>, value: bool) : ViewElement<'view> =
            let attr = Attr.create<'view, bool>(value, fun (c, v) ->
                c.SetValue(Avalonia.Input.InputElement.IsPointerOverProperty, v)
            )
            { view with attrs = attr :: view.attrs }

    type ControlBuilder<'view when 'view :> IControl>() =
        inherit InputElementBuilder<'view>()
    
        [<CustomOperation("focusAdorner")>]
        member __.FocusAdorner (view: ViewElement<'view>, value: ITemplate<IControl>) : ViewElement<'view> =
            let attr = Attr.create<'view, ITemplate<IControl>>(value, fun (c, v) ->
                c.SetValue(Control.FocusAdornerProperty, v)
            )
            { view with attrs = attr :: view.attrs }

        [<CustomOperation("tag")>]
        member __.Tag (view: ViewElement<'view>, value: obj) : ViewElement<'view> =
            let attr = Attr.create<'view, obj>(value, fun (c, v) ->
                c.SetValue(Control.TagProperty, v)
            )
            { view with attrs = attr :: view.attrs }

        [<CustomOperation("contextMenu")>]
        member __.ContextMenu (view: ViewElement<'view>, value: ContextMenu) : ViewElement<'view> =
            let attr = Attr.create<'view, ContextMenu>(value, fun (c, v) ->
                c.SetValue(Control.ContextMenuProperty, v)
            )
            { view with attrs = attr :: view.attrs }

    type TemplatedControlBuilder<'view when 'view :> IControl>() =
        inherit ControlBuilder<'view>()
    
        [<CustomOperation("background")>]
        member __.Background (view: ViewElement<'view>, value: Avalonia.Media.IBrush) : ViewElement<'view> =
            let attr = Attr.create<'view, Avalonia.Media.IBrush>(value, fun (c, v) ->
                c.SetValue(Avalonia.Controls.Primitives.TemplatedControl.BackgroundProperty, v)
            )
            { view with attrs = attr :: view.attrs }

        [<CustomOperation("borderBrush")>]
        member __.BorderBrush (view: ViewElement<'view>, value: Avalonia.Media.IBrush) : ViewElement<'view> =
            let attr = Attr.create<'view, Avalonia.Media.IBrush>(value, fun (c, v) ->
                c.SetValue(Avalonia.Controls.Primitives.TemplatedControl.BorderBrushProperty, v)
            )
            { view with attrs = attr :: view.attrs }

        [<CustomOperation("borderThickness")>]
        member __.BorderThickness (view: ViewElement<'view>, value: Thickness) : ViewElement<'view> =
            let attr = Attr.create<'view, Thickness>(value, fun (c, v) ->
                c.SetValue(Avalonia.Controls.Primitives.TemplatedControl.BorderThicknessProperty, v)
            )
            { view with attrs = attr :: view.attrs }

        [<CustomOperation("fontFamily")>]
        member __.FontFamily (view: ViewElement<'view>, value: Thickness) : ViewElement<'view> =
            let attr = Attr.create<'view, Thickness>(value, fun (c, v) ->
                c.SetValue(Avalonia.Controls.Primitives.TemplatedControl.FontFamilyProperty, v)
            )
            { view with attrs = attr :: view.attrs }

        [<CustomOperation("fontSize")>]
        member __.FontSize (view: ViewElement<'view>, value: double) : ViewElement<'view> =
            let attr = Attr.create<'view, double>(value, fun (c, v) ->
                c.SetValue(Avalonia.Controls.Primitives.TemplatedControl.FontSizeProperty, v)
            )
            { view with attrs = attr :: view.attrs }

        [<CustomOperation("fontStyle")>]
        member __.FontStyle (view: ViewElement<'view>, value: Avalonia.Media.FontStyle) : ViewElement<'view> =
            let attr = Attr.create<'view, Avalonia.Media.FontStyle>(value, fun (c, v) ->
                c.SetValue(Avalonia.Controls.Primitives.TemplatedControl.FontStyleProperty, v)
            )
            { view with attrs = attr :: view.attrs }

        [<CustomOperation("fontWeight")>]
        member __.FontWeight (view: ViewElement<'view>, value: Avalonia.Media.FontWeight) : ViewElement<'view> =
            let attr = Attr.create<'view, Avalonia.Media.FontWeight>(value, fun (c, v) ->
                c.SetValue(Avalonia.Controls.Primitives.TemplatedControl.FontWeightProperty, v)
            )
            { view with attrs = attr :: view.attrs }

        [<CustomOperation("foreground")>]
        member __.Foreground (view: ViewElement<'view>, value: Avalonia.Media.IBrush) : ViewElement<'view> =
            let attr = Attr.create<'view, Avalonia.Media.IBrush>(value, fun (c, v) ->
                c.SetValue(Avalonia.Controls.Primitives.TemplatedControl.ForegroundProperty, v)
            )
            { view with attrs = attr :: view.attrs }

        [<CustomOperation("padding")>]
        member __.Padding (view: ViewElement<'view>, value: Thickness) : ViewElement<'view> =
            let attr = Attr.create<'view, Thickness>(value, fun (c, v) ->
                c.SetValue(Avalonia.Controls.Primitives.TemplatedControl.PaddingProperty, v)
            )
            { view with attrs = attr :: view.attrs }

        [<CustomOperation("template")>]
        member __.Template (view: ViewElement<'view>, value: Avalonia.Controls.Templates.IControlTemplate) : ViewElement<'view> =
            let attr = Attr.create<'view, Avalonia.Controls.Templates.IControlTemplate>(value, fun (c, v) ->
                c.SetValue(Avalonia.Controls.Primitives.TemplatedControl.TemplateProperty, v)
            )
            { view with attrs = attr :: view.attrs }

    type ButtonBuilder<'view when 'view :> Button>() =
        inherit ControlBuilder<Button>()

        member __.Yield (item: 'a) =
            {
                create = (fun () -> new Button());
                update = (fun (view, attrs) -> attrs |> List.iter (fun attr -> attr.Apply view));
                attrs = []
            }     

        [<CustomOperation("command")>]
        member __.Command (view: ViewElement<'view>, value: unit -> unit) : ViewElement<'view>=
            let attr = Attr.create<'view, unit -> unit>(value, fun (c, v) -> c.SetValue(Button.CommandProperty, v))
            { view with attrs = attr :: view.attrs }

    let button = ButtonBuilder<Button>()

    type TextBlockBuilder<'view when 'view :> TextBlock>() =
        inherit ControlBuilder<TextBlock>()

        member __.Yield (item: 'a) =
            {
                create = (fun () -> new TextBlock());
                update = (fun (view, attrs) -> attrs |> List.iter (fun attr -> attr.Apply view));
                attrs = []
            }
            
        [<CustomOperation("text")>]
        member __.Text (view: ViewElement<'view>, value: string) : ViewElement<'view>=
            let attr = Attr.create<'view, string>(value, fun (c, v) -> c.SetValue(TextBlock.TextProperty, v))
            { view with attrs = attr :: view.attrs }

    let textblock = TextBlockBuilder<TextBlock>()
