namespace Avalonia.FuncUI.DSL

open Avalonia
open Avalonia.Controls
open Avalonia.Controls.Diagnostics
open Avalonia.Controls.Primitives
open Avalonia.Controls.Primitives.PopupPositioning
open Avalonia.Controls.Templates
open Avalonia.Input
open Avalonia.Styling

[<AutoOpen>]
module Flyout =
    open System
    open System.Threading
    open System.ComponentModel

    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder

    let create (attrs: IAttr<Flyout> list): IView<Flyout> =
        ViewBuilder.Create<Flyout>(attrs)
    
    type FlyoutBase with
        static member onOpened<'t when 't :> FlyoutBase>(func: 't -> unit, ?subPatchOptions) : IAttr<'t> =
            let name = nameof Unchecked.defaultof<'t>.Opened
            let factory: AvaloniaObject * ('t -> unit) * CancellationToken -> unit = 
                (fun (control, func, token) ->
                    let control = control :?> 't
                    let hander = EventHandler(fun s _ -> s :?> 't |> func)
                    let event = control.Opened

                    event.AddHandler(hander)
                    token.Register(fun () -> event.RemoveHandler(hander)) |> ignore)
            
            AttrBuilder<'t>.CreateSubscription(name, factory, func, ?subPatchOptions = subPatchOptions)
        
        static member onClosed<'t when 't :> FlyoutBase>(func: 't -> unit, ?subPatchOptions) : IAttr<'t> =
            let name = nameof Unchecked.defaultof<'t>.Closed
            let factory: AvaloniaObject * ('t -> unit) * CancellationToken -> unit = 
                (fun (control, func, token) ->
                    let control = control :?> 't
                    let hander = EventHandler(fun s _ -> s :?> 't |> func)
                    let event = control.Closed

                    event.AddHandler(hander)
                    token.Register(fun () -> event.RemoveHandler(hander)) |> ignore)
            
            AttrBuilder<'t>.CreateSubscription(name, factory, func, ?subPatchOptions = subPatchOptions)

    type Control with
        static member attachedFlyout<'t when 't :> Control>(value: FlyoutBase) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<FlyoutBase>(FlyoutBase.AttachedFlyoutProperty, value, ValueNone)

    type PopupFlyoutBase with

        /// <summary>
        /// A value indicating how the flyout is positioned.
        /// </summary>
        static member placement<'t when 't :> PopupFlyoutBase>(value: PlacementMode) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<PlacementMode>(PopupFlyoutBase.PlacementProperty, value, ValueNone)

        static member horizontalOffset<'t when 't :> PopupFlyoutBase>(value: double) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<double>(PopupFlyoutBase.HorizontalOffsetProperty, value, ValueNone)

        static member verticalOffset<'t when 't :> PopupFlyoutBase>(value: double) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<double>(PopupFlyoutBase.VerticalOffsetProperty, value, ValueNone)

        static member placementAnchor<'t when 't :> PopupFlyoutBase>(value: PopupAnchor) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<PopupAnchor>(PopupFlyoutBase.PlacementAnchorProperty, value, ValueNone)

        static member placementGravity<'t when 't :> PopupFlyoutBase>(value: PopupGravity) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<PopupGravity>(PopupFlyoutBase.PlacementGravityProperty, value, ValueNone)

        /// <summary>
        /// A value indicating flyout show mode.
        /// </summary>
        static member showMode<'t when 't :> PopupFlyoutBase>(value: FlyoutShowMode) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<FlyoutShowMode>(PopupFlyoutBase.ShowModeProperty, value, ValueNone)

        static member overlayInputPassThroughElement<'t when 't :> PopupFlyoutBase>(value: IInputElement) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<IInputElement>(PopupFlyoutBase.OverlayInputPassThroughElementProperty, value, ValueNone)
        
        static member onPopupHostChanged<'t when 't :> PopupFlyoutBase>(func: IPopupHost voption -> unit, ?subPatchOptions) : IAttr<'t> =
            let name = (nameof Unchecked.defaultof<IPopupHostProvider>.add_PopupHostChanged).Substring(4)
            let factory: AvaloniaObject * (IPopupHost voption -> unit) * CancellationToken -> unit = 
                (fun (control, func, token) ->
                    let control = (control :?> 't) :> IPopupHostProvider
                    let hander (host:IPopupHost) =
                        match host with
                        | null -> func ValueNone
                        | host -> func (ValueSome host)

                    control.add_PopupHostChanged hander
                    token.Register(fun () -> control.remove_PopupHostChanged hander) |> ignore)
            
            AttrBuilder<'t>.CreateSubscription(name, factory, func, ?subPatchOptions = subPatchOptions)

        static member onOpening<'t when 't :> PopupFlyoutBase>(func: 't -> unit, ?subPatchOptions) : IAttr<'t> =
            let name = nameof Unchecked.defaultof<'t>.Opening
            let factory: AvaloniaObject * ('t -> unit) * CancellationToken -> unit = 
                (fun (control, func, token) ->
                    let control = control :?> 't
                    let hander = EventHandler(fun s _ -> s :?> 't |> func)
                    let event = control.Opening

                    event.AddHandler(hander)
                    token.Register(fun () -> event.RemoveHandler(hander)) |> ignore)
            
            AttrBuilder<'t>.CreateSubscription(name, factory, func, ?subPatchOptions = subPatchOptions)

        static member onClosing<'t when 't :> PopupFlyoutBase>(func: 't * CancelEventArgs -> unit, ?subPatchOptions) : IAttr<'t> =
            let name = nameof Unchecked.defaultof<'t>.Closing
            let factory: AvaloniaObject * ('t * CancelEventArgs -> unit) * CancellationToken -> unit = 
                (fun (control, func, token) ->
                    let control = control :?> 't
                    let hander = EventHandler<_>(fun s e -> func(s :?> 't, e))
                    let event = control.Closing

                    event.AddHandler(hander)
                    token.Register(fun () -> event.RemoveHandler(hander)) |> ignore)
            
            AttrBuilder<'t>.CreateSubscription(name, factory, func, ?subPatchOptions = subPatchOptions)

    type Flyout with
        static member content<'t when 't :> Flyout>(value: IView option) : IAttr<'t> =
            AttrBuilder<'t>.CreateContentSingle(Flyout.ContentProperty, value)

        static member content<'t when 't :> Flyout>(value: IView) : IAttr<'t> =
            value
            |> Some
            |> Flyout.content

        /// <summary>
        /// A value indicating flyout style classes to apply. See https://docs.avaloniaui.net/docs/controls/flyouts#styling-flyouts
        /// </summary>
        static member flyoutPresenterClasses<'t when 't :> Flyout>(value: Classes) : IAttr<'t> =
            let getter : ('t -> Classes) = (fun control -> control.FlyoutPresenterClasses)
            let setter : ('t * Classes -> unit) = (fun (control, value) ->
                ClassesInternals.patchStandardClasses control.FlyoutPresenterClasses value)
            
            let compare = ClassesInternals.compareClasses<Classes>

            let factory = (fun () -> Classes())
            
            AttrBuilder<'t>.CreateProperty<Classes>("FlyoutPresenterClasses", value, ValueSome getter, ValueSome setter, ValueSome compare, factory)

        /// <summary>
        /// A value indicating flyout style classes to apply. See https://docs.avaloniaui.net/docs/controls/flyouts#styling-flyouts
        /// </summary>
        static member flyoutPresenterClasses<'t when 't :> Flyout>(classes: string list) : IAttr<'t> =
            let getter : ('t -> string list) = (fun control -> Seq.toList control.FlyoutPresenterClasses)
            let setter : ('t * string list -> unit) = (fun (control, value) ->
                ClassesInternals.patchStandardClasses control.FlyoutPresenterClasses value)
            
            let compare = ClassesInternals.compareClasses<string list>
            
            AttrBuilder<'t>.CreateProperty<string list>("FlyoutPresenterClasses", classes, ValueSome getter, ValueSome setter, ValueSome compare)

        static member flyoutPresenterTheme<'t when 't :> Flyout>(theme: ControlTheme) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<ControlTheme>(Flyout.FlyoutPresenterThemeProperty, theme, ValueNone)

        static member contentTemplate<'t when 't :> Flyout>(value : IDataTemplate): IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<IDataTemplate>(Flyout.ContentTemplateProperty, value, ValueNone)

[<AutoOpen>]
module MenuFlyout =
    open System.Collections

    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder

    let create (attrs: IAttr<MenuFlyout> list): IView<MenuFlyout> =
        ViewBuilder.Create<MenuFlyout>(attrs)

    type MenuFlyout with

        static member viewItems<'t when 't :> MenuFlyout>(views: List<IView>): IAttr<'t> =
            let getter : ('t -> obj) = (fun control -> control.Items :> obj)

            AttrBuilder<'t>.CreateContentMultiple("Items", ValueSome getter, ValueNone, views)

        static member dataItems<'t when 't :> MenuFlyout>(data : IEnumerable): IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<IEnumerable>(MenuFlyout.ItemsSourceProperty, data, ValueNone)

        static member itemTemplate<'t when 't :> MenuFlyout>(value : IDataTemplate): IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<IDataTemplate>(MenuFlyout.ItemTemplateProperty, value, ValueNone)

        static member itemContainerTheme<'t when 't :> MenuFlyout>(theme: ControlTheme) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<ControlTheme>(MenuFlyout.ItemContainerThemeProperty, theme, ValueNone)

        static member flyoutPresenterTheme<'t when 't :> MenuFlyout>(theme: ControlTheme) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<ControlTheme>(MenuFlyout.FlyoutPresenterThemeProperty, theme, ValueNone)

        static member flyoutPresenterClasses<'t when 't :> MenuFlyout>(value: Classes) : IAttr<'t> =
            let getter : ('t -> Classes) = (fun control -> control.FlyoutPresenterClasses)
            let setter : ('t * Classes -> unit) = (fun (control, value) ->
                ClassesInternals.patchStandardClasses control.FlyoutPresenterClasses value)
            
            let compare = ClassesInternals.compareClasses<Classes>

            let factory = (fun () -> Classes())
            
            AttrBuilder<'t>.CreateProperty<Classes>("FlyoutPresenterClasses", value, ValueSome getter, ValueSome setter, ValueSome compare, factory)

        static member flyoutPresenterClasses<'t when 't :> MenuFlyout>(classes: string list) : IAttr<'t> =
            let getter : ('t -> string list) = (fun control -> Seq.toList control.FlyoutPresenterClasses)
            let setter : ('t * string list -> unit) = (fun (control, value) ->
                ClassesInternals.patchStandardClasses control.FlyoutPresenterClasses value)
            
            let compare = ClassesInternals.compareClasses<string list>
            
            AttrBuilder<'t>.CreateProperty<string list>("FlyoutPresenterClasses", classes, ValueSome getter, ValueSome setter, ValueSome compare)
