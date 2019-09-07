namespace Avalonia.FuncUI.Hosts

open Avalonia.Controls

open Avalonia.FuncUI.Core.Domain

type IViewHost =
    abstract member Update: IView option -> unit 

type HostWindow() as this =
    inherit Window()
    
    let mutable lastViewElement : IView option = None
    
    let update (newViewElement : IView option) : unit =
        match lastViewElement with
        | Some last ->
            let control = this.Content :?> IControl
            let del
    
    interface IViewHost with
        member this.Update next =
            update next