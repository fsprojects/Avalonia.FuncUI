namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module AutoCompleteBox =
    open System
    open System.Threading
    open System.Collections
    
    open FSharp.Data.UnitSystems.SI.UnitNames
    
    open Avalonia.Controls.Templates    
    open Avalonia.Controls
    open Avalonia.FuncUI.Types
    
    let create (attrs: IAttr<AutoCompleteBox> list): IView<AutoCompleteBox> =
        View.create<AutoCompleteBox>(attrs)
    
    type AutoCompleteBox with
        static member onSelectionChanged<'t when 't :> AutoCompleteBox>(func: SelectionChangedEventArgs -> unit) =
            let factory (control: IControl, func: SelectionChangedEventArgs -> unit, token: CancellationToken) =
                (control :?> AutoCompleteBox).SelectionChanged.Subscribe(func, token)

            let subscription = Subscription.createFromEvent ("SelectionChanged", factory, func)
            let attr = Attr.createSubscription<'t>(subscription)
            attr :> IAttr<'t>
        
        static member watermark<'t when 't :> AutoCompleteBox>(watermark: string) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty AutoCompleteBox.WatermarkProperty
            let property = Property.createDirect(accessor, watermark)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member minimumPrefixLength<'t when 't :> AutoCompleteBox>(length: int) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty AutoCompleteBox.MinimumPrefixLengthProperty
            let property = Property.createDirect(accessor, length)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member minimumPopulationDelay<'t when 't :> AutoCompleteBox>(delay: TimeSpan) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty AutoCompleteBox.MinimumPopulateDelayProperty
            let property = Property.createDirect(accessor, delay)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member minimumPopulationDelay<'t when 't :> AutoCompleteBox>(delay: float<second>) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty AutoCompleteBox.MinimumPopulateDelayProperty
            let property = Property.createDirect(accessor, TimeSpan.FromSeconds(float delay))
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member maxDropDownHeight<'t when 't :> AutoCompleteBox>(height: float) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty AutoCompleteBox.MaxDropDownHeightProperty
            let property = Property.createDirect(accessor, height)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member isTextCompletionEnabled<'t when 't :> AutoCompleteBox>(value: bool) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty AutoCompleteBox.IsTextCompletionEnabledProperty
            let property = Property.createDirect(accessor, value)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member itemTemplate<'t when 't :> AutoCompleteBox>(template: IDataTemplate) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty AutoCompleteBox.ItemTemplateProperty
            let property = Property.createDirect(accessor, template)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member isDropDownOpen<'t when 't :> AutoCompleteBox>(value: bool) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty AutoCompleteBox.IsDropDownOpenProperty
            let property = Property.createDirect(accessor, value)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member selectedItem<'t when 't :> AutoCompleteBox>(value: obj) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty AutoCompleteBox.SelectedItemProperty
            let property = Property.createDirect(accessor, value)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member onSelectedItemChanged<'t when 't :> AutoCompleteBox>(func: obj -> unit) =
            let subscription = Subscription.createFromProperty(AutoCompleteBox.SelectedItemProperty, func)
            let attr = Attr.createSubscription<'t>(subscription)
            attr :> IAttr<'t>

        static member text<'t when 't :> AutoCompleteBox>(text: string) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty AutoCompleteBox.TextProperty
            let property = Property.createDirect(accessor, text)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member onTextChanged<'t when 't :> AutoCompleteBox>(func: string -> unit) =
            let subscription = Subscription.createFromProperty(AutoCompleteBox.TextProperty, func)
            let attr = Attr.createSubscription<'t>(subscription)
            attr :> IAttr<'t>
            
        static member searchText<'t when 't :> AutoCompleteBox>(text: string) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty AutoCompleteBox.SearchTextProperty
            let property = Property.createDirect(accessor, text)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member onSearchTextChanged<'t when 't :> AutoCompleteBox>(func: string -> unit) =
            let subscription = Subscription.createFromProperty(AutoCompleteBox.SearchTextProperty, func)
            let attr = Attr.createSubscription<'t>(subscription)
            attr :> IAttr<'t>
            
        static member filterMode<'t when 't :> AutoCompleteBox>(mode: AutoCompleteFilterMode) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty AutoCompleteBox.FilterModeProperty
            let property = Property.createDirect(accessor, mode)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member itemFilter<'t when 't :> AutoCompleteBox>(filterFunc: string * obj -> bool) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty AutoCompleteBox.ItemFilterProperty
            let property = Property.createDirect(accessor, filterFunc)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member textFilter<'t when 't :> AutoCompleteBox>(filter: string) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty AutoCompleteBox.TextFilterProperty
            let property = Property.createDirect(accessor, filter)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member items<'t when 't :> AutoCompleteBox>(items: #IEnumerable) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty AutoCompleteBox.ItemsProperty
            let property = Property.createDirect(accessor, items)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        
