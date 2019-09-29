namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module CheckBox =  
    open Avalonia.Controls
    open Avalonia.FuncUI.Types
    
    let create (attrs: IAttr<CheckBox> list): IView<CheckBox> =
        View.create<CheckBox>(attrs)
    
    type CheckBox with end

