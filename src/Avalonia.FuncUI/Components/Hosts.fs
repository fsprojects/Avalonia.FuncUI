namespace Avalonia.FuncUI.Components.Hosts

open Avalonia.Controls
open Avalonia.Controls.Presenters
open Avalonia.FuncUI.Types
open Avalonia.FuncUI.VirtualDom

type IViewHost =
    abstract member Update: IView option -> unit 

type HostWindow() as this =
    inherit Window()
    
    let mutable lastViewElement : IView option = None
    
    let update (nextViewElement : IView option) : unit =
        match nextViewElement with
        | Some next ->
            match lastViewElement with
            | Some last ->
                let root = this.Content :?> IControl
                VirtualDom.update(root, last, next)
            | None ->
                this.Content <- VirtualDom.create next
                
            lastViewElement <- nextViewElement
        | None ->
            this.Content <- null
                
    interface IViewHost with
        member this.Update next =
            update next

type HostControl() as this =
    inherit ContentPresenter()
    
    let mutable lastViewElement : IView option = None
    
    let update (nextViewElement : IView option) : unit =
        match nextViewElement with
        | Some next ->
            match lastViewElement with
            | Some last ->
                let root = this.Content :?> IControl
                VirtualDom.update(root, last, next)
            | None ->
                this.Content <- VirtualDom.create next
                
            lastViewElement <- nextViewElement
        | None ->
            this.Content <- null
                
    interface IViewHost with
        member this.Update next =
            update next