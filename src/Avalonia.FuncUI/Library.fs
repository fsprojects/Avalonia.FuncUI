namespace Avalonia.FuncUI.Library

module FunctionAnalysis =
    open System
    open System.Reflection
    open System.Collections.Concurrent
    
    let internal cache = ConcurrentDictionary<Type, bool>()
    
    let private flags =
        BindingFlags.Instance |||
        BindingFlags.NonPublic |||
        BindingFlags.Public
    
    let capturesState (func : 'a) : bool =
        let type' = func.GetType()

        let hasValue, value = cache.TryGetValue type'
        
        match hasValue with
        | true -> value
        | false ->
            let capturesState =
                type'.GetConstructors(flags)
                |> Array.map (fun info -> info.GetParameters().Length)
                |> Array.exists (fun parameterLength -> parameterLength > 0)
                
            cache.AddOrUpdate(type', capturesState, (fun identifier lastValue -> capturesState))
        
module Observable =
    open System
    
    let subscribeWeakly (source: IObservable<'a>, callback: 'a -> unit, target: 'target) = 

        let mutable sub:IDisposable = null
        let mutable disposed = false
        let wr = new WeakReference<_>(target)

        let dispose() =
            lock (sub) (fun () -> 
                if not disposed then sub.Dispose(); disposed <- true)

        let callback' x =
            let isAlive, target = wr.TryGetTarget()
            if isAlive then callback x else dispose()

        sub <- Observable.subscribe callback' source
        sub

[<AutoOpen>]
module internal Extensions =
    open Avalonia
    open Avalonia.Interactivity
    open System
    open System.Reactive.Linq
    
    type IObservable<'a> with
        member this.SubscribeWeakly(callback: 'a -> unit, target) =
            Observable.subscribeWeakly(this, callback, target)
        
    type IInteractive with
        member this.GetObservable<'args when 'args :> RoutedEventArgs>(routedEvent: RoutedEvent) =
            
            let sub = Func<IObserver<'args>, IDisposable>(fun x ->
                let act = Action<obj, 'args>(fun _ e -> x.OnNext e)
                this.AddHandler(routedEvent, act)    
            )
            Observable.Create(sub)