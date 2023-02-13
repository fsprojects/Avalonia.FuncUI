namespace Avalonia.FuncUI

open System.ComponentModel
open Avalonia
open Avalonia.Controls
open Avalonia.Data
open Avalonia.FuncUI.Types
open Avalonia.Styling
open System.Runtime.CompilerServices
open System.Runtime.InteropServices

[<AutoOpen>]
module __Bindable =

    type Control with
        static member init<'t when 't :> Control>(func: 't -> unit) : IAttr<'t> =
            Attr.InitFunction {
                InitFunction.Function = (fun (control: obj) -> func (control :?> 't))
            }

    type IAvaloniaObject with

        member this.Bind(prop: DirectPropertyBase<'value>, readable: IReadable<'value>) : unit =
            let _ = this.Bind(property = prop, source = readable.ImmediateObservable)
            ()

        member this.Bind(prop: StyledPropertyBase<'value>, readable: IReadable<'value>) : unit =
            let _ = this.Bind(property = prop, source = readable.ImmediateObservable)
            ()

[<AbstractClass>]
type StaticComponent () as this =
    inherit Border ()

    override this.OnInitialized () =
        base.OnInitialized ()

        this.DataContext <- this
        this.Child <-
            ()
            |> this.Build
            |> VirtualDom.VirtualDom.create

    abstract member Build: unit -> IView

    interface IStyleable with
        member this.StyleKey = typeof<Border>