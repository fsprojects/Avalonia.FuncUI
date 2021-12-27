namespace Avalonia.FuncUI.Components

open System
open Avalonia
open Avalonia.Controls
open Avalonia.Controls.Templates
open Avalonia.FuncUI.Library
open Avalonia.FuncUI.Types
open Avalonia.FuncUI.Components.Hosts
open Avalonia.Data
open Avalonia.Data.Core
open System.Linq.Expressions

type DataTemplateView<'data, 'childData, 'view when 'view :> IView>
    (viewFunc: 'data -> 'view,
     matchFunc: ('data -> bool) voption,
     itemsSource: Expression<Func<'data, 'childData seq>> voption,
     supportsRecycling: bool) =

    member this.ViewFunc = viewFunc
    member this.MatchFunc = matchFunc
    member this.ItemsSource = itemsSource
    member this.SupportsRecycling = supportsRecycling

    override this.Equals (other: obj) : bool =
        match other with
        | :? DataTemplateView<'data, 'childData, 'view> as other ->
            this.ViewFunc.GetType() = other.ViewFunc.GetType() &&
            this.MatchFunc.GetType() = other.MatchFunc.GetType() &&
            this.SupportsRecycling = other.SupportsRecycling
        | _ -> false

    override this.GetHashCode () =
        (this.ViewFunc.GetType(), this.SupportsRecycling).GetHashCode()

    interface ITreeDataTemplate with
        member this.ItemsSelector (item: obj) : InstancedBinding =
            match this.ItemsSource with
            | ValueNone -> null
            | ValueSome expression ->
                match item with
                | :? 'data as data ->
                    InstancedBinding.OneTime(expression.Compile().Invoke(data))
                | _ -> null

        member this.Match (data: obj) : bool =
            match data, matchFunc with
            | :? 'data as data, ValueSome f -> f data
            | :? 'data, ValueNone -> true
            | _ -> false

        member this.Build (_data: obj) : IControl =
            let host = HostControl()

            let update (data: 'data) : unit =
                let view = Some (this.ViewFunc data :> IView)
                (host :> IViewHost).Update view

            host
                .GetObservable(Control.DataContextProperty)
                .SubscribeWeakly<obj>((fun data ->
                    match data with
                    | :? 'data as t -> update(t)
                    | _ -> ()
                ), this) |> ignore

            host :> IControl

type DataTemplateView<'data, 'view when 'view :> IView> =
    /// <summary>
    /// Create a DataTemplateView for data matching type ('data)
    /// </summary>
    /// <typeparam name="'data">The Type of the data.</typeparam>
    /// <param name="viewFunc">The function that creates a view from the passed data.</param>
    static member create(viewFunc: 'data -> 'view when 'view :> IView) : DataTemplateView<'data, 'childData, 'view> =
        DataTemplateView<'data, 'childData, 'view>(viewFunc = (fun a -> viewFunc a),
                                matchFunc = ValueNone,
                                itemsSource = ValueNone,
                                supportsRecycling = true)

    /// <summary>
    /// Create a DataTemplateView for data matching type ('data)
    /// </summary>
    /// <typeparam name="'data">The Type of the data.</typeparam>
    /// <param name="viewFunc">The function that creates a view from the passed data.</param>
    /// <param name="matchFunc">The function that decides if this template is capable of creating a view from the passed data.</param>
    static member create(viewFunc: 'data ->  'view when 'view :> IView, matchFunc: 'data -> bool) : DataTemplateView<'data, 'childData, 'view> =
        DataTemplateView<'data, 'childData, 'view>(viewFunc = (fun a -> viewFunc a),
                                matchFunc = ValueSome matchFunc,
                                itemsSource = ValueNone,
                                supportsRecycling = true)

    /// <summary>
    /// Create a DataTemplateView for data matching type ('data)
    /// </summary>
    /// <typeparam name="'data">The Type of the data.</typeparam>
    /// <param name="viewFunc">The function that creates a view from the passed data.</param>
    static member create(itemsSelector, viewFunc: 'data -> 'view when 'view :> IView) : DataTemplateView<'data, 'childData, 'view> =
        DataTemplateView<'data, 'childData, 'view>(viewFunc = (fun a -> viewFunc a),
                                matchFunc = ValueNone,
                                itemsSource = ValueSome itemsSelector,
                                supportsRecycling = true)

    /// <summary>
    /// Create a DataTemplateView for data matching type ('data)
    /// </summary>
    /// <typeparam name="'data">The Type of the data.</typeparam>
    /// <param name="viewFunc">The function that creates a view from the passed data.</param>
    /// <param name="matchFunc">The function that decides if this template is capable of creating a view from the   passed data.</param>
    static member create(itemsSelector, viewFunc: 'data -> 'view when 'view :> IView, matchFunc: 'data -> bool) : DataTemplateView<'data, 'childData, 'view> =
        DataTemplateView<'data, 'childData, 'view>(viewFunc = (fun a -> viewFunc a),
                                matchFunc = ValueSome matchFunc,
                                itemsSource = ValueSome itemsSelector,
                                supportsRecycling = true)

