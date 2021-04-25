namespace Avalonia.FuncUI.VirtualDom

open System.Threading
open System
open Avalonia
open Avalonia.Controls
open System.Collections.Concurrent
open Avalonia.FuncUI.Types

module ActivePatterns =
    let internal (|Property'|_|) (attr: IAttr)  =
        attr.Property
    
    let internal (|Content'|_|) (attr: IAttr)  =
        attr.Content
        
    let internal (|Subscription'|_|) (attr: IAttr)  =
        attr.Subscription
        
    let internal (|ControlledProperty'|_|) (attr: IAttr) =
        attr.ControlledProperty

type ViewMetaData() =
    inherit AvaloniaObject()

    static let viewId = AvaloniaProperty.RegisterAttached<ViewMetaData, IControl, Guid>("ViewId")
        
    /// Avalonia automatically adds subscriptions that are setup in XAML to a disposable bag (or something along the lines).
    /// This basically is what FuncUI uses instead to make sure it cancels subscriptions. 
    static let viewSubscriptions = AvaloniaProperty.RegisterAttached<ViewMetaData, IControl, ConcurrentDictionary<string, CancellationTokenSource>>("ViewSubscriptions")    
    static let viewPropertyControllers = AvaloniaProperty.RegisterAttached<ViewMetaData, IControl, ConcurrentDictionary<string, PropertyController>>("ViewPropertyControllers")
    
    static member ViewIdProperty = viewId
    
    static member ViewSubscriptionsProperty = viewSubscriptions
    static member ViewPropertyControllersProperty = viewPropertyControllers
    static member GetViewId(control: IControl) : Guid =
        control.GetValue(ViewMetaData.ViewIdProperty)
        
    static member SetViewId(control: IControl, value: Guid) : unit =
        control.SetValue(ViewMetaData.ViewIdProperty, value) |> ignore
        
    static member GetViewSubscriptions(control: IControl) : ConcurrentDictionary<_, _> =
        control.GetValue(ViewMetaData.ViewSubscriptionsProperty)
        
    static member SetViewSubscriptions(control: IControl, value) : unit =
        control.SetValue(ViewMetaData.ViewSubscriptionsProperty, value) |> ignore        
    
    static member GetViewPropertyControllers(control: IControl) : ConcurrentDictionary<_, _> =
        control.GetValue(ViewMetaData.ViewPropertyControllersProperty)
        
    static member SetViewPropertyControllers(control: IControl, value) : unit =
        control.SetValue(ViewMetaData.ViewPropertyControllersProperty, value) |> ignore
