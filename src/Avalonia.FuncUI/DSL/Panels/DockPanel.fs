namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module DockPanel =  
    open Avalonia.Controls
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
    
    let create (attrs: IAttr<DockPanel> list): IView<DockPanel> =
        ViewBuilder.Create<DockPanel>(attrs)
    
    type Control with
        /// An attached property specifying the dock relative to the parent DockPanel. The applied control should be a child of a DockPanel for this property to take effect.
        static member dock<'t when 't :> Control>(dock: Dock) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<Dock>(DockPanel.DockProperty, dock, ValueNone)
    
    type DockPanel with
        static member lastChildFill<'t when 't :> DockPanel>(fill: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(DockPanel.LastChildFillProperty, fill, ValueNone)
