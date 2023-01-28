namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module DataValidationErrors =
    open System.Collections.Generic
    open Avalonia.Controls.Templates
    open Avalonia.Controls
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder

    let create (attrs: Attr<DataValidationErrors> list): View<DataValidationErrors> =
        ViewBuilder.Create<DataValidationErrors>(attrs)

    type Control with
        static member errors<'t when 't :> Control>(errors: IEnumerable<obj>) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty(DataValidationErrors.ErrorsProperty, errors, ValueNone)

        static member hasErrors<'t when 't :> Control>(hasErrors: bool) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty(DataValidationErrors.HasErrorsProperty, hasErrors, ValueNone)

    type DataValidationErrors with
        static member errorTemplate<'t when 't :> DataValidationErrors>(template: IDataTemplate) : Attr<'t> =
            AttrBuilder<'t>.CreateProperty(DataValidationErrors.ErrorTemplateProperty, template, ValueNone)