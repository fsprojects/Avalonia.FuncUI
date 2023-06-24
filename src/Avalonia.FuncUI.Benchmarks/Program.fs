namespace Avalonia.FuncUI.Benchmarks

open BenchmarkDotNet.Attributes
open BenchmarkDotNet.Running

module Program =

    [<EntryPoint>]
    let main (args: string array) : int =
        let _ = BenchmarkRunner.Run<Scenarios.Diffing.GameOfLifeBench>()
        0
