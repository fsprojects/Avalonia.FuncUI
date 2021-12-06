namespace Avalonia.FuncUI

[<RequireQualifiedAccess>]
type Deferred<'t> =
    | NotStartedYet
    | Pending
    | Resolved of 't
    | Failed of exn