namespace Avalonia.FuncUI.Elmish

open Elmish
open Avalonia.FuncUI.Components.Hosts
open Avalonia.FuncUI.Types
open Avalonia.Threading

module Program =

    let private syncDispatch (dispatch: Dispatch<'msg>) : Dispatch<'msg> =
        match Dispatcher.UIThread.CheckAccess() with
        | true -> fun msg -> Dispatcher.UIThread.Post (fun () -> dispatch msg)
        | false -> fun msg -> dispatch msg
    
    let withHost (host: IViewHost) (program: Program<'arg, 'model, 'msg, #IView>) =
        let setState state dispatch =
            let view = ((Program.view program) state dispatch)
            host.Update (Some (view :> IView))

        program
        |> Program.withSetState setState
        |> Program.withSyncDispatch syncDispatch
