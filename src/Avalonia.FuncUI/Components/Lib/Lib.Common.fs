namespace Avalonia.FuncUI

open System
open System.Collections.Generic
open Microsoft.FSharp.Core

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


[<RequireQualifiedAccess>]
module internal RenderFunctionAnalysis =
    open System
    open System.Reflection
    open System.Collections.Concurrent

    let internal cache = ConcurrentDictionary<Type, bool>()

    let private flags =
        BindingFlags.Instance |||
        BindingFlags.NonPublic |||
        BindingFlags.Public

    let capturesState (func : obj) : bool =
        let type' = func.GetType()

        let hasValue, value = cache.TryGetValue type'

        match hasValue with
        | true -> value
        | false ->
            let capturesState =
                type'.GetConstructors(flags)
                |> Array.map (fun info -> info.GetParameters().Length)
                |> Array.exists (fun parameterLength -> parameterLength > 0)

            cache.AddOrUpdate(type', capturesState, (fun identifier lastValue -> capturesState))