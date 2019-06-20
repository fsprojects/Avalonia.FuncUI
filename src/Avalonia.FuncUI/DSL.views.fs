namespace Avalonia.FuncUI

open Avalonia.Controls
open System
open Types

[<AutoOpen>]
module DSL_Views =

    type Views with

        static member button (attrs: TypedAttr<Button> list): View =
            Views.create<Button>(attrs)

        static member repeatbutton (attrs: TypedAttr<RepeatButton> list): View =
            Views.create<RepeatButton>(attrs)

        static member textblock (attrs: TypedAttr<TextBlock> list): View =
            Views.create<TextBlock>(attrs)

        static member stackPanel (attrs: TypedAttr<StackPanel> list): View =
            Views.create<StackPanel>(attrs)

        static member dockpanel (attrs: TypedAttr<DockPanel> list): View =
            Views.create<DockPanel>(attrs)

        static member scrollviewer (attrs: TypedAttr<ScrollViewer> list): View =
            Views.create<ScrollViewer>(attrs)

        static member border (attrs: TypedAttr<Border> list): View =
            Views.create<Border>(attrs)

        static member tabControl (attrs: TypedAttr<TabControl> list): View =
            Views.create<TabControl>(attrs)

        static member tabItem (attrs: TypedAttr<TabItem> list): View =
            Views.create<TabItem>(attrs)

        static member textBox (attrs: TypedAttr<TextBox> list): View =
            Views.create<TextBox>(attrs)

        static member checkBox (attrs: TypedAttr<CheckBox> list): View =
            Views.create<CheckBox>(attrs)
