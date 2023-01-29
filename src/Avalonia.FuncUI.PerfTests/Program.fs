open System
open BenchmarkDotNet.Running
open Avalonia.FuncUI.PerfTests

[<EntryPoint>]
let main argv =
    BenchmarkRunner.Run<VirtualDomCreationBenchmarks>() |> ignore
    0 // return an integer exit code