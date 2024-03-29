﻿namespace Avalonia.FuncUI.DSL

[<AutoOpen>]
module Spinner =
    open Avalonia.Controls
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder

    type Spinner with

        /// <summary>
        /// Sets <see cref="ValidSpinDirections"/> allowed for this control.
        /// </summary>
        static member validSpinDirection<'t when 't :> Spinner>(value: ValidSpinDirections) : IAttr<'t> =
            AttrBuilder<'t>.CreateProperty<ValidSpinDirections>(Spinner.ValidSpinDirectionProperty, value, ValueNone)

        /// <summary>
        /// Occurs when spinning is initiated by the end-user.
        /// </summary>
        static member onSpin<'t when 't :> Spinner>(func: SpinEventArgs -> unit, ?subPatchOptions) =
            AttrBuilder<'t>.CreateSubscription<SpinEventArgs>(Spinner.SpinEvent, func, ?subPatchOptions = subPatchOptions)

