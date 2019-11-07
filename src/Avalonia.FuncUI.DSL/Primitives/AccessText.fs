namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module AccessText =
    open Avalonia.Controls
    open Avalonia.Controls.Primitives
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
    
    let create (attrs: IAttr<AccessText> list): IView<AccessText> =
        ViewBuilder.Create<AccessText>(attrs)
     
    type Control with

        static member showAccessKey<'t when 't :> Control>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty(AccessText.ShowAccessKeyProperty, value, ValueNone)
