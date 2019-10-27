namespace Avalonia.FuncUI.DSL

open Avalonia.Media

[<AutoOpen>]
module Spinner =
    open Avalonia.Controls.Primitives
    open Avalonia.Controls
    open Avalonia.Layout
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder
   
    let create (attrs: IAttr<Spinner> list): IView<Spinner> =
        ViewBuilder.Create<Spinner>(attrs)

    type Viewbox with

        static member validSpinDirection<'t when 't :> Spinner>(value: ValidSpinDirections) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<ValidSpinDirections>(Spinner.ValidSpinDirectionProperty, value, ValueNone)
            
        //static member onSpin<'t when 't :> Spinner>(func: bool -> unit) =
        //    AttrBuilder<'t>.CreateSubscription<SpinDirection>(Spinner.SpinEvent, func)
            

            ///// <summary>
            ///// Defines the <see cref="ValidSpinDirection"/> property.
            ///// </summary>
            //public static readonly StyledProperty<ValidSpinDirections> ValidSpinDirectionProperty =
            //    AvaloniaProperty.Register<Spinner, ValidSpinDirections>(nameof(ValidSpinDirection),
            //        ValidSpinDirections.Increase | ValidSpinDirections.Decrease);

            ///// <summary>
            ///// Defines the <see cref="Spin"/> event.
            ///// </summary>
            //public static readonly RoutedEvent<SpinEventArgs> SpinEvent =
            //    RoutedEvent.Register<Spinner, SpinEventArgs>(nameof(Spin), RoutingStrategies.Bubble);