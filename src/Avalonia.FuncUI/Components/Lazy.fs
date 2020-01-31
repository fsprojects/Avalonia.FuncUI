namespace rec Avalonia.FuncUI.Components

open System
open Avalonia
open Avalonia.FuncUI.Types
open Avalonia.FuncUI.Components.Hosts

[<Sealed>]
type LazyView<'state, 'args>() =
    inherit HostControl()
    
    let mutable subscription : IDisposable = null
    let mutable _state : 'state = Unchecked.defaultof<'state>
    let mutable _args : 'args = Unchecked.defaultof<'args>
    let mutable _viewFunc : voption<('state -> 'args -> IView)> = ValueNone
    
    static let stateProperty =
        AvaloniaProperty.RegisterDirect<LazyView<'state, 'args>, 'state>(
            "State",
            Func<LazyView<'state, 'args>, 'state>(fun control -> control.State),
            Action<LazyView<'state, 'args>, 'state>(fun control value -> control.State <- value)
        )
        
    static let argsProperty =
        AvaloniaProperty.RegisterDirect<LazyView<'state, 'args>, 'args>(
            "Args",
            Func<LazyView<'state, 'args>, 'args>(fun control -> control.Args),
            Action<LazyView<'state, 'args>, 'args>(fun control value -> control.Args <- value)
        )
        
    static let viewFuncProperty =
        AvaloniaProperty.RegisterDirect<LazyView<'state, 'args>, ('state -> 'args -> IView) voption>(
            "ViewFunc",
            Func<LazyView<'state, 'args>, voption<('state -> 'args -> IView)>>(fun control -> control.ViewFunc),
            Action<LazyView<'state, 'args>, voption<('state -> 'args -> IView)>>(fun control value -> control.ViewFunc <- value)
        )
        
    member this.State
        with get () : 'state = _state
        and set (value: 'state) = this.SetAndRaise(LazyView<'state, 'args>.StateProperty, &_state, value) |> ignore
            
    member this.Args
        with get () : 'args = _args
        and set (value: 'args) = this.SetAndRaise(LazyView<'state, 'args>.ArgsProperty, &_args, value) |> ignore
            
    member this.ViewFunc
        with get () : voption<('state -> 'args -> IView)> = _viewFunc
        and set (value) = this.SetAndRaise(LazyView<'state, 'args>.ViewFuncProperty, &_viewFunc, value) |> ignore  
        
    override this.OnAttachedToVisualTree _args =
        let onNext (state: 'state) : unit =
            let nextView =
                match this.ViewFunc with
                | ValueSome func ->
                    func state this.Args
                    |> Some
                    
                | ValueNone -> None
                
            (this :> IViewHost).Update nextView
        
        subscription <-
            this
                .GetObservable(LazyView<'state, 'args>.StateProperty)
                .Subscribe(onNext)
                
    override this.OnDetachedFromLogicalTree _args =
        if subscription <> null then
            subscription.Dispose()
            subscription <- null
        
    static member StateProperty = stateProperty
    
    static member ArgsProperty = argsProperty
    
    static member ViewFuncProperty = viewFuncProperty