namespace Avalonia.FuncUI.DSL

open Avalonia.FuncUI.VirtualDom

[<AutoOpen>]
module Control =
    open Avalonia.Controls
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder

    let create (attrs: Attr<Control> list): View<Control> =
        ViewBuilder.Create<Control>(attrs)

    type Control with

        static member focusAdorner<'t, 'c when 't :> Control and 'c :> IControl>(value: ITemplate<'c>) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<ITemplate<'c>>(Control.FocusAdornerProperty, value, ValueNone)

        static member tag<'t when 't :> Control>(value: obj) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<obj>(Control.TagProperty, value, ValueNone)

        static member contextMenu<'t when 't :> Control>(menuView: View<ContextMenu> voption) : Attr<'t> =
            let view =
                match menuView with
                | ValueSome view -> ValueSome (view :> IView)
                | ValueNone -> ValueNone

            // TODO: think about exposing less generic View<'c>
            AttrBuilder<'t>.CreateContentSingle(Control.ContextMenuProperty, view)

        static member contextMenu<'t when 't :> Control>(menuView: View<ContextMenu>) : Attr<'t> =
            AttrBuilder<'t>.CreateContentSingle(Control.ContextMenuProperty, ValueSome (menuView :> IView))

        static member contextMenu<'t when 't :> Control>(menu: ContextMenu) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<ContextMenu>(Control.ContextMenuProperty, menu, ValueNone)

