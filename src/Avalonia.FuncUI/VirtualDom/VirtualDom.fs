namespace Avalonia.FuncUI.VirtualDom

open Avalonia.Controls

open Avalonia.FuncUI.Types
open Avalonia.FuncUI.VirtualDom.Delta

module rec VirtualDom =

    let create (view: IView) : Avalonia.IAvaloniaObject =
        view
        |> ViewDelta.From
        |> Patcher.create

    let update (root: IControl, last: IView, next: IView) : unit =
        let delta = Differ.diff(last, next)
        Patcher.patch(root, delta)

    let updateRoot (host: IContentControl, last: IView option, next: IView option) =
        let root : IControl voption =
            if host.Content <> null then
                match host.Content with
                | :? IControl as control -> ValueSome control
                | _ -> ValueNone
            else ValueNone

        let delta : ViewDelta voption =
            match last with
            | Some last ->
                match next with
                | Some next -> Differ.diff (last, next) |> ValueSome
                | None -> ValueNone
            | None ->
                match next with
                | Some next -> ViewDelta.From (next) |> ValueSome
                | None -> ValueNone

        match root with
        | ValueSome control ->
            match delta with
            | ValueSome delta ->
                match control.GetType() = delta.viewType with
                | true -> Patcher.patch (control, delta)
                | false -> host.Content <- Patcher.create (delta)
            | ValueNone -> host.Content <- null
        | ValueNone ->
            match delta with
            | ValueSome delta -> host.Content <- Patcher.create (delta)
            | ValueNone -> host.Content <- null