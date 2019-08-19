namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module Extensions =
    open Avalonia.FuncUI
    open Avalonia.FuncUI.Types
    
    open Avalonia.Controls
    
    type StackPanel with
        static member create (attrs: TypedAttr<StackPanel> list): View = 
            Views.create<StackPanel>(attrs)
        
        static member attrOrientation<'t when 't :> StackPanel>(orientation: Orientation) : TypedAttr<'t> =
            TypedAttr<_>.Property { Name = "Text"; Value = orientation }
            
        static member attrChildren<'t when 't :> StackPanel>(children: View list) : TypedAttr<'t> =
            TypedAttr<_>.Content {
                Name = "Children"
                Content = (ViewContent.Multiple children)
            }
            
    type DockPanel with
        static member create (attrs: TypedAttr<DockPanel> list): View =
            Views.create<DockPanel>(attrs)
        
        static member attrChildren<'t when 't :> DockPanel>(children: View list) : TypedAttr<'t> =
            TypedAttr<_>.Content {
                Name = "Children"
                Content = (ViewContent.Multiple children)
            }
    
    type TextBlock with
        static member create (attrs: TypedAttr<TextBlock> list): View =
            Views.create<TextBlock>(attrs)
        
        static member attrText<'t when 't :> TextBlock>(text: string) : TypedAttr<'t> =
            TypedAttr<_>.Property { Name = "Text"; Value = text }
    
    type Button with
        static member create (attrs: TypedAttr<Button> list): View =
            Views.create<Button>(attrs)
        
        static member attrContent<'t when 't :> Button>(text: string) : TypedAttr<'t> =
            TypedAttr<_>.Property { Name = "Content"; Value = text }
    
        static member attrContent<'t when 't :> Button>(view: View) : TypedAttr<'t> =
            TypedAttr<_>.Property { Name = "Content"; Value = view }