namespace Avalonia.FuncUI.DSL
open Avalonia.Input
open Avalonia.Interactivity
open System.Windows.Input

[<AutoOpen>]
module MenuItem =
    open Avalonia.Controls
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
    
    let create (attrs: IAttr<MenuItem> list): IView<MenuItem> =
        ViewBuilder.Create<MenuItem>(attrs)
     
    type MenuItem with

        static member command<'t when 't :> MenuItem>(command: ICommand) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<ICommand>(MenuItem.CommandProperty, command, ValueNone)
            
        static member commandParameter<'t when 't :> MenuItem>(parameter: obj) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<obj>(MenuItem.CommandParameterProperty, parameter, ValueNone)
        
        static member hotKey<'t when 't :> MenuItem>(value: KeyGesture) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<KeyGesture>(MenuItem.HotKeyProperty, value, ValueNone)
        
        static member icon<'t when 't :> MenuItem>(value: obj) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<obj>(MenuItem.IconProperty, value, ValueNone)     
        
        static member isSelected<'t when 't :> MenuItem>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(MenuItem.IsSelectedProperty, value, ValueNone)

        static member onIsSelectedChanged<'t when 't :> MenuItem>(func: bool -> unit, ?subPatchOptions) =
            AttrBuilder<'t>.CreateSubscription<bool>(MenuItem.IsSelectedProperty, func, ?subPatchOptions = subPatchOptions)
            
        static member isSubMenuOpen<'t when 't :> MenuItem>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(MenuItem.IsSubMenuOpenProperty, value, ValueNone)
            
        static member onIsSubMenuOpenChanged<'t when 't :> MenuItem>(func: bool -> unit, ?subPatchOptions) =
            AttrBuilder<'t>.CreateSubscription<bool>(MenuItem.IsSubMenuOpenProperty, func, ?subPatchOptions = subPatchOptions)
            
        static member onClick<'t when 't :> MenuItem>(func: RoutedEventArgs -> unit, ?subPatchOptions) =
            AttrBuilder<'t>.CreateSubscription<RoutedEventArgs>(MenuItem.ClickEvent, func, ?subPatchOptions = subPatchOptions)
            
        static member onPointerEnterItem<'t when 't :> MenuItem>(func: PointerEventArgs -> unit, ?subPatchOptions) =
            AttrBuilder<'t>.CreateSubscription<PointerEventArgs>(MenuItem.PointerEnterItemEvent, func, ?subPatchOptions = subPatchOptions)
            
        static member onPointerLeaveItem<'t when 't :> MenuItem>(func: PointerEventArgs -> unit, ?subPatchOptions) =
            AttrBuilder<'t>.CreateSubscription<PointerEventArgs>(MenuItem.PointerLeaveItemEvent, func, ?subPatchOptions = subPatchOptions)
            
        static member onSubMenuOpened<'t when 't :> MenuItem>(func: RoutedEventArgs -> unit, ?subPatchOptions) =
            AttrBuilder<'t>.CreateSubscription<RoutedEventArgs>(MenuItem.SubmenuOpenedEvent, func, ?subPatchOptions = subPatchOptions)