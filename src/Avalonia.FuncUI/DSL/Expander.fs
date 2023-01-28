namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module Expander =
    open Avalonia.Animation
    open Avalonia.Controls
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder

    let create (attrs: Attr<Expander> list): View<Expander> =
        ViewBuilder.Create<Expander>(attrs)

    type Expander with

        static member contentTransition<'t when 't :> Expander>(value: IPageTransition) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<IPageTransition>(Expander.ContentTransitionProperty, value, ValueNone)

        static member expandDirection<'t when 't :> Expander>(value: ExpandDirection) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<ExpandDirection>(Expander.ExpandDirectionProperty, value, ValueNone)

        static member isExpanded<'t when 't :> Expander>(value: bool) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(Expander.IsExpandedProperty, value, ValueNone)

        static member onIsExpandedChanged<'t when 't :> Expander>(func: bool -> unit, ?subPatchOptions: SubPatchOptions) : Attr<'t> =
            AttrBuilder<'t>.CreateSubscription(Expander.IsExpandedProperty, func, ?subPatchOptions = subPatchOptions)
