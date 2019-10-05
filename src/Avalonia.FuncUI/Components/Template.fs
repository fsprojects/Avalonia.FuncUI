namespace rec Avalonia.FuncUI.Components

open System
open Avalonia
open Avalonia.Controls
open Avalonia.Controls.Templates
open Avalonia.FuncUI
open Avalonia.FuncUI.Types
open Avalonia.FuncUI.VirtualDom
open Avalonia.FuncUI.Components.Hosts

// ITemplate<TParam, TControl>
// IDataTemplate

type TemplateView =
    {
        viewFunc: obj -> IView
        matchFunc: obj -> bool
        supportsRecycling: bool
    }
    
    interface IDataTemplate with
        member this.SupportsRecycling =
            this.supportsRecycling
            
        member this.Match (data: obj) : bool =
            match data with
            | :? 'state as data ->
                this.matchFunc data
            | _ -> false
            
        member this.Build (data: obj) : IControl =
            let host = HostControl()

            let update (data: obj) : unit =
                match data with
                | null -> (host :> IViewHost).Update None
                | data ->
                    printfn "updating template %A" data
                    let view = Some (this.viewFunc data)
                    (host :> IViewHost).Update view
            
            host
                .GetObservable(Control.DataContextProperty)
                .Subscribe(fun data -> update data)
                
            host :> IControl

module TemplateView =
    
    let create(viewFunc: obj -> IView) : TemplateView =
        {
            viewFunc = viewFunc;
            supportsRecycling = true;
            matchFunc = (fun data -> true)
        }