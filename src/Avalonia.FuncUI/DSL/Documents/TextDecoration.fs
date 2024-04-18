namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module TextDecoration =
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
    open Avalonia.Media
    open Avalonia.Media.Immutable
    open Avalonia.Collections

    let create(attrs: IAttr<TextDecoration> list): IView<TextDecoration> =
        ViewBuilder.Create<TextDecoration>(attrs)
    
    module private Internals =
        open System.Linq
        let patchStrokeDashArray (strokeDashArray: AvaloniaList<float>) (newValues: float seq) =
            if Seq.isEmpty newValues then
                strokeDashArray.Clear()
            else
                strokeDashArray |> Seq.except newValues |> strokeDashArray.RemoveAll

                for newIndex, value in Seq.indexed newValues do
                    let oldValue = strokeDashArray |> Seq.tryFindIndex (fun x -> x = value)

                    match oldValue with
                    | Some oldValue when oldValue = newIndex -> ()
                    | Some oldValue -> strokeDashArray.Move(oldValue, newIndex)
                    | None -> strokeDashArray.Insert(newIndex, value)

        let compareStrokeDashArray<'e when 'e :> float seq> (a: obj) (b: obj) =
            let a = a :?> 'e
            let b = b :?> 'e
            
            Enumerable.SequenceEqual(a, b)

    type TextDecoration with
        static member location<'t when 't :> TextDecoration>(value: TextDecorationLocation) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<TextDecorationLocation>(TextDecoration.LocationProperty, value, ValueNone)
        
        static member stroke<'t when 't :> TextDecoration>(value: IBrush) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<IBrush>(TextDecoration.StrokeProperty, value, ValueNone)
        
        static member stroke<'t when 't :> TextDecoration>(s: string) : IAttr<'t> =
            SolidColorBrush.Parse s |> TextDecoration.stroke

        static member stroke<'t when 't :> TextDecoration>(color: Color) : IAttr<'t> =
            ImmutableSolidColorBrush color |> TextDecoration.stroke
        
        static member strokeThicknessUnit<'t when 't :> TextDecoration>(value: TextDecorationUnit) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<TextDecorationUnit>(TextDecoration.StrokeThicknessUnitProperty, value, ValueNone)
        
        static member strokeDashArray<'t when 't :> TextDecoration>(value: AvaloniaList<float>) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<AvaloniaList<float>>(TextDecoration.StrokeDashArrayProperty, value, ValueNone)
        
        static member strokeDashArray<'t when 't :> TextDecoration>(value: float list) : IAttr<'t> =
            let name = nameof Unchecked.defaultof<'t>.StrokeDashArray
            let getter: 't -> float list = (fun control -> Seq.toList control.StrokeDashArray)
            let setter: 't * float list -> unit = (fun (control, value) -> Internals.patchStrokeDashArray control.StrokeDashArray value)
            let compare: 'obj * 'obj -> bool = fun (a, b) -> Internals.compareStrokeDashArray<float list> a b
            let factory = fun () -> []

            AttrBuilder<'t>.CreateProperty<float list>(name, value, ValueSome getter, ValueSome setter, ValueSome compare, factory)

        static member strokeDashOffset<'t when 't :> TextDecoration>(value: float) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<float>(TextDecoration.StrokeDashOffsetProperty, value, ValueNone)
        
        static member strokeThickness<'t when 't :> TextDecoration>(value: float) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<float>(TextDecoration.StrokeThicknessProperty, value, ValueNone)
        
        static member strokeLineCap<'t when 't :> TextDecoration>(value: PenLineCap) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<PenLineCap>(TextDecoration.StrokeLineCapProperty, value, ValueNone)

        static member strokeOffset<'t when 't :> TextDecoration>(value: double) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<double>(TextDecoration.StrokeOffsetProperty, value, ValueNone)
        
        static member strokeOffsetUnit<'t when 't :> TextDecoration>(value: TextDecorationUnit) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<TextDecorationUnit>(TextDecoration.StrokeOffsetUnitProperty, value, ValueNone)
