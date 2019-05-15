namespace rec Avalonia.FuncUI.Hosts

open Avalonia.FuncUI.Core
open Avalonia.Controls

type IViewHost =
    abstract member View: IViewElement -> unit

type HostWindow() =
    inherit Window()

    interface IViewHost with
        member this.View viewElement =
            match this.Content with
            | null ->
                this.Content <- viewElement.Create()
            | _ ->
                viewElement.Update(this.Content :?> Avalonia.Controls.IControl)
       
    