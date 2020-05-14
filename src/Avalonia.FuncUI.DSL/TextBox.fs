namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module TextBox =  
    open Avalonia.Controls
    open Avalonia.Media.Immutable
    open Avalonia.Media
    open Avalonia.FuncUI.Builder
    open Avalonia.FuncUI.Types
    
    let create (attrs: IAttr<TextBox> list): IView<TextBox> =
        ViewBuilder.Create<TextBox>(attrs)
    
    type TextBox with

        static member acceptsReturn<'t when 't :> TextBox>(value: bool) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, bool>(TextBox.AcceptsReturnProperty, value, ValueNone)

        static member acceptsTab<'t when 't :> TextBox>(value: bool) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, bool>(TextBox.AcceptsTabProperty, value, ValueNone)        

        static member caretIndex<'t when 't :> TextBox>(value: int) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, int>(TextBox.CaretIndexProperty, value, ValueNone)                
                        
        static member isReadOnly<'t when 't :> TextBox>(value: bool) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, bool>(TextBox.IsReadOnlyProperty, value, ValueNone)                        
                        
        static member passwordChar<'t when 't :> TextBox>(value: char) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, char>(TextBox.PasswordCharProperty, value, ValueNone)
            
        static member selectionBrush<'t when 't :> TextBox>(value: IBrush) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, IBrush>(TextBox.SelectionBrushProperty, value, ValueNone)
            
        static member selectionBrush<'t when 't :> TextBox>(color: string) : IAttr<'t> =
            color |> Color.Parse |> ImmutableSolidColorBrush |> TextBox.selectionBrush
            
        static member selectionForegroundBrush<'t when 't :> TextBox>(value: IBrush) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, IBrush>(TextBox.SelectionForegroundBrushProperty, value, ValueNone)
            
        static member selectionForegroundBrush<'t when 't :> TextBox>(color: string) : IAttr<'t> =
            color |> Color.Parse |> ImmutableSolidColorBrush |> TextBox.selectionForegroundBrush       
            
        static member caretBrush<'t when 't :> TextBox>(value: IBrush) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, IBrush>(TextBox.CaretBrushProperty, value, ValueNone)
            
        static member caretBrush<'t when 't :> TextBox>(color: string) : IAttr<'t> =
            color |> Color.Parse |> ImmutableSolidColorBrush |> TextBox.selectionBrush
            
        static member selectionStart<'t when 't :> TextBox>(value: int) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, int>(TextBox.SelectionStartProperty, value, ValueNone)     
            
        static member selectionEnd<'t when 't :> TextBox>(value: int) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, int>(TextBox.SelectionEndProperty, value, ValueNone)     
            
        static member maxLength<'t when 't :> TextBox>(value: int) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, int>(TextBox.MaxLengthProperty, value, ValueNone)     
            
        static member text<'t when 't :> TextBox>(value: string) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, string>(TextBox.TextProperty, value, ValueNone)
            
        static member onTextChanged<'t when 't :> TextBox>(func: string -> unit, ?subPatchOptions) =
            AttrBuilder.CreateSubscription<'t, string>(TextBox.TextProperty, func, ?subPatchOptions = subPatchOptions)
            
        static member textAlignment<'t when 't :> TextBox>(alignment: TextAlignment) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, TextAlignment>(TextBox.TextAlignmentProperty, alignment, ValueNone)
            
        static member textWrapping<'t when 't :> TextBox>(value: TextWrapping) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, TextWrapping>(TextBox.TextWrappingProperty, value, ValueNone)
            
        static member useFloatingWatermark<'t when 't :> TextBox>(value: bool) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, bool>(TextBox.UseFloatingWatermarkProperty, value, ValueNone)
            
        static member newLine<'t when 't :> TextBox>(value: string) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, string>(TextBox.NewLineProperty, value, ValueNone)
            
        static member watermark<'t when 't :> TextBox>(value: string) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, string>(TextBox.WatermarkProperty, value, ValueNone)