namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module Layoutable =  
    open System.Threading
    open Avalonia
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
    open Avalonia.Layout
    open System
              
    type Layoutable with
        static member onEffectiveViewportChanged<'t when 't :> Layoutable>(func: EffectiveViewportChangedEventArgs -> unit, ?subPatchOptions) : IAttr<'t> =
            let name = nameof Unchecked.defaultof<'t>.EffectiveViewportChanged
            let factory: AvaloniaObject * (EffectiveViewportChangedEventArgs -> unit) * CancellationToken -> unit =
                (fun (control, func, token) ->
                    let control = control :?> 't
                    let disposable = control.EffectiveViewportChanged.Subscribe(func)

                    token.Register(fun () -> disposable.Dispose()) |> ignore)
            
            AttrBuilder<'t>.CreateSubscription<EffectiveViewportChangedEventArgs>(name, factory, func, ?subPatchOptions = subPatchOptions)

        static member onLayoutUpdated<'t when 't :> Layoutable>(func: 't -> unit, ?subPatchOptions) : IAttr<'t> =
            let name = nameof Unchecked.defaultof<'t>.LayoutUpdated
            let factory: AvaloniaObject * ('t -> unit) * CancellationToken -> unit =
                (fun (control, func, token) ->
                    let control = control :?> 't
                    let handler = EventHandler(fun s _ -> func (s :?> 't))
                    let event = control.LayoutUpdated

                    event.AddHandler(handler)
                    token.Register(fun () -> event.RemoveHandler(handler)) |> ignore)
            
            AttrBuilder<'t>.CreateSubscription<'t>(name, factory, func, ?subPatchOptions = subPatchOptions)

        static member width<'t when 't :> Layoutable>(value: double) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<double>(Layoutable.WidthProperty, value, ValueNone)
            
        static member height<'t when 't :> Layoutable>(value: double) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<double>(Layoutable.HeightProperty, value, ValueNone)
            
        static member minWidth<'t when 't :> Layoutable>(value: double) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<double>(Layoutable.MinWidthProperty, value, ValueNone)
            
        static member minHeight<'t when 't :> Layoutable>(value: double) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<double>(Layoutable.MinHeightProperty, value, ValueNone)
      
        static member maxWidth<'t when 't :> Layoutable>(value: double) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<double>(Layoutable.MaxWidthProperty, value, ValueNone)
            
        static member maxHeight<'t when 't :> Layoutable>(value: double) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<double>(Layoutable.MaxHeightProperty, value, ValueNone)
            
        static member margin<'t when 't :> Layoutable>(margin: Thickness) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<Thickness>(Layoutable.MarginProperty, margin, ValueNone)
            
        static member margin<'t when 't :> Layoutable>(margin: float) : IAttr<'t> =
            Thickness(margin) |> Layoutable.margin
            
        static member margin<'t when 't :> Layoutable>(horizontal: float, vertical: float) : IAttr<'t> =
            Thickness(horizontal, vertical) |> Layoutable.margin
            
        static member margin<'t when 't :> Layoutable>(left: float, top: float, right: float, bottom: float) : IAttr<'t> =
            Thickness(left, top, right, bottom) |> Layoutable.margin
    
        static member horizontalAlignment<'t when 't :> Layoutable>(value: HorizontalAlignment) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<HorizontalAlignment>(Layoutable.HorizontalAlignmentProperty, value, ValueNone)
   
        static member verticalAlignment<'t when 't :> Layoutable>(value: VerticalAlignment) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<VerticalAlignment>(Layoutable.VerticalAlignmentProperty, value, ValueNone)
           
        static member useLayoutRounding<'t when 't :> Layoutable>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(Layoutable.UseLayoutRoundingProperty, value, ValueNone)