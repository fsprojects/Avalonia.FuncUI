namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module DragDrop =
    open Avalonia.Controls
    open Avalonia.Input
    open Avalonia.FuncUI.Builder
    open Avalonia.FuncUI.Types

    type Control with

        static member onDragEnter<'t when 't :> Control> (func: DragEventArgs -> unit, ?subPatchOptions) =
            AttrBuilder<'t>.CreateSubscription<DragEventArgs>
                (DragDrop.DragEnterEvent, func, ?subPatchOptions = subPatchOptions)

        static member onDragLeave<'t when 't :> Control> (func: DragEventArgs -> unit, ?subPatchOptions) =
            AttrBuilder<'t>.CreateSubscription<DragEventArgs>
                (DragDrop.DragLeaveEvent, func, ?subPatchOptions = subPatchOptions)

        static member onDragOver<'t when 't :> Control> (func: DragEventArgs -> unit, ?subPatchOptions) =
            AttrBuilder<'t>.CreateSubscription<DragEventArgs>
                (DragDrop.DragOverEvent, func, ?subPatchOptions = subPatchOptions)

        static member onDrop<'t when 't :> Control> (func: DragEventArgs -> unit, ?subPatchOptions) =
            AttrBuilder<'t>.CreateSubscription<DragEventArgs>
                (DragDrop.DropEvent, func, ?subPatchOptions = subPatchOptions)

        static member allowDrop<'t when 't :> Control> (allow: bool): IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool> (DragDrop.AllowDropProperty, allow, ValueNone)
