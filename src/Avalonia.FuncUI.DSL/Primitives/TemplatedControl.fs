namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module TemplatedControl =  
    open Avalonia.Media.Immutable
    open Avalonia
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
    open Avalonia.Media
    open Avalonia.Controls.Primitives
    open Avalonia.Controls.Templates
    
    let create (attrs: IAttr<TemplatedControl> list): IView<TemplatedControl> =
        ViewBuilder.Create<TemplatedControl>(attrs)
        
    type TemplatedControl with
        static member background<'t when 't :> TemplatedControl>(value: IBrush) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<IBrush>(TemplatedControl.BackgroundProperty, value, ValueNone)
            
        static member background<'t when 't :> TemplatedControl>(color: string) : IAttr<'t> =
            Color.Parse(color) |> ImmutableSolidColorBrush |> TemplatedControl.background

        static member borderBrush<'t when 't :> TemplatedControl>(value: IBrush) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<IBrush>(TemplatedControl.BorderBrushProperty, value, ValueNone)
            
        static member borderBrush<'t when 't :> TemplatedControl>(color: string) : IAttr<'t> =
            Color.Parse(color) |> ImmutableSolidColorBrush |> TemplatedControl.borderBrush

        static member fontFamily<'t when 't :> TemplatedControl>(value: FontFamily) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<FontFamily>(TemplatedControl.FontFamilyProperty, value, ValueNone)
            
        static member fontSize<'t when 't :> TemplatedControl>(value: double) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<double>(TemplatedControl.FontSizeProperty, value, ValueNone)
            
        static member fontWeight<'t when 't :> TemplatedControl>(value: FontWeight) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<FontWeight>(TemplatedControl.FontWeightProperty, value, ValueNone)
            
        static member foreground<'t when 't :> TemplatedControl>(value: IBrush) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<IBrush>(TemplatedControl.ForegroundProperty, value, ValueNone)
            
        static member foreground<'t when 't :> TemplatedControl>(color: string) : IAttr<'t> =
            Color.Parse(color) |> ImmutableSolidColorBrush |> TemplatedControl.foreground
            
        static member padding<'t when 't :> TemplatedControl>(value: Thickness) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<Thickness>(TemplatedControl.PaddingProperty, value, ValueNone)
            
        static member padding<'t when 't :> TemplatedControl>(value: float) : IAttr<'t> =
            Thickness(value) |> TemplatedControl.padding
            
        static member padding<'t when 't :> TemplatedControl>(horizontal: float, vertical: float) : IAttr<'t> =
            Thickness(horizontal, vertical) |> TemplatedControl.padding
            
        static member padding<'t when 't :> TemplatedControl>(left: float, top: float, right: float, bottom: float) : IAttr<'t> =
            Thickness(left, top, right, bottom) |> TemplatedControl.padding 

        static member template<'t when 't :> TemplatedControl>(value: IControlTemplate) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<IControlTemplate>(TemplatedControl.TemplateProperty, value, ValueNone)  
        
        static member isTemplateFocusTarget<'t when 't :> TemplatedControl>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(TemplatedControl.IsTemplateFocusTargetProperty, value, ValueNone)  