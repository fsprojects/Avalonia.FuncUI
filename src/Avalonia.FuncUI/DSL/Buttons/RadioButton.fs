namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module RadioButton =
    open Avalonia.Controls
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
    open Avalonia.Interactivity
   
    let create (attrs: IAttr<RadioButton> list): IView<RadioButton> =
        ViewBuilder.Create<RadioButton>(attrs)

    type RadioButton with

        static member groupName<'t when 't :> RadioButton>(value: string) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<string>(RadioButton.GroupNameProperty, value, ValueNone)
        
        static member onChecked<'t when 't :> RadioButton>(func: RoutedEventArgs -> unit, ?subPatchOptions) : IAttr<'t> =

            let onCheckedHandler (args: RoutedEventArgs) =
                match args.Source with
                | :? RadioButton as selection when selection.IsChecked.GetValueOrDefault() = true ->
                    func(args)
                | _ -> ()

            AttrBuilder<'t>.CreateSubscription<RoutedEventArgs>(RadioButton.IsCheckedChangedEvent, onCheckedHandler, ?subPatchOptions = subPatchOptions)