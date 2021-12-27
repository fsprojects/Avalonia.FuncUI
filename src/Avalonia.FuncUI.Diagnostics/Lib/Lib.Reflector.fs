namespace Avalonia.FuncUI.Diagnostics

open System

[<RequireQualifiedAccess>]
module internal Reflector =

    let rec fullTypeName (forType: Type) : string =
        let genericDefault =
            forType.GetGenericArguments()
            |> Seq.map fullTypeName
            |> String.joinWith ", "

        let genericTuple =
            forType.GetGenericArguments()
            |> Seq.map fullTypeName
            |> String.joinWith " * "

        let genericFunc =
            forType.GetGenericArguments()
            |> Seq.map fullTypeName
            |> String.joinWith " -> "

        if String.IsNullOrEmpty genericDefault then
            if forType.FullName.StartsWith "Microsoft.FSharp.Core.Unit" then
                "unit"
            elif forType.FullName.StartsWith "System.String" then
                "string"
            elif forType.FullName.StartsWith "System.Guid" then
                "Guid"
            elif forType.FullName.StartsWith "System.Int32" then
                "int"
            elif forType.FullName.StartsWith "System.Decimal" then
                "decimal"
            elif forType.FullName.StartsWith "System.Double" then
                "float"
            elif forType.FullName.StartsWith "System.Single" then
                "float32"
            elif forType.FullName.StartsWith "System.Boolean" then
                "bool"
            else
                $"{forType.Name}"
        else
            if forType.FullName.StartsWith "Microsoft.FSharp.Collections.FSharpList" then
                $"{genericTuple} list"
            elif forType.FullName.StartsWith "Maecos.Shared.NonEmptyList" then
                $"{genericTuple} nelist"
            elif forType.FullName.StartsWith "Microsoft.FSharp.Core.FSharpOption" then
                $"{genericTuple} option"
            elif forType.FullName.StartsWith "Microsoft.FSharp.Control.FSharpAsync" then
                $"Async<{genericDefault}>"
            elif forType.FullName.StartsWith "Microsoft.FSharp.Core.FSharpResult" then
                $"Result<{genericDefault}>"
            elif forType.FullName.StartsWith "System.Tuple" then
                $"({genericTuple})"
            elif forType.FullName.StartsWith "Microsoft.FSharp.Core.FSharpFunc" then
                $"{genericFunc}"
            else
                $"{forType.Name}<{genericDefault}>"