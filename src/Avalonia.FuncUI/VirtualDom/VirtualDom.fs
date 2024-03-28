namespace Avalonia.FuncUI.VirtualDom

open Avalonia.Controls

open Avalonia.FuncUI.Types
open Avalonia.FuncUI.VirtualDom.Delta

module rec VirtualDom =

    let create (view: IView) : Control =
        view
        |> ViewDelta.From
        |> Patcher.create :?> Control

    let update (root: Control, last: IView, next: IView) : unit =
        let delta = Differ.diff(last, next)
        Patcher.patch(root, delta)

    let updateRoot (host: ContentControl, last: IView option, next: IView option) =
        let root : Control voption =
            if not (isNull (host.Content)) then
                match host.Content with
                | :? Control as control -> ValueSome control
                | _ -> ValueNone
            else
                ValueNone

        let delta : ViewDelta voption =
            match last with
            | Some last ->
                match next with
                | Some next -> Differ.diff (last, next) |> ValueSome
                | None -> ValueNone
            | None ->
                match next with
                | Some next -> ViewDelta.From next |> ValueSome
                | None -> ValueNone

        match root with
        | ValueSome control ->
            match delta with
            | ValueSome delta ->
                match control.GetType () = delta.ViewType && not delta.KeyDidChange with
                | true -> Patcher.patch (control, delta)
                | false -> host.Content <- Patcher.create delta
            | ValueNone ->
                host.Content <- null

        | ValueNone ->
            match delta with
            | ValueSome delta -> host.Content <- Patcher.create delta
            | ValueNone -> host.Content <- null

    // TODO: share code with updateRoot
    let internal updateBorderRoot (host: Border, last: IView option, next: IView option) =
        let root : Control voption =
            if not (isNull (host.Child)) then
                ValueSome host.Child
            else
                ValueNone

        let delta : ViewDelta voption =
            match last with
            | Some last ->
                match next with
                | Some next -> Differ.diff (last, next) |> ValueSome
                | None -> ValueNone
            | None ->
                match next with
                | Some next -> ViewDelta.From next |> ValueSome
                | None -> ValueNone

        match root with
        | ValueSome control ->
            match delta with
            | ValueSome delta ->
                match control.GetType () = delta.ViewType && not delta.KeyDidChange with
                | true -> Patcher.patch (control, delta)
                | false -> host.Child <- (Patcher.create delta) :?> Control
            | ValueNone ->
                host.Child <- null

        | ValueNone ->
            match delta with
            | ValueSome delta -> host.Child <- (Patcher.create delta) :?> Control
            | ValueNone -> host.Child <- null