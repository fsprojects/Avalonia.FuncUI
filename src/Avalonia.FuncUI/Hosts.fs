namespace Avalonia.FuncUI.Hosts

open Avalonia.Controls

open Avalonia.FuncUI.Core.Domain
open Avalonia.FuncUI.VirtualDom

type IViewHost =
    abstract member Update: IView option -> unit 

type HostWindow() as this =
    inherit Window()
    
    let mutable lastViewElement : IView option = None
    
    let update (nextViewElement : IView option) : unit =
        let control = this.Content :?> IControl
        VirtualDom.update (control, lastViewElement, nextViewElement)
    
    interface IViewHost with
        member this.Update next =
            update next