namespace Avalonia.FuncUI.Benchmarks

open BenchmarkDotNet.Attributes
open BenchmarkDotNet.Running

[<MemoryDiagnoser; >]
type Bench () =

    [<Benchmark(Baseline = true)>]
    member this.Baseline () : unit =
        ()

    [<Benchmark>]
    member this.Champion () : unit =
        ()

module Program =

    [<EntryPoint>]
    let main (args: string array) : int =
        let _ = BenchmarkRunner.Run<Bench>()
        0
