namespace Avalonia.FuncUI.DSL

open Avalonia.Controls.Primitives

[<AutoOpen>]
module VisualLayerManager =
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder

    let create (attrs: IAttr<VisualLayerManager> list): IView<VisualLayerManager> =
        ViewBuilder.Create<VisualLayerManager>(attrs)

    type VisualLayerManager with
        static member isPopup<'t when 't :> VisualLayerManager>(value: bool) : IAttr<'t> =
            let name = nameof Unchecked.defaultof<'t>.IsPopup
            let getter: 't -> bool = (fun control -> control.IsPopup)
            let setter: 't * bool -> unit = (fun (control, value) -> control.IsPopup <- value)

            AttrBuilder<'t>.CreateProperty<bool>(name, value, ValueSome getter, ValueSome setter, ValueNone)
