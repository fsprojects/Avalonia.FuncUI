namespace Avalonia.FuncUI.DSL
open Avalonia.Controls

[<AutoOpen>]
module AutoCompleteBox =
    open System
    open System.Collections
    open FSharp.Data.UnitSystems.SI.UnitNames
    open Avalonia.Controls.Templates    
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
    
    let create (attrs: IAttr<AutoCompleteBox> list): IView<AutoCompleteBox> =
        View.create<AutoCompleteBox>(attrs)
    
    type AutoCompleteBox with
        static member onSelectionChanged<'t when 't :> AutoCompleteBox>(func: SelectionChangedEventArgs -> unit) =
            AttrBuilder<'t>.CreateSubscription<_>(AutoCompleteBox.SelectionChangedEvent, func)
        
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
            
        static member selectedItem<'t when 't :> AutoCompleteBox>(value: obj) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<obj>(AutoCompleteBox.SelectedItemProperty, value, ValueNone)
            
        static member onSelectedItemChanged<'t when 't :> AutoCompleteBox>(func: obj -> unit) =
            AttrBuilder<'t>.CreateSubscription<obj>(AutoCompleteBox.SelectedItemProperty, func)

        static member text<'t when 't :> AutoCompleteBox>(text: string) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<string>(AutoCompleteBox.TextProperty, text, ValueNone)
            
        static member onTextChanged<'t when 't :> AutoCompleteBox>(func: string -> unit) =
            AttrBuilder<'t>.CreateSubscription<string>(AutoCompleteBox.TextProperty, func)
            
        static member searchText<'t when 't :> AutoCompleteBox>(text: string) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<string>(AutoCompleteBox.SearchTextProperty, text, ValueNone)
            
        static member onSearchTextChanged<'t when 't :> AutoCompleteBox>(func: string -> unit) =
            AttrBuilder<'t>.CreateSubscription<string>(AutoCompleteBox.SearchTextProperty, func)
            
        static member filterMode<'t when 't :> AutoCompleteBox>(mode: AutoCompleteFilterMode) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<AutoCompleteFilterMode>(AutoCompleteBox.FilterModeProperty, mode, ValueNone)
            
        static member itemFilter<'t when 't :> AutoCompleteBox>(filterFunc: string * obj -> bool) : IAttr<'t> =
            // TODO: implement custom comparer (value is a function)
            AttrBuilder<'t>.CreateProperty<_>(AutoCompleteBox.ItemFilterProperty, filterFunc, ValueNone)
            
        static member textFilter<'t when 't :> AutoCompleteBox>(filter: string) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<string>(AutoCompleteBox.TextFilterProperty, filter, ValueNone)
            
        static member dataItems<'t when 't :> AutoCompleteBox>(items: IEnumerable) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<IEnumerable>(AutoCompleteBox.ItemsProperty, items, ValueNone)
            
        static member viewItems<'t when 't :> AutoCompleteBox>(views: IView list) : IAttr<'t> =
            AttrBuilder<'t>.CreateContentMultiple(AutoCompleteBox.ItemsProperty, views)