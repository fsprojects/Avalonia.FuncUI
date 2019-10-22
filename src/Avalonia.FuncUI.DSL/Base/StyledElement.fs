namespace Avalonia.FuncUI.DSL

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