namespace Avalonia.FuncUI

open Avalonia.Controls
open System
open Types
open Avalonia.Controls.Shapes
open Avalonia.Controls.Presenters
open Avalonia.Controls.Primitives

[<AutoOpen>]
module DSL_Views =

    type Views with

        static member autoCompleteBox (attrs: TypedAttr<AutoCompleteBox> list): View =
            Views.create<AutoCompleteBox>(attrs)

        static member border (attrs: TypedAttr<Border> list): View =
            Views.create<Border>(attrs)

        static member button (attrs: TypedAttr<Button> list): View =
            Views.create<Button>(attrs)

        static member buttonSpinner (attrs: TypedAttr<ButtonSpinner> list): View =
            Views.create<ButtonSpinner>(attrs)

        static member calendar (attrs: TypedAttr<Calendar> list): View =
            Views.create<Calendar>(attrs)

        static member datePicker (attrs: TypedAttr<DatePicker> list): View =
            Views.create<DatePicker>(attrs)

        static member canvas (attrs: TypedAttr<Canvas> list): View =
            Views.create<Canvas>(attrs)

        static member carousel (attrs: TypedAttr<Carousel> list): View =
            Views.create<Carousel>(attrs)

        static member checkBox (attrs: TypedAttr<CheckBox> list): View =
            Views.create<CheckBox>(attrs)

        static member comboBox (attrs: TypedAttr<ComboBox> list): View =
            Views.create<ComboBox>(attrs)

        static member comboBoxItem (attrs: TypedAttr<ComboBoxItem> list): View =
            Views.create<ComboBoxItem>(attrs)

        static member contentControl (attrs: TypedAttr<ContentControl> list): View =
            Views.create<ContentControl>(attrs)

        static member contextMenu (attrs: TypedAttr<ContextMenu> list): View =
            Views.create<ContextMenu>(attrs)

        static member dataValidationErrors (attrs: TypedAttr<DataValidationErrors> list): View =
            Views.create<DataValidationErrors>(attrs)

        static member decorator (attrs: TypedAttr<Decorator> list): View =
            Views.create<Decorator>(attrs)

        static member dockpanel (attrs: TypedAttr<DockPanel> list): View =
            Views.create<DockPanel>(attrs)

        static member drawingPresenter (attrs: TypedAttr<DrawingPresenter> list): View =
            Views.create<DrawingPresenter>(attrs)

        static member expander (attrs: TypedAttr<Expander> list): View =
            Views.create<Expander>(attrs)

        static member grid (attrs: TypedAttr<Grid> list): View =
            Views.create<Grid>(attrs)

        static member gridSplitter (attrs: TypedAttr<GridSplitter> list): View =
            Views.create<GridSplitter>(attrs)

        static member image (attrs: TypedAttr<Image> list): View =
            Views.create<Image>(attrs)

        static member itemsControl (attrs: TypedAttr<ItemsControl> list): View =
            Views.create<ItemsControl>(attrs)

        static member layoutTransformControl (attrs: TypedAttr<LayoutTransformControl> list): View =
            Views.create<LayoutTransformControl>(attrs)

        static member listBox (attrs: TypedAttr<ListBox> list): View =
            Views.create<ListBox>(attrs)

        static member listBoxItem (attrs: TypedAttr<ListBoxItem> list): View =
            Views.create<ListBoxItem>(attrs)

        static member menu (attrs: TypedAttr<Menu> list): View =
            Views.create<Menu>(attrs)

        static member menuItem (attrs: TypedAttr<MenuItem> list): View =
            Views.create<MenuItem>(attrs)

        static member numericUpDown (attrs: TypedAttr<NumericUpDown> list): View =
            Views.create<NumericUpDown>(attrs)

        static member progressBar (attrs: TypedAttr<ProgressBar> list): View =
            Views.create<ProgressBar>(attrs)

        static member radioButton (attrs: TypedAttr<RadioButton> list): View =
            Views.create<RadioButton>(attrs)

        static member repeatButton (attrs: TypedAttr<RepeatButton> list): View =
            Views.create<RepeatButton>(attrs)

        static member scrollViewer (attrs: TypedAttr<ScrollViewer> list): View =
            Views.create<ScrollViewer>(attrs)

        static member separator (attrs: TypedAttr<Separator> list): View =
            Views.create<Separator>(attrs)

        static member slider (attrs: TypedAttr<Slider> list): View =
            Views.create<Slider>(attrs)

        static member spinner (attrs: TypedAttr<Spinner> list): View =
            Views.create<Spinner>(attrs)

        static member stackPanel (attrs: TypedAttr<StackPanel> list): View =
            Views.create<StackPanel>(attrs)

        static member tabControl (attrs: TypedAttr<TabControl> list): View =
            Views.create<TabControl>(attrs)

        static member tabItem (attrs: TypedAttr<TabItem> list): View =
            Views.create<TabItem>(attrs)

        static member textBlock (attrs: TypedAttr<TextBlock> list): View =
            Views.create<TextBlock>(attrs)

        static member textBox (attrs: TypedAttr<TextBox> list): View =
            Views.create<TextBox>(attrs)

        static member toolTip (attrs: TypedAttr<ToolTip> list): View =
            Views.create<ToolTip>(attrs)

        static member treeView (attrs: TypedAttr<TreeView> list): View =
            Views.create<TreeView>(attrs)

        static member treeViewItem (attrs: TypedAttr<TreeViewItem> list): View =
            Views.create<TreeViewItem>(attrs)

        static member viewbox (attrs: TypedAttr<Viewbox> list): View =
            Views.create<Viewbox>(attrs)

        static member virtualizingStackPanel (attrs: TypedAttr<VirtualizingStackPanel> list): View =
            Views.create<VirtualizingStackPanel>(attrs)

        static member wrapPanel (attrs: TypedAttr<WrapPanel> list): View =
            Views.create<WrapPanel>(attrs)

        static member ellipse (attrs: TypedAttr<Ellipse> list): View =
            Views.create<Ellipse>(attrs)

        static member line (attrs: TypedAttr<Line> list): View =
            Views.create<Line>(attrs)

        static member path (attrs: TypedAttr<Path> list): View =
            Views.create<Path>(attrs)

        static member polygon (attrs: TypedAttr<Polygon> list): View =
            Views.create<Polygon>(attrs)

        static member polyline (attrs: TypedAttr<Polyline> list): View =
            Views.create<Polyline>(attrs)

        static member rectangle (attrs: TypedAttr<Rectangle> list): View =
            Views.create<Rectangle>(attrs)

        static member shape (attrs: TypedAttr<Shape> list): View =
            Views.create<Shape>(attrs)

        static member carouselPresenter (attrs: TypedAttr<CarouselPresenter> list): View =
            Views.create<CarouselPresenter>(attrs)

        static member contentPresenter (attrs: TypedAttr<ContentPresenter> list): View =
            Views.create<ContentPresenter>(attrs)

        static member itemsPresenter (attrs: TypedAttr<ItemsPresenter> list): View =
            Views.create<ItemsPresenter>(attrs)

        static member textPresenter (attrs: TypedAttr<TextPresenter> list): View =
            Views.create<TextPresenter>(attrs)

        static member accessText (attrs: TypedAttr<AccessText> list): View =
            Views.create<AccessText>(attrs)

        static member adornerDecorator (attrs: TypedAttr<AdornerDecorator> list): View =
            Views.create<AdornerDecorator>(attrs)

        static member adornerLayer (attrs: TypedAttr<AdornerLayer> list): View =
            Views.create<AdornerLayer>(attrs)
    
        static member headeredContentControl (attrs: TypedAttr<HeaderedContentControl> list): View =
            Views.create<HeaderedContentControl>(attrs)

        static member headeredItemsControl (attrs: TypedAttr<HeaderedItemsControl> list): View =
            Views.create<HeaderedItemsControl>(attrs)

        static member headeredSelectiongItemsControl (attrs: TypedAttr<HeaderedSelectingItemsControl> list): View =
            Views.create<HeaderedSelectingItemsControl>(attrs)

        static member popup (attrs: TypedAttr<Popup> list): View =
            Views.create<Popup>(attrs)

        static member selectingItemsControl (attrs: TypedAttr<SelectingItemsControl> list): View =
            Views.create<SelectingItemsControl>(attrs)

        static member tabStrip (attrs: TypedAttr<TabStrip> list): View =
            Views.create<TabStrip>(attrs)

        static member tabStripItem (attrs: TypedAttr<TabStripItem> list): View =
            Views.create<TabStripItem>(attrs)

        static member thumb (attrs: TypedAttr<Thumb> list): View =
            Views.create<Thumb>(attrs)

        static member toggleButton (attrs: TypedAttr<ToggleButton> list): View =
            Views.create<ToggleButton>(attrs)

        static member track (attrs: TypedAttr<Track> list): View =
            Views.create<Track>(attrs)

        static member uniformGrid (attrs: TypedAttr<UniformGrid> list): View =
            Views.create<UniformGrid>(attrs)
