namespace Avalonia.FuncUI.VirtualDom

open System.Threading
open System
open Avalonia
open Avalonia.Controls
open System.Collections.Concurrent

type ViewMetaData() =
    inherit AvaloniaObject()

    static let viewId = AvaloniaProperty.RegisterAttached<ViewMetaData, IControl, Guid>("ViewId")

    /// Avalonia automatically adds subscriptions that are setup in XAML to a disposable bag (or something along the lines).
    /// This basically is what FuncUI uses instead to make sure it cancels subscriptions.
    static let viewSubscriptions = AvaloniaProperty.RegisterAttached<ViewMetaData, IControl, ConcurrentDictionary<string, CancellationTokenSource>>("ViewSubscriptions")

    static member ViewIdProperty = viewId

    static member ViewSubscriptionsProperty = viewSubscriptions

    static member GetViewId(control: IControl) : Guid =
        control.GetValue(ViewMetaData.ViewIdProperty)

    static member SetViewId(control: IControl, value: Guid) : unit =
        control.SetValue(ViewMetaData.ViewIdProperty, value) |> ignore

    static member GetViewSubscriptions(control: IControl) : ConcurrentDictionary<_, _> =
        control.GetValue(ViewMetaData.ViewSubscriptionsProperty)

    static member SetViewSubscriptions(control: IControl, value) : unit =
        control.SetValue(ViewMetaData.ViewSubscriptionsProperty, value) |> ignore


