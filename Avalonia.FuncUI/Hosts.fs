namespace rec Avalonia.FuncUI.Hosts

open Avalonia.Controls
open Avalonia.FuncUI.Core
open Avalonia.FuncUI.Core.Types
open Avalonia.FuncUI.Core.VirtualDom.Delta

type IViewHost =
    abstract member UpdateView: View -> unit

type HostWindow() =
    inherit Window()

    let mutable lastViewElement : View option = None

    interface IViewHost with
        member this.UpdateView viewElement =
            match lastViewElement with
            | Some last ->
                let view = this.Content :?> IControl
                let delta = VirtualDom.Differ.diff(last, viewElement)
                VirtualDom.Patcher.patch(view, delta)
            | None ->
                this.Content <- VirtualDom.createView viewElement
                let view = this.Content :?> IControl
                let delta = ViewDelta.From viewElement
                VirtualDom.Patcher.patch(view, delta)

            lastViewElement <- Some viewElement
       
    