namespace Avalonia.FuncUI.Components

open System
open Avalonia
open Avalonia.Controls
open Avalonia.Controls.Templates

open Avalonia.FuncUI.Library
open Avalonia.FuncUI.Types
open Avalonia.FuncUI.Components.Hosts

[<CustomEquality; NoComparison>]
type DataTemplateView<'data> =
    { viewFunc: 'data -> IView
      matchFunc: ('data -> bool) voption
      supportsRecycling: bool }
    with
        override this.Equals (other: obj) : bool =
            match other with
            | :? DataTemplateView<'data> as other ->
                this.viewFunc.GetType() = other.viewFunc.GetType() &&
                this.matchFunc.GetType() = other.matchFunc.GetType() &&
                this.supportsRecycling = other.supportsRecycling
            | _ -> false
            
        override this.GetHashCode () =
            (this.viewFunc.GetType(), this.supportsRecycling).GetHashCode()
    
        interface IDataTemplate with
            member this.SupportsRecycling =
                this.supportsRecycling
                
            member this.Match (data: obj) : bool =
                match data with
                | :? 'data as data -> true
                | _ -> false
                
            member this.Build (data: obj) : IControl =
                let host = HostControl()

                let update (data: 'data) : unit =
                    let view = Some (this.viewFunc data)
                    (host :> IViewHost).Update view
                
                host
                    .GetObservable(Control.DataContextProperty)
                    .SubscribeWeakly<obj>((fun data ->
                        match data with
                        | :? 'data as t -> update(t)
                        | _ -> ()
                    ), this) |> ignore
                    
                host :> IControl

type DataTemplateView<'data> with

    /// <summary>
    /// Create a DataTemplateView for data matching type ('data)
    /// </summary>
    /// <typeparam name="'data">The Type of the data.</typeparam>
    /// <param name="viewFunc">The function that creates a view from the passed data.</param>
    static member create(viewFunc: 'data -> IView<'view>) : DataTemplateView<'data> =
        { viewFunc = (fun a -> (viewFunc a) :> IView)
          matchFunc = ValueNone
          supportsRecycling = true }

    /// <summary>
    /// Create a DataTemplateView for data matching type ('data)
    /// </summary>
    /// <typeparam name="'data">The Type of the data.</typeparam>
    /// <param name="viewFunc">The function that creates a view from the passed data.</param>
    /// <param name="matchFunc">The function that decides if this template is capable of creating a view from the passed data.</param>     
    static member create(viewFunc: 'data -> IView<'view>, matchFunc: 'data -> bool) : DataTemplateView<'data> =
        { viewFunc = (fun a -> (viewFunc a) :> IView)
          matchFunc = ValueSome matchFunc
          supportsRecycling = true }