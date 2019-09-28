namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module TemplatedControl =  
    open Avalonia.Controls
    open Avalonia.Media.Immutable
    open System    
    open System.Windows.Input
    open Avalonia
    open Avalonia.FuncUI.Core.Domain
    open Avalonia.Media
    open Avalonia.Styling
    open Avalonia.Controls.Primitives
    open Avalonia.Controls.Templates
    open Avalonia.Animation
    open Avalonia.Layout
    open Avalonia.Interactivity
    open Avalonia.Input
      
    type TemplatedControl with
        static member background<'t when 't :> TemplatedControl>(value: #IBrush) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty TemplatedControl.BackgroundProperty
            let property = Property.createDirect(accessor, value)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member background<'t when 't :> TemplatedControl>(color: string) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty TemplatedControl.BackgroundProperty
            let property = Property.createDirect(accessor, ImmutableSolidColorBrush(Color.Parse(color)))
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member borderBrush<'t when 't :> TemplatedControl>(value: #IBrush) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty TemplatedControl.BorderBrushProperty
            let property = Property.createDirect(accessor, value)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>

        static member fontFamily<'t when 't :> TemplatedControl>(value: FontFamily) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty TemplatedControl.FontFamilyProperty
            let property = Property.createDirect(accessor, value)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member fontSize<'t when 't :> TemplatedControl>(value: double) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty TemplatedControl.FontSizeProperty
            let property = Property.createDirect(accessor, value)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member fontWeight<'t when 't :> TemplatedControl>(value: FontWeight) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty TemplatedControl.FontWeightProperty
            let property = Property.createDirect(accessor, value)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member foreground<'t when 't :> TemplatedControl>(value: #IBrush) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty TemplatedControl.ForegroundProperty
            let property = Property.createDirect(accessor, value)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member padding<'t when 't :> TemplatedControl>(value: Thickness) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty TemplatedControl.PaddingProperty
            let property = Property.createDirect(accessor, value)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>

        static member template<'t when 't :> TemplatedControl>(value: #IControlTemplate) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty TemplatedControl.TemplateProperty
            let property = Property.createDirect(accessor, value)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>        
        
        static member isTemplateFocusTarget<'t when 't :> TemplatedControl>(value: bool) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty TemplatedControl.IsTemplateFocusTargetProperty
            let property = Property.createAttached(accessor, value)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>

