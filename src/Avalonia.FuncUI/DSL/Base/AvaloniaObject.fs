namespace Avalonia.FuncUI.DSL

open Avalonia
open Avalonia.FuncUI
open Avalonia.FuncUI.Types

[<AutoOpen>]
module AvaloniaObject =

    type AvaloniaObject with

        /// <summary>
        /// Hook into the controls lifetime. This is called when the backing avalonia control is created.
        /// <example>
        /// <code>
        /// TextBlock.create [
        ///     TextBlock.init (fun textBlock -&gt;
        ///         textBlock.Bind(TextBlock.TextProperty, counter.Map string)
        ///         textBlock.Bind(TextBlock.ForegroundProperty, counter.Map (fun c -&gt;
        ///             if c &lt; 0
        ///             then Brushes.Red :&gt; IBrush
        ///             else Brushes.Green :&gt; IBrush
        ///         ))
        ///     )
        /// ]
        /// </code>
        /// </example>
        /// </summary>
        static member init<'t when 't :> AvaloniaObject>(func: 't -> unit) : IAttr<'t> =
            Attr.InitFunction {
                InitFunction.Function = (fun (control: obj) -> func (control :?> 't))
            }

        member this.Bind(prop: DirectPropertyBase<'value>, readable: #IReadable<'value>) : unit =
            let _ = this.Bind(property = prop, source = readable.ImmediateObservable)
            ()

        member this.Bind(prop: StyledProperty<'value>, readable: #IReadable<'value>) : unit =
            let _ = this.Bind(property = prop, source = readable.ImmediateObservable)
            ()