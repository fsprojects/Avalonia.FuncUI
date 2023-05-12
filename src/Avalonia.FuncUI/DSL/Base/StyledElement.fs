namespace Avalonia.FuncUI.DSL
open Avalonia.Controls
open Avalonia.Controls.Primitives

[<AutoOpen>]
module StyledElement =  
    open Avalonia
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
    open Avalonia.Styling
            
    type StyledElement with
        static member dataContext<'t when 't :> StyledElement>(dataContext: obj) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<obj>(StyledElement.DataContextProperty, dataContext, ValueNone)
            
        static member name<'t when 't :> StyledElement>(name: string) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<string>(StyledElement.NameProperty, name, ValueNone)
            
        static member templatedParent<'t when 't :> StyledElement>(template: TemplatedControl) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<AvaloniaObject>(StyledElement.TemplatedParentProperty, template, ValueNone)
        
        static member theme<'t when 't :> StyledElement>(theme: ControlTheme) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<ControlTheme>(StyledElement.ThemeProperty, theme, ValueNone)

        static member classes<'t when 't :> StyledElement>(value: Classes) : IAttr<'t> =
            let getter : ('t -> Classes) = (fun control -> control.Classes)
            let setter : ('t * Classes -> unit) = (fun (control, value) ->              
                control.Classes.Clear()
                control.Classes.AddRange(value))
            
            AttrBuilder<'t>.CreateProperty<Classes>("Classes", value, ValueSome getter, ValueSome setter, ValueNone, fun () -> Classes())
            
        static member classes<'t when 't :> StyledElement>(classes: string list) : IAttr<'t> =
            classes |> Classes |> StyledElement.classes  
             
        /// Use 'classes' instead when possible.
        static member styles<'t when 't :> StyledElement>(value: Styles) : IAttr<'t> =
            let getter : ('t -> Styles) = (fun control -> control.Styles)
            let setter : ('t * Styles -> unit) = 
                (fun (control, value) -> 
                     control.Styles.Clear()
                     control.Styles.AddRange(value))

            AttrBuilder<'t>.CreateProperty<Styles>("Styles", value, ValueSome getter, ValueSome setter, ValueNone, fun () -> Styles())
            
        static member resources<'t when 't :> StyledElement>(value: IResourceDictionary) : IAttr<'t> =
            let getter : ('t -> IResourceDictionary) = (fun control -> control.Resources)
            let setter : ('t * IResourceDictionary -> unit) = (fun (control, value) -> control.Resources <- value)
            let factory = fun () -> ResourceDictionary() :> IResourceDictionary
            
            AttrBuilder<'t>.CreateProperty<IResourceDictionary>("Resources", value, ValueSome getter, ValueSome setter, ValueNone, factory)
            
        // Attached properties related to text input
        
        static member contentType<'t when 't :> StyledElement>(value) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<_>(Avalonia.Input.TextInput.TextInputOptions.ContentTypeProperty, value, ValueNone)
            
        static member returnKeyType<'t when 't :> StyledElement>(value) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<_>(Avalonia.Input.TextInput.TextInputOptions.ReturnKeyTypeProperty, value, ValueNone)
            
        static member multiline<'t when 't :> StyledElement>(value) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<_>(Avalonia.Input.TextInput.TextInputOptions.MultilineProperty, value, ValueNone)
            
        static member autoCapitalization<'t when 't :> StyledElement>(value) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<_>(Avalonia.Input.TextInput.TextInputOptions.AutoCapitalizationProperty, value, ValueNone)
            
        static member isSensitive<'t when 't :> StyledElement>(value) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<_>(Avalonia.Input.TextInput.TextInputOptions.IsSensitiveProperty, value, ValueNone)
            
        static member uppercase<'t when 't :> StyledElement>(value) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<_>(Avalonia.Input.TextInput.TextInputOptions.UppercaseProperty, value, ValueNone)
            
        static member lowercase<'t when 't :> StyledElement>(value) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<_>(Avalonia.Input.TextInput.TextInputOptions.LowercaseProperty, value, ValueNone)
