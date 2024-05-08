namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module TickBar = 
    open Avalonia
    open Avalonia.Collections
    open Avalonia.Controls
    open Avalonia.Media
    open Avalonia.Media.Immutable
    open Avalonia.Layout
    open Avalonia.FuncUI
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder

    let create (attrs: IAttr<TickBar> list): IView<TickBar> =
        ViewBuilder.Create<TickBar>(attrs)

    type TickBar with

        static member fill<'t when 't :> TickBar>(value: IBrush) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<IBrush>(TickBar.FillProperty, value, ValueNone)

        static member fill<'t when 't :> TickBar>(value: Color) : IAttr<'t> =
            value |> ImmutableSolidColorBrush |> TickBar.fill

        static member fill<'t when 't :> TickBar>(color: string) : IAttr<'t> =
            color |> Color.Parse |> TickBar.fill

        static member isDirectionReversed<'t when 't :> TickBar>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(TickBar.IsDirectionReversedProperty, value, ValueNone)

        static member maximum<'t when 't :> TickBar>(value: float) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<float>(TickBar.MaximumProperty, value, ValueNone)

        static member minimum<'t when 't :> TickBar>(value: float) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<float>(TickBar.MinimumProperty, value, ValueNone)

        static member orientation<'t when 't :> TickBar>(value: Orientation) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<Orientation>(TickBar.OrientationProperty, value, ValueNone)

        static member placement<'t when 't :> TickBar>(value: TickBarPlacement) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<TickBarPlacement>(TickBar.PlacementProperty, value, ValueNone)

        static member reservedSpace<'t when 't :> TickBar>(value: Rect) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<Rect>(TickBar.ReservedSpaceProperty, value, ValueNone)

        static member tickFrequency<'t when 't :> TickBar>(value: float) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<float>(TickBar.TickFrequencyProperty, value, ValueNone)

        static member ticks<'t when 't :> TickBar>(value: AvaloniaList<float>) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<float AvaloniaList>(TickBar.TicksProperty, value, ValueNone)

        static member ticks<'t when 't :> TickBar>(value: seq<float>) : IAttr<'t> =
            let name = nameof Unchecked.defaultof<'t>.Ticks
            let getter: 't -> seq<float> = (fun control -> control.Ticks)
            let setter: 't * seq<float> -> unit = (fun (control, value) -> Setters.avaloniaList control.Ticks value)
            let compare: obj * obj -> bool = fun (a, b) -> EqualityComparers.compareSeq<_,float>(a, b)
            let factory = fun () -> Seq.empty

            AttrBuilder<'t>.CreateProperty<seq<float>>(name, value, ValueSome getter, ValueSome setter, ValueSome compare, factory)
