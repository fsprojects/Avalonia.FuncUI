namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module DockPanel =  
    open Avalonia.Controls
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
    
    let create (attrs: IAttr<DockPanel> list): IView<DockPanel> =
        ViewBuilder.Create<DockPanel>(attrs)
    
    type Control with
        static member dock<'t when 't :> Control>(dock: Dock) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, Dock>(DockPanel.DockProperty, dock, ValueNone)
    
    type DockPanel with
        static member lastChildFill<'t when 't :> DockPanel>(fill: bool) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, bool>(DockPanel.LastChildFillProperty, fill, ValueNone)