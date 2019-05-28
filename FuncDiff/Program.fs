// Learn more about F# at http://fsharp.org

open System

type Msg = Increment | Decrement



[<EntryPoint>]
let main argv =
    let dispatch (msg: Msg) =
        printf "%A" msg

    let logger (msg: string) =
        printf "%s" msg

    let a () =
        dispatch Increment
        logger "dispatched increment"

    let b = fun () ->
        dispatch Increment
        logger "dispatched increment"

    let c = fun () -> ()

    0
