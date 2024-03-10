namespace Avalonia.FuncUI

open Avalonia.Controls

[<AutoOpen>]
module Helpers =
    open Avalonia.FuncUI.Types

    let generalize (view: IView<'t>) : IView =
        view :> IView

[<AutoOpen>]
module AvaloniaExtensions =
    open Avalonia.Markup.Xaml.Styling
    open System
    open Avalonia.Styling

    type Styles with

        /// <summary>
        /// Loads a style from a XAML file.
        /// <remarks>
        /// TabItem.axaml is a Style in the example below.
        /// </remarks>
        /// <example>
        /// <code>
        /// this.Styles.Load "avares://MyApp/Assets/Styles/TabItem.axaml"
        /// </code>
        /// </example>
        /// </summary>
        member this.Load (source: string) =
            let style = StyleInclude(baseUri = null)
            style.Source <- Uri(source)
            this.Add(style)

    type IResourceDictionary with

        /// <summary>
        /// Loads a resource dictionary from a XAML file.
        /// <remarks>
        /// TabItem.axaml is a ResourceDictionary in the example below.
        /// </remarks>
        /// <example>
        /// <code>
        /// this.Resources.Load "avares://MyApp/Assets/Styles/TabItem.axaml"
        /// </code>
        /// </example>
        /// </summary>
        member this.Load (source: string) =
            let resource = ResourceInclude(baseUri = null)
            resource.Source <- Uri(source)
            this.MergedDictionaries.Add(resource)


module internal EqualityComparers =
    open Avalonia.Media

    let compareTransforms (t1: obj, t2: obj) =
        match t1, t2 with
        | :? ITransform as t1, (:? ITransform as t2) when t1.GetType() = t2.GetType() ->
            t1.Value.Equals(t2.Value)
        | _ -> false
