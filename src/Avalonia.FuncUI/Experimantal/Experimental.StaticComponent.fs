namespace Avalonia.FuncUI.Experimental

open Avalonia.Controls
open Avalonia.FuncUI.Types
open Avalonia.FuncUI.VirtualDom

/// A component that only evaluates its build/render function once. All other changes to the UI are driven by
/// bindings / manually manipulating controls.
[<AbstractClass>]
type StaticComponent () =
    inherit Border ()

    override this.OnInitialized () =
        base.OnInitialized ()

        this.DataContext <- this
        this.Child <-
            ()
            |> this.Build
            |> VirtualDom.create

    abstract member Build: unit -> IView

    override this.StyleKeyOverride = typeof<Border>