namespace Avalonia.FuncUI

open Avalonia

type PreviewAttribute () =
    inherit System.Attribute ()

    member this.Args = Array.empty