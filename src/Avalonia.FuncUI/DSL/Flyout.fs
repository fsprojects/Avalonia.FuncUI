namespace Avalonia.FuncUI.DSL

open Avalonia.Controls.Templates

[<AutoOpen>]
module Flyout =
    open Avalonia.Controls
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
    open Avalonia.Controls.Primitives

    let create (attrs: Attr<Flyout> list): View<Flyout> =
        ViewBuilder.Create<Flyout>(attrs)

    type FlyoutBase with

        /// <summary>
        /// A value indicating how the flyout is positioned.
        /// </summary>
        static member placement<'t when 't :> FlyoutBase>(value: FlyoutPlacementMode) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<FlyoutPlacementMode>(FlyoutBase.PlacementProperty, value, ValueNone)

        /// <summary>
        /// A value indicating flyout show mode.
        /// </summary>
        static member showMode<'t when 't :> FlyoutBase>(value: FlyoutShowMode) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<FlyoutShowMode>(FlyoutBase.ShowModeProperty, value, ValueNone)

    type Flyout with
        static member content<'t when 't :> Flyout>(value: IView option) : Attr<'t> =
            AttrBuilder<'t>.CreateContentSingle(Flyout.ContentProperty, value)

        static member content<'t when 't :> Flyout>(value: IView) : Attr<'t> =
            value
            |> Some
            |> Flyout.content

        /// <summary>
        /// A value indicating flyout style classes to apply. See https://docs.avaloniaui.net/docs/controls/flyouts#styling-flyouts
        /// </summary>
        static member flyoutPresenterClasses<'t when 't :> Flyout>(value: string list) : Attr<'t> =
            let getter : ('t -> string list) = (fun control -> control.FlyoutPresenterClasses |> Seq.map id |> List.ofSeq)
            let setter : ('t * string list -> unit) = (fun (control, value) ->
                control.FlyoutPresenterClasses.Clear()
                control.FlyoutPresenterClasses.AddRange(value))

            AttrBuilder<'t>.CreateProperty<string list>("FlyoutPresenterClasses", value, ValueSome getter, ValueSome setter, ValueNone)


[<AutoOpen>]
module MenuFlyout =
    open System.Collections
    open Avalonia.Controls
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder

    let create (attrs: Attr<MenuFlyout> list): View<MenuFlyout> =
        ViewBuilder.Create<MenuFlyout>(attrs)

    type MenuFlyout with

        static member viewItems<'t when 't :> MenuFlyout>(views: List<IView>): Attr<'t> =
            AttrBuilder<'t>.CreateContentMultiple(MenuFlyout.ItemsProperty, views)

        static member dataItems<'t when 't :> MenuFlyout>(data : IEnumerable): Attr<'t> =
            AttrBuilder<'t>.CreateProperty<IEnumerable>(MenuFlyout.ItemsProperty, data, ValueNone)

        static member itemTemplate<'t when 't :> MenuFlyout>(value : IDataTemplate): Attr<'t> =
            AttrBuilder<'t>.CreateProperty<IDataTemplate>(MenuFlyout.ItemTemplateProperty, value, ValueNone)

        /// <summary>
        /// A value indicating menu flyout style classes to apply. See https://docs.avaloniaui.net/docs/controls/flyouts#styling-flyouts
        /// </summary>
        static member flyoutPresenterClasses<'t when 't :> MenuFlyout>(value: string list) : Attr<'t> =
            let getter : ('t -> string list) = (fun control -> control.FlyoutPresenterClasses |> Seq.map id |> List.ofSeq)
            let setter : ('t * string list -> unit) = (fun (control, value) ->
                control.FlyoutPresenterClasses.Clear()
                control.FlyoutPresenterClasses.AddRange(value))

            AttrBuilder<'t>.CreateProperty<string list>("FlyoutPresenterClasses", value, ValueSome getter, ValueSome setter, ValueNone)
