namespace Avalonia.FuncUI.DSL

open Avalonia.Controls.Templates

[<AutoOpen>]
module Flyout =
    open Avalonia.Controls
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
    open Avalonia.Controls.Primitives

    let create (attrs: IAttr<Flyout> list): IView<Flyout> =
        ViewBuilder.Create<Flyout>(attrs)

    type FlyoutBase with

        /// <summary>
        /// A value indicating how the flyout is positioned.
        /// </summary>
        static member placement<'t when 't :> FlyoutBase>(value: FlyoutPlacementMode) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<FlyoutPlacementMode>(FlyoutBase.PlacementProperty, value, ValueNone)

        /// <summary>
        /// A value indicating flyout show mode.
        /// </summary>
        static member showMode<'t when 't :> FlyoutBase>(value: FlyoutShowMode) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<FlyoutShowMode>(FlyoutBase.ShowModeProperty, value, ValueNone)

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
        static member flyoutPresenterClasses<'t when 't :> Flyout>(value: string list) : IAttr<'t> =
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

    let create (attrs: IAttr<MenuFlyout> list): IView<MenuFlyout> =
        ViewBuilder.Create<MenuFlyout>(attrs)

    type MenuFlyout with

        static member viewItems<'t when 't :> MenuFlyout>(views: List<IView>): IAttr<'t> =
            AttrBuilder<'t>.CreateContentMultiple(MenuFlyout.ItemsProperty, views)

        static member dataItems<'t when 't :> MenuFlyout>(data : IEnumerable): IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<IEnumerable>(MenuFlyout.ItemsProperty, data, ValueNone)

        static member itemTemplate<'t when 't :> MenuFlyout>(value : IDataTemplate): IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<IDataTemplate>(MenuFlyout.ItemTemplateProperty, value, ValueNone)

        /// <summary>
        /// A value indicating menu flyout style classes to apply. See https://docs.avaloniaui.net/docs/controls/flyouts#styling-flyouts
        /// </summary>
        static member flyoutPresenterClasses<'t when 't :> MenuFlyout>(value: string list) : IAttr<'t> =
            let getter : ('t -> string list) = (fun control -> control.FlyoutPresenterClasses |> Seq.map id |> List.ofSeq)
            let setter : ('t * string list -> unit) = (fun (control, value) ->
                control.FlyoutPresenterClasses.Clear()
                control.FlyoutPresenterClasses.AddRange(value))

            AttrBuilder<'t>.CreateProperty<string list>("FlyoutPresenterClasses", value, ValueSome getter, ValueSome setter, ValueNone)
