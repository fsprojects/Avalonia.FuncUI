namespace Avalonia.FuncUI.VirtualDom


module Caching =
    open System.Collections.Concurrent
    open Avalonia.FuncUI.Library
    open Avalonia.FuncUI.Core.Domain

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
                
module CustomEquality =
    open System
    open System.Collections.Concurrent
    open Avalonia.FuncUI.Library
    open Avalonia.FuncUI.Core.Domain

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
                
module SubscriptionHandling =
    open System
    open System.Threading
    open System.Collections.Concurrent
    open Avalonia.FuncUI.Library
    open Avalonia.FuncUI.Core.Domain

    type SubscriptionHandles() =
        
        let subscriptions = ConcurrentDictionary<Guid, ConcurrentDictionary<string, CancellationTokenSource>>()
        
        let addOrUpdate (id: Guid, attrIdentifier: string, token: CancellationTokenSource) : unit =
            // TODO: implement
            ()
                
module Tagging =
    open System
    open Avalonia
    open Avalonia.Controls
    
    type ViewTag() =
        // normally not needed.
        inherit AvaloniaObject()
   
        static let viewId = AvaloniaProperty.RegisterAttached<ViewTag, IControl, Guid>("ViewId")
        static member ViewIdProperty = viewId
        
        static member GetViewId(control: IControl) =
            control.GetValue(ViewTag.ViewIdProperty)
            
        static member SetViewId(control: IControl, value: Guid) =
            control.SetValue(ViewTag.ViewIdProperty, value)
            
type VirtualDomEnv= {
    viewCache : Caching.LazyViewCache
    customEquality : CustomEquality.CustomEqualityImplementations
    
}