namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module DatePicker =
    open System
    open Avalonia.Controls
    open Avalonia.Controls.Templates
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
    
    let create (attrs: IAttr<DatePicker> list): IView<DatePicker> =
        ViewBuilder.Create<DatePicker>(attrs)
     
    type DatePicker with

        static member selectedDate<'t when 't :> DatePicker>(value: DateTimeOffset Nullable) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<DateTimeOffset Nullable>(DatePicker.SelectedDateProperty, value, ValueNone)

        static member selectedDate<'t when 't :> DatePicker>(value: DateTimeOffset) : IAttr<'t> =
            value |> Nullable |> DatePicker.selectedDate

        static member selectedDate<'t when 't :> DatePicker>(value: DateTimeOffset option) : IAttr<'t> =
            value |> Option.toNullable |> DatePicker.selectedDate

        static member onSelectedDateChanged<'t when 't :> DatePicker>(func: Nullable<DateTimeOffset> -> unit, ?subPatchOptions) : IAttr<'t> =
            AttrBuilder<'t>.CreateSubscription<DateTimeOffset Nullable>(DatePicker.SelectedDateProperty, func, ?subPatchOptions = subPatchOptions)

        static member dayVisible<'t when 't :> DatePicker>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(DatePicker.DayVisibleProperty, value, ValueNone)

        static member monthVisible<'t when 't :> DatePicker>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(DatePicker.MonthVisibleProperty, value, ValueNone)

        static member yearVisible<'t when 't :> DatePicker>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(DatePicker.YearVisibleProperty, value, ValueNone)

        static member dayFormat<'t when 't :> DatePicker>(value: string) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<string>(DatePicker.DayFormatProperty, value, ValueNone)

        static member monthFormat<'t when 't :> DatePicker>(value: string) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<string>(DatePicker.MonthFormatProperty, value, ValueNone)

        static member yearFormat<'t when 't :> DatePicker>(value: string) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<string>(DatePicker.YearFormatProperty, value, ValueNone)

        static member minYear<'t when 't :> DatePicker>(value: DateTimeOffset) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<DateTimeOffset>(DatePicker.MinYearProperty, value, ValueNone)
            
        static member maxYear<'t when 't :> DatePicker>(value: DateTimeOffset) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<DateTimeOffset>(DatePicker.MaxYearProperty, value, ValueNone)

        static member header<'t when 't :> DatePicker>(value: obj) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<obj>(DatePicker.HeaderProperty, value, ValueNone)
        
        static member headerTemplate<'t when 't :> DatePicker>(template: IDataTemplate) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<IDataTemplate>(DatePicker.HeaderTemplateProperty, template, ValueNone)
        
            




