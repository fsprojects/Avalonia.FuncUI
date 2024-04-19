namespace Avalonia.FuncUI.DSL

open System
open System.Collections
open System.Threading

open Avalonia
open Avalonia.Controls
open Avalonia.Controls.Templates
open Avalonia.Data
open Avalonia.Styling

[<AutoOpen>]
module ItemsControl =
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder

    let create (attrs: IAttr<ItemsControl> list): IView<ItemsControl> =
        ViewBuilder.Create<ItemsControl>(attrs)

    type ItemsControl with

        static member displayMemberBinding<'t when 't :> ItemsControl>(value: IBinding) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<IBinding>(ItemsControl.DisplayMemberBindingProperty, value, ValueNone)

        static member viewItems<'t when 't :> ItemsControl>(views: IView list) : IAttr<'t> =
            let getter : ('t -> obj) = (fun control -> control.Items :> obj)
            AttrBuilder<'t>.CreateContentMultiple("Items", ValueSome getter, ValueNone, views)

        static member dataItems<'t when 't :> ItemsControl>(data: IEnumerable) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<IEnumerable>(ItemsControl.ItemsSourceProperty, data, ValueNone)

        static member itemContainerTheme<'t when 't :> ItemsControl>(value: ControlTheme) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<ControlTheme>(ItemsControl.ItemContainerThemeProperty, value, ValueNone)

        static member itemsPanel<'t when 't :> ItemsControl>(value: ITemplate<Panel>) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<ITemplate<Panel>>(ItemsControl.ItemsPanelProperty, value, ValueNone)

        static member itemTemplate<'t when 't :> ItemsControl>(value: IDataTemplate) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<IDataTemplate>(ItemsControl.ItemTemplateProperty, value, ValueNone)

        static member onContainerPrepared<'t when 't :> ItemsControl>(func: ('t * ContainerPreparedEventArgs) -> unit, ?subPatchOptions) =
            let name = nameof Unchecked.defaultof<'t>.ContainerPrepared
            let factory: AvaloniaObject * ('t * ContainerPreparedEventArgs -> unit) * CancellationToken -> unit =
                fun (control, func, token) ->
                    let control = control :?> 't
                    let handler = EventHandler<_>(fun s e -> func(s :?> 't, e))
                    let event = control.ContainerPrepared

                    event.AddHandler(handler)
                    token.Register(fun _ -> event.RemoveHandler(handler)) |> ignore

            AttrBuilder<'t>.CreateSubscription<'t * ContainerPreparedEventArgs>(name, factory, func, ?subPatchOptions = subPatchOptions)
        
        static member onContainerIndexChanged<'t when 't :> ItemsControl>(func: ('t * ContainerIndexChangedEventArgs) -> unit, ?subPatchOptions) =
            let name = nameof Unchecked.defaultof<'t>.ContainerIndexChanged
            let factory: AvaloniaObject * ('t * ContainerIndexChangedEventArgs -> unit) * CancellationToken -> unit =
                fun (control, func, token) ->
                    let control = control :?> 't
                    let handler = EventHandler<_>(fun s e -> func(s :?> 't, e))
                    let event = control.ContainerIndexChanged

                    event.AddHandler(handler)
                    token.Register(fun _ -> event.RemoveHandler(handler)) |> ignore

            AttrBuilder<'t>.CreateSubscription<'t * ContainerIndexChangedEventArgs>(name, factory, func, ?subPatchOptions = subPatchOptions)

        static member onContainerClearing<'t when 't :> ItemsControl>(func: ('t * ContainerClearingEventArgs) -> unit, ?subPatchOptions) =
            let name = nameof Unchecked.defaultof<'t>.ContainerClearing
            let factory: AvaloniaObject * ('t * ContainerClearingEventArgs -> unit) * CancellationToken -> unit =
                fun (control, func, token) ->
                    let control = control :?> 't
                    let handler = EventHandler<_>(fun s e -> func(s :?> 't, e))
                    let event = control.ContainerClearing

                    event.AddHandler(handler)
                    token.Register(fun _ -> event.RemoveHandler(handler)) |> ignore

            AttrBuilder<'t>.CreateSubscription<'t * ContainerClearingEventArgs>(name, factory, func, ?subPatchOptions = subPatchOptions)

        static member onItemsChanged<'t when 't :> ItemsControl>(func: IEnumerable -> unit, ?subPatchOptions) =
            AttrBuilder<'t>.CreateSubscription(
                ItemsControl.ItemsSourceProperty :> AvaloniaProperty<IEnumerable>,
                func,
                ?subPatchOptions = subPatchOptions
            )
