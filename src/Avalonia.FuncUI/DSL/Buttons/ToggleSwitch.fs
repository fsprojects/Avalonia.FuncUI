namespace Avalonia.FuncUI.DSL


[<AutoOpen>]
module ToggleSwitch =

    open Avalonia.Animation
    open Avalonia.Controls
    open Avalonia.Controls.Templates
    open Avalonia.FuncUI
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
    open Avalonia.Collections

    let create (attrs: IAttr<ToggleSwitch> list): IView<ToggleSwitch> =
        ViewBuilder.Create<ToggleSwitch>(attrs)

    type ToggleSwitch with

        static member onContent<'t when 't :> ToggleSwitch>(text: string) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<string>(ToggleSwitch.OnContentProperty, text, ValueNone)

        static member onContent<'t when 't :> ToggleSwitch>(value: obj) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<obj>(ToggleSwitch.OnContentProperty, value, ValueNone)

        static member onContent<'t when 't :> ToggleSwitch>(view: IView option) : IAttr<'t> =
            AttrBuilder<'t>.CreateContentSingle(ToggleSwitch.OnContentProperty, view)

        static member onContent<'t when 't :> ToggleSwitch>(view: IView) : IAttr<'t> =
            Some view |> ToggleSwitch.onContent

        static member onContentTemplate<'t when 't :> ToggleSwitch>(template: IDataTemplate) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<IDataTemplate>(ToggleSwitch.OnContentTemplateProperty, template, ValueNone)

        static member offContent<'t when 't :> ToggleSwitch>(text: string) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<string>(ToggleSwitch.OffContentProperty, text, ValueNone)

        static member offContent<'t when 't :> ToggleSwitch>(value: obj) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<obj>(ToggleSwitch.OffContentProperty, value, ValueNone)

        static member offContent<'t when 't :> ToggleSwitch>(view: IView option) : IAttr<'t> =
            AttrBuilder<'t>.CreateContentSingle(ToggleSwitch.OffContentProperty, view)

        static member offContent<'t when 't :> ToggleSwitch>(view: IView) : IAttr<'t> =
            Some view |> ToggleSwitch.offContent

        static member offContentTemplate<'t when 't :> ToggleSwitch>(template: IDataTemplate) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<IDataTemplate>(ToggleSwitch.OffContentTemplateProperty, template, ValueNone)

        static member knobTransitions<'t when 't :> ToggleSwitch>(value: Transitions) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<Transitions>(ToggleSwitch.KnobTransitionsProperty, value, ValueNone)

        static member knobTransitions<'t when 't :> ToggleSwitch>(transitions: ITransition list) : IAttr<'t> =
            let name = nameof Unchecked.defaultof<'t>.Transitions
            let getter: 't -> ITransition list = fun x -> x.KnobTransitions |> Seq.toList
            let setter: 't * ITransition list -> unit = fun (x, value) -> Setters.avaloniaList x.KnobTransitions value
            
            AttrBuilder<'t>.CreateProperty(name, transitions, ValueSome getter, ValueSome setter, ValueNone)
