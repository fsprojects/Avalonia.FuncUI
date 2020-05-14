namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module Thumb =
    open Avalonia.Input
    open Avalonia.Controls.Primitives
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
    
    let create (attrs: IAttr<Thumb> list): IView<Thumb> =
        ViewBuilder.Create<Thumb>(attrs)
     
    type Thumb with

        static member onDragStarted<'t when 't :> Thumb>(func: VectorEventArgs -> unit, ?subPatchOptions) : IAttr<'t> =
            AttrBuilder.CreateSubscription<'t, VectorEventArgs>(Thumb.DragStartedEvent, func, ?subPatchOptions = subPatchOptions)
            
        static member onDragDelta<'t when 't :> Thumb>(func: VectorEventArgs -> unit, ?subPatchOptions) : IAttr<'t> =
            AttrBuilder.CreateSubscription<'t, VectorEventArgs>(Thumb.DragDeltaEvent, func, ?subPatchOptions = subPatchOptions)
            
        static member onDragCompleted<'t when 't :> Thumb>(func: VectorEventArgs -> unit, ?subPatchOptions) : IAttr<'t> =
            AttrBuilder.CreateSubscription<'t, VectorEventArgs>(Thumb.DragCompletedEvent, func, ?subPatchOptions = subPatchOptions)