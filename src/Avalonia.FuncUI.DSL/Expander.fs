namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module Expander =
    open Avalonia.Animation
    open Avalonia.Controls
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
   
    let create (attrs: IAttr<Expander> list): IView<Expander> =
        ViewBuilder.Create<Expander>(attrs)

    type Expander with            

        static member contentTransition<'t when 't :> Expander>(value: IPageTransition) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, IPageTransition>(Expander.ContentTransitionProperty, value, ValueNone)

        static member expandDirection<'t when 't :> Expander>(value: ExpandDirection) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, ExpandDirection>(Expander.ExpandDirectionProperty, value, ValueNone)

        static member isExpanded<'t when 't :> Expander>(value: bool) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, bool>(Expander.IsExpandedProperty, value, ValueNone)