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
            AttrBuilder.CreateProperty<'t, obj>(StyledElement.DataContextProperty, dataContext, ValueNone)
            
        static member name<'t when 't :> StyledElement>(name: string) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, string>(StyledElement.NameProperty, name, ValueNone)
            
        static member templatedParent<'t when 't :> StyledElement>(template: ITemplatedControl) : IAttr<'t> =
            AttrBuilder.CreateProperty<'t, ITemplatedControl>(StyledElement.TemplatedParentProperty, template, ValueNone)
            
        static member classes<'t when 't :> StyledElement>(value: Classes) : IAttr<'t> =
            let getter : ('t -> Classes) = (fun control -> control.Classes)
            let setter : ('t * Classes -> unit) = (fun (control, value) -> control.Classes <- value)
            
            AttrBuilder.CreateProperty<'t, Classes>("Classes", value, ValueSome getter, ValueSome setter, ValueNone, fun () -> Classes())
            
        static member classes<'t when 't :> StyledElement>(classes: string list) : IAttr<'t> =
            classes |> Classes |> StyledElement.classes  
             
        static member styles<'t when 't :> StyledElement>(value: Styles) : IAttr<'t> =
            let getter : ('t -> Styles) = (fun control -> control.Styles)
            let setter : ('t * Styles -> unit) = (fun (control, value) -> control.Styles <- value)
            
            AttrBuilder.CreateProperty<'t, Styles>("Styles", value, ValueSome getter, ValueSome setter, ValueNone, fun () -> Styles())
            
        static member resources<'t when 't :> StyledElement>(value: IResourceDictionary) : IAttr<'t> =
            let getter : ('t -> IResourceDictionary) = (fun control -> control.Resources)
            let setter : ('t * IResourceDictionary -> unit) = (fun (control, value) -> control.Resources <- value)
            let factory = fun () -> ResourceDictionary() :> IResourceDictionary
            
            AttrBuilder.CreateProperty<'t, IResourceDictionary>("Resources", value, ValueSome getter, ValueSome setter, ValueNone, factory)