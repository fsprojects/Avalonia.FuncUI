namespace rec Avalonia.FuncUI.Components

open System
open Avalonia
open Avalonia.Controls
open Avalonia.Controls.Templates
open Avalonia.FuncUI
open Avalonia.FuncUI.Types
open Avalonia.FuncUI.VirtualDom
open Avalonia.FuncUI.Components.Hosts


// TODO: Split into different implementations so this can be used for styles/control templates too. 
// - ITemplate
// - IDataTemplate

type DataTemplateView<'T> =
    {
        viewFunc: 'T -> IView
        supportsRecycling: bool
    }
    
    interface IDataTemplate with
        member this.SupportsRecycling =
            this.supportsRecycling
            
        member this.Match (data: obj) : bool =
            match data with
            | :? 'T as data -> true
            | _ -> false
            
        member this.Build (data: obj) : IControl =
            let host = HostControl()

            let update (data: 'T) : unit =
                //printfn "updating template %A" data
                let view = Some (this.viewFunc data)
                (host :> IViewHost).Update view
            
            let disposable =
                host
                    .GetObservable(Control.DataContextProperty)
                    .Subscribe(fun data ->
                        match data with
                            | :? 'T as t -> update t
                            | _ -> ()
                        )
                
            host :> IControl

module DataTemplateView =
    
    let create(viewFunc: 'T -> IView<'view>) : DataTemplateView<'T> =
        {
            viewFunc = (fun a -> (viewFunc a) :> IView);
            supportsRecycling = true;
        }