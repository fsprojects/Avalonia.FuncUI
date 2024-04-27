namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module Visual =  
    open Avalonia
    open Avalonia.Media
    open Avalonia.FuncUI
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
    open System.Threading
            
    type Visual with
        static member onAttachedToVisualTree<'t when 't :> Visual>(func: VisualTreeAttachmentEventArgs -> unit, ?subPatchOptions) =
            let name = nameof Unchecked.defaultof<'t>.AttachedToVisualTree
            let factory: AvaloniaObject * (VisualTreeAttachmentEventArgs -> unit) * CancellationToken -> unit =
                (fun (control, func, token) ->
                    let control = control :?> 't
                    let disposable = control.AttachedToVisualTree.Subscribe(func)

                    token.Register(fun () -> disposable.Dispose()) |> ignore)

            AttrBuilder<'t>.CreateSubscription<VisualTreeAttachmentEventArgs>(name, factory, func, ?subPatchOptions = subPatchOptions)

        static member onDetachedFromVisualTree<'t when 't :> Visual>(func: VisualTreeAttachmentEventArgs -> unit, ?subPatchOptions) =
            let name = nameof Unchecked.defaultof<'t>.DetachedFromVisualTree
            let factory:AvaloniaObject * (VisualTreeAttachmentEventArgs -> unit) * CancellationToken -> unit =
                (fun (control, func, token) ->
                    let control = control :?> 't
                    let disposable = control.DetachedFromVisualTree.Subscribe(func)

                    token.Register(fun () -> disposable.Dispose()) |> ignore)

            AttrBuilder<'t>.CreateSubscription<VisualTreeAttachmentEventArgs>(name, factory, func, ?subPatchOptions = subPatchOptions)

        static member clipToBounds<'t when 't :> Visual>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(Visual.ClipToBoundsProperty, value, ValueNone)
        
        static member clip<'t when 't :> Visual>(mask: Geometry) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<Geometry>(Visual.ClipProperty, mask, ValueNone)
            
        static member isVisible<'t when 't :> Visual>(visible: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(Visual.IsVisibleProperty, visible, ValueNone)
    
        static member opacity<'t when 't :> Visual>(value: float) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<float>(Visual.OpacityProperty, value, ValueNone)
      
        static member opacityMask<'t when 't :> Visual>(value: IBrush) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<IBrush>(Visual.OpacityMaskProperty, value, ValueNone)

        static member effect<'t when 't :> Visual>(value: IEffect) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<IEffect>(Visual.EffectProperty, value, ValueNone)

        static member renderTransform<'t when 't :> Visual>(transform: ITransform) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<ITransform>(Visual.RenderTransformProperty, transform, ValueSome EqualityComparers.compareTransforms)
    
        static member renderTransformOrigin<'t when 't :> Visual>(origin: RelativePoint) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<RelativePoint>(Visual.RenderTransformOriginProperty, origin, ValueNone)

        static member flowDirection<'t when 't :> Visual>(direction: FlowDirection) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<FlowDirection>(Visual.FlowDirectionProperty, direction, ValueNone)

        static member zIndex<'t when 't :> Visual>(index: int) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<int>(Visual.ZIndexProperty, index, ValueNone)