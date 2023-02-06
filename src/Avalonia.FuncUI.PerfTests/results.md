

## Baseline (master - M1 - net 6.0)

|     Method |     Mean |     Error |    StdDev |    Gen 0 |    Gen 1 |    Gen 2 | Allocated |
|----------- |---------:|----------:|----------:|---------:|---------:|---------:|----------:|
| CreateView | 2.258 ms | 0.0174 ms | 0.0163 ms | 738.2813 | 269.5313 | 101.5625 |   2.45 MB |

## Changes (use structs everywhere - M1 - net 6.0)

|     Method |     Mean |     Error |    StdDev |    Gen 0 |    Gen 1 |    Gen 2 | Allocated |
|----------- |---------:|----------:|----------:|---------:|---------:|---------:|----------:|
| CreateView | 3.468 ms | 0.0332 ms | 0.0294 ms | 906.2500 | 339.8438 | 144.5313 |   3.63 MB |
