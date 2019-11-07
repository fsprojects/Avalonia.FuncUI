namespace Avalonia.FuncUI.DSL
open Avalonia.Input
open Avalonia.Interactivity
open Avalonia.Interactivity
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

        static member onIsSelectedChanged<'t when 't :> MenuItem>(func: bool -> unit) =
            AttrBuilder<'t>.CreateSubscription<bool>(MenuItem.IsSelectedProperty, func)
            
        static member isSubMenuOpen<'t when 't :> MenuItem>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(MenuItem.IsSubMenuOpenProperty, value, ValueNone)
            
        static member onIsSubMenuOpenChanged<'t when 't :> MenuItem>(func: bool -> unit) =
            AttrBuilder<'t>.CreateSubscription<bool>(MenuItem.IsSubMenuOpenProperty, func)
            
        static member onClick<'t when 't :> MenuItem>(func: RoutedEventArgs -> unit) =
            AttrBuilder<'t>.CreateSubscription<RoutedEventArgs>(MenuItem.ClickEvent, func)
            
        static member onPointerEnterItem<'t when 't :> MenuItem>(func: PointerEventArgs -> unit) =
            AttrBuilder<'t>.CreateSubscription<PointerEventArgs>(MenuItem.PointerEnterItemEvent, func)
            
        static member onPointerLeaveItem<'t when 't :> MenuItem>(func: PointerEventArgs -> unit) =
            AttrBuilder<'t>.CreateSubscription<PointerEventArgs>(MenuItem.PointerLeaveItemEvent, func)
            
        static member onSubMenuOpened<'t when 't :> MenuItem>(func: RoutedEventArgs -> unit) =
            AttrBuilder<'t>.CreateSubscription<RoutedEventArgs>(MenuItem.SubmenuOpenedEvent, func)