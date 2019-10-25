namespace Avalonia.FuncUI.DSL
open Avalonia.Media.Immutable

[<AutoOpen>]
module TextBlock =  
    open Avalonia.Controls
    open Avalonia.Media    
    open Avalonia.FuncUI.Builder
    open Avalonia.FuncUI.Types

    let create (attrs: IAttr<TextBlock> list): IView<TextBlock> =
        ViewBuilder.Create<TextBlock>(attrs)
    
    type TextBlock with
            
        static member text<'t when 't :> TextBlock>(value: string) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<string>(TextBlock.TextProperty, value, ValueNone)
            
        static member background<'t when 't :> TextBlock>(value: IBrush) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<IBrush>(TextBlock.BackgroundProperty, value, ValueNone)
            
        static member background<'t when 't :> TextBlock>(color: string) : IAttr<'t> =
            color |> Color.Parse |> ImmutableSolidColorBrush |> TextBlock.background
        
        static member fontFamily<'t when 't :> TextBlock>(value: FontFamily) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<FontFamily>(TextBlock.FontFamilyProperty, value, ValueNone)
            
        static member fontSize<'t when 't :> TextBlock>(value: double) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<double>(TextBlock.FontSizeProperty, value, ValueNone)
            
        static member fontStyle<'t when 't :> TextBlock>(value: FontStyle) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<FontStyle>(TextBlock.FontStyleProperty, value, ValueNone)
            
        static member fontWeight<'t when 't :> TextBlock>(value: FontWeight) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<FontWeight>(TextBlock.FontWeightProperty, value, ValueNone)
            
        static member foreground<'t when 't :> TextBlock>(value: IBrush) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<IBrush>(TextBlock.ForegroundProperty, value, ValueNone)
            
        static member foreground<'t when 't :> TextBlock>(color: string) : IAttr<'t> =
            color |> Color.Parse |> ImmutableSolidColorBrush |> TextBlock.foreground
            
        static member textAlignment<'t when 't :> TextBlock>(alignment: TextAlignment) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<TextAlignment>(TextBlock.TextAlignmentProperty, alignment, ValueNone)
            
        static member textWrapping<'t when 't :> TextBlock>(value: TextWrapping) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<TextWrapping>(TextBlock.TextWrappingProperty, value, ValueNone)