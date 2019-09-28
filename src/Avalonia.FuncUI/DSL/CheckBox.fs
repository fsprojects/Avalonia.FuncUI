namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module CheckBox =  
    open Avalonia.Controls
    open Avalonia.Media.Immutable
    open System    
    open System.Windows.Input
    open Avalonia
    open Avalonia.FuncUI.Core.Domain
    open Avalonia.Media
    open Avalonia.Styling
    open Avalonia.Controls.Primitives
    open Avalonia.Controls.Templates
    open Avalonia.Animation
    open Avalonia.Layout
    open Avalonia.Interactivity
    open Avalonia.Input
    
    let create (attrs: IAttr<CheckBox> list): IView<CheckBox> =
        View.create<CheckBox>(attrs)
    
    type CheckBox with end

