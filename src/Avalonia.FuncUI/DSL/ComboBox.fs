namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module ComboBox =
    open Avalonia.Controls
    open Avalonia.Layout
    open Avalonia.Media
    open Avalonia.Media.Immutable
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
    
    let create (attrs: IAttr<ComboBox> list): IView<ComboBox> =
        ViewBuilder.Create<ComboBox>(attrs)
    
    type ComboBox with
        static member onDropDownClosed<'t when 't :> ComboBox>(func: 't -> unit, ?subPatchOptions) =
            let name = nameof Unchecked.defaultof<'t>.DropDownClosed

            let factory: SubscriptionFactory<'t> =
                fun (control, func, token) ->
                    let control = control :?> 't
                    let handler = System.EventHandler(fun s _ -> func (s :?> 't))
                    let event = control.DropDownClosed

                    event.AddHandler(handler)
                    token.Register(fun _ -> event.RemoveHandler(handler)) |> ignore

            AttrBuilder<'t>.CreateSubscription<'t>(name, factory, func, ?subPatchOptions = subPatchOptions)

        static member onDropDownOpened<'t when 't :> ComboBox>(func: 't -> unit, ?subPatchOptions) =
            let name = nameof Unchecked.defaultof<'t>.DropDownOpened

            let factory: SubscriptionFactory<'t> =
                fun (control, func, token) ->
                    let control = control :?> 't
                    let handler = System.EventHandler(fun s _ -> func (s :?> 't))
                    let event = control.DropDownOpened

                    event.AddHandler(handler)
                    token.Register(fun _ -> event.RemoveHandler(handler)) |> ignore

            AttrBuilder<'t>.CreateSubscription<'t>(name, factory, func, ?subPatchOptions = subPatchOptions)
        static member isDropDownOpen<'t when 't :> ComboBox>(value: bool) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<bool>(ComboBox.IsDropDownOpenProperty, value, ValueNone)
            
        static member horizontalContentAlignment<'t when 't :> ComboBox>(alignment: HorizontalAlignment) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<HorizontalAlignment>(ComboBox.HorizontalContentAlignmentProperty, alignment, ValueNone)
            
        static member maxDropDownHeight<'t when 't :> ComboBox>(height: float) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<float>(ComboBox.MaxDropDownHeightProperty, height, ValueNone)

        static member placeholderText<'t when 't :> ComboBox>(text: string) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<string>(ComboBox.PlaceholderTextProperty, text, ValueNone)

        static member placeholderForeground<'t when 't :> ComboBox>(value: IBrush) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<IBrush>(ComboBox.PlaceholderForegroundProperty, value, ValueNone)

        static member placeholderForeground<'t when 't :> ComboBox>(color:string) : IAttr<'t> =
            color |> Color.Parse |> ImmutableSolidColorBrush |> ComboBox.placeholderForeground

        static member placeholderForeground<'t when 't :> ComboBox>(color:Color) : IAttr<'t> =
            color |> ImmutableSolidColorBrush |> ComboBox.placeholderForeground
        static member verticalContentAlignment<'t when 't :> ComboBox>(alignment: VerticalAlignment) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<VerticalAlignment>(ComboBox.VerticalContentAlignmentProperty, alignment, ValueNone)
