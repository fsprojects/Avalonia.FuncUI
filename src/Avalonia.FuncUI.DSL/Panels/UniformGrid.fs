namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module UniformGrid =
    open Avalonia.Controls.Primitives
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
   
    let create (attrs: IAttr<UniformGrid> list): IView<UniformGrid> =
        ViewBuilder.Create<UniformGrid>(attrs)

    type UniformGrid with
        static member rows<'t when 't :> UniformGrid>(count: int) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<int>(UniformGrid.RowsProperty, count, ValueNone)
            
        static member columns<'t when 't :> UniformGrid>(count: int) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<int>(UniformGrid.ColumnsProperty, count, ValueNone)
            
        static member firstColumn<'t when 't :> UniformGrid>(column: int) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<int>(UniformGrid.FirstColumnProperty, column, ValueNone)