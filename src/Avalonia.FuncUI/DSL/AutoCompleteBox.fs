namespace Avalonia.FuncUI.DSL

open System.Threading
open System.Threading.Tasks

[<AutoOpen>]
module AutoCompleteBox =
    open System
    open System.Collections
    open FSharp.Data.UnitSystems.SI.UnitNames
    open Avalonia.Controls
    open Avalonia.Controls.Templates
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
    open System.ComponentModel
    open Avalonia.Data

    let create (attrs: IAttr<AutoCompleteBox> list): IView<AutoCompleteBox> =
        ViewBuilder.Create<AutoCompleteBox>(attrs)

    type AutoCompleteBox with
        static member onSelectionChanged<'t when 't :> AutoCompleteBox>(func: SelectionChangedEventArgs -> unit, ?subPatchOptions) =
            AttrBuilder<'t>.CreateSubscription<_>(AutoCompleteBox.SelectionChangedEvent, func, ?subPatchOptions = subPatchOptions)

        static member watermark<'t when 't :> AutoCompleteBox>(watermark: string) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<string>(AutoCompleteBox.WatermarkProperty, watermark, ValueNone)

        static member minimumPrefixLength<'t when 't :> AutoCompleteBox>(length: int) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<int>(AutoCompleteBox.MinimumPrefixLengthProperty, length, ValueNone)

        static member minimumPopulationDelay<'t when 't :> AutoCompleteBox>(delay: TimeSpan) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<TimeSpan>(AutoCompleteBox.MinimumPopulateDelayProperty, delay, ValueNone)

        static member minimumPopulationDelay<'t when 't :> AutoCompleteBox>(delay: float<second>) : IAttr<'t> =
            delay |> float |> TimeSpan.FromSeconds |> AutoCompleteBox.minimumPopulationDelay

        static member maxDropDownHeight<'t when 't :> AutoCompleteBox>(height: float) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<float>(AutoCompleteBox.MaxDropDownHeightProperty, height, ValueNone)

        static member isTextCompletionEnabled<'t when 't :> AutoCompleteBox>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(AutoCompleteBox.IsTextCompletionEnabledProperty, value, ValueNone)

        static member itemTemplate<'t when 't :> AutoCompleteBox>(template: IDataTemplate) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<IDataTemplate>(AutoCompleteBox.ItemTemplateProperty, template, ValueNone)

        static member isDropDownOpen<'t when 't :> AutoCompleteBox>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(AutoCompleteBox.IsDropDownOpenProperty, value, ValueNone)

        static member valueMemberBinding<'t when 't :> AutoCompleteBox>(binding: BindingBase) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<_>(AutoCompleteBox.ValueMemberBindingProperty, binding, ValueNone)

        static member selectedItem<'t when 't :> AutoCompleteBox>(value: obj) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<obj>(AutoCompleteBox.SelectedItemProperty, value, ValueNone)

        static member onSelectedItemChanged<'t when 't :> AutoCompleteBox>(func: obj -> unit, ?subPatchOptions) =
            AttrBuilder<'t>.CreateSubscription<obj>(AutoCompleteBox.SelectedItemProperty, func, ?subPatchOptions = subPatchOptions)

        static member text<'t when 't :> AutoCompleteBox>(text: string) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<string>(AutoCompleteBox.TextProperty, text, ValueNone)

        static member onTextChanged<'t when 't :> AutoCompleteBox>(func: string -> unit, ?subPatchOptions) =
            AttrBuilder<'t>.CreateSubscription<string>(AutoCompleteBox.TextProperty, func, ?subPatchOptions = subPatchOptions)

        static member onPopulating<'t when 't :> AutoCompleteBox>(func: PopulatingEventArgs -> unit, ?subPatchOptions) =
            let name = nameof Unchecked.defaultof<'t>.Populating
            let factory: SubscriptionFactory<PopulatingEventArgs> =
                fun (control, func, token) ->
                    let control = control :?> 't
                    let handler = EventHandler<PopulatingEventArgs>(fun sender args -> func args)
                    let event = control.Populating

                    event.AddHandler(handler)
                    token.Register(fun () -> event.RemoveHandler(handler)) |> ignore

            AttrBuilder<'t>.CreateSubscription<PopulatingEventArgs>(name, factory, func, ?subPatchOptions = subPatchOptions)

        static member onPopulated<'t when 't :> AutoCompleteBox>(func: PopulatedEventArgs -> unit, ?subPatchOptions) =
            let name = nameof Unchecked.defaultof<'t>.Populated
            let factory: SubscriptionFactory<PopulatedEventArgs> =
                fun (control, func, token) ->
                    let control = control :?> 't
                    let handler = EventHandler<PopulatedEventArgs>(fun sender args -> func args)
                    let event = control.Populated

                    event.AddHandler(handler)
                    token.Register(fun () -> event.RemoveHandler(handler)) |> ignore

            AttrBuilder<'t>.CreateSubscription<PopulatedEventArgs>(name, factory, func, ?subPatchOptions = subPatchOptions)

        static member onSearchTextChanged<'t when 't :> AutoCompleteBox>(func: string -> unit, ?subPatchOptions) =
            AttrBuilder<'t>.CreateSubscription<string>(AutoCompleteBox.SearchTextProperty, func, ?subPatchOptions = subPatchOptions)

        static member onDropDownOpening<'t when 't :> AutoCompleteBox>(func: 't * CancelEventArgs -> unit, ?subPatchOptions) =
            let name = nameof Unchecked.defaultof<'t>.DropDownOpening
            let factory: SubscriptionFactory<'t * CancelEventArgs> =
                fun (control, func, token) ->
                    let control = control :?> 't
                    let handler = EventHandler<CancelEventArgs>(fun sender args -> func (sender :?> 't, args))
                    let event = control.DropDownOpening

                    event.AddHandler(handler)
                    token.Register(fun () -> event.RemoveHandler(handler)) |> ignore

            AttrBuilder<'t>.CreateSubscription<'t * CancelEventArgs>(name, factory, func, ?subPatchOptions = subPatchOptions)

        static member onDropDownOpened<'t when 't :> AutoCompleteBox>(func: 't -> unit, ?subPatchOptions) =
            let name = nameof Unchecked.defaultof<'t>.DropDownOpened
            let factory: SubscriptionFactory<'t> =
                fun (control, func, token) ->
                    let control = control :?> 't
                    let handler = EventHandler(fun sender args -> func (sender :?> 't))
                    let event = control.DropDownOpened

                    event.AddHandler(handler)
                    token.Register(fun () -> event.RemoveHandler(handler)) |> ignore

            AttrBuilder<'t>.CreateSubscription<'t>(name, factory, func, ?subPatchOptions = subPatchOptions)

        static member onDropDownClosing<'t when 't :> AutoCompleteBox>(func: 't * CancelEventArgs -> unit, ?subPatchOptions) =
            let name = nameof Unchecked.defaultof<'t>.DropDownClosing
            let factory: SubscriptionFactory<'t * CancelEventArgs> =
                fun (control, func, token) ->
                    let control = control :?> 't
                    let handler = EventHandler<CancelEventArgs>(fun sender args -> func (sender :?> 't, args))
                    let event = control.DropDownClosing

                    event.AddHandler(handler)
                    token.Register(fun () -> event.RemoveHandler(handler)) |> ignore

            AttrBuilder<'t>.CreateSubscription<'t * CancelEventArgs>(name, factory, func, ?subPatchOptions = subPatchOptions)

        static member onDropDownClosed<'t when 't :> AutoCompleteBox>(func: 't -> unit, ?subPatchOptions) =
            let name = nameof Unchecked.defaultof<'t>.DropDownClosed
            let factory: SubscriptionFactory<'t> =
                fun (control, func, token) ->
                    let control = control :?> 't
                    let handler = EventHandler(fun sender args -> func (sender :?> 't))
                    let event = control.DropDownClosed

                    event.AddHandler(handler)
                    token.Register(fun () -> event.RemoveHandler(handler)) |> ignore

            AttrBuilder<'t>.CreateSubscription<'t>(name, factory, func, ?subPatchOptions = subPatchOptions)

        static member filterMode<'t when 't :> AutoCompleteBox>(mode: AutoCompleteFilterMode) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<AutoCompleteFilterMode>(AutoCompleteBox.FilterModeProperty, mode, ValueNone)

        static member itemFilter<'t when 't :> AutoCompleteBox>(filterFunc: string -> obj -> bool) : IAttr<'t> =
            // TODO: implement custom comparer (value is a function)
            AttrBuilder<'t>.CreateProperty<_>(AutoCompleteBox.ItemFilterProperty, filterFunc, ValueNone)

        static member itemSelector<'t when 't :> AutoCompleteBox>(selector: AutoCompleteSelector<obj>) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<_>(AutoCompleteBox.ItemSelectorProperty, selector, ValueNone)

        static member textFilter<'t when 't :> AutoCompleteBox>(filterFunc: string -> obj -> bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<_>(AutoCompleteBox.TextFilterProperty, filterFunc, ValueNone)

        static member textSelector<'t when 't :> AutoCompleteBox>(selector: AutoCompleteSelector<string>) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<_>(AutoCompleteBox.TextSelectorProperty, selector, ValueNone)

        static member dataItems<'t when 't :> AutoCompleteBox>(items: IEnumerable) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<IEnumerable>(AutoCompleteBox.ItemsSourceProperty, items, ValueNone)

        static member asyncPopulator<'t when 't :> AutoCompleteBox>(populator: Func<string, CancellationToken, Task<seq<obj>>>) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<_>(AutoCompleteBox.AsyncPopulatorProperty, populator, ValueNone)

