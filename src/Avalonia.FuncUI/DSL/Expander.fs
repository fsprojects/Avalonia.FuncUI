namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module Expander =
    open Avalonia.Animation
    open Avalonia.Controls
    open Avalonia.Interactivity
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
   
    let create (attrs: IAttr<Expander> list): IView<Expander> =
        ViewBuilder.Create<Expander>(attrs)

    type Expander with            

        static member onCollapsed<'t when 't :> Expander>(func: RoutedEventArgs -> unit, ?subPatchOptions) =
            AttrBuilder<'t>.CreateSubscription<RoutedEventArgs>(Expander.CollapsedEvent, func, ?subPatchOptions = subPatchOptions)

        static member onCollapsing<'t when 't :> Expander>(func: CancelRoutedEventArgs -> unit, ?subPatchOptions) =
            AttrBuilder<'t>.CreateSubscription<CancelRoutedEventArgs>(Expander.CollapsingEvent, func, ?subPatchOptions = subPatchOptions)

        static member onExpanded<'t when 't :> Expander>(func: RoutedEventArgs -> unit, ?subPatchOptions) =
            AttrBuilder<'t>.CreateSubscription<RoutedEventArgs>(Expander.ExpandedEvent, func, ?subPatchOptions = subPatchOptions)

        static member onExpanding<'t when 't :> Expander>(func: CancelRoutedEventArgs -> unit, ?subPatchOptions) =
            AttrBuilder<'t>.CreateSubscription<CancelRoutedEventArgs>(Expander.ExpandingEvent, func, ?subPatchOptions = subPatchOptions)

        static member contentTransition<'t when 't :> Expander>(value: IPageTransition) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<IPageTransition>(Expander.ContentTransitionProperty, value, ValueNone)

        static member expandDirection<'t when 't :> Expander>(value: ExpandDirection) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<ExpandDirection>(Expander.ExpandDirectionProperty, value, ValueNone)

        static member isExpanded<'t when 't :> Expander>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(Expander.IsExpandedProperty, value, ValueNone)

        static member onIsExpandedChanged<'t when 't :> Expander>(func: bool -> unit, ?subPatchOptions: SubPatchOptions) : IAttr<'t> =
            AttrBuilder<'t>.CreateSubscription(Expander.IsExpandedProperty, func, ?subPatchOptions = subPatchOptions)
