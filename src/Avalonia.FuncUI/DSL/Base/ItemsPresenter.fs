namespace Avalonia.FuncUI.DSL

open Avalonia.Controls
open Avalonia.Controls.Presenters

[<AutoOpen>]
module ItemsPresenter =
    open Avalonia.FuncUI.Builder
    open Avalonia.FuncUI.Types

    let create (attrs: IAttr<ItemsPresenter> list): IView<ItemsPresenter> =
        ViewBuilder.Create<ItemsPresenter>(attrs)

    type ItemsPresenter with
        static member itemsPanel<'t when 't :> ItemsPresenter>(value: ITemplate<Panel>) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<ITemplate<Panel>>(ItemsPresenter.ItemsPanelProperty, value, ValueNone)
