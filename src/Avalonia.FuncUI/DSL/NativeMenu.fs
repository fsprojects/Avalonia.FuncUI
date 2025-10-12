namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module NativeMenu =
    open Avalonia.Controls
    open Avalonia.FuncUI.Builder
    open Avalonia.FuncUI.Types
     
    let create (attrs: IAttr<NativeMenu> list): IView<NativeMenu> =
        ViewBuilder.Create<NativeMenu>(attrs)
     
    type NativeMenu with
         static member items<'t when 't :> NativeMenu>(value: IView list) : IAttr<'t> =
            let getter : ('t -> obj) = (fun control -> control.Items :> obj)

            AttrBuilder<'t>.CreateContentMultiple("Items", ValueSome getter, ValueNone, value)