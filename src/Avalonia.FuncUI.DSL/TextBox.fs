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

        static member text<'t when 't :> TextBox>(value: string) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<string>(TextBox.TextProperty, value, ValueNone)
            
        static member onTextChanged<'t when 't :> TextBox>(func: string -> unit) =
            AttrBuilder<'t>.CreateSubscription<string>(TextBox.TextProperty, func)
            
        static member background<'t when 't :> TextBox>(value: IBrush) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<IBrush>(TextBox.BackgroundProperty, value, ValueNone)
            
        static member background<'t when 't :> TextBox>(color: string) : IAttr<'t> =
            color |> Color.Parse |> ImmutableSolidColorBrush |> TextBox.background
        
        static member fontFamily<'t when 't :> TextBox>(value: FontFamily) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<FontFamily>(TextBox.FontFamilyProperty, value, ValueNone)
            
        static member fontSize<'t when 't :> TextBox>(value: double) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<double>(TextBox.FontSizeProperty, value, ValueNone)
            
        static member fontStyle<'t when 't :> TextBox>(value: FontStyle) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<FontStyle>(TextBox.FontStyleProperty, value, ValueNone)
            
        static member fontWeight<'t when 't :> TextBox>(value: FontWeight) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<FontWeight>(TextBox.FontWeightProperty, value, ValueNone)
            
        static member foreground<'t when 't :> TextBox>(value: IBrush) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<IBrush>(TextBox.ForegroundProperty, value, ValueNone)
            
        static member foreground<'t when 't :> TextBox>(color: string) : IAttr<'t> =
            color |> Color.Parse |> ImmutableSolidColorBrush |> TextBox.foreground
            
        static member textAlignment<'t when 't :> TextBox>(alignment: TextAlignment) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<TextAlignment>(TextBox.TextAlignmentProperty, alignment, ValueNone)
            
        static member textWrapping<'t when 't :> TextBox>(value: TextWrapping) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<TextWrapping>(TextBox.TextWrappingProperty, value, ValueNone)