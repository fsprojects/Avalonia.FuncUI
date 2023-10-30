namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module GridSplitter =
    open Avalonia.Controls
    open Avalonia.FuncUI.Builder
    open Avalonia.FuncUI.Types
     
    let create (attrs: IAttr<GridSplitter> list): IView<GridSplitter> =
        ViewBuilder.Create<GridSplitter>(attrs)
     
    type GridSplitter with
        /// <summary>
        /// Restricts splitter to move a multiple of the specified units.  
        /// </summary>
        static member dragIncrement<'t when 't :> GridSplitter>(value: double) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<double>(GridSplitter.DragIncrementProperty, value, ValueNone)

        /// <summary>
        /// The Distance to move the splitter when pressing the keyboard arrow keys. 
        /// </summary>
        static member keyboardIncrement<'t when 't :> GridSplitter>(value: double) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<double>(GridSplitter.KeyboardIncrementProperty, value, ValueNone)

        ///// <summary>
        ///// Gets or sets content that will be shown when ShowsPreview is enabled and user starts resize operation.
        ///// </summary>
        static member previewContent<'t, 'c when 'c :> Control and 't :> GridSplitter>(preview: ITemplate<'c>) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<ITemplate<'c>>(GridSplitter.PreviewContentProperty, preview, ValueNone)

        /// <summary>
        /// Indicates which Columns or Rows the Splitter resizes. 
        /// </summary>
        static member resizebehavior<'t when 't :> GridSplitter>(resizeBehavior: GridResizeBehavior) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<GridResizeBehavior>(GridSplitter.ResizeBehaviorProperty, resizeBehavior, ValueNone)

        /// <summary>
        /// Indicates whether the Splitter resizes the Columns, Rows, or Both
        /// </summary>
        static member resizeDirection<'t when 't :> GridSplitter>(resizeDirection: GridResizeDirection) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<GridResizeDirection>(GridSplitter.ResizeDirectionProperty, resizeDirection, ValueNone)

        /// <summary>
        /// Indicates whether to Preview the column resizing without updating layout. 
        /// </summary>
        static member showsPreview<'t when 't :> GridSplitter>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(GridSplitter.ShowsPreviewProperty, value, ValueNone)