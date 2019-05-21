namespace rec Avalonia.FuncUI.Hosts

open Avalonia.FuncUI.Core
open Avalonia.Controls
open Avalonia.FuncUI.VirtualDom

type IViewHost =
    abstract member View: ViewElement -> unit

type HostWindow() =
    inherit Window()

    interface IViewHost with
        member this.View viewElement =
            match this.Content with
            | null ->
                this.Content <- View.create viewElement
            | _ ->
                View.update (this.Content :?> IControl) None viewElement
       
    