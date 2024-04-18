namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module Control =
    open Avalonia.Controls
    open Avalonia.Controls.Primitives
    open Avalonia.Interactivity
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder

    let create (attrs: IAttr<Control> list): IView<Control> =
        ViewBuilder.Create<Control>(attrs)

    type Control with

        static member focusAdorner<'t, 'c when 't :> Control and 'c :> Control>(value: ITemplate<'c>) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<ITemplate<'c>>(Control.FocusAdornerProperty, value, ValueNone)

        static member tag<'t when 't :> Control>(value: obj) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<obj>(Control.TagProperty, value, ValueNone)

        static member contextMenu<'t when 't :> Control>(menuView: IView<ContextMenu> option) : IAttr<'t> =
            let view =
                match menuView with
                | Some view -> Some (view :> IView)
                | None -> None

            // TODO: think about exposing less generic IView<'c>
            AttrBuilder<'t>.CreateContentSingle(Control.ContextMenuProperty, view)

        static member contextMenu<'t when 't :> Control>(menuView: IView<ContextMenu>) : IAttr<'t> =
            AttrBuilder<'t>.CreateContentSingle(Control.ContextMenuProperty, Some (menuView :> IView))

        static member contextMenu<'t when 't :> Control>(menu: ContextMenu) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<ContextMenu>(Control.ContextMenuProperty, menu, ValueNone)

        static member contextFlyout<'t when 't :> Control>(flyoutView: IView<FlyoutBase> option) : IAttr<'t> =
            let view =
                match flyoutView with
                | Some view -> Some (view :> IView)
                | None -> None

            AttrBuilder<'t>.CreateContentSingle(Control.ContextFlyoutProperty, view)

        static member contextFlyout<'t when 't :> Control>(flyoutView: IView<FlyoutBase>) : IAttr<'t> =
            AttrBuilder<'t>.CreateContentSingle(Control.ContextFlyoutProperty, Some (flyoutView :> IView))

        static member contextFlyout<'t when 't :> Control>(flyout: FlyoutBase) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<FlyoutBase>(Control.ContextFlyoutProperty, flyout, ValueNone)

        static member onContextRequested<'t when 't :> Control>(func: ContextRequestedEventArgs -> unit, ?subPatchOptions) =
            AttrBuilder<'t>.CreateSubscription<ContextRequestedEventArgs>(Control.ContextRequestedEvent, func, ?subPatchOptions = subPatchOptions)

        static member onLoaded<'t when 't :> Control>(func: RoutedEventArgs -> unit, ?subPatchOptions) =
            AttrBuilder<'t>.CreateSubscription<RoutedEventArgs>(Control.LoadedEvent, func, ?subPatchOptions = subPatchOptions)

        static member onUnloaded<'t when 't :> Control>(func: RoutedEventArgs -> unit, ?subPatchOptions) =
            AttrBuilder<'t>.CreateSubscription<RoutedEventArgs>(Control.UnloadedEvent, func, ?subPatchOptions = subPatchOptions)

        static member onSizeChanged<'t when 't :> Control>(func: SizeChangedEventArgs -> unit, ?subPatchOptions) =
            AttrBuilder<'t>.CreateSubscription<SizeChangedEventArgs>(Control.SizeChangedEvent, func, ?subPatchOptions = subPatchOptions)
