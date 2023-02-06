open System
open BenchmarkDotNet.Running
open Avalonia.FuncUI.PerfTests

[<EntryPoint>]
let main argv =
    //BenchmarkRunner.Run<CollectionBenchmarks>() |> ignore

    //VirtualDomCreationBenchmarks().CreateView() |> ignore

    CollectionBenchmarks().FSharpList() |> ignore
    0 // return an integer exit code