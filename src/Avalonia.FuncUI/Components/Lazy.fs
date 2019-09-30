namespace rec Avalonia.FuncUI.Components

open System
open Avalonia
open Avalonia.Controls
open Avalonia.FuncUI
open Avalonia.FuncUI.Types
open Avalonia.FuncUI.VirtualDom
open Avalonia.FuncUI.Components.Hosts


type LazyView<'state, 'args>() as this =
    inherit HostControl()
    
    let mutable subscription : IDisposable = null
    let mutable _state : 'state = Unchecked.defaultof<'state>
    let mutable _args : 'attr = Unchecked.defaultof<'attr>
    let mutable _viewFunc : option<('state -> 'args -> IView)> = None
    
    static let stateProperty =
        AvaloniaProperty.RegisterDirect<LazyView<'state, 'args>, 'state>(
            "State",
            new Func<LazyView<'state, 'args>, 'state>(fun control -> control.State),
            new Action<LazyView<'state, 'args>, 'state>(fun control value -> control.State <- value)
        )
        
    static let argsProperty =
        AvaloniaProperty.RegisterDirect<LazyView<'state, 'args>, 'args>(
            "args",
            new Func<LazyView<'state, 'args>, 'args>(fun control -> control.Args),
            new Action<LazyView<'state, 'args>, 'args>(fun control value -> control.Args <- value)
        )
        
    static let viewFuncProperty =
        AvaloniaProperty.RegisterDirect<LazyView<'state, 'args>, ('state -> 'args -> IView) option>(
            "ViewFunc",
            new Func<LazyView<'state, 'args>, option<('state -> 'args -> IView)>>(fun control -> control.ViewFunc),
            new Action<LazyView<'state, 'args>, option<('state -> 'args -> IView)>>(fun control value -> control.ViewFunc <- value)
        )
        
    member this.State
        with get () : 'state = _state
        and set (value: 'state) = this.SetAndRaise(LazyView<'state, 'args>.StateProperty, &_state, value) |> ignore
            
    member this.Args
        with get () : 'args = _args
        and set (value: 'args) = this.SetAndRaise(LazyView<'state, 'args>.ArgsProperty, &_args, value) |> ignore
            
    member this.ViewFunc
        with get () : option<('state -> 'args -> IView)> = _viewFunc
        and set (value) = this.SetAndRaise(LazyView<'state, 'args>.ViewFuncProperty, &_viewFunc, value) |> ignore  
        
    override this.OnAttachedToVisualTree args =
        let onNext (state: 'state) : unit =
            let nextView =
                match this.ViewFunc with
                | Some func ->
                    func state this.Args
                    |> Some
                    
                | None -> None
                
            (this :> IViewHost).Update nextView
        
        subscription <-
            this
                .GetObservable(LazyView<'state, 'args>.StateProperty)
                .Subscribe(onNext)
        
        
        
    static member StateProperty = stateProperty
    
    static member ArgsProperty = argsProperty
    
    static member ViewFuncProperty = viewFuncProperty

    // TODO
    // 1. implement (direct) avalonia properties for
    // - state (currently displayed state)
    // - view (the current view - needed to diff)
    // [ - cache configuration options ]
    
    // 2. Subscribe to state property changes -> (Observable.next) no diffing needed
    // 3. Update view using the virtual dom differ