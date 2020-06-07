namespace Avalonia.FuncUI.Library

open System.Reactive.Disposables

module Observable =
    open System

    let subscribeWeakly (source: IObservable<'a>, callback: 'a -> unit, target: 'target) =

        let mutable sub:IDisposable = null
        let mutable disposed = false
        let wr = WeakReference<_>(target)

        let dispose() =
            lock (sub) (fun () ->
                if not disposed then sub.Dispose(); disposed <- true)

        let callback' x =
            let isAlive, _target = wr.TryGetTarget()
            if isAlive then callback x else dispose()

        sub <- Observable.subscribe callback' source
        sub

module internal Utils =
    let cast<'t>(a: obj) : 't =
        a :?> 't

[<AutoOpen>]
module internal Extensions =
    open Avalonia.Interactivity
    open System
    open System.Reactive.Linq

    type IObservable<'a> with
        member this.SubscribeWeakly(callback: 'a -> unit, target) =
            Observable.subscribeWeakly(this, callback, target)

    type IInteractive with
        member this.GetObservable<'args when 'args :> RoutedEventArgs>(routedEvent: RoutedEvent) : IObservable<'args> =

            let sub = Func<IObserver<'args>, IDisposable>(fun observer ->
                // push new update to subscribers
                let publish = Action<obj, 'args>(fun _ e ->
                    observer.OnNext e
                )
                
                // subscribe to event changes so they can be pushed to subscribers
                this.AddHandler(routedEvent, publish, routedEvent.RoutingStrategies)
                
                let disposableState = (this, publish, routedEvent)
                       
                let dispose = Action<IInteractive * _ * RoutedEvent>(fun (state, publish, routedEvent) ->  state.RemoveHandler(routedEvent, publish))
                
                Disposable.Create(disposableState, dispose)
            )
            
            Observable.Create(sub)