namespace Avalonia.FuncUI.DSL
open Avalonia.FuncUI.Builder

[<AutoOpen>]
module Popup =
    open Avalonia.Controls
    open Avalonia.Controls.Primitives
    open Avalonia.FuncUI.Types
    
    let create (attrs: IAttr<Popup> list): IView<Popup> =
        ViewBuilder.Create<Popup>(attrs)
    
    type Popup with
        
        static member child<'t when 't :> Popup>(value: IView option) : IAttr<'t> =
            AttrBuilder.CreateContentSingle(Popup.ChildProperty, value)
        
        static member child<'t when 't :> Popup>(value: IView) : IAttr<'t> =
            value |> Some |> Popup.child
            
        static member isOpen<'t when 't :> Popup>(value: bool) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, bool>(Popup.IsOpenProperty, value, ValueNone)
            
        static member staysOpen<'t when 't :> Popup>(value: bool) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, bool>(Popup.StaysOpenProperty, value, ValueNone)
            
        static member topmost<'t when 't :> Popup>(value: bool) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, bool>(Popup.TopmostProperty, value, ValueNone)
            
        static member placementMode<'t when 't :> Popup>(value: PlacementMode) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, PlacementMode>(Popup.PlacementModeProperty, value, ValueNone)
            
        static member placementTarget<'t when 't :> Popup>(value: Control) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, Control>(Popup.PlacementTargetProperty, value, ValueNone)
            
        static member verticalOffset<'t when 't :> Popup>(value: double) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, double>(Popup.VerticalOffsetProperty, value, ValueNone)
            
        static member horizontalOffset<'t when 't :> Popup>(value: double) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, double>(Popup.HorizontalOffsetProperty, value, ValueNone)    