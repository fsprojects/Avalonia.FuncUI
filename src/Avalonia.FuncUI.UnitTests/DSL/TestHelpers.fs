namespace Avalonia.FuncUI.UnitTests.DSL

open Avalonia

module Headless =
    open System
    open System.Threading
    open System.Threading.Tasks
    open Avalonia.Headless

    let useSession () =
        typeof<Application> |> HeadlessUnitTestSession.StartNew

    let dispatch fn =
        use session = useSession ()
        let action = Action fn
        session.Dispatch(action, CancellationToken.None)

    let dispatchAsync fn =
        use session = useSession ()
        let action = Func<Task>(fun () -> fn)
        session.Dispatch(action, CancellationToken.None)

module VirtualDom =
    open Avalonia.FuncUI.VirtualDom
    open Avalonia.FuncUI.Types

    let create<'t when 't :> AvaloniaObject> (view: IView<'t>) : 't = VirtualDom.createObject view :?> 't

    let update<'t when 't :> AvaloniaObject> (target: 't) (last: IView<'t>) (next: IView<'t>) : unit =
        let diff = Differ.diff (last, next)
        Patcher.patch (target, diff)

module Assert =
    open global.Xunit
    open Avalonia.Styling
    open Avalonia.Controls

    module ResourceDictionary =

        let containsThemeKey (dict: ResourceDictionary) (theme: ThemeVariant) (key: obj) =
            let isFound, value = dict.TryGetResource(key, theme)
            Assert.True(isFound, $"ResourceDictionary does not contain expected key: {key} for theme: {theme}.")
            value

        let containsKey (dict: ResourceDictionary) (key: obj) =
            let isFound, value = dict.TryGetResource(key, null)
            Assert.True(isFound, $"ResourceDictionary does not contain expected key: {key}.")
            value

        let notContainsThemeKey (dict: ResourceDictionary) (theme: ThemeVariant) (key: obj) =
            let isFound, value = dict.TryGetResource(key, theme)

            Assert.False(
                isFound,
                $"ResourceDictionary unexpectedly contains key: {key} for theme: {theme}. Found value: {value}"
            )

        let notContainsKey (dict: ResourceDictionary) (key: obj) =
            let isFound, value = dict.TryGetResource(key, null)
            Assert.False(isFound, $"ResourceDictionary unexpectedly contains key: {key}. Found value: {value}")

        let containsThemeKeyAndValueEqualWith
            (dict: ResourceDictionary)
            (theme: ThemeVariant)
            (key: 'k)
            (expectedValue: 'v)
            (converter: obj -> 'v)
            =
            let actualValue = containsThemeKey dict theme key
            let expectedValue = box expectedValue

            Assert.Equal(expectedValue, converter actualValue)

        let containsThemeKeyAndValueEqual
            (dict: ResourceDictionary)
            (theme: ThemeVariant)
            (key: 'k)
            (expectedValue: 'v)
            =
            containsThemeKeyAndValueEqualWith dict theme key expectedValue (fun o -> o :?> 'v)

        let containsKeyAndValueEqualWith
            (dict: ResourceDictionary)
            (key: 'k)
            (expectedValue: 'v)
            (converter: obj -> 'v)
            =
            let actualValue = containsKey dict key
            let expectedValue = box expectedValue

            Assert.Equal(expectedValue, converter actualValue)

        let containsKeyAndValueEqual (dict: ResourceDictionary) (key: 'k) (expectedValue: 'v) =
            containsKeyAndValueEqualWith dict key expectedValue (fun o -> o :?> 'v)