namespace Avalonia.FuncUI.DSL
open Avalonia.Controls

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
            
        static member templatedParent<'t when 't :> StyledElement>(template: ITemplatedControl) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<ITemplatedControl>(StyledElement.TemplatedParentProperty, template, ValueNone)
            
        static member classes<'t when 't :> StyledElement>(value: Classes) : IAttr<'t> =
            let getter : ('t -> Classes) = (fun control -> control.Classes)
            let setter : ('t * Classes -> unit) = (fun (control, value) -> control.Classes <- value)
            
            AttrBuilder<'t>.CreateProperty<Classes>("Classes", value, ValueSome getter, ValueSome setter, ValueNone)
            
        static member classes<'t when 't :> StyledElement>(classes: string list) : IAttr<'t> =
            classes |> Classes |> StyledElement.classes  
             
        static member styles<'t when 't :> StyledElement>(value: Styles) : IAttr<'t> =
            let getter : ('t -> Styles) = (fun control -> control.Styles)
            let setter : ('t * Styles -> unit) = (fun (control, value) -> control.Styles <- value)
            
            AttrBuilder<'t>.CreateProperty<Styles>("Styles", value, ValueSome getter, ValueSome setter, ValueNone)
            
        static member resources<'t when 't :> StyledElement>(value: IResourceDictionary) : IAttr<'t> =
            let getter : ('t -> IResourceDictionary) = (fun control -> control.Resources)
            let setter : ('t * IResourceDictionary -> unit) = (fun (control, value) -> control.Resources <- value)
            
            AttrBuilder<'t>.CreateProperty<IResourceDictionary>("Resources", value, ValueSome getter, ValueSome setter, ValueNone)