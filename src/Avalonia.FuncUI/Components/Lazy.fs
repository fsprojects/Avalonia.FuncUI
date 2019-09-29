namespace Avalonia.FuncUI.Components.Hosts

open Avalonia
open Avalonia.Controls
open Avalonia.FuncUI.Types
open Avalonia.FuncUI.VirtualDom

type LazyView<'state>() as this =
    inherit HostControl()

    // TODO
    // 1. implement (direct) avalonia properties for
    // - state (currently displayed state)
    // - view (the current view - needed to diff)
    // [ - cache configuration options ]
    
    // 2. Subscribe to state property changes -> (Observable.next) no diffing needed
    // 3. Update view using the virtual dom differ