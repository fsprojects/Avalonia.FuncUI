namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module DataValidationErrors =
    open System.Collections.Generic
    open Avalonia.Controls.Templates
    open Avalonia.Controls
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
    
    let create (attrs: IAttr<DataValidationErrors> list): IView<DataValidationErrors> =
        ViewBuilder.Create<DataValidationErrors>(attrs)
    
    type Control with
        static member errors<'t when 't :> Control>(errors: IEnumerable<obj>) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty(DataValidationErrors.ErrorsProperty, errors, ValueNone)
            
        static member hasErrors<'t when 't :> Control>(hasErrors: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty(DataValidationErrors.HasErrorsProperty, hasErrors, ValueNone)
            
    type DataValidationErrors with

        static member owner<'t when 't :> DataValidationErrors>(value: Control) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty(DataValidationErrors.OwnerProperty, value, ValueNone)

        static member errorTemplate<'t when 't :> DataValidationErrors>(template: IDataTemplate) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty(DataValidationErrors.ErrorTemplateProperty, template, ValueNone)