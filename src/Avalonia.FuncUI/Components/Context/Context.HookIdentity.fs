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

    member this.StringValue =
        match this with
        | CallerCodeLocation line -> $"L%i{line}"
        | HookIdentity.StringIdentity identity -> identity

    member this.WithPrefix (prefix: string) : HookIdentity =
        HookIdentity.StringIdentity $"%s{prefix}.{this.StringValue}"

    override this.ToString () =
        this.StringValue

