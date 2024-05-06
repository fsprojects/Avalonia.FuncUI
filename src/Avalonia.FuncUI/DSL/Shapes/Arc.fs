namespace Avalonia.FuncUI.DSL

open Avalonia.Controls.Shapes

[<AutoOpen>]
module Arc =
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder

    let create (attrs: IAttr<Arc> list): IView<Arc> =
        ViewBuilder.Create<Arc>(attrs)

    type Arc with
        static member startAngle<'t when 't :> Arc>(value: double) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<double>(Arc.StartAngleProperty, value, ValueNone)

        static member swwpAngle<'t when 't :> Arc>(value: double) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<double>(Arc.SweepAngleProperty, value, ValueNone)
