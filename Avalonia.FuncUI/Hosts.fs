namespace rec Avalonia.FuncUI.Hosts

open Avalonia.Controls
open Avalonia.FuncUI.Core

type IViewHost =
    abstract member UpdateView: ViewElement -> unit

type HostWindow() =
    inherit Window()

    let mutable lastViewElement : ViewElement option = None

    interface IViewHost with
        member this.UpdateView viewElement =
            match lastViewElement with
            | Some last -> ()
                //View.update (this.Content :?> IControl) last viewElement
            | None -> ()
                //this.Content <- View.create viewElement

            lastViewElement <- Some viewElement
       
    