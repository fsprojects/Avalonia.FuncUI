namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module AutoCompleteBox =
    open System
    open System.Collections
    open FSharp.Data.UnitSystems.SI.UnitNames
    open Avalonia.Controls
    open Avalonia.Controls.Templates    
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
    
    let create (attrs: IAttr<AutoCompleteBox> list): IView<AutoCompleteBox> =
        ViewBuilder.Create<AutoCompleteBox>(attrs)
    
    type AutoCompleteBox with
        static member onSelectionChanged<'t when 't :> AutoCompleteBox>(func: SelectionChangedEventArgs -> unit, ?subPatchOptions) =
             AttrBuilder.CreateSubscription<'t, _>(AutoCompleteBox.SelectionChangedEvent, func, ?subPatchOptions = subPatchOptions)
        
        static member watermark<'t when 't :> AutoCompleteBox>(watermark: string) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, string>(AutoCompleteBox.WatermarkProperty, watermark, ValueNone)
            
        static member minimumPrefixLength<'t when 't :> AutoCompleteBox>(length: int) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, int>(AutoCompleteBox.MinimumPrefixLengthProperty, length, ValueNone)
            
        static member minimumPopulationDelay<'t when 't :> AutoCompleteBox>(delay: TimeSpan) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, TimeSpan>(AutoCompleteBox.MinimumPopulateDelayProperty, delay, ValueNone)
            
        static member minimumPopulationDelay<'t when 't :> AutoCompleteBox>(delay: float<second>) : IAttr<'t> =
            delay |> float |> TimeSpan.FromSeconds |> AutoCompleteBox.minimumPopulationDelay
            
        static member maxDropDownHeight<'t when 't :> AutoCompleteBox>(height: float) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, float>(AutoCompleteBox.MaxDropDownHeightProperty, height, ValueNone)
            
        static member isTextCompletionEnabled<'t when 't :> AutoCompleteBox>(value: bool) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, bool>(AutoCompleteBox.IsTextCompletionEnabledProperty, value, ValueNone)
            
        static member itemTemplate<'t when 't :> AutoCompleteBox>(template: IDataTemplate) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, IDataTemplate>(AutoCompleteBox.ItemTemplateProperty, template, ValueNone)
            
        static member isDropDownOpen<'t when 't :> AutoCompleteBox>(value: bool) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, bool>(AutoCompleteBox.IsDropDownOpenProperty, value, ValueNone)
            
        static member selectedItem<'t when 't :> AutoCompleteBox>(value: obj) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, obj>(AutoCompleteBox.SelectedItemProperty, value, ValueNone)
            
        static member onSelectedItemChanged<'t when 't :> AutoCompleteBox>(func: obj -> unit, ?subPatchOptions) =
            AttrBuilder.CreateSubscription<'t, obj>(AutoCompleteBox.SelectedItemProperty, func, ?subPatchOptions = subPatchOptions)

        static member text<'t when 't :> AutoCompleteBox>(text: string) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, string>(AutoCompleteBox.TextProperty, text, ValueNone)
            
        static member onTextChanged<'t when 't :> AutoCompleteBox>(func: string -> unit, ?subPatchOptions) =
            AttrBuilder.CreateSubscription<'t, string>(AutoCompleteBox.TextProperty, func, ?subPatchOptions = subPatchOptions)
            
        static member searchText<'t when 't :> AutoCompleteBox>(text: string) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, string>(AutoCompleteBox.SearchTextProperty, text, ValueNone)
            
        static member onSearchTextChanged<'t when 't :> AutoCompleteBox>(func: string -> unit, ?subPatchOptions) =
            AttrBuilder.CreateSubscription<'t, string>(AutoCompleteBox.SearchTextProperty, func, ?subPatchOptions = subPatchOptions)
            
        static member filterMode<'t when 't :> AutoCompleteBox>(mode: AutoCompleteFilterMode) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, AutoCompleteFilterMode>(AutoCompleteBox.FilterModeProperty, mode, ValueNone)
            
        static member itemFilter<'t when 't :> AutoCompleteBox>(filterFunc: string * obj -> bool) : IAttr<'t> =
            // TODO: implement custom comparer (value is a function)
            AttrBuilder.CreateProperty<'t, _>(AutoCompleteBox.ItemFilterProperty, filterFunc, ValueNone)
            
        static member textFilter<'t when 't :> AutoCompleteBox>(filter: string) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, string>(AutoCompleteBox.TextFilterProperty, filter, ValueNone)
            
        static member dataItems<'t when 't :> AutoCompleteBox>(items: IEnumerable) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, IEnumerable>(AutoCompleteBox.ItemsProperty, items, ValueNone)
            
        static member viewItems<'t when 't :> AutoCompleteBox>(views: IView list) : IAttr<'t> =
            AttrBuilder.CreateContentMultiple(AutoCompleteBox.ItemsProperty, views)