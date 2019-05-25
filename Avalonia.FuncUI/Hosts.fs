namespace rec Avalonia.FuncUI.Hosts

open Avalonia.FuncUI.Core
open Avalonia.Controls
open Avalonia.FuncUI.Core.VirtualDom
open Avalonia.FuncUI.Core.Model

type IViewHost =
    abstract member UpdateView: ViewElement -> unit

type HostWindow() =
    inherit Window()

    interface IViewHost with
        member this.UpdateView viewElement =
            match this.Content with
            | null ->
                this.Content <- View.create viewElement
            | _ ->
                View.update (this.Content :?> IControl) None viewElement
       
    