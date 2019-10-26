namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module UniformGrid =
    open Avalonia.Controls.Primitives
    open Avalonia.Controls
    open Avalonia.Layout
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
   
    let create (attrs: IAttr<UniformGrid> list): IView<UniformGrid> =
        ViewBuilder.Create<UniformGrid>(attrs)

    type UniformGrid with

        /// <summary>
        /// Specifies the column count. If set to 0, column count will be calculated automatically.
        /// </summary>
        static member columns<'t when 't :> UniformGrid>(value: int) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<int>(UniformGrid.ColumnsProperty, value, ValueNone)

        /// <summary>
        /// Specifies the row count. If set to 0, row count will be calculated automatically.
        /// </summary>
        static member rows<'t when 't :> UniformGrid>(value: int) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<int>(UniformGrid.RowsProperty, value, ValueNone)
           
        /// <summary>
        /// Specifies, for the first row, the column where the items should start.
        /// </summary>
        static member firstColumn<'t when 't :> UniformGrid>(value: int) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<int>(UniformGrid.FirstColumnProperty, value, ValueNone)
