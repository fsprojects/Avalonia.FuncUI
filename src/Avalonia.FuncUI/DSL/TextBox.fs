namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module TextBox =  
    open Avalonia.Controls
    open Avalonia.Media.Immutable
    open Avalonia.Media
    
    open Avalonia.FuncUI.Types
    
    let create (attrs: IAttr<TextBox> list): IView<TextBox> =
        View.create<TextBox>(attrs)
    
    type TextBox with
            
        static member text<'t when 't :> TextBox>(value: string) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty TextBox.TextProperty
            let property = Property.createDirect(accessor, value)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member onTextChanged<'t when 't :> TextBox>(func: string -> unit) =
            let subscription = Subscription.createFromProperty(TextBox.TextProperty, func)
            let attr = Attr.createSubscription<'t>(subscription)
            attr :> IAttr<'t>
            
        static member background<'t when 't :> TextBox>(value: #IBrush) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty TextBox.BackgroundProperty
            let property = Property.createDirect(accessor, value)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member background<'t when 't :> TextBox>(color: string) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty TextBox.BackgroundProperty
            let property = Property.createDirect(accessor, ImmutableSolidColorBrush(Color.Parse(color)))
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>    
        
        static member fontFamily<'t when 't :> TextBox>(value: FontFamily) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty TextBox.FontFamilyProperty
            let property = Property.createDirect(accessor, value)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member fontSize<'t when 't :> TextBox>(value: double) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty TextBox.FontSizeProperty
            let property = Property.createDirect(accessor, value)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member fontStyle<'t when 't :> TextBox>(value: FontStyle) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty TextBox.FontStyleProperty
            let property = Property.createDirect(accessor, value)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member fontWeight<'t when 't :> TextBox>(value: FontWeight) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty TextBox.FontWeightProperty
            let property = Property.createDirect(accessor, value)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member foreground<'t when 't :> TextBox>(value: #IBrush) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty TextBox.ForegroundProperty
            let property = Property.createDirect(accessor, value)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member textAlignment<'t when 't :> TextBox>(alignment: TextAlignment) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty TextBox.TextAlignmentProperty
            let property = Property.createDirect(accessor, alignment)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>
            
        static member textWrapping<'t when 't :> TextBox>(value: TextWrapping) : IAttr<'t> =
            let accessor = Accessor.AvaloniaProperty TextBox.TextWrappingProperty
            let property = Property.createDirect(accessor, value)
            let attr = Attr.createProperty<'t> property
            attr :> IAttr<'t>

