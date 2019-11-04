namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module Path =
    open Avalonia.Media
    open Avalonia.Controls.Shapes
    open Avalonia.FuncUI.Builder
    open Avalonia.FuncUI.Types
    
    let create (attrs: IAttr<Path> list): IView<Path> =
        ViewBuilder.Create<Path>(attrs)
     
    type Path with

        static member data<'t when 't :> Path>(geometry: Geometry) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<Geometry>(Path.DataProperty, geometry, ValueNone)
            
        static member data<'t when 't :> Path>(data: string) : IAttr<'t> =
            data |> Geometry.Parse |> Path.data