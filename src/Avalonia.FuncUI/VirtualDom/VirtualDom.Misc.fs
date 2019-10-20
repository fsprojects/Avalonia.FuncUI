namespace Avalonia.FuncUI.VirtualDom

open System.Threading
open System
open Avalonia
open Avalonia.Controls
open System.Collections.Concurrent

module ProvidedEquality =
    let private cache = ConcurrentDictionary<Type, Func<obj * obj, bool>>()
               
    let provideFor (type': Type, func: obj * obj -> bool) : unit =
        cache.AddOrUpdate(type', func, fun _ _ -> Func<obj * obj, bool>(func)) |> ignore
        
    let remove (type': Type) : bool =
        match cache.TryRemove(type') with
        | (true, _) -> true
        | (false, _) -> false
        
    let tryGet (type': Type) : Func<obj * obj, bool> option =
        match cache.TryGetValue(type') with
        | (true, impl) -> Some impl
        | (false, _) -> None

type ViewMetaData() =
    inherit AvaloniaObject()

    static let viewId = AvaloniaProperty.RegisterAttached<ViewMetaData, IControl, Guid>("ViewId")
        
    static let viewSubscriptions = AvaloniaProperty.RegisterAttached<ViewMetaData, IControl, ConcurrentDictionary<string, CancellationTokenSource>>("ViewSubscriptions")        
    
    static member ViewIdProperty = viewId
    
    static member ViewSubscriptionsProperty = viewSubscriptions
    
    static member GetViewId(control: IControl) =
        control.GetValue(ViewMetaData.ViewIdProperty)
        
    static member SetViewId(control: IControl, value: Guid) =
        control.SetValue(ViewMetaData.ViewIdProperty, value)
        
    static member GetViewSubscriptions(control: IControl) =
        control.GetValue(ViewMetaData.ViewSubscriptionsProperty)
        
    static member SetViewSubscriptions(control: IControl, value) =
        control.SetValue(ViewMetaData.ViewSubscriptionsProperty, value)
    
    
    
