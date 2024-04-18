namespace Avalonia.FuncUI.DSL

open Avalonia
open Avalonia.FuncUI
open Avalonia.FuncUI.Types
open System.Threading

[<AutoOpen>]
module AvaloniaObject =
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.Builder

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

        static member onPropertyChanged<'t when 't :> AvaloniaObject>(func: AvaloniaPropertyChangedEventArgs -> unit, ?subPatchOptions)  : IAttr<'t> =
            let factory: AvaloniaObject * (AvaloniaPropertyChangedEventArgs -> unit) * CancellationToken -> unit =
                (fun (control, func, token) ->
                    let control = control :?> 't
                    let disposable = control.PropertyChanged.Subscribe(func)

                    token.Register(fun () -> disposable.Dispose()) |> ignore)

            AttrBuilder<'t>.CreateSubscription<AvaloniaPropertyChangedEventArgs>("PropertyChanged", factory, func, ?subPatchOptions = subPatchOptions)

        member this.Bind(prop: DirectPropertyBase<'value>, readable: #IReadable<'value>) : unit =
            let _ = this.Bind(property = prop, source = readable.ImmediateObservable)
            ()

        member this.Bind(prop: StyledProperty<'value>, readable: #IReadable<'value>) : unit =
            let _ = this.Bind(property = prop, source = readable.ImmediateObservable)
            ()