namespace Avalonia.FuncUI.Assembly

open System.Runtime.CompilerServices

[<assembly: InternalsVisibleTo("Avalonia.FuncUI.UnitTests")>]
[<assembly: InternalsVisibleTo("Avalonia.FuncUI.Benchmarks")>]
[<assembly: InternalsVisibleTo("Avalonia.FuncUI.Diagnostics")>]
do()