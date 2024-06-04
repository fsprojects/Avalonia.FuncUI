namespace Avalonia.FuncUI.DSL

open Avalonia
open Avalonia.Controls
open Avalonia.Data

[<AutoOpen>]
module BindingEvaluator =
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder

    let create<'x>
        (attrs: IAttr<AutoCompleteBox.BindingEvaluator<'x>> list)
        : IView<AutoCompleteBox.BindingEvaluator<'x>> =
        ViewBuilder.Create<AutoCompleteBox.BindingEvaluator<'x>>(attrs)

    let createWith<'x>
        (binding: IBinding)
        (attrs: IAttr<AutoCompleteBox.BindingEvaluator<'x>> list)
        : IView<AutoCompleteBox.BindingEvaluator<'x>> =
        create attrs |> View.withConstructorArgs [| binding |]

    type AutoCompleteBox.BindingEvaluator<'x> with

        static member value<'t, 'x when 't :> AutoCompleteBox.BindingEvaluator<'x>>(value: 'x) : IAttr<'t> =
            let prop: StyledProperty<'x> = AutoCompleteBox.BindingEvaluator.ValueProperty
            AttrBuilder<'t>.CreateProperty<'x>(prop, value, ValueNone)

        static member valueBinding<'t, 'x when 't :> AutoCompleteBox.BindingEvaluator<'x>>
            (value: IBinding)
            : IAttr<'t> =
            let name = nameof Unchecked.defaultof<'t>.ValueBinding
            let getter: 't -> IBinding = fun x -> x.ValueBinding
            let setter: 't * IBinding -> unit = fun (x, v) -> x.ValueBinding <- v

            AttrBuilder<'t>.CreateProperty<IBinding>(name, value, ValueSome getter, ValueSome setter, ValueNone)
