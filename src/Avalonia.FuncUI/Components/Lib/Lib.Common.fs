namespace Avalonia.FuncUI

open System
open System.Collections.Generic

[<RequireQualifiedAccess>]
module internal ComponentHelpers =

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
module internal String =

    let ofLines (lines: #seq<string>) : string =
        String.Join (Environment.NewLine, lines)

[<RequireQualifiedAccess>]
module internal Map =

    let ofDict (items: IDictionary<'key, 'value>) : Map<'key, 'value> =
        items
        |> Seq.map (fun pair -> pair.Key, pair.Value)
        |> Map.ofSeq

[<AutoOpen>]
module internal CommonExtensions =

    type Guid with
        member this.StringValue with get () = this.ToString()
        static member Unique with get () = Guid.NewGuid()
