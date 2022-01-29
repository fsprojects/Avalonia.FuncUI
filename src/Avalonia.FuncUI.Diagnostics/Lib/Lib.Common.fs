namespace Avalonia.FuncUI.Diagnostics

open System
open Avalonia.FuncUI

[<RequireQualifiedAccess>]
module internal String =

    let joinWith (separator: string) (parts: string seq) =
        String.Join(separator, parts)
