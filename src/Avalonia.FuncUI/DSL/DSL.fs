namespace Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Types
open System.Threading

[<AutoOpen>]
module Helpers =
    let generalize (view: IView<'t>) : IView =
        view :> IView
    

            
module Playground =
     open System
     open Avalonia.Interactivity
     open Avalonia.Layout
     open Avalonia.Controls.Primitives
     open Avalonia.Controls
     open Avalonia

     let view =      
         StackPanel.create [
             StackPanel.orientation Orientation.Horizontal
             StackPanel.children [
                 CheckBox.create [
                     CheckBox.content "click me"
                 ]
                 ToggleButton.create [
                     ToggleButton.isChecked true
                 ]
             ]
         ]
     