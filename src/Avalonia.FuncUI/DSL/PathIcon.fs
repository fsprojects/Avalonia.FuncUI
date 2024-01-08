namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module PathIcon =
    open Avalonia.Media
    open Avalonia.FuncUI.Builder
    open Avalonia.FuncUI.Types
    open Avalonia.Controls
    
    let create (attrs: IAttr<PathIcon> list): IView<PathIcon> =
        ViewBuilder.Create<PathIcon>(attrs)
     
    type PathIcon with

        static member data<'t when 't :> PathIcon>(geometry: Geometry) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<Geometry>(PathIcon.DataProperty, geometry, ValueNone)
            
        static member data<'t when 't :> PathIcon>(data: string) : IAttr<'t> =
            data |> Geometry.Parse |> PathIcon.data