namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module Control =  
    open Avalonia.Controls
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
    
    let create (attrs: IAttr<Control> list): IView<Control> =
        ViewBuilder.Create<Control>(attrs)
    
    type Control with
        static member focusAdorner<'t, 'c when 't :> Control and 'c :> IControl>(value: ITemplate<'c>) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, ITemplate<'c>>(Control.FocusAdornerProperty, value, ValueNone)
            
        static member tag<'t when 't :> Control>(value: obj) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, obj>(Control.TagProperty, value, ValueNone)

        static member contextMenu<'t when 't :> Control>(menuView: IView<ContextMenu> option) : IAttr<'t> =
            let view =
                match menuView with
                | Some view -> Some (view :> IView)
                | None -> None
            
            // TODO: think about exposing less generic IView<'c> 
            AttrBuilder.CreateContentSingle(Control.ContextMenuProperty, view)
            
        static member contextMenu<'t when 't :> Control>(menuView: IView<ContextMenu>) : IAttr<'t> =
            AttrBuilder.CreateContentSingle(Control.ContextMenuProperty, Some (menuView :> IView))
        
        static member contextMenu<'t when 't :> Control>(menu: ContextMenu) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, ContextMenu>(Control.ContextMenuProperty, menu, ValueNone)
           
           