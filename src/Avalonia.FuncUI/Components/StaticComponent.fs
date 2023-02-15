namespace Avalonia.FuncUI

open Avalonia
open Avalonia.Controls
open Avalonia.FuncUI.Types
open Avalonia.Styling

[<AbstractClass>]
[<Experimental "Statically construct views with F#">]
type StaticComponent () =
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