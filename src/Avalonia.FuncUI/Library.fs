namespace Avalonia.FuncUI.Library

module internal Hashing =
    open System
    open MBrace.FsPickler
    
    let private binarySerializer = FsPickler.CreateBinarySerializer()
    
    let hash (value: 'value) : string =
        binarySerializer.ComputeHash(value).Id
        
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