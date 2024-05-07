namespace Avalonia.FuncUI.DSL

open Avalonia.Controls.Primitives
open Avalonia.Input

[<AutoOpen>]
module LightDismissOverlayLayer =
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder

    let create (attrs: IAttr<LightDismissOverlayLayer> list): IView<LightDismissOverlayLayer> =
        ViewBuilder.Create<LightDismissOverlayLayer>(attrs)

    type LightDismissOverlayLayer with
        static member inputPassThroughElement<'t when 't :> LightDismissOverlayLayer>(element: IInputElement) : IAttr<'t> =
            let name = nameof Unchecked.defaultof<'t>.InputPassThroughElement
            let getter: 't -> IInputElement = (fun control -> control.InputPassThroughElement)
            let setter: 't * IInputElement -> unit = (fun (control, value) -> control.InputPassThroughElement <- value)

            AttrBuilder<'t>.CreateProperty<IInputElement>(name, element, ValueSome getter, ValueSome setter, ValueNone)
