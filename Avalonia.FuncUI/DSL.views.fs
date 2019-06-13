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

        static member stackpanel (attrs: TypedAttr<StackPanel> list): View =
            Views.create<StackPanel>(attrs)

        static member dockpanel (attrs: TypedAttr<DockPanel> list): View =
            Views.create<DockPanel>(attrs)

        static member scrollviewer (attrs: TypedAttr<ScrollViewer> list): View =
            Views.create<ScrollViewer>(attrs)
