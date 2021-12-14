namespace Avalonia.FuncUI

open System

[<RequireQualifiedAccess>]
module internal Helpers =

    let safeFastEquals (a: 't, b: 't) =
        let ao: obj = a :> _
        let bo: obj = b :> _

        if ao <> null then
            if bo <> null then
                ao.Equals(bo)
            else
                false
        else
            bo = null

[<RequireQualifiedAccess>]
module String =

    let ofLines (lines: #seq<string>) : string =
        String.Join (Environment.NewLine, lines)
