namespace Avalonia.FuncUI.Elmish

open Elmish
open Avalonia.FuncUI.Hosts
open Avalonia.FuncUI.Types
open Avalonia.Threading

module Program =
    
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

    /// Starts the program loop. Ensures that all view changes from non-UI threads are synchronized via the Avalonia Dispatcher.
    let runWithAvaloniaSyncDispatch (arg: 'arg) (program : Program<'arg, 'model, 'msg, #IView>) = 
        
        // Syncs view changes from non-UI threads through the Avalonia dispatcher.
        let syncDispatch (dispatch: Dispatch<'msg>) (msg: 'msg) =
            if Dispatcher.UIThread.CheckAccess() // Is this already on the UI thread?
            then dispatch msg
            else Dispatcher.UIThread.Post (fun () -> dispatch msg)

        Program.runWithDispatch syncDispatch arg program
