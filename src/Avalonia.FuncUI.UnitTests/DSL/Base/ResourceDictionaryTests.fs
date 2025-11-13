namespace Avalonia.FuncUI.UnitTests.DSL

open Avalonia
open Avalonia.Controls
open global.Xunit

module ResourceDictionaryTests =
    open Avalonia.FuncUI.VirtualDom
    open Avalonia.FuncUI.DSL
    open Avalonia.FuncUI.Types
    open Avalonia.FuncUI.VirtualDom

    module private Headless =
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

    module private VirtualDom =
        open Avalonia.FuncUI.Builder

        let create<'t when 't :> AvaloniaObject> (view: IView<'t>) : 't = VirtualDom.createObject view :?> 't

        let update<'t when 't :> AvaloniaObject> (target: 't) (last: IView<'t>) (next: IView<'t>) : unit =
            let diff = Differ.diff (last, next)
            Patcher.patch (target, diff)

    module private Assert =
        open Avalonia.Styling

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

    module ``keyValue`` =


        [<Fact>]
        let ``can set`` () =
            let initView = ResourceDictionary.create []
            let target = VirtualDom.create initView

            let key = "myKey"
            let value = "myValue"

            Assert.notContainsKey target key

            let updatedView =
                ResourceDictionary.create [ ResourceDictionary.keyValue (key, value) ]

            VirtualDom.update target initView updatedView

            Assert.containsKeyAndValueEqual target key value

            let revertedView = ResourceDictionary.create []
            VirtualDom.update target updatedView revertedView

            Assert.notContainsKey target key

        [<Fact>]
        let ``can set null value`` () =
            let initView = ResourceDictionary.create []
            let target = VirtualDom.create initView

            let key = "myKey"
            let value = null
            Assert.notContainsKey target key

            let updatedView =
                ResourceDictionary.create [ ResourceDictionary.keyValue (key, value = value) ]

            VirtualDom.update target initView updatedView
            Assert.containsKeyAndValueEqual target key value

            VirtualDom.update target updatedView initView
            Assert.notContainsKey target key


        [<Fact>]
        let ``can update value`` () =
            let key = "myKey"

            let createView (value: obj) =
                ResourceDictionary.create [ ResourceDictionary.keyValue (key, value = value) ]

            let createViewWithIView value =
                ResourceDictionary.create [ ResourceDictionary.keyValue (key, view = value) ]

            let initialValue = "initialValue"
            let initView = createView initialValue
            let target = VirtualDom.create initView

            Assert.containsKeyAndValueEqual target key initialValue

            List.fold
                (fun lastView acc ->
                    match acc with
                    | Choice1Of2(nextValue: obj) ->
                        let updatedView = createView nextValue
                        VirtualDom.update target lastView updatedView
                        Assert.containsKeyAndValueEqual target key nextValue
                        updatedView
                    | Choice2Of2(nextValue, expected, converter) ->
                        let updatedView = createViewWithIView nextValue
                        VirtualDom.update target lastView updatedView
                        Assert.containsKeyAndValueEqualWith target key expected converter
                        updatedView)
                initView
                [ Choice1Of2 42
                  Choice1Of2 "a string"
                  Choice1Of2 null
                  Choice1Of2 3.14
                  Choice2Of2(TextBlock.create [ TextBlock.text "Hello" ], "Hello", fun o -> (o :?> TextBlock).Text)
                  Choice1Of2 true
                  Choice1Of2(TextBox(Text = "Hello, World!"))
                  Choice1Of2(ContentControl(Content = Button(Content = "Click Me"))) ]


        [<Fact>]
        let ``can update key`` () =
            let initialKey = "initialKey"
            let updatedKey = "updatedKey"
            let value = "myValue"

            let initView =
                ResourceDictionary.create [ ResourceDictionary.keyValue (initialKey, value) ]

            let target = VirtualDom.create initView

            Assert.containsKeyAndValueEqual target initialKey value

            let updatedView =
                ResourceDictionary.create [ ResourceDictionary.keyValue (updatedKey, value) ]

            VirtualDom.update target initView updatedView

            Assert.notContainsKey target initialKey
            Assert.containsKeyAndValueEqual target updatedKey value


        [<Fact>]
        let ``can set multiple`` () =
            let key1 = "key1"
            let value1 = "value1"
            let updatedValue1 = "updatedValue1"
            let key2 = typeof<Button>
            let value2 = Styling.ControlTheme(key2)
            let updatedValue2 = Styling.ControlTheme(key2)

            let initView =
                ResourceDictionary.create
                    [ ResourceDictionary.keyValue (key1, value1)
                      ResourceDictionary.keyValue (key2, value2) ]

            let target = VirtualDom.create initView

            Assert.containsKeyAndValueEqual target key1 value1
            Assert.containsKeyAndValueEqual target key2 value2

            let updatedView =
                ResourceDictionary.create
                    [ ResourceDictionary.keyValue (key1, updatedValue1)
                      ResourceDictionary.keyValue (key2, updatedValue2) ]

            VirtualDom.update target initView updatedView

            Assert.containsKeyAndValueEqual target key1 updatedValue1
            Assert.containsKeyAndValueEqual target key2 updatedValue2

            let revertedView = ResourceDictionary.create []
            VirtualDom.update target updatedView revertedView

            Assert.notContainsKey target key1
            Assert.notContainsKey target key2

        [<Fact>]
        let ``can set with IView`` () =
            let assertResourceTextEquals dict key expectedText =
                Assert.containsKeyAndValueEqualWith dict key expectedText (fun o -> (o :?> TextBlock).Text)

            let initView = ResourceDictionary.create []
            let target = VirtualDom.create initView

            let key = "myKey"

            Assert.notContainsKey target key

            let initText = "Hello, World!"
            let valueView = TextBlock.create [ TextBlock.text initText ]

            let updatedView =
                ResourceDictionary.create [ ResourceDictionary.keyValue (key, valueView) ]

            VirtualDom.update target initView updatedView
            assertResourceTextEquals target key initText

            let updatedText = "Goodbye, World!"
            let updatedValueView = TextBlock.create [ TextBlock.text updatedText ]

            let updatedView2 =
                ResourceDictionary.create [ ResourceDictionary.keyValue (key, updatedValueView) ]

            VirtualDom.update target updatedView updatedView2

            assertResourceTextEquals target key updatedText

            let revertedView = ResourceDictionary.create []
            VirtualDom.update target updatedView revertedView

            Assert.notContainsKey target key

        [<Fact>]
        let ``with object creates new instance on update`` () =
            let key = "myKey"
            let text = "TextBlock Text"

            let view () =
                ResourceDictionary.create [ ResourceDictionary.keyValue (key, TextBlock(Text = text)) ]

            let initialView = view ()
            let target = VirtualDom.create initialView
            let initialTextBlock = Assert.containsKey target key :?> TextBlock
            Assert.Equal(text, initialTextBlock.Text)

            let updatedView = view ()
            VirtualDom.update target initialView updatedView

            let updatedTextBlock = Assert.containsKey target key :?> TextBlock
            Assert.NotEqual(initialTextBlock, updatedTextBlock)
            Assert.Equal(text, updatedTextBlock.Text)

        [<Fact>]
        let ``with IView reuses existing instance when patchable`` () =
            let key = "myKey"
            let initialText = "Initial Text"
            let updatedText = "Updated Text"

            let view text =
                ResourceDictionary.create
                    [ ResourceDictionary.keyValue (key, TextBlock.create [ TextBlock.text text ]) ]

            let initialView = view initialText
            let target = VirtualDom.create initialView
            let initialTextBlock = Assert.containsKey target key :?> TextBlock
            Assert.Equal(initialText, initialTextBlock.Text)

            let updatedView = view updatedText
            VirtualDom.update target initialView updatedView

            let updatedTextBlock = Assert.containsKey target key :?> TextBlock
            Assert.Equal(initialTextBlock, updatedTextBlock)
            Assert.Equal(updatedText, updatedTextBlock.Text)

    module ``mergedDictionaries`` =
        open System
        open Avalonia.Media

        [<Fact>]
        let ``can set merged dictionaries`` () =
            let initView = ResourceDictionary.create []
            let target = VirtualDom.create initView

            let dict1 =
                ResourceDictionary.create [ ResourceDictionary.keyValue ("key1", "value1") ]

            let dict2 =
                ResourceDictionary.create [ ResourceDictionary.keyValue ("key2", "value2") ]

            let updatedView =
                ResourceDictionary.create [ ResourceDictionary.mergedDictionaries [ dict1; dict2 ] ]

            VirtualDom.update target initView updatedView

            Assert.containsKeyAndValueEqual target "key1" "value1"
            Assert.containsKeyAndValueEqual target "key2" "value2"

        [<Fact>]
        let ``TryGetResource should find resource from later merged dictionary`` () =
            let initView = ResourceDictionary.create []
            let target = VirtualDom.create initView

            let sameKey = "sharedKey"

            let dict1 =
                ResourceDictionary.create [ ResourceDictionary.keyValue (sameKey, "valueFromDict1") ]

            let dict2 =
                ResourceDictionary.create [ ResourceDictionary.keyValue (sameKey, "valueFromDict2") ]

            let updatedView =
                ResourceDictionary.create [ ResourceDictionary.mergedDictionaries [ dict1; dict2 ] ]

            VirtualDom.update target initView updatedView

            Assert.containsKeyAndValueEqual target sameKey "valueFromDict2"

        [<Fact>]
        let ``can add ResourcesInclude to merged dictionaries`` () =
            Headless.dispatch (fun () ->
                let initView = ResourceDictionary.create []
                let target = VirtualDom.create initView
                let host = StyledElement()
                host.Resources <- target

                let assertResources kv =
                    for (key, color) in kv do
                        let brush = Assert.containsKey target key |> Assert.IsType<SolidColorBrush>
                        Assert.Equal(color, brush.Color)

                let include1 =
                    ResourceInclude.fromUri (new Uri("avares://Avalonia.FuncUI.UnitTests/Assets/TestResources1.xaml"))

                let updatedView =
                    ResourceDictionary.create [ ResourceDictionary.mergedDictionaries [ include1 ] ]

                VirtualDom.update target initView updatedView

                let include1KeyValues =
                    [ ("BrushA", Colors.Red); ("BrushB", Colors.Green); ("BrushC", Colors.Blue) ]

                // Assert initial resources from include1
                assertResources include1KeyValues

                let include2 =
                    ResourceInclude.fromString "avares://Avalonia.FuncUI.UnitTests/Assets/TestResources2.xaml"

                // Add another ResourceInclude
                let updatedView2 =
                    ResourceDictionary.create [ ResourceDictionary.mergedDictionaries [ include1; include2 ] ]

                VirtualDom.update target updatedView updatedView2

                let include2KeyValues =
                    [ ("BrushA", Colors.White); ("BrushB", Colors.Black); ("BrushC", Colors.Gray) ]

                // Now the values should come from include2
                assertResources include2KeyValues

                // Switch the order of includes
                let updatedView3 =
                    ResourceDictionary.create [ ResourceDictionary.mergedDictionaries [ include2; include1 ] ]

                VirtualDom.update target updatedView2 updatedView3

                // Now the values should come from include1 again
                assertResources include1KeyValues)


    [<Fact>]
    let ``TryGetResource should find itself before merged dictionaries`` () =
        let initView = ResourceDictionary.create []
        let target = VirtualDom.create initView

        let key = "myKey"

        let updatedView =
            ResourceDictionary.create
                [ ResourceDictionary.keyValue (key, "valueFromMainDict")
                  ResourceDictionary.mergedDictionaries
                      [ ResourceDictionary.create [ ResourceDictionary.keyValue (key, "valueFromMergedDict") ] ] ]

        VirtualDom.update target initView updatedView

        Assert.containsKeyAndValueEqual target key "valueFromMainDict"

    module ``themeDictionariesKeyValue`` =
        open Avalonia.Styling

        [<Fact>]
        let ``can set theme dictionaries key value`` () =
            let key = "themeKey"
            let darkValue = "darkValue"
            let lightValue = "lightValue"

            let view =
                ResourceDictionary.create
                    [ ResourceDictionary.themeDictionariesKeyValue (
                          ThemeVariant.Dark,
                          ResourceDictionary.create [ ResourceDictionary.keyValue (key, darkValue) ]
                      )
                      ResourceDictionary.themeDictionariesKeyValue (
                          ThemeVariant.Light,
                          ResourceDictionary.create [ ResourceDictionary.keyValue (key, lightValue) ]
                      ) ]

            let target = VirtualDom.create view

            Assert.containsThemeKeyAndValueEqual target ThemeVariant.Dark key darkValue
            Assert.containsThemeKeyAndValueEqual target ThemeVariant.Light key lightValue


    module ``onResourceObservable`` =
        open Avalonia.Styling

        [<Fact>]
        let ``when Owner is not set, callback is not fired`` () =
            let key = "observableKey"
            let value = "observableValue"
            let mutable isCalled = false

            let callback (_: obj option) = isCalled <- true

            let initView =
                ResourceDictionary.create
                    [ ResourceDictionary.onResourceObservable (key, callback)
                      ResourceDictionary.keyValue (key, value) ]

            let target = VirtualDom.create initView
            Assert.Null(target.Owner)

            // No Owner set, so callback should not be called
            Assert.False(isCalled, "Callback was unexpectedly called when Owner is not set.")


        [<Fact>]
        let ``when Owner is set, callback is fired`` () =
            let target = ResourceDictionary()
            let host = StyledElement()
            host.Resources <- target
            Assert.NotNull(target.Owner)

            let key = "observableKey"
            let initialValue = "initialValue"
            let updatedValue = "updatedValue"

            let mutable observedValue: obj option = Some(box "no value changed.")
            let callback v = observedValue <- v

            let initView =
                ResourceDictionary.create [ ResourceDictionary.onResourceObservable (key, callback) ]

            VirtualDom.update target (ResourceDictionary.create []) initView

            // No initial value set, so observedValue should be None
            Assert.Equal(None, observedValue)

            let updatedView1 =
                ResourceDictionary.create
                    [ ResourceDictionary.keyValue (key, initialValue)
                      ResourceDictionary.onResourceObservable (key, callback) ]

            VirtualDom.update target initView updatedView1


            // Initial notification
            Assert.Equal(Some(box initialValue), observedValue)

            let updatedView2 =
                ResourceDictionary.create
                    [ ResourceDictionary.onResourceObservable (key, callback)
                      ResourceDictionary.keyValue (key, updatedValue) ]

            VirtualDom.update target updatedView1 updatedView2

            // Notification after update
            Assert.Equal(Some(box updatedValue), observedValue)

        [<Fact>]
        let ``defaultThemeVariant tests`` () =
            let target = ResourceDictionary()
            let host = StyledElement()
            host.Resources <- target

            let key = "themeObservableKey"
            let darkValue = "darkValue"
            let lightValue = "lightValue"

            let mutable observedValue: obj option = None
            let callback v = observedValue <- v

            let initView =
                ResourceDictionary.create
                    [ ResourceDictionary.themeDictionariesKeyValue (
                          ThemeVariant.Dark,
                          ResourceDictionary.create [ ResourceDictionary.keyValue (key, darkValue) ]
                      )
                      ResourceDictionary.themeDictionariesKeyValue (
                          ThemeVariant.Light,
                          ResourceDictionary.create [ ResourceDictionary.keyValue (key, lightValue) ]
                      )
                      ResourceDictionary.onResourceObservable (key, callback, defaultThemeVariant = ThemeVariant.Dark) ]

            VirtualDom.update target (ResourceDictionary.create []) initView

            // Initial notification with Dark theme
            Assert.Equal(Some(box darkValue), observedValue)

            let updatedView =
                ResourceDictionary.create
                    [ ResourceDictionary.themeDictionariesKeyValue (
                          ThemeVariant.Dark,
                          ResourceDictionary.create [ ResourceDictionary.keyValue (key, darkValue) ]
                      )
                      ResourceDictionary.themeDictionariesKeyValue (
                          ThemeVariant.Light,
                          ResourceDictionary.create [ ResourceDictionary.keyValue (key, lightValue) ]
                      )
                      ResourceDictionary.onResourceObservable (key, callback, defaultThemeVariant = ThemeVariant.Light) ]

            VirtualDom.update target initView updatedView

            // Notification after update with Light theme
            Assert.Equal(Some(box lightValue), observedValue)
