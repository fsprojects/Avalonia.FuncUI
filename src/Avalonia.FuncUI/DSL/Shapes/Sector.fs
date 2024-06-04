namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module Sector =
    open Avalonia.Controls.Shapes
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder

    let create (attrs: IAttr<Sector> list): IView<Sector> =
        ViewBuilder.Create<Sector>(attrs)

    type Sector with
        static member startAngle<'t when 't :> Sector>(value: double) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<double>(Sector.StartAngleProperty, value, ValueNone)

        static member sweepAngle<'t when 't :> Sector>(value: double) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<double>(Sector.SweepAngleProperty, value, ValueNone)
