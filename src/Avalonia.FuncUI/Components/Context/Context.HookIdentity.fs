namespace Avalonia.FuncUI

[<RequireQualifiedAccess; Struct>]
type HookIdentity =
    | CallerCodeLocation of line: int

    override this.ToString () =
        match this with
        | CallerCodeLocation line -> $"Line: {line}"


