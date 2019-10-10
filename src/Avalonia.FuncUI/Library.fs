namespace Avalonia.FuncUI.Library

module internal Hashing =
    open System
    open MBrace.FsPickler
    
    let private binarySerializer = FsPickler.CreateBinarySerializer()
    
    let hash (value: 'value) : string =
        binarySerializer.ComputeHash(value).Id
        
         
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
        
[<AutoOpen>]
module internal Extensions =
    open Avalonia
    open Avalonia.Interactivity
    open System
    open System.Reactive.Linq
        
    type IInteractive with
        member this.GetObservable<'args when 'args :> RoutedEventArgs>(routedEvent: RoutedEvent) =
            
            let sub = Func<IObserver<'args>, IDisposable>(fun x ->
                let act = Action<obj, 'args>(fun _ e -> x.OnNext e)
                this.AddHandler(
                    routedEvent,
                    act
                )    
            )
            Observable.Create(sub)