namespace Avalonia.FuncUI.DSL

open Avalonia.Controls
open Avalonia.Input

[<AutoOpen>]
module RefreshContainer =
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
    
    let create (attrs: IAttr<RefreshContainer> list): IView<RefreshContainer> =
        ViewBuilder.Create<RefreshContainer>(attrs)
     
    type RefreshContainer with

        static member onRefreshRequested<'t when 't :> RefreshContainer>(func: RefreshRequestedEventArgs -> unit, ?subPatchOptions) =
            AttrBuilder<'t>.CreateSubscription<RefreshRequestedEventArgs>(RefreshContainer.RefreshRequestedEvent, func, ?subPatchOptions = subPatchOptions)

        static member refreshVisualizer<'t when 't :> RefreshContainer>(value: RefreshVisualizer) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty(RefreshContainer.VisualizerProperty, value, ValueNone)

        static member pullDirection<'t when 't :> RefreshContainer>(value: PullDirection) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty(RefreshContainer.PullDirectionProperty, value, ValueNone)