type DataTemplateView<'data> =
    /// <summary>
    /// Create a DataTemplateView for data matching type ('data)
    /// </summary>
    /// <typeparam name="'data">The Type of the data.</typeparam>
    /// <param name="viewFunc">The function that creates a view from the passed data.</param>
    static member create(viewFunc: 'data -> 'view when 'view :> IView) : DataTemplateView<'data, 'childData, 'view> =
        DataTemplateView<'data, 'childData, 'view>(viewFunc = (fun a -> viewFunc a),
                                matchFunc = ValueNone,
                                itemsSource = ValueNone,
                                supportsRecycling = true)

    /// <summary>
    /// Create a DataTemplateView for data matching type ('data)
    /// </summary>
    /// <typeparam name="'data">The Type of the data.</typeparam>
    /// <param name="viewFunc">The function that creates a view from the passed data.</param>
    /// <param name="matchFunc">The function that decides if this template is capable of creating a view from the passed data.</param>
    static member create(viewFunc: 'data ->  'view when 'view :> IView, matchFunc: 'data -> bool) : DataTemplateView<'data, 'childData, 'view> =
        DataTemplateView<'data, 'childData, 'view>(viewFunc = (fun a -> viewFunc a),
                                matchFunc = ValueSome matchFunc,
                                itemsSource = ValueNone,
                                supportsRecycling = true)

    /// <summary>
    /// Create a DataTemplateView for data matching type ('data)
    /// </summary>
    /// <typeparam name="'data">The Type of the data.</typeparam>
    /// <param name="viewFunc">The function that creates a view from the passed data.</param>
    static member create(itemsSelector, viewFunc: 'data -> 'view when 'view :> IView) : DataTemplateView<'data, 'childData, 'view> =
        DataTemplateView<'data, 'childData, 'view>(viewFunc = (fun a -> viewFunc a),
                                matchFunc = ValueNone,
                                itemsSource = ValueSome itemsSelector,
                                supportsRecycling = true)

    /// <summary>
    /// Create a DataTemplateView for data matching type ('data)
    /// </summary>
    /// <typeparam name="'data">The Type of the data.</typeparam>
    /// <param name="viewFunc">The function that creates a view from the passed data.</param>
    /// <param name="matchFunc">The function that decides if this template is capable of creating a view from the   passed data.</param>
    static member create(itemsSelector, viewFunc: 'data -> 'view when 'view :> IView, matchFunc: 'data -> bool) : DataTemplateView<'data, 'childData, 'view> =
        DataTemplateView<'data, 'childData, 'view>(viewFunc = (fun a -> viewFunc a),
                                matchFunc = ValueSome matchFunc,
                                itemsSource = ValueSome itemsSelector,
                                supportsRecycling = true)
