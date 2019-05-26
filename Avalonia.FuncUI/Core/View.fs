namespace Avalonia.FuncUI.Core

open Model
open Avalonia.Controls
open System

module View =
    
    let create (viewElement: ViewElement) : IControl =
        Activator.CreateInstance(viewElement.ViewType) :?> IControl

    let update (view: IControl) (last: ViewElement option) (next: ViewElement) =
        ()