namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module ComboBoxItem =
    open Avalonia.Controls
    open Avalonia.FuncUI.Types
    
    let create (attrs: IAttr<ComboBoxItem> list): IView<ComboBoxItem> =
        View.create<ComboBoxItem>(attrs)
    
    type ComboBoxItem with
        end