namespace Avalonia.FuncUI.DSL

open Avalonia.Controls

[<AutoOpen>]
module RefreshVisualizer =
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder

    let create (attrs: IAttr<RefreshVisualizer> list): IView<RefreshVisualizer> =
        ViewBuilder.Create<RefreshVisualizer>(attrs)
    
    type RefreshVisualizer with

        static member onRefreshRequested<'t when 't :> RefreshVisualizer>(func: RefreshRequestedEventArgs -> unit, ?subPatchOptions) : IAttr<'t> =
            AttrBuilder<'t>.CreateSubscription<RefreshRequestedEventArgs>(RefreshVisualizer.RefreshRequestedEvent, func, ?subPatchOptions = subPatchOptions)

        static member orientation<'t when 't :> RefreshVisualizer>(value: RefreshVisualizerOrientation) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty(RefreshVisualizer.OrientationProperty, value, ValueNone)        
