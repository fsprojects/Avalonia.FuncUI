namespace Avalonia.FuncUI.Core

open Model
open Avalonia.Controls
open System

module View =
    
    let create (viewElement: ViewElement) : IControl =
        let control = VirtualDom.create viewElement
        VirtualDom.Patcher.patch control viewElement
        control

    let update (view: IControl) (last: ViewElement) (next: ViewElement) =
        ()