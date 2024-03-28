namespace Avalonia.FuncUI.Diagnostics

open System
open Avalonia
open Avalonia.Controls.ApplicationLifetimes
open Avalonia.FuncUI
open Avalonia.FuncUI.Diagnostics
open Avalonia.LogicalTree
open Avalonia.Threading

[<CustomComparison; CustomEquality>]
type ComponentRef =
    { Ref: Component }

    member this.ComponentId with get () = this.Ref.ComponentId

    override this.Equals(other: obj) =
        match other with
        | :? ComponentRef as other -> this.ComponentId = other.ComponentId
        | _ -> false

    override this.GetHashCode() = this.ComponentId.GetHashCode()

    interface IComparable with
        member this.CompareTo (other: obj) =
            match other with
            | :? ComponentRef as other -> this.ComponentId.CompareTo(other.ComponentId)
            | _ -> -1

    static member Create(comp: Component) =
        { ComponentRef.Ref = comp }

[<RequireQualifiedAccess>]
type internal InspectorWindowPosition =
    | Pinned
    | Normal

type internal InspectorState =
    { InspectorWindowPosition: IWritable<InspectorWindowPosition>
      Components: IWritable<Set<ComponentRef>>
      SelectedComponentId: IWritable<Guid option>
      SelectedComponent: IReadable<ComponentRef option> }

    static member Create () =
        let inspectorWindowPosition: IWritable<InspectorWindowPosition> = State InspectorWindowPosition.Pinned
        let componentsState: IWritable<Set<ComponentRef>> = State Set.empty :> _
        let selectedComponentIdState: IWritable<Guid option> = UniqueValue(State None) :> _
        let selectedComponentState =
            componentsState
            |> State.readMap (fun comps ->
                comps
                |> Set.toList
                |> List.sortBy (fun comp -> comp.ComponentId)
            )
            |> State.readTryFindByKey (fun c -> Some c.ComponentId) selectedComponentIdState

        let state =
            { InspectorWindowPosition = inspectorWindowPosition
              Components = componentsState
              SelectedComponentId = selectedComponentIdState
              SelectedComponent = selectedComponentState }

        state

module internal InspectorState =
    let shared = InspectorState.Create ()

    let private fetchComponents () =
        match Application.Current.ApplicationLifetime with
        | :? IClassicDesktopStyleApplicationLifetime as lifetime ->
            lifetime.Windows
            |> Seq.filter (fun window -> window.Tag <> ("inspector" :> _))
            |> Seq.map (fun window -> LogicalExtensions.GetSelfAndLogicalDescendants(window))
            |> Seq.concat
            |> Seq.filter (fun control -> control.GetType() = typeof<Component>)
            |> Seq.cast
            |> Seq.map ComponentRef.Create
            |> Set.ofSeq
        | _ ->
            Set.empty

    let private refreshComponents (last: Set<ComponentRef>) =
        let next = fetchComponents ()

        let removed = Set.difference last next
        for item in removed do
            if not (isNull (item.Ref)) then
                ComponentHighlightAdorner.Remove item.Ref

        shared.Components.Set next

    let private refreshTimer =
        let timer = DispatcherTimer ()
        timer.Interval <- TimeSpan.FromSeconds 1.0
        timer.IsEnabled <- true
        timer.Tick.Add (fun _ ->
            refreshComponents shared.Components.Current
        )

        timer
