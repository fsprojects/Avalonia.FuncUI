namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module CalendarItem =
    open Avalonia.Controls.Primitives
    open Avalonia.Controls.Templates
    open Avalonia.Media
    open Avalonia.Media.Immutable
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder

    let create (attrs: IAttr<CalendarItem> list): IView<CalendarItem> =
        ViewBuilder.Create<CalendarItem>(attrs)

    type CalendarItem with

        /// <summary>Set the background color of the header</summary>
        /// 
        /// <remarks>Because of CalendarItem is sealed, this binding is only for CalendarItem</remarks>
        static member headerBackground(value: IBrush) : IAttr<CalendarItem> =
            AttrBuilder<CalendarItem>.CreateProperty<IBrush>(CalendarItem.HeaderBackgroundProperty, value, ValueNone)


        /// <summary>Set the background color of the header</summary>
        /// 
        /// <remarks>Because of CalendarItem is sealed, this binding is only for CalendarItem</remarks>
        static member headerBackground(color: string) : IAttr<CalendarItem> =
            color |> Color.Parse |> ImmutableSolidColorBrush |> CalendarItem.headerBackground

        /// <summary>Set the foreground color of the header</summary>
        /// 
        /// <remarks>Because of CalendarItem is sealed, this binding is only for CalendarItem</remarks>
        static member headerForeground(color: Color) : IAttr<CalendarItem> =
            color |> ImmutableSolidColorBrush |> CalendarItem.headerBackground

        /// <summary>Set day title template.</summary>
        /// 
        /// <remarks>Because of CalendarItem is sealed, this binding is only for CalendarItem</remarks>
        static member dayTitleTemplate(value: IDataTemplate) : IAttr<CalendarItem> =
            AttrBuilder<CalendarItem>.CreateProperty<IDataTemplate>(CalendarItem.DayTitleTemplateProperty, value, ValueNone)
