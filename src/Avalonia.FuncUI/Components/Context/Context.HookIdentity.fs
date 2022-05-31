namespace Avalonia.FuncUI


[<RequireQualifiedAccess; Struct>]
type HookIdentity =
    /// Line number of the hooks caller
    | CallerCodeLocation of line: int
    /// <summary>
    /// String identity of the hook (provided explicitly or is implicitly composed)
    /// </summary>
    /// <remarks>
    /// Useful when multiple (primitive) hooks are combined to create a more complex hook, so a single caller line
    /// number is not enough to uniquely identify sub-hooks.
    /// </remarks>
    | StringIdentity of identity: string

    override this.ToString () =
        match this with
        | CallerCodeLocation line -> $"Line: {line}"
        | HookIdentity.StringIdentity identity -> identity