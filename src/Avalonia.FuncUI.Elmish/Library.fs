namespace Avalonia.FuncUI.Elmish

open Elmish
open Avalonia.FuncUI.Hosts
open Avalonia.FuncUI.Types
open Avalonia.Threading

module Program =

    let private syncDispatch (dispatch: Dispatch<'msg>) : Dispatch<'msg> =
        fun msg ->
            let checkAccess = Dispatcher.UIThread.CheckAccess()
            if checkAccess then
                dispatch msg
            else
                Dispatcher.UIThread.Post (fun () -> dispatch msg)

    let withHost (host : IViewHost) (program : Program<'arg, 'model, 'msg, #IView>) =
        let stateRef = ref None
        let setState state dispatch =
            // create new view and update the host only if new model is not equal to a prev one
            let stateDiffers = (Some state).Equals(stateRef.Value) |> not
            
            if stateDiffers then
                stateRef.Value <- Some state
                let view = ((Program.view program) state dispatch)
                host.Update (Some (view :> IView))

        program
        |> Program.withSetState setState
        |> Program.withSyncDispatch syncDispatch
