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

type TemplateView<'state, 'view when 'view :> IControl> =
    {
        viewFunc: 'state -> IView<'view>
        matchFunc: 'state -> bool
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

            let update (data: 'state) : unit =
                let view = Some ((this.viewFunc data) :> IView)
                (host :> IViewHost).Update view
            
            host
                .GetObservable(HostControl.DataContextProperty)
                .Add(fun data -> update (data :?> 'state))
                
            host :> IControl

module TemplateView =
    
    let create<'state>(viewFunc: 'state -> IView<'view>) : TemplateView<'state, 'view> =
        {
            viewFunc = viewFunc;
            supportsRecycling = true;
            matchFunc = (fun data -> true)
        }