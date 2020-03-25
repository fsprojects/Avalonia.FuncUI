namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module Track =
    open Avalonia.Layout
    open Avalonia.Controls.Primitives
    open Avalonia.Controls
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
    
    let create (attrs: IAttr<Track> list): IView<Track> =
        ViewBuilder.Create<Track>(attrs)
     
    type Track with

        static member minimum<'t when 't :> Track>(value: double) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<double>(Track.MinimumProperty, value, ValueNone)
            
        static member maximum<'t when 't :> Track>(value: double) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<double>(Track.MaximumProperty, value, ValueNone)
            
        static member value<'t when 't :> Track>(value: double) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<double>(Track.ValueProperty, value, ValueNone)
            
        static member onValueChanged<'t when 't :> Track>(func: double -> unit, subPatchOptions) : IAttr<'t> =
            AttrBuilder<'t>.CreateSubscription<double>(Track.ValueProperty, func, subPatchOptions)

        static member viewportSize<'t when 't :> Track>(value: double) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<double>(Track.ViewportSizeProperty, value, ValueNone)
            
        static member orientation<'t when 't :> Track>(orientation: Orientation) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<Orientation>(Track.OrientationProperty, orientation, ValueNone)
            
        static member isDirectionReversed<'t when 't :> Track>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(Track.IsDirectionReversedProperty, value, ValueNone)
            
        static member thumb<'t when 't :> Track>(value: Thumb) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<Thumb>(Track.ThumbProperty, value, ValueNone)
            
        static member thumb<'t when 't :> Track>(value: IView<Thumb> voption) : IAttr<'t> =
            let value =
                match value with
                | ValueSome view -> Some (view :> IView)
                | ValueNone -> None
            
            AttrBuilder<'t>.CreateContentSingle(Track.ThumbProperty, value)
            
        static member thumb<'t when 't :> Track>(value: IView<Thumb>) : IAttr<'t> =
            value |> ValueSome |> Track.thumb
       
        static member increaseButton<'t when 't :> Track>(value: Button) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<Button>(Track.IncreaseButtonProperty, value, ValueNone)
            
        static member increaseButton<'t when 't :> Track>(value: IView<Button> voption) : IAttr<'t> =
            let value =
                match value with
                | ValueSome view -> Some (view :> IView)
                | ValueNone -> None
            
            AttrBuilder<'t>.CreateContentSingle(Track.IncreaseButtonProperty, value)
            
        static member increaseButton<'t when 't :> Track>(value: IView<Button>) : IAttr<'t> =
            value |> ValueSome |> Track.increaseButton 
            
        static member decreaseButton<'t when 't :> Track>(value: Button) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<Button>(Track.DecreaseButtonProperty, value, ValueNone)
            
        static member decreaseButton<'t when 't :> Track>(value: IView<Button> voption) : IAttr<'t> =
            let value =
                match value with
                | ValueSome view -> Some (view :> IView)
                | ValueNone -> None
            
            AttrBuilder<'t>.CreateContentSingle(Track.DecreaseButtonProperty, value)
            
        static member decreaseButton<'t when 't :> Track>(value: IView<Button>) : IAttr<'t> =
            value |> ValueSome |> Track.decreaseButton 