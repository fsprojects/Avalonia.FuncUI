namespace Avalonia.FuncUI.VirtualDom
 
module Caching =
    open System.Collections.Concurrent
    open Avalonia.FuncUI.Library
    open Avalonia.FuncUI.Types

    type LazyViewCache() =
        
        // TODO: 
        // maybe add a timestamp to each cached value so
        // last used values can be kept / only not used elements
        // can be deleted
        
        let cache = ConcurrentDictionary<string, IView>()
        let maxLength = 1000
        
        let getOrAdd (state: 'state ,args: 'args, func: 'state -> 'args -> IView) : IView =
            let hash = Hashing.hash (state, func)
            
            if (cache.Count >= maxLength) then
                cache.Clear()
            
            let isCached, view = cache.TryGetValue hash

            match isCached with
            | true -> view
            | false ->
                cache.AddOrUpdate(
                    hash,
                    (fun _ -> func state args),
                    (fun _ _ -> func state args)
                )
                
        member __.GetOrAdd (state: 'state ,args: 'args, func: 'state -> 'args -> IView) : IView =
            getOrAdd (state, args, func)
            
module FunctionAnalysis =
    open System
    open System.Reflection
    open System.Collections.Concurrent
    
    let internal cache = ConcurrentDictionary<Type, bool>()
    
    let private flags =
        BindingFlags.Instance |||
        BindingFlags.NonPublic |||
        BindingFlags.Public
    
    let capturesState (func : 'a -> 'b) : bool =
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

module CustomEquality =
    open System
    open System.Collections.Concurrent
    open Avalonia.FuncUI.Library
    open Avalonia.FuncUI.Types

    type CustomEqualityImplementations() =
              
        let cache = ConcurrentDictionary<Type, Func<obj * obj, bool>>()
        
        let addCustom (type': Type, implementation: obj * obj -> bool) : unit =
            let func = Func<obj * obj, bool>(implementation)
            
            cache.AddOrUpdate(type', func, (fun t v -> func))
            |> ignore
            
        let hasCustomImplementation (type': Type) : Func<obj * obj, bool> option =
            let hasImplementation, implementation = cache.TryGetValue(type')
            
            if hasImplementation then
                Some implementation
            else
                None
                
        member __.AddCustomImplementation (type': Type, implementation: obj * obj -> bool) : unit =
            addCustom (type', implementation)
            
        member __.HasCustomImplementation (type': Type) : Func<obj * obj, bool> option =
            hasCustomImplementation type'

module Tagging =
    open System
    open System.Threading
    open Avalonia
    open Avalonia.Controls
    open System.Collections.Concurrent
    
    type ViewTag() =
        // normally not needed.
        inherit AvaloniaObject()
   
        static let viewId = AvaloniaProperty.RegisterAttached<ViewTag, IControl, Guid>("ViewId")
            
        static let viewSubscriptions = AvaloniaProperty.RegisterAttached<ViewTag, IControl, ConcurrentDictionary<string, CancellationTokenSource>>("ViewSubscriptions")        
        
        static member ViewIdProperty = viewId
        
        static member ViewSubscriptionsProperty = viewSubscriptions
        
        static member GetViewId(control: IControl) =
            control.GetValue(ViewTag.ViewIdProperty)
            
        static member SetViewId(control: IControl, value: Guid) =
            control.SetValue(ViewTag.ViewIdProperty, value)
            
        static member GetViewSubscriptions(control: IControl) =
            control.GetValue(ViewTag.ViewSubscriptionsProperty)
            
        static member SetViewSubscriptions(control: IControl, value) =
            control.SetValue(ViewTag.ViewSubscriptionsProperty, value)
            
type VirtualDomEnv= {
    viewCache : Caching.LazyViewCache
    customEquality : CustomEquality.CustomEqualityImplementations
    
}
