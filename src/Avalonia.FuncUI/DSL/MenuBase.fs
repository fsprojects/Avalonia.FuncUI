namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module MenuBase =
    open Avalonia.Controls
    open Avalonia.Interactivity
    open Avalonia.FuncUI.Builder
    open Avalonia.FuncUI.Types
     
    type MenuBase with
            
        static member onMenuOpened<'t when 't :> MenuBase>(func: RoutedEventArgs -> unit, ?subPatchOptions) : IAttr<'t> =
            AttrBuilder<'t>.CreateSubscription(MenuBase.OpenedEvent, func, ?subPatchOptions = subPatchOptions);
            
        static member onMenuClosed<'t when 't :> MenuBase>(func: RoutedEventArgs -> unit, ?subPatchOptions) : IAttr<'t> =
            AttrBuilder<'t>.CreateSubscription(MenuBase.ClosedEvent, func, ?subPatchOptions = subPatchOptions);